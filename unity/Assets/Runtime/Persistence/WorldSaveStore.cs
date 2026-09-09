using System;
using System.IO;

namespace SomethingDownThere
{
    public enum SaveWriteStage { BeforeWrite, TemporaryFlushed, BeforeReplace, Replaced }

    public sealed class SaveProfileInUseException : IOException
    {
        public SaveProfileInUseException(IOException cause) : base("This saved game is already open in another game window.", cause) { }
    }

    public sealed class SaveLoadResult
    {
        public WorldSnapshot Snapshot;
        public bool Recovered;
    }

    // One writer per profile, one atomic file per world. Never delete a checkpoint
    // to make room for its replacement; a failed replacement leaves a valid copy.
    public sealed class WorldSaveStore : IDisposable
    {
        public string DirectoryPath { get; }
        public string PrimaryPath => Path.Combine(DirectoryPath, "world.sav");
        public string BackupPath => Path.Combine(DirectoryPath, "world.previous.sav");
        public string PendingPath => Path.Combine(DirectoryPath, "world.pending");
        private readonly Action<SaveWriteStage> stage;
        private FileStream sessionLock;
        private bool loaded, preservePrimary;

        public WorldSaveStore(string directory, Action<SaveWriteStage> writeStage = null)
        { DirectoryPath = Path.GetFullPath(directory); stage = writeStage; }

        public static bool HasCheckpoint(string directory) => File.Exists(Path.Combine(directory, "world.sav"))
            || File.Exists(Path.Combine(directory, "world.previous.sav")) || File.Exists(Path.Combine(directory, "world.pending"));

        public void ReplaceWithNewGame(WorldSnapshot snapshot, bool replaceExisting)
        {
            if (loaded) throw new InvalidOperationException("This profile is already loaded.");
            Directory.CreateDirectory(DirectoryPath);
            AcquireSessionLock();
            if (HasCheckpoint(DirectoryPath) && !replaceExisting)
                throw new IOException("A saved game appeared while starting. Return to the menu to confirm its replacement.");
            stage?.Invoke(SaveWriteStage.BeforeWrite);
            string candidate = Path.Combine(DirectoryPath, "world.new.pending");
            using (var stream = new FileStream(candidate, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                WorldSaveCodec.Write(stream, snapshot);
                stream.Flush(true);
            }
            stage?.Invoke(SaveWriteStage.TemporaryFlushed);
            if (HasCheckpoint(DirectoryPath))
            {
                string archive = Path.Combine(DirectoryPath, "PreviousGames", DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(archive);
                foreach (string path in new[] { PrimaryPath, BackupPath, PendingPath })
                    if (File.Exists(path)) File.Copy(path, Path.Combine(archive, Path.GetFileName(path)));
            }
            // Publish a complete fresh recovery checkpoint before switching the
            // primary, so recovery after a new game cannot revive the old world.
            string backupCandidate = Path.Combine(DirectoryPath, "world.new.previous.pending");
            File.Copy(candidate, backupCandidate, true);
            using (var stream = new FileStream(backupCandidate, FileMode.Open, FileAccess.Write, FileShare.None)) stream.Flush(true);
            stage?.Invoke(SaveWriteStage.BeforeReplace);
            Publish(backupCandidate, BackupPath);
            Publish(candidate, PrimaryPath);
            if (File.Exists(PendingPath)) File.Delete(PendingPath);
            loaded = true;
            preservePrimary = false;
            stage?.Invoke(SaveWriteStage.Replaced);
        }

        private static void Publish(string candidate, string destination)
        {
            if (File.Exists(destination)) File.Replace(candidate, destination, null);
            else File.Move(candidate, destination);
        }

        public SaveLoadResult Load()
        {
            if (loaded) throw new InvalidOperationException("This profile is already loaded.");
            Directory.CreateDirectory(DirectoryPath);
            AcquireSessionLock();
            bool primary = File.Exists(PrimaryPath), backup = File.Exists(BackupPath), pending = File.Exists(PendingPath);
            if (primary)
            {
                try { var snapshot = Read(PrimaryPath); loaded = true; return new SaveLoadResult { Snapshot = snapshot }; }
                catch (InvalidDataException) { preservePrimary = true; }
                catch (EndOfStreamException) { preservePrimary = true; }
                // An unsupported version or an access/IO failure is never treated as
                // an empty world, nor silently downgraded to an older backup.
            }
            if (backup)
            {
                var snapshot = Read(BackupPath);
                loaded = true;
                return new SaveLoadResult { Snapshot = snapshot, Recovered = true };
            }
            if (!primary && pending)
            {
                var snapshot = Read(PendingPath);
                loaded = true;
                return new SaveLoadResult { Snapshot = snapshot, Recovered = true };
            }
            if (primary) throw new InvalidDataException("The checkpoint is damaged and no recovery copy is available. Its files have been kept.");
            loaded = true;
            return new SaveLoadResult();
        }

        private void AcquireSessionLock()
        {
            try
            {
                sessionLock ??= new FileStream(Path.Combine(DirectoryPath, "session.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            // Windows sharing/lock violations are recoverable contention. Other
            // IO failures still need storage guidance, never a claim of another game.
            catch (IOException error) when ((error.HResult & 0xffff) == 32 || (error.HResult & 0xffff) == 33)
            { throw new SaveProfileInUseException(error); }
        }

        public void Commit(WorldSnapshot snapshot)
        {
            if (!loaded || sessionLock == null) throw new InvalidOperationException("Load and validate the profile before writing.");
            stage?.Invoke(SaveWriteStage.BeforeWrite);
            using (var stream = new FileStream(PendingPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                WorldSaveCodec.Write(stream, snapshot);
                stream.Flush(true);
            }
            stage?.Invoke(SaveWriteStage.TemporaryFlushed);
            if (preservePrimary && File.Exists(PrimaryPath))
            {
                File.Move(PrimaryPath, Path.Combine(DirectoryPath, "world.damaged-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N") + ".sav"));
                preservePrimary = false;
            }
            stage?.Invoke(SaveWriteStage.BeforeReplace);
            if (File.Exists(PrimaryPath)) File.Replace(PendingPath, PrimaryPath, BackupPath);
            else File.Move(PendingPath, PrimaryPath);
            stage?.Invoke(SaveWriteStage.Replaced);
        }

        public static WorldSnapshot Read(string path)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return WorldSaveCodec.Read(stream);
        }

        public void Dispose() { sessionLock?.Dispose(); sessionLock = null; }
    }
}
