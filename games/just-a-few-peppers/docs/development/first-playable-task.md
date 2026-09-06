# M1 contract: first physical batch loop

M1 spans **1_01–1_08**. 1_05 is an interim crate-to-food checkpoint; 1_06 adds loose objects, 1_07 adds deliberate single/bulk physical peppers and measures representation; 1_08 adds direct machine operation and one provisional output-grouping gesture. M2 then adds Coins and two purchases before the human gate. The [queue](tasks/readme.md) owns selection/readiness; [status](status.md) owns the aggregate summary. Read [core mechanics](../core-loop-and-mechanics.md), [scope](../scope-and-validation.md), [state](state-and-saving.md), [architecture](../../ARCHITECTURE.md) and relevant [Unity policy](unity-and-assets.md).

## Deliverable

One accessible outdoor work corner with finite nearby harvest, freely handled crate, broad feeder, directly responsive handle/rack, safe compressed processing, reusable finished carrier and generous Finished Food Handoff Rack. Players can pick exactly one intended pepper, gather a previewed set into the container, pour recoverable material, help group prepared output into a neat batch, stage loads and play with a few loose props. Repeated batches and the partial last batch can all become stored winter food. No blocked-passage objective, hidden wheel or required discovery.

Reuse the delivered PepperYard scene and safe processing backend. Current automatic operation is an interim implementation, extended in 1_08. The agent owns geometry/components, controls, UI, assets and ordinary Windows build configuration; no routine Inspector chores fall to the playtester.

## Numbered implementation tasks

1. [1_01 — Foundation](tasks/1_01_unity-foundation-and-walkable-scene.md): movement, input, pause/focus and scene.
2. [1_02 — Crate handling revision](tasks/1_02_scooping-and-crate-carrying.md): finite scooping plus free placement/rotation/drop and recovery.
3. [1_03 — Processing backend](tasks/1_03_tipping-and-automatic-processing.md): accepted transfers, safe batches and reserved output; already delivered automatic groundwork.
4. [1_04 — Finished carrier/handoff](tasks/1_04_finished-carrier-and-storage-rack.md): usable food output, broad deposit, free set-down/regrab and empty return.
5. [1_05 — Interim complete-loop checkpoint](tasks/1_05_first-playable-comfort-and-handoff.md): comfort/reset, complete food transfer and ordinary build.
6. [1_06 — Loose-object play](tasks/1_06_loose-yard-objects-and-playful-handling.md): shared grab/place/drop/toss, stable stacks and recovery.
7. [1_07 — Physical pepper comparison](tasks/1_07_physical-pepper-batch-comparison.md): single/bulk pickup, container filling/pouring, recoverable scattered food and measured bounded simulation.
8. [1_08 — Direct machine operation](tasks/1_08_direct-machine-operation.md): input-driven handle/rack, provisional grouping within finished-output collection, safe transaction boundaries and complete physical food cycle.

Each task leaves an inspectable artifact and stops after its own handoff. Early deliveries do not need later actions to claim their narrow technical scope; M1 is not complete until 1_08's aggregate operation/handling result is ready.

## Acceptance criteria

- Pick exactly one intended pepper or deliberately gather several into the container. Bulk affected-set previews match reachable targets and capacity; contents targeting never grabs the container instead. Fill/pour with convincing physical contact and exact partial quantities. Individual handling is optional, bulk convenient.
- Carriers can be placed/rotated/dropped at chosen positions, regrabbed and recovered without food loss or compulsory mats.
- The representative scattered peppers remain selectable, movable, placeable into containers and pourable under the chosen measured representation. Spills recover as the same food. A shrinking pile with only cosmetic particles/counts cannot satisfy 1_07.
- A substantial mechanism responds directly to input; completing the operation starts the accepted batch once with output reserved. Interruption/pause/recovery never loses food or repeats input.
- Internal processing safely completes partial batches and waits at full output; finished food never burns/spoils while the player stages another load.
- The sole finished carrier can be freely parked/regrabbed and deposited once at a broad handoff, with no empty-container errand or label/shelf sorting.
- 1_04 provides the automatic jar/food receiving baseline; 1_08 requires one player-controlled grouping action within that output path, with the exact guide gesture provisional. Loose prepared material becomes a recognizable neat batch, without per-jar filling/capping. Careful placement settles predictably, and each accepted handoff grows/fills one nearby graybox food group visible from the work area. This derives from stored units and does not pull the four M5 household compositions or polished materials forward.
- Brief contextual guidance teaches single pickup, bulk pickup, careful placement, pouring and grouping as they arrive. No hidden modes/modifiers or selections through obstacles; quiet full/invalid cues and hidden placement indicators remain. Loose props use coherent controls/collision without an objective.
- All finite food can become stored; pause/focus freezes physical motion, reset restores the authored test, and recovery preserves work/valid arrangements.
- Scene/input and ordinary-player checks cover actual interactions, not only compilation. The developer receives a ready-to-play artifact, exact controls and honest untested feel status.

M1 excludes Coins implementation until 2_01, full-yard production, final powered conversion, household display art/dialogue, second activities and disk persistence. Those boundaries do not postpone Coins past 2_03.

## Handoff

Follow the queue's [handoff rules](tasks/readme.md#handoff-and-recording): exact scene/build, controls, a short task-specific play checklist, measured checks, limitations and next ID. At the full M1 handoff, pick one pepper and a deliberate bulk set, fill/pour a container, operate and group a finished batch, then store it. Try interrupted/partial transfers and recovery; record individual handling, bulk handling, packaging and control clarity separately from mechanism enjoyment. Keep technical delivery separate from human acceptance. No routine Stage0 rebuild/test is required.
