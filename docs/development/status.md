# Current status

Task `34` is done: small attached terrain remnants now disappear with their collision during digging.

- Cleanup removes small thin protrusions from the existing density field in the accepted stroke, then refreshes matching meshes/collision and discovery bounds. Large thin sheets, permanent boundary attachments and necks joining separate supports are retained. Task `26` disconnected-soil removal stays in place.
- All 128 checks pass: 67 EditMode + 61 PlayMode. Official CLI review cleared an obstructed player-sized capsule sweep in one ordinary shovel hit: 16 remnant samples, 2 energy, one revision and 8/864 rebuilt chunks. Actual player traversal, downward/lateral cuts and before/after presentation were checked. Evidence: `unity/Logs/Task34/`.
- Cut timing including collision, 24 samples each: level 1 mean/max 5.73/7.71 ms; level 6 17.40/28.30 ms. Large synchronous updates can still hitch; a 4 m radius stress cut outside normal shovel sizes peaked at 286 ms.
- Windows development build rebuilt at `2026-09-08 16:28 UTC`: `builds/windows/SomethingDownThere.exe`, zero errors and the expected Pipeline-disabled-in-player warning. Native 1920x1080 held digging, movement, readable HUD and pause review passed with no game exceptions.
- No new art/audio, materials, prefabs, packages or scene edits. Existing approved/user-owned terrain presentation is reused.
- No active task. Next ready: `35` saving, then `36` permanent discoveries and `37` full-run pacing subject to their existing dependencies. See the [queue](tasks.md).

No blocker. State is still scene-session only; unrelated art and progression work retain their existing scope. No commit.
