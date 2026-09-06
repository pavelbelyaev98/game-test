# 1_04 — Finished carrier and storage rack

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Complete the scoop-to-stored-food loop in the same scene.

**Depends on:** [1_03 — Tipping and automatic processing](1_03_tipping-and-automatic-processing.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Household and ending](../../household-readiness-and-parcels.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add the sole reusable finished-food carrier. Pick up all available output within its tier capacity while preserving active output reservations; new output can accumulate while it is away.
- Park the raw carrier safely during finished-food handling. Deposit the finished load once at the always-accessible rack, then return the empty carrier automatically to its dock.
- Show stored winter food and clear next-action prompts. Keep all visual jar groups subordinate to exact pepper-unit state, including incomplete final groups.

## Acceptance

- Every unit in a small non-multiple-of-12 harvest can reach the rack; no second output carrier or retrievable duplicate food appears.
- Check repeated/empty deposits, occupied/full output, interrupted pickup/deposit, and loaded-carrier recovery. There is no empty-container return trip.

## Pavel's check

Finish several loads and the partial last load; leave the carrier away from the station briefly, then collect again after depositing.

**Outside this task:** Household display art, parcel allocation, temporary storage relocation, disk saves, or the meal ending.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md). Stop after this task's handoff unless the user explicitly requested a larger range.
