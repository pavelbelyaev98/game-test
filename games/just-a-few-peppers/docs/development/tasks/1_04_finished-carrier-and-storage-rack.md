# 1_04 — Finished carrier and handoff rack

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Complete the scoop-to-stored-food loop in the same scene.

**Depends on:** [1_03 — Tipping and automatic processing](1_03_tipping-and-automatic-processing.md). All earlier play gates must also be resolved under the queue rules.

The reopened [1_02 free-placement revision](1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) must be technically complete first; read its new delivery alongside 1_03. The developer reported unusable jars in the current processing build; this task makes that output actionable.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Household and ending](../../household-readiness-and-parcels.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add the sole reusable finished-food carrier. Pick up all available output within its tier capacity while preserving active output reservations; new output can accumulate while it is away. Walking, sprinting, and jumping do not transfer ownership or lose its load; boundaries and recovery preserve the exact carried units.
- Make the [finished batch recognizable](../../core-loop-and-mechanics.md#make-the-finished-batch-worth-handling) using simple prepared-food/jar shapes and a readable receiving motion. Packing/arrangement is automatic; no individual jar alignment, lid action or confirmation per item. A partial last batch is visibly valid output, not an apparently incomplete task.
- Reuse the [free handling contract](../../core-loop-and-mechanics.md#pick-up-place-and-play): let the player rotate, place, drop and regrab the loaded finished carrier without depositing or losing its exact contents. Keep the raw carrier where placed; a convenient switch may place it in nearby clear space, never require a mat errand. If that space is blocked, keep ownership and explain what needs moving.
- Deposit the finished load once at the always-accessible **Finished Food Handoff Rack**, then return the empty carrier automatically to its dock. Update the existing foundation placeholder's label/target guidance during this implementation, preserving its asset references. Make output readiness/collection clear so visible finished jars do not appear inert.
- Show stored winter food, clear next-action prompts and one small nearby graybox food group whose volume/fill grows with each accepted deposit. Keep it readable from the work area and derive it from stored units, including partial amounts; counters alone are insufficient. All jar groups remain subordinate to exact pepper-unit state.
- Make the one handoff read as the player's final food-handling responsibility. The nearby group previews accumulation under the [display contract](../../household-readiness-and-parcels.md#progress-drives-presentation); M5 owns the four combined cellar/family compositions and polished food art. Add no distribution or ending in this task.

## Acceptance

- Every unit in a small non-multiple-of-12 harvest can reach the rack; no second output carrier or retrievable duplicate food appears.
- Check repeated/empty deposits, occupied/full output, interrupted pickup/deposit, and loaded-carrier recovery. There is no empty-container return trip.
- Place/drop the loaded finished carrier at chosen ground/worktop positions, pause while it moves, recover and regrab it, then deposit exactly once. Verify raw/finished switching with clear and obstructed nearby space, overlapping targets, and output accumulating while the loaded carrier is parked. Placing near the rack is not an accidental deposit.
- Careful placements settle at the chosen supported pose/orientation without a launch, prolonged bounce or repeated valid-target rejection. Decorative jars do not steal the carrier prompt; deliberate dropping remains available and preserves contents.
- The same permanent target accepts all finished food. Several successive deposits, including a partial amount, visibly grow/fill the nearby stored-food group without an extra collectible copy or secondary inventory. Empty/repeated deposits and recovery leave it unchanged; the empty carrier remains reusable. No rack-to-cellar transfer, household destination choice or helper.

## Human playtest check

Finish several loads and the partial last load. Place the carrier carefully somewhere you choose, resume other work, then regrab and deposit it. Watch the food group grow from the work area and collect again. Report which transfer felt best, whether the finished batch was worth looking at/handling, and whether placement or switching added needless effort.

**Outside this task:** Household display art, parcel allocation, temporary storage relocation, disk saves, or later completion presentation.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


