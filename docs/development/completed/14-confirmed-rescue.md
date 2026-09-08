# Task 14 - confirmed rescue

- Why: provide a safe recovery from depletion/stranding as the next gameplay increment without requiring deferred art/audio.
- Integrated: Pause > Call rescue previews exact lost finds/value, a fee capped at 10 credits/current balance, and remaining credits. Cancel is selected first; Escape returns to Pause. Confirmation applies once, checks landing clearance and stale costs, returns to the existing surface anchor and refills the battery.
- Preservation: excavation, owned shovel level and collected discovery records survive. Lost finds never respawn; held input cannot trigger digging/thrust after rescue.
- Evidence: 55/55 EditMode and 55/55 PlayMode checks passed, including actual collected loot, bounded fees, cancel/repeat/stale commands, focus, blocked landing, depletion and restored control. CLI reviewed a 1-find/10-credit rescue retaining terrain and shovel level. Native Windows review covered depletion, 960x540 confirmation, Escape/click actions and movement afterward; 1280x800 HUD/Pause also inspected. Evidence: `unity/Logs/Task14/`.
- Delivery: Windows build succeeded 2026-09-08 14:08 UTC, zero errors and the expected disabled Pipeline player-services notice; no player script exceptions. `builds/windows/SomethingDownThere.exe`. No commit.
- Limitation: wallet starts at zero until Task `12` adds earning through selling. Session-only state; protected-item rules, fall consequences and disk persistence remain later work.
