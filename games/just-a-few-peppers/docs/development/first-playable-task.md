# M1 contract: first physical batch loop

M1 spans **1_01–1_08**. 1_05 is an interim crate-to-food checkpoint; 1_06 adds loose objects, 1_07 chooses physical pepper representation and 1_08 adds direct machine operation. M2 then adds Coins and two purchases before the human gate. The [queue](tasks/readme.md) owns selection/readiness; [status](status.md) owns the aggregate summary. Read [core mechanics](../core-loop-and-mechanics.md), [scope](../scope-and-validation.md), [state](state-and-saving.md), [architecture](../../ARCHITECTURE.md) and relevant [Unity policy](unity-and-assets.md).

## Deliverable

One accessible outdoor work corner with finite nearby harvest, freely handled crate, broad feeder, directly responsive handle/rack, safe compressed processing, reusable finished carrier and generous Finished Food Handoff Rack. Players can stage loads and play with a few loose props. Repeated batches and the partial last batch can all become stored winter food. No blocked-passage objective, hidden wheel or required discovery.

Reuse the delivered PepperYard scene and safe processing backend. Current automatic operation is an interim implementation, extended in 1_08. The agent owns geometry/components, controls, UI, assets and ordinary Windows build configuration; no routine Inspector chores fall to the playtester.

## Numbered implementation tasks

1. [1_01 — Foundation](tasks/1_01_unity-foundation-and-walkable-scene.md): movement, input, pause/focus and scene.
2. [1_02 — Crate handling revision](tasks/1_02_scooping-and-crate-carrying.md): finite scooping plus free placement/rotation/drop and recovery.
3. [1_03 — Processing backend](tasks/1_03_tipping-and-automatic-processing.md): accepted transfers, safe batches and reserved output; already delivered automatic groundwork.
4. [1_04 — Finished carrier/handoff](tasks/1_04_finished-carrier-and-storage-rack.md): usable food output, broad deposit, free set-down/regrab and empty return.
5. [1_05 — Interim complete-loop checkpoint](tasks/1_05_first-playable-comfort-and-handoff.md): comfort/reset, complete food transfer and ordinary build.
6. [1_06 — Loose-object play](tasks/1_06_loose-yard-objects-and-playful-handling.md): shared grab/place/drop/toss, stable stacks and recovery.
7. [1_07 — Physical pepper comparison](tasks/1_07_physical-pepper-batch-comparison.md): contact/flow, bounded simulation and reliable representation choice.
8. [1_08 — Direct machine operation](tasks/1_08_direct-machine-operation.md): input-driven handle/rack, safe phase/transaction boundaries and complete physical food cycle.

Each task leaves an inspectable artifact and stops after its own handoff. Early deliveries do not need later actions to claim their narrow technical scope; M1 is not complete until 1_08's aggregate operation/handling result is ready.

## Acceptance criteria

- Useful batches can be gathered/poured with convincing local material movement and exact partial quantities.
- Carriers can be placed/rotated/dropped at chosen positions, regrabbed and recovered without food loss or compulsory mats.
- The chosen pepper representation contacts/settles usefully within measured limits; important physical objects are allowed and all food has one owner.
- A substantial mechanism responds directly to input; completing the operation starts the accepted batch once with output reserved. Interruption/pause/recovery never loses food or repeats input.
- Internal processing safely completes partial batches and waits at full output; finished food never burns/spoils while the player stages another load.
- The sole finished carrier can be freely parked/regrabbed and deposited once at a broad handoff, with no empty-container errand or label/shelf sorting.
- Automatically arranged simple jar/food shapes make receiving a recognizable finished batch. Careful placement settles predictably, and each accepted handoff grows/fills one nearby graybox food group visible from the work area. This derives from stored units and does not pull the four M5 household compositions or polished materials forward.
- Loose props use coherent controls/collision and can be arranged or played with without an objective.
- All finite food can become stored; pause/focus freezes physical motion, reset restores the authored test, and recovery preserves work/valid arrangements.
- Scene/input and ordinary-player checks cover actual interactions, not only compilation. The developer receives a ready-to-play artifact, exact controls and honest untested feel status.

M1 excludes Coins implementation until 2_01, full-yard production, final powered conversion, household display art/dialogue, second activities and disk persistence. Those boundaries do not postpone Coins past 2_03.

## Handoff

Follow the queue's [handoff rules](tasks/readme.md#handoff-and-recording): exact scene/build, controls, a short task-specific play checklist, measured checks, limitations and next ID. At the full M1 handoff, play a physical batch through direct operation to stored food, try an interrupted/partial batch, place/drop/recover objects and judge the mechanism. Keep technical delivery separate from human acceptance. No routine Stage0 rebuild/test is required.
