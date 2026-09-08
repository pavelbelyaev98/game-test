# Current status

Task `16` is complete: adaptive Windows sizing and jump/hold flight are verified in the main scene and executable. [Completion](completed/16-window-and-jetpack.md).

- Fixed final-fuel depletion: the jetpack consumes the remaining fraction for proportional thrust, reaches zero, and cannot restart when a shorter frame arrives. Existing movement, initial hold delay, immediate airborne restart and menu/focus safety remain.
- Evidence: 42/42 EditMode and 46/46 PlayMode checks pass. The new regression first reproduced stranded fuel at 30/60/144 FPS; it now verifies exhaustion across changing frame durations. Input System integration also verifies held Space across inventory close.
- Official CLI inspected `MainGame`: 4 m/s walking, 1.09 m free jump, 8 m/s ascent cap, immediate arrest of a 7 m/s fall, depletion and landing. HUD, jump, flight and pause captures are under `unity/Logs/Task16/`.
- Windows build succeeded with zero errors and the expected disabled Pipeline player-services notice. Native keyboard review verifies tap/hold/restart, held Space across pause, exhaustion, and empty-battery jump. Bordered/resizable startup is 1920x1080; HUD/menu fit 960x540 and 1280x800, and resizing persists during play. No script errors/exceptions in the player log.
- Build: `builds/windows/SomethingDownThere.exe`. No new art/audio, dependencies, scene assets or launchers; asset ownership is unchanged. No commit.

No task is currently active or ready. Remaining items keep their planned scope: user-owned art is deferred (`08`/`09`), detector feedback is `10`, independent speed/strength upgrades remain future TODO `25`, and `22` is the production release gate. Subjective control tuning and final presentation remain open to user review; no tool or implementation blocker.
