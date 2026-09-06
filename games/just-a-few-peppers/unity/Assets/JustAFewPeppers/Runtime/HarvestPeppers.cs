using System;
using System.Collections.Generic;

namespace JustAFewPeppers
{
    public enum PepperOwner { Source, Carrier, Held, Loose, Transit, Processed }

    public sealed class PepperRecord
    {
        public string Id { get; internal set; }
        public string Source { get; internal set; }
        public int Units => 1;
        public PepperOwner Owner { get; internal set; }
        public CarrierPose Pose { get; internal set; }
        public CarrierPose SafePose { get; internal set; }
        public CarrierPose InitialPose { get; internal set; }
    }

    public sealed partial class HarvestState
    {
        public IReadOnlyList<PepperRecord> Peppers { get; private set; }
        readonly Dictionary<string, PepperRecord> pepperIds = new Dictionary<string, PepperRecord>();
        public bool PourOpen { get; private set; }
        bool SingleHeld
        {
            get { if (Peppers != null) foreach (var p in Peppers) if (p.Owner == PepperOwner.Held) return true; return false; }
        }
        public int UncontainedUnits
        {
            get
            {
                int n = 0;
                if (Peppers != null) foreach (var p in Peppers)
                    if (p.Owner == PepperOwner.Held || p.Owner == PepperOwner.Loose || p.Owner == PepperOwner.Transit) n++;
                return n;
            }
        }
        public PepperRecord Pepper(string id) => id != null && pepperIds.TryGetValue(id, out var p) ? p : null;

        // Authored poses are registered once before these units become selectable.
        public void RegisterPeppers(string[] sources, CarrierPose[] poses)
        {
            if (Peppers != null || sources.Length != InitialHarvest || poses.Length != sources.Length || Remaining != InitialHarvest)
                throw new ArgumentException("Register the initial harvest exactly once.");
            var counts = new Dictionary<string, int>();
            for (int i = 0; i < sources.Length; i++)
            {
                if (!initial.ContainsKey(sources[i]) || !poses[i].IsValid) throw new ArgumentException("Invalid pepper configuration.");
                counts.TryGetValue(sources[i], out int n); counts[sources[i]] = n + 1;
            }
            foreach (var source in initial)
                if (!counts.TryGetValue(source.Key, out int n) || n != source.Value) throw new ArgumentException("Pepper membership differs from supply.");
            var records = new PepperRecord[sources.Length];
            for (int i = 0; i < records.Length; i++)
            {
                records[i] = new PepperRecord { Id = "pepper-" + i.ToString("D3"), Source = sources[i],
                    InitialPose = poses[i], Pose = poses[i], SafePose = poses[i], Owner = PepperOwner.Source };
                pepperIds.Add(records[i].Id, records[i]);
            }
            Peppers = Array.AsReadOnly(records);
        }

        void ResetPeppers()
        {
            PourOpen = false;
            if (Peppers == null) return;
            foreach (var p in Peppers) { p.Owner = PepperOwner.Source; p.Pose = p.SafePose = p.InitialPose; }
        }

        void ChangeOwner(PepperRecord p, PepperOwner next)
        {
            if (p.Owner == PepperOwner.Source) { remaining[p.Source]--; Remaining--; }
            if (p.Owner == PepperOwner.Carrier) RawUnits--;
            p.Owner = next;
            if (next == PepperOwner.Source) { remaining[p.Source]++; Remaining++; }
            if (next == PepperOwner.Carrier) RawUnits++;
        }

        public bool PickPepper(string id)
        {
            var p = Pepper(id);
            if (p == null || IsHeld || FinishedHeld || PourOpen ||
                (p.Owner != PepperOwner.Source && p.Owner != PepperOwner.Loose && p.Owner != PepperOwner.Carrier)) return false;
            foreach (var other in Peppers) if (other.Owner == PepperOwner.Held) return false;
            ChangeOwner(p, PepperOwner.Held);
            return true;
        }

        public bool ReleasePepper(string id, CarrierPose pose)
        {
            var p = Pepper(id);
            if (p == null || p.Owner != PepperOwner.Held || !pose.IsValid) return false;
            p.Pose = pose; ChangeOwner(p, PepperOwner.Loose); return true;
        }

        public bool ObservePepper(string id, CarrierPose pose, bool safe)
        {
            var p = Pepper(id);
            if (p == null || p.Owner == PepperOwner.Processed || !pose.IsValid) return false;
            p.Pose = pose; if (safe) p.SafePose = pose; return true;
        }

        public bool PutPepperInCarrier(string id)
        {
            var p = Pepper(id);
            if (p == null || RawUnits >= Capacity || PourOpen ||
                (p.Owner != PepperOwner.Held && p.Owner != PepperOwner.Loose && p.Owner != PepperOwner.Source)) return false;
            ChangeOwner(p, PepperOwner.Carrier); return true;
        }

        // Geometry and preview visibility are revalidated by the caller; only the shown IDs may move.
        public int GatherPeppers(IReadOnlyList<string> preview)
        {
            if (!IsHeld || PourOpen || preview == null) return 0;
            int n = 0;
            foreach (string id in preview)
            {
                var p = Pepper(id);
                if (p != null && (p.Owner == PepperOwner.Source || p.Owner == PepperOwner.Loose) && PutPepperInCarrier(id)) n++;
            }
            return n;
        }

        int GatherRegistered(string source, int requested)
        {
            int n = 0;
            foreach (var p in Peppers)
            {
                if (n >= requested || RawUnits >= Capacity) break;
                if (p.Source == source && p.Owner == PepperOwner.Source && PutPepperInCarrier(p.Id)) n++;
            }
            return n;
        }

        void RetireCarrierPeppers(int count)
        {
            if (Peppers == null) return;
            foreach (var p in Peppers)
                if (count > 0 && p.Owner == PepperOwner.Carrier) { p.Owner = PepperOwner.Processed; count--; }
        }

        public int BeginPepperPour(bool intoIntake)
        {
            if (!IsHeld || PourOpen || Peppers == null) return 0;
            int amount = intoIntake ? Math.Min(RawUnits, InputCapacity - QueuedUnits) : RawUnits;
            if (amount == 0) return 0;
            PourOpen = true;
            int n = 0;
            foreach (var p in Peppers)
                if (p.Owner == PepperOwner.Carrier && n < amount) { ChangeOwner(p, intoIntake ? PepperOwner.Transit : PepperOwner.Loose); n++; }
            return n;
        }

        public bool AcceptPepperAtIntake(string id)
        {
            var p = Pepper(id);
            if (p == null || p.Owner != PepperOwner.Transit || QueuedUnits >= InputCapacity) return false;
            ChangeOwner(p, PepperOwner.Processed); QueuedUnits++; return true;
        }

        public void EndPepperPour()
        {
            if (Peppers != null) foreach (var p in Peppers)
                if (p.Owner == PepperOwner.Transit) ChangeOwner(p, PepperOwner.Loose);
            PourOpen = false; StartBatch();
        }

        public bool SpillPepper(string id)
        {
            var p = Pepper(id);
            if (p == null || p.Owner != PepperOwner.Carrier) return false;
            ChangeOwner(p, PepperOwner.Loose); return true;
        }

        // One recovery gesture regroups strays at their original source, preserving their IDs and exact units.
        public void RecoverPepper(string id)
        {
            var p = Pepper(id);
            if (p == null || p.Owner == PepperOwner.Processed || p.Owner == PepperOwner.Carrier) return;
            ChangeOwner(p, PepperOwner.Source); p.Pose = p.SafePose = p.InitialPose;
        }
    }
}
