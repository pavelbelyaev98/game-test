# 4_03 — Harvest completion and ending state

Milestone: M4 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make the graybox game finish exactly when the finite harvest is stored.

**Depends on:** [4_02 — Final processor and upgrade order](4_02_final-processor-and-upgrade-order.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Household and ending](../../household-readiness-and-parcels.md) · [State and saving](../state-and-saving.md) · [Yard and progression](../../yard-and-progression.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Derive harvest completion from all authored supply being cleared and all initial units stored, with no food left in carriers or processing. Equipment and scenery add no extra conditions.
- The final accepted deposit commits the ended flag automatically, then gives deposit feedback and starts a simple finished-day transition. No Ready-to-finish state, Finish command, or trip to the vine-table stand-in is required. Follow the [automatic completion contract](../state-and-saving.md#automatic-completion-contract-for-m4-onward).
- Run the functional arc in different valid discovery orders with a deliberately partial final batch. Extend snapshot/version fixtures for completion and reconstruct ended state without replaying rewards.

## Acceptance

- An empty yard with queued, active, uncollected, or carried food remaining cannot end. The final accepted deposit, including a partial final batch, automatically ends it exactly once.
- Pause/focus loss during the transition freezes presentation without undoing completion. Reloading before, during, or after the ending restores the appropriate state without requiring another action or replaying a gift. Repeated/empty deposits cannot retrigger it.
- The full graybox arc reaches the end without a household task, surprise pile, required equipment checklist, or duplicated food.

## Human playtest check

Leave the last output at the station and confirm the day stays unfinished. Deposit it and confirm the ending starts automatically; pause and reload during the transition, then reload an ended save.

**Outside this task:** Final meal art, new quotas, required upgrades/collectibles, or post-game chores.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [5_01 — Representative assets and yard section](5_01_representative-assets-and-yard-section.md). Stop after this task's handoff unless the user explicitly requested a larger range.


