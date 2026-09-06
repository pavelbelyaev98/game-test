# 4_03 — Harvest completion and ending state

Milestone: M4 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make the graybox game finish exactly when the finite harvest is stored.

**Depends on:** [4_02 — Final processor and upgrade order](4_02_final-processor-and-upgrade-order.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Household and ending](../../household-readiness-and-parcels.md) · [State and saving](../state-and-saving.md) · [Yard and progression](../../yard-and-progression.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Derive readiness from all authored supply being cleared and all initial units stored, with no food left in carriers or processing. Equipment and scenery add no extra conditions.
- Add Ready to finish the day and a voluntary Finish action at the vine-table stand-in. Transition to a simple finished-day representation and persist the ended flag.
- Run the functional arc in different valid discovery orders with a deliberately partial final batch. Extend snapshot/version fixtures for completion and reconstruct ended state without replaying rewards.

## Acceptance

- An empty yard with unprocessed/uncarried output remaining cannot finish; the final deposit enables the voluntary action.
- Reloading before/after ending preserves completion, and the full graybox arc reaches the end without a final household task, surprise pile, or duplicated food.

## Pavel's check

Leave the last output at the station, confirm finishing is unavailable, store it, inspect the yard, then finish and reload.

**Outside this task:** Final meal art, new quotas, required upgrades/collectibles, or post-game chores.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [5_01 — Representative assets and yard section](5_01_representative-assets-and-yard-section.md). Stop after this task's handoff unless the user explicitly requested a larger range.
