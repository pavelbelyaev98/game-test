using System;
using System.IO;

namespace SomethingDownThere
{
    public enum SaveWriteStage { BeforeWrite, TemporaryFlushed, BeforeReplace, Replaced }

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

        public SaveLoadResult Load()
        {
            if (loaded) throw new InvalidOperationException("This profile is already loaded.");
            Directory.CreateDirectory(DirectoryPath);
            sessionLock ??= new FileStream(Path.Combine(DirectoryPath, "session.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
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
