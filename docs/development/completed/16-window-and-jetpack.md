# Task 16 - adaptive window and jump/hold jetpack

- Why: close the window/control acceptance left open by earlier tasks; flight could strand less than a frame's fuel and restart during a shorter frame.
- Integrated result: the existing `MainGame` player burns the final battery fraction for proportional thrust and then remains exhausted. Free jump, 0.22-second initial hold, immediate airborne restart, landing reset and menu/focus safety are preserved.
- Evidence: 42/42 EditMode and 46/46 PlayMode checks pass. The depletion regression failed before the fix at 30/60/144 FPS and passes afterward; inventory-close integration requires Space release before thrust resumes.
- Official CLI inspection: 4 m/s walking, 1.09 m free jump, 8 m/s ascent cap, immediate arrest of a 7 m/s fall, zero charge after depletion and safe landing. Existing HUD and pause presentation inspected.
- Windows: zero build errors; expected disabled Pipeline player-services warning. Native keyboard tap/hold/restart, pause suppression, depletion and empty-battery jump inspected. Bordered/resizable 1920x1080 startup and persistent 960x540/1280x800 resizing verified; player log has no script errors/exceptions.
- Evidence/captures: `unity/Logs/Task16/`. Delivery: `builds/windows/SomethingDownThere.exe`. No art/audio, dependencies, scene assets or launchers added; asset ledger ownership is unchanged.
- Limitation: subjective control tuning remains available through user review; production HUD/art acceptance remains with `05`/`08`/`09`. No remaining ready task and no commit.
