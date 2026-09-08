# Task 31 - gentler shovels and pickup tuning

- Why: the user requested 50% exposure, clarified slightly smaller/weaker scoops, and removed the low-reserve subtitle.
- Integrated: all three existing find prefabs require 50%; held collection, visibility and capacity rules remain. Six MainGame shovel radii now span 0.41–0.96 m in 0.11 m steps, preserving reach/cadence/energy. "RESERVE RUNNING LOW" stands alone.
- Evidence: 46/46 EditMode and 51/51 PlayMode checks passed. Matched fresh strokes remove 18.9–21.8% less soil than the previous tuning while every upgrade stays stronger.
- Official CLI review: 14% exposure blocked pickup; 54% allowed one free held pickup with no extra dig. Inspected the single-line warning at 35% charge. Windows smoke review at 1280x800 confirmed 0.82 m starter scoop and 50% prompt. Evidence: `unity/Logs/Task31/`.
- Delivery: `builds/windows/SomethingDownThere.exe`, build succeeded 2026-09-08 13:35 UTC with zero errors and the expected disabled Pipeline player-services notice; no player script exceptions. No commit.
- Assets: existing prefab thresholds only; no new art/audio or dependencies. Ledger records reversal without undoing the larger finds.
- Limitation: measured reduction describes fresh soil; overlapping cuts and detached-soil cleanup naturally vary. Final find art remains Task `09`.
