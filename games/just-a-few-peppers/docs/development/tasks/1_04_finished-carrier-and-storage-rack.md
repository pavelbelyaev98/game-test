# 1_04 — Finished carrier and handoff rack

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Complete the scoop-to-stored-food loop in the same scene.

**Depends on:** [1_03 — Tipping and automatic processing](1_03_tipping-and-automatic-processing.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Household and ending](../../household-readiness-and-parcels.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add the sole reusable finished-food carrier. Pick up all available output within its tier capacity while preserving active output reservations; new output can accumulate while it is away. Walking, sprinting, and jumping do not transfer ownership or lose its load; boundaries and recovery preserve the exact carried units.
- Park the raw carrier safely during finished-food handling. Deposit the finished load once at the always-accessible **Finished Food Handoff Rack**, then return the empty carrier automatically to its dock. Update the existing foundation placeholder's label/target guidance during this implementation, preserving its asset references.
- Show stored winter food and clear next-action prompts. Keep all visual jar groups subordinate to exact pepper-unit state, including incomplete final groups.
- Make the one handoff read as the player's final food-handling responsibility. Its single stored total is the future source for household displays; do not implement cellar/family props, distribution, or an ending in this prototype task.

## Acceptance

- Every unit in a small non-multiple-of-12 harvest can reach the rack; no second output carrier or retrievable duplicate food appears.
- Check repeated/empty deposits, occupied/full output, interrupted pickup/deposit, and loaded-carrier recovery. There is no empty-container return trip.
- The same permanent target accepts all finished food. No secondary rack inventory, rack-to-cellar transfer, household destination choice, or helper is introduced; plain stored-progress feedback is enough here.

## Human playtest check

Finish several loads and the partial last load; leave the carrier away from the station briefly, then collect again after depositing.

**Outside this task:** Household display art, parcel allocation, temporary storage relocation, disk saves, or later completion presentation.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


