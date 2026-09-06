using System;

namespace Chushkopek.Stage0
{
    public enum PepperStage { Raw, Roasting, Perfect, Burnt, Steaming, Peelable, Peeled, Finished }
    public enum PepperLocation { Start, Held, Roaster, Steam, Finished }

    // The single authority for this pepper. No Unity, physics, animation, or input dependencies.
    public sealed class PepperState
    {
        public const int StripCount = 2;
        readonly float[] strips = new float[StripCount];
        readonly float idealSeconds;
        readonly float graceSeconds;
        readonly float steamSeconds;
        PepperLocation carryOrigin;

        public PepperStage Stage { get; private set; }
        public PepperLocation Location { get; private set; }
        public float RoastElapsed { get; private set; }
        public float SteamElapsed { get; private set; }
        public bool WasBurnt { get; private set; }
        public int CompletionCount { get; private set; }
        public bool IsHeating => Location == PepperLocation.Roaster;
        public float RoastFraction => Math.Min(1f, RoastElapsed / idealSeconds);
        public float SteamFraction => Math.Min(1f, SteamElapsed / steamSeconds);
        public float PerfectSecondsRemaining => Math.Max(0f, idealSeconds + graceSeconds - RoastElapsed);

        public PepperState(float idealSeconds = 10f, float graceSeconds = 7f, float steamSeconds = 3f)
        {
            if (!PositiveFinite(idealSeconds) || !PositiveFinite(graceSeconds) || !PositiveFinite(steamSeconds))
                throw new ArgumentOutOfRangeException(nameof(idealSeconds), "Durations must be finite and positive.");
            this.idealSeconds = idealSeconds;
            this.graceSeconds = graceSeconds;
            this.steamSeconds = steamSeconds;
            Reset();
        }

        public float StripProgress(int strip) => strips[strip];

        public void Reset()
        {
            Stage = PepperStage.Raw;
            Location = PepperLocation.Start;
            carryOrigin = PepperLocation.Start;
            RoastElapsed = SteamElapsed = 0f;
            WasBurnt = false;
            CompletionCount = 0;
            Array.Clear(strips, 0, strips.Length);
        }

        public bool TryPick(out string reason)
        {
            reason = "";
            if (Location == PepperLocation.Held) return Reject("You are already holding the pepper.", out reason);
            if (Stage == PepperStage.Finished) return Reject("Pepper finished. Press R to try again.", out reason);
            if (Stage == PepperStage.Steaming) return Reject("Let the short steam finish before lifting it.", out reason);
            if (Stage == PepperStage.Peelable) return Reject("Peel both broad skin strips here first.", out reason);
            carryOrigin = Location;
            Location = PepperLocation.Held; // Removing from the roaster stops heating immediately.
            return true;
        }

        public bool TryPlace(PepperLocation destination, out string reason)
        {
            reason = "";
            if (Location != PepperLocation.Held) return Reject("Pick up the pepper first.", out reason);
            switch (destination)
            {
                case PepperLocation.Start:
                    Location = destination;
                    return true;
                case PepperLocation.Roaster:
                    if (Stage != PepperStage.Raw && Stage != PepperStage.Roasting &&
                        Stage != PepperStage.Perfect && Stage != PepperStage.Burnt)
                        return Reject("This pepper is already steamed. Finish peeling and use the tray.", out reason);
                    Location = destination;
                    if (Stage == PepperStage.Raw) Stage = PepperStage.Roasting;
                    return true;
                case PepperLocation.Steam:
                    if (Stage != PepperStage.Perfect && Stage != PepperStage.Burnt)
                        return Reject("Roast until the ready cue before steaming.", out reason);
                    Location = destination;
                    SteamElapsed = 0f;
                    Stage = PepperStage.Steaming;
                    return true;
                case PepperLocation.Finished:
                    if (Stage != PepperStage.Peeled)
                        return Reject("The tray needs a roasted, steamed, fully peeled pepper.", out reason);
                    Location = destination;
                    Stage = PepperStage.Finished;
                    CompletionCount++;
                    return true;
                default:
                    return Reject("That is not a placement position.", out reason);
            }
        }

        public bool TryCancelCarry()
        {
            if (Location != PepperLocation.Held) return false;
            Location = carryOrigin;
            return true;
        }

        public void Tick(float seconds)
        {
            if (!PositiveFinite(seconds)) return;
            if (IsHeating)
            {
                RoastElapsed = Math.Min(idealSeconds + graceSeconds + 3f, RoastElapsed + seconds);
                if (RoastElapsed >= idealSeconds + graceSeconds)
                {
                    Stage = PepperStage.Burnt;
                    WasBurnt = true;
                }
                else if (RoastElapsed >= idealSeconds) Stage = PepperStage.Perfect;
            }
            else if (Location == PepperLocation.Steam && Stage == PepperStage.Steaming)
            {
                SteamElapsed = Math.Min(steamSeconds, SteamElapsed + seconds);
                if (SteamElapsed >= steamSeconds) Stage = PepperStage.Peelable;
            }
        }

        // Amount is movement supplied by the interaction, never elapsed time.
        public bool AdvancePeel(int strip, float amount)
        {
            if (Stage != PepperStage.Peelable || Location != PepperLocation.Steam ||
                strip < 0 || strip >= StripCount || !PositiveFinite(amount) || strips[strip] >= 1f)
                return false;
            strips[strip] = Math.Min(1f, strips[strip] + amount);
            if (strips[0] >= 1f && strips[1] >= 1f) Stage = PepperStage.Peeled;
            return true;
        }

        static bool PositiveFinite(float value) => value > 0f && !float.IsNaN(value) && !float.IsInfinity(value);
        static bool Reject(string message, out string reason) { reason = message; return false; }
    }
}
