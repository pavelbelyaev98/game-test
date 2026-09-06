using Chushkopek.Stage0;

int failures = 0;
Run("Raw cannot finish; invalid placement preserves ownership", () => {
    var p = HeldRaw();
    Check(!p.TryPlace(PepperLocation.Finished, out var reason) && reason.Length > 0);
    Check(p.Stage == PepperStage.Raw && p.Location == PepperLocation.Held && p.CompletionCount == 0);
});
Run("Removing an under-roasted pepper immediately stops heating and permits resumption", () => {
    var p = Roasting(); p.Tick(4); Check(p.TryPick(out _)); p.Tick(100);
    Check(p.RoastElapsed == 4 && p.Stage == PepperStage.Roasting);
    Check(!p.TryPlace(PepperLocation.Steam, out _));
    Check(p.TryPlace(PepperLocation.Roaster, out _)); p.Tick(6);
    Check(p.Stage == PepperStage.Perfect);
});
Run("Perfect only burns while in the roaster, at the grace boundary", () => {
    var p = Roasting(); p.Tick(10); Check(p.Stage == PepperStage.Perfect);
    Check(p.TryPick(out _)); p.Tick(100); Check(p.Stage == PepperStage.Perfect && !p.WasBurnt);
    Check(p.TryPlace(PepperLocation.Roaster, out _)); p.Tick(6.5f); Check(p.Stage == PepperStage.Perfect);
    p.Tick(.5f); Check(p.Stage == PepperStage.Burnt && p.WasBurnt);
});
Run("Burnt remains processable and retains its quality through completion", () => {
    var p = Roasting(); p.Tick(25); Check(p.TryPick(out _)); Check(p.TryPlace(PepperLocation.Steam, out _));
    p.Tick(3); Check(p.AdvancePeel(0, 1)); Check(p.AdvancePeel(1, 1));
    Check(p.TryPick(out _)); Check(p.TryPlace(PepperLocation.Finished, out _));
    Check(p.Stage == PepperStage.Finished && p.WasBurnt);
});
Run("Steaming becomes peelable after three seconds and cannot be lifted early", () => {
    var p = Steaming(); p.Tick(2.9f); Check(p.Stage == PepperStage.Steaming && !p.TryPick(out _));
    p.Tick(.11f); Check(p.Stage == PepperStage.Peelable && p.SteamFraction == 1);
});
Run("Peeling needs movement on both strips; partial progress survives waiting", () => {
    var p = Steaming(); p.Tick(3); Check(p.AdvancePeel(0, .3f)); p.Tick(100);
    Check(Math.Abs(p.StripProgress(0) - .3f) < .0001f && p.Stage == PepperStage.Peelable);
    Check(!p.AdvancePeel(0, 0) && !p.AdvancePeel(0, -1) && !p.TryPick(out _));
    Check(p.AdvancePeel(0, 1)); Check(p.Stage == PepperStage.Peelable);
    Check(p.AdvancePeel(1, 1)); Check(p.Stage == PepperStage.Peeled);
});
Run("Finished is reached only on tray placement and cannot trigger twice", () => {
    var p = Peeled(); Check(p.CompletionCount == 0); Check(p.TryPick(out _));
    Check(p.TryPlace(PepperLocation.Finished, out _)); Check(!p.TryPlace(PepperLocation.Finished, out _));
    Check(!p.TryPick(out _)); p.Tick(100); Check(p.CompletionCount == 1);
});
Run("Cancel restores each reserved origin, including a peeled pepper", () => {
    var p = HeldRaw(); Check(p.TryCancelCarry() && p.Location == PepperLocation.Start);
    Check(p.TryPick(out _)); Check(p.TryPlace(PepperLocation.Roaster, out _)); p.Tick(2);
    Check(p.TryPick(out _)); Check(p.TryCancelCarry()); p.Tick(1); Check(p.RoastElapsed == 3);
    p = Peeled(); Check(p.TryPick(out _)); Check(p.TryCancelCarry());
    Check(p.Location == PepperLocation.Steam && p.Stage == PepperStage.Peeled && p.TryPick(out _));
});
Run("Reset clears completion, timers, quality, ownership and both strips", () => {
    var p = Peeled(); Check(p.TryPick(out _)); Check(p.TryPlace(PepperLocation.Finished, out _)); p.Reset();
    Check(p.Stage == PepperStage.Raw && p.Location == PepperLocation.Start && p.CompletionCount == 0);
    Check(p.RoastElapsed == 0 && p.SteamElapsed == 0 && !p.WasBurnt && p.StripProgress(0) == 0 && p.StripProgress(1) == 0);
    Check(p.TryPick(out _));
});
Run("Non-finite, negative and out-of-range inputs cannot corrupt state", () => {
    var p = Roasting(); p.Tick(float.NaN); p.Tick(float.PositiveInfinity); p.Tick(-1); Check(p.RoastElapsed == 0);
    p = Steaming(); p.Tick(3); Check(!p.AdvancePeel(0, float.NaN) && !p.AdvancePeel(0, float.PositiveInfinity));
    Check(!p.AdvancePeel(-1, 1) && !p.AdvancePeel(2, 1)); Check(p.Stage == PepperStage.Peelable);
});
Console.WriteLine($"{10 - failures}/10 state tests passed.");
return failures == 0 ? 0 : 1;

void Run(string name, Action test) { try { test(); Console.WriteLine("PASS " + name); } catch (Exception e) { failures++; Console.WriteLine("FAIL " + name + ": " + e.Message); } }
static void Check(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
static PepperState HeldRaw() { var p = new PepperState(); Check(p.TryPick(out _)); return p; }
static PepperState Roasting() { var p = HeldRaw(); Check(p.TryPlace(PepperLocation.Roaster, out _)); return p; }
static PepperState Steaming() { var p = Roasting(); p.Tick(10); Check(p.TryPick(out _)); Check(p.TryPlace(PepperLocation.Steam, out _)); return p; }
static PepperState Peeled() { var p = Steaming(); p.Tick(3); Check(p.AdvancePeel(0, 1)); Check(p.AdvancePeel(1, 1)); return p; }
