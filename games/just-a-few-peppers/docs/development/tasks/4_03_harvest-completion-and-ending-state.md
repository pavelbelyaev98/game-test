# 4_03 — Harvest completion and completed-yard state

Milestone: M4 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Commit harvest completion exactly when the finite harvest is stored, while keeping the completed yard playable.

**Depends on:** [4_02 — Final processor and upgrade order](4_02_final-processor-and-upgrade-order.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Household and ending](../../household-readiness-and-parcels.md) · [State and saving](../state-and-saving.md) · [Yard and progression](../../yard-and-progression.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Derive winter-preparation completion from all initial food being permanently stored and every other owner empty, including optional registered loose/transit units. Yard tidiness, available Coins and unpurchased equipment add no conditions.
- The final valid deposit commits its normal transfer and harvest-completion state together, then gives deposit feedback and a quiet nonmodal acknowledgement such as **Harvest complete**. No Ready-to-finish state, Finish Day command, trip to the vine table, forced cutscene, credit fade, or menu ejection is required. Follow the [harvest completion contract](../state-and-saving.md#harvest-completion-contract-for-m4-onward).
- Keep normal camera and movement control in the completed yard. Machines become idle naturally, completed winter-food displays remain visible, and ordinary pause/menu controls provide the exit. Add no new supply, chores, deadlines, or surprise delivery.
- Run the functional arc with either prototype purchase order, paid installation and a partial final batch. The last deposit atomically stores food, awards proportional Coins and completes once; save/reload reconstructs without replaying any reward or required sequence.

## Acceptance

- An empty yard with queued, active, uncollected, or carried food remaining cannot complete. The final valid deposit, including a partial final batch, commits completion exactly once.
- Pause/focus loss and reload before or after completion preserve the appropriate state and normal control without requiring another action, replaying a gift, or running a transition. Repeated/empty deposits cannot retrigger completion feedback as a reward.
- The full graybox arc reaches completion without a household task, surprise pile, required equipment checklist, or duplicated food.

## Human playtest check

Leave the last output at the station and confirm the harvest stays incomplete. Deposit it, confirm the quiet acknowledgement and continued yard control, then pause/reload and inspect a completed save.

**Outside this task:** Meal interaction/cinematics, table/gift art, new quotas, required upgrades/collectibles, credits flow, or post-completion chores.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [5_01 — Representative assets and yard section](5_01_representative-assets-and-yard-section.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
