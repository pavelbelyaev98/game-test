# Discovery reveal and collection

Status: Task `08` is ready.

Idea coverage: sections 19-22 and relevant tuning in section 53.

## Purpose

Turn excavation into readable discovery moments without tedious final cleaning or collection through covering soil.

## Task 08 - authored finds

- Scope after Task `07`: add a finite registry and two authored ordinary `BuriedFind` objects to [the main game scene](core-loop.md), using terrain occupancy and inventory records; no randomized generation or manual exposed flag.
- Acceptance: terrain changes drive hidden/partial/collectible states at a tunable 80% initial exposure threshold; occlusion prevents naming/collection through soil. One E press adds one ID exactly once; a full inventory leaves the world item intact, and collected records remain absent across surface trips.
- Evidence: controlled excavation/exposure, occlusion, capacity and repeated-interaction integration checks; expose only position/eligibility to the future detector.
- Next: Task `09`. Distinctive-object presentation, generated placement and disk persistence remain later work.

## Later expansion

Replace validation-only exposure flags with production eligibility. Model hidden, partially exposed, collectible, and collected states; connect eligible ordinary finds to the existing interaction/inventory contracts.

## Required behavior

- Covered finds cannot be named or collected through terrain.
- Cheap finds collect quickly with clear identity feedback; the player should notice what an upgraded tool uncovered.
- Ordinary finds become collectible after a forgiving exposure threshold.
- Distinctive finds remain visible while their shape becomes recognizable, then require deliberate collection.
- Start playtesting with a forgiving threshold around 70-85%; never require the final hidden speck, precision brushing, washing, or a cleaning minigame.
- Normal objects identify immediately without experts, analysis timers, mailing, or per-item bureaucracy.
- Full inventory leaves the find intact and available.

## Done when

- Exposure transitions are stable as nearby terrain changes.
- One E press collects one eligible find exactly once.
- Occlusion, capacity, and persistence integration checks pass.
