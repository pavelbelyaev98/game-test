# Task 121 - Complete input-preference recovery

Why: damaged keybindings could fall back to default keys while retaining Toggle digging, contrary to the existing complete-default recovery contract.

Integrated: validate the binding map before accepting its mode. Rejected maps restore Hold and every default key; loading or closing Controls preserves the damaged file until an explicit edit/reset. Valid remaps and legacy Sprint/Grab migration retain their selected behavior.

Evidence: the new regression failed before the fix. Afterwards, 140 EditMode checks passed, including eleven damaged-file variants and release-to-stop/edit/reset recovery. All 18 UI input cases passed across the suite and an isolated rerun of one transient Back-focus assertion.

MainGame CLI review: Controls showed Hold/LMB/default keys, Back focused Controls on Pause, and the correct Hold hint remained visible. The isolated damaged file stayed intact; all 22 user save/preference files retained their hashes. MainGame returned clean and stopped. [Results and capture](../../../unity/Logs/Task121/).

Windows: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), 2026-09-10 18:34 UTC, zero errors; the existing Pipeline runtime-config warning remains.

Next: `106` remains the first eligible queued task and requires UI design review; its work and the paused/deferred tasks were not advanced by this maintenance fix.
