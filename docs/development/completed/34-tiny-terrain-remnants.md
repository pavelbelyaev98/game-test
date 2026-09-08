# Task 34 - tiny terrain remnants

- Why: Task `26` removes disconnected soil but retains small attached spikes/slivers that can snag movement.
- Integrated result: local density cleanup removes thin, small protrusions in the same accepted dig. It preserves broad sheets, bridges between separate supports, permanent boundary attachments and untouched ground; retained terrain does not erode on idle/repeated stale hits.
- Mesh/collision updates, discovery bounds, volume accounting and the single energy charge/revision include cleanup. Existing terrain materials/content are unchanged; no new assets, audio, packages or scene edits.
- Evidence: 67/67 EditMode + 61/61 PlayMode checks pass. New cases cover floor/wall/diagonal tips, substantial supported structures, no refill, accounting/reset, stale hits, cross-chunk clearance and actual walking across the former obstruction.
- Official CLI review: one ordinary shovel hit cleared a blocked capsule sweep and removed 16 remnant samples (0.02345 m3), costing 2 energy and rebuilding 8/864 chunks. Before/after views and 21 larger downward/lateral cuts were inspected. Evidence: `unity/Logs/Task34/`.
- Measured full edit/collision cost across 24 cuts each: level 1 mean/max 5.73/7.71 ms; level 6 17.40/28.30 ms. A six-cut, 4 m radius stress case outside normal shovel sizes reached 286 ms; large synchronous mesh/collider edits remain the performance limit.
- Windows development build succeeded at `2026-09-08 16:28 UTC`, zero errors and the expected Pipeline-disabled-in-player warning. Native 1920x1080 held digging, movement, HUD and pause review passed with no game exceptions.
- Limitation/next: thresholds intentionally preserve larger thin structures; broadening them needs traversal evidence. Task `35` is ready for disk saving; excavation is still scene-session only. No commit.
