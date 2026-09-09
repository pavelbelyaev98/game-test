using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SomethingDownThere
{
    public enum WorldSaveState { Loading, Ready, Saving, Recovery, LoadFailed, WriteFailed, ConfirmQuit }

    [DefaultExecutionOrder(500), DisallowMultipleComponent, RequireComponent(typeof(FpsPlayer))]
    public sealed class WorldSaveController : MonoBehaviour
    {
        public const double AutosaveSeconds = 10;
        public WorldSaveState State { get; private set; } = WorldSaveState.Loading;
        public bool BlocksPlay => exitRequested || State != WorldSaveState.Ready && State != WorldSaveState.Saving;
        public string SaveDirectory { get; private set; }
        public string ErrorDetail { get; private set; } = "";
        public string LastSavedLabel { get; private set; } = "No checkpoint yet";
        public bool ExitRequested => exitRequested;
        public long CompletedSequence { get; private set; }
        public double LastCaptureMilliseconds { get; private set; }
        public double LastWriteMilliseconds { get; private set; }
        public double LastCheckpointLatencyMilliseconds { get; private set; }
        public double MaximumDirtySeconds { get; private set; }
        public long CapturedTerrainCopies { get; private set; }
        public event Action Changed;

        private FpsPlayer player;
        private TerrainVolume terrain;
        private DiscoveryField discoveries;
        private WorldSaveStore store;
        private Task write;
        private Task<SaveLoadResult> read;
        private WorldSnapshot writing;
        private GridSnapshot cachedTerrain;
        private long cachedTerrainRevision = -1, dirtyVersion, capturedVersion, savedVersion, nextSequence;
        private double dirtySince, requestedAt = -1, captureAt;
        private StateStamp observed;
        private bool initialized, checkpointRequested, exitRequested, allowQuit, ownsSession;

        private struct StateStamp
        {
            public long Terrain, Inventory, Wallet;
            public int Shovel, Strokes;
            public float Charge, Pitch, VerticalSpeed, CrouchAmount;
            public Vector3 Position;
            public Quaternion Rotation;
            public bool Same(StateStamp b) => Terrain == b.Terrain && Inventory == b.Inventory && Wallet == b.Wallet
                && Shovel == b.Shovel && Strokes == b.Strokes && Charge == b.Charge && Pitch == b.Pitch && VerticalSpeed == b.VerticalSpeed
                && CrouchAmount == b.CrouchAmount && Position.Equals(b.Position) && Rotation.Equals(b.Rotation);
        }

        private void Awake()
        {
            // Additive scene fixtures do not own a running game/profile. Dedicated
            // integration tests opt in with BeginSession and an isolated temp path.
            if (gameObject.scene != SceneManager.GetActiveScene()) { enabled = false; return; }
            BeginSession(Path.Combine(Application.persistentDataPath, Application.isEditor ? "EditorSave" : "Save"));
        }

        public void BeginSession(string directory)
        {
            if (ownsSession) throw new InvalidOperationException("A save session is already running.");
            player = GetComponent<FpsPlayer>();
            terrain = player.ExcavationTerrain;
            discoveries = player.Discoveries;
            discoveries?.DeferGeneration();
            SaveDirectory = Path.GetFullPath(directory);
            ownsSession = true;
            enabled = true;
            player.Persistence = this;
            Application.wantsToQuit += WantsToQuit;
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            SetState(WorldSaveState.Loading);
            player.ShowPersistenceMenu();
            // Let the existing HUD paint before any IO or restored meshing starts.
            yield return null;
            store?.Dispose();
            store = new WorldSaveStore(SaveDirectory);
            read = Task.Run(() => store.Load());
            while (!read.IsCompleted) yield return null;
            if (read.IsFaulted) { Fail(read.Exception.GetBaseException(), true); yield break; }
            var result = read.Result;
            read = null; // Do not retain a second full density array after restoration.
            if (terrain == null || discoveries == null) { Fail(new InvalidDataException("The excavation scene is incomplete."), true); yield break; }
            var snapshot = result.Snapshot;
            if (snapshot == null)
            {
                Exception generation = null;
                try
                {
                    discoveries.InitializePopulation();
                    WorldSnapshot.Require(discoveries.Initialized, "The discovery population could not be initialized.");
                }
                catch (Exception error) { generation = error; }
                if (generation != null) { Fail(generation, true); yield break; }
            }
            if (snapshot != null)
            {
                Exception validation = null;
                try
                {
                    WorldSnapshot.Require(snapshot.Terrain.Size == terrain.Dimensions && snapshot.Terrain.CellSize == terrain.CellSize
                        && Vector3.Distance(snapshot.TerrainPosition, terrain.transform.position) < 0.001f
                        && Quaternion.Angle(snapshot.TerrainRotation, terrain.transform.rotation) < 0.001f, "This game version has a different excavation layout. The save has been kept.");
                    discoveries.ValidateRestore(snapshot.Finds);
                }
                catch (Exception error) { validation = error; }
                if (validation != null) { Fail(validation, true); yield break; }
                // Drive the rebuild here so any failure stays behind the recovery UI.
                var rebuild = terrain.Restore(snapshot.Terrain, snapshot.ExcavationSeed);
                bool next = true;
                while (next)
                {
                    try { next = rebuild.MoveNext(); }
                    catch (Exception error) { validation = error; next = false; }
                    if (next) yield return rebuild.Current;
                }
                if (validation == null)
                {
                    try { discoveries.Restore(snapshot.Finds, snapshot.DiscoverySeed); player.Restore(snapshot); }
                    catch (Exception error) { validation = error; }
                }
                if (validation != null) { Fail(validation, true); yield break; }
                CompletedSequence = snapshot.Sequence;
                nextSequence = snapshot.Sequence;
                LastSavedLabel = "Saved " + new DateTime(snapshot.UtcTicks, DateTimeKind.Utc).ToLocalTime().ToString("HH:mm:ss");
            }
            observed = Observe();
            dirtyVersion = capturedVersion = savedVersion = 0;
            dirtySince = Time.realtimeSinceStartupAsDouble;
            initialized = true;
            if (result.Recovered) { SetState(WorldSaveState.Recovery); yield break; }
            FinishLoading(snapshot == null);
        }

        private void FinishLoading(bool checkpoint)
        {
            SetState(WorldSaveState.Ready);
            player.CloseMenu();
            player.OpenMenu(PlayerMenu.Pause);
            if (checkpoint) RequestCheckpoint();
        }

        public void AcceptRecovery()
        {
            if (State != WorldSaveState.Recovery) return;
            FinishLoading(true);
        }

        private StateStamp Observe() => new StateStamp { Terrain = terrain.StateRevision, Inventory = player.Inventory.Revision,
            Wallet = player.Wallet.Revision, Shovel = player.Shovel.Level, Strokes = player.SuccessfulStrokes, Charge = player.Battery.Charge,
            Position = player.transform.position, Rotation = player.transform.rotation, Pitch = player.Pitch, VerticalSpeed = player.VerticalSpeed,
            CrouchAmount = player.CrouchAmount };

        private void LateUpdate()
        {
            if (!initialized) return;
            var current = Observe();
            if (!current.Same(observed))
            {
                if (dirtyVersion == capturedVersion) dirtySince = Time.realtimeSinceStartupAsDouble;
                dirtyVersion++;
                if (current.Wallet != observed.Wallet || current.Shovel != observed.Shovel) RequestCheckpoint();
                observed = current;
            }
            if (write != null && write.IsCompleted)
            {
                var finished = write;
                write = null;
                if (finished.IsFaulted) { Fail(finished.Exception.GetBaseException(), false); return; }
                CompletedSequence = writing.Sequence;
                savedVersion = capturedVersion;
                LastSavedLabel = "Saved " + new DateTime(writing.UtcTicks, DateTimeKind.Utc).ToLocalTime().ToString("HH:mm:ss");
                LastCheckpointLatencyMilliseconds = (Time.realtimeSinceStartupAsDouble - captureAt) * 1000;
                writing = null;
                SetState(WorldSaveState.Ready);
            }
            if (State != WorldSaveState.Ready && State != WorldSaveState.Saving) return;
            if (write == null && (checkpointRequested || dirtyVersion != savedVersion && Time.realtimeSinceStartupAsDouble - dirtySince >= AutosaveSeconds))
                StartWrite();
            if (exitRequested && write == null && !checkpointRequested && dirtyVersion == savedVersion) QuitNow();
        }

        public void RequestCheckpoint()
        {
            checkpointRequested = true;
            if (requestedAt < 0) requestedAt = Time.realtimeSinceStartupAsDouble;
        }

        public WorldSnapshot Capture(long sequence)
        {
            var timer = Stopwatch.StartNew();
            if (cachedTerrain == null || cachedTerrainRevision != terrain.StateRevision)
            {
                cachedTerrain = terrain.Capture();
                cachedTerrainRevision = terrain.StateRevision;
                CapturedTerrainCopies++;
            }
            var snapshot = new WorldSnapshot { Sequence = sequence, UtcTicks = DateTime.UtcNow.Ticks, Terrain = cachedTerrain,
                TerrainPosition = terrain.transform.position, TerrainRotation = terrain.transform.rotation,
                ExcavationSeed = terrain.ExcavationSeed, DiscoverySeed = discoveries.Seed, Finds = discoveries.Capture() };
            player.Capture(snapshot);
            LastCaptureMilliseconds = timer.Elapsed.TotalMilliseconds;
            return snapshot;
        }

        private void StartWrite()
        {
            try
            {
                writing = Capture(++nextSequence);
                capturedVersion = dirtyVersion;
                captureAt = requestedAt >= 0 ? requestedAt : Time.realtimeSinceStartupAsDouble;
                if (dirtyVersion != savedVersion) MaximumDirtySeconds = Math.Max(MaximumDirtySeconds, Time.realtimeSinceStartupAsDouble - dirtySince);
                requestedAt = -1;
                checkpointRequested = false;
                SetState(WorldSaveState.Saving);
                var snapshot = writing;
                write = Task.Run(() =>
                {
                    var timer = Stopwatch.StartNew();
                    store.Commit(snapshot);
                    LastWriteMilliseconds = timer.Elapsed.TotalMilliseconds;
                });
            }
            catch (Exception error) { Fail(error, false); }
        }

        private void Fail(Exception error, bool loading)
        {
            ErrorDetail = error is IOException || error is UnauthorizedAccessException ? error.Message : "The checkpoint could not be processed. Its files have been kept.";
            UnityEngine.Debug.LogWarning("World save: " + error);
            exitRequested = false;
            SetState(loading ? WorldSaveState.LoadFailed : WorldSaveState.WriteFailed);
            player.ShowPersistenceMenu();
        }

        public void Retry()
        {
            if (State == WorldSaveState.LoadFailed) { initialized = false; StartCoroutine(Load()); }
            else if (State == WorldSaveState.WriteFailed)
            {
                SetState(WorldSaveState.Ready);
                RequestCheckpoint();
                player.CloseMenu();
                player.OpenMenu(PlayerMenu.Pause);
            }
        }

        public void RequestExit()
        {
            if (State == WorldSaveState.Loading || State == WorldSaveState.LoadFailed || State == WorldSaveState.Recovery) { QuitNow(); return; }
            if (State == WorldSaveState.WriteFailed) { SetState(WorldSaveState.ConfirmQuit); return; }
            if (State == WorldSaveState.ConfirmQuit) return;
            exitRequested = true;
            RequestCheckpoint();
            player.ShowPersistenceMenu();
            Changed?.Invoke();
        }

        public void CancelUnsavedExit() { if (State == WorldSaveState.ConfirmQuit) SetState(WorldSaveState.WriteFailed); }
        public void ConfirmUnsavedExit() { if (State == WorldSaveState.ConfirmQuit) QuitNow(); }
        public void OpenSaveFolder() => Application.OpenURL(new Uri(SaveDirectory + Path.DirectorySeparatorChar).AbsoluteUri);
        private bool WantsToQuit()
        {
            if (allowQuit || State == WorldSaveState.Loading || State == WorldSaveState.LoadFailed || State == WorldSaveState.Recovery) return true;
            RequestExit();
            return false;
        }
        private void QuitNow()
        {
            allowQuit = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        private void SetState(WorldSaveState state) { State = state; Changed?.Invoke(); }

        private void OnDestroy()
        {
            if (!ownsSession) return;
            Application.wantsToQuit -= WantsToQuit;
            // Editor Stop cannot be vetoed. Finish its current immutable write before
            // releasing the profile; the native player's normal exit is awaited above.
            var owner = store;
            var pending = write ?? (Task)read;
            if (pending != null && !pending.IsCompleted) pending.ContinueWith(_ => owner?.Dispose());
            else owner?.Dispose();
        }
    }
}
