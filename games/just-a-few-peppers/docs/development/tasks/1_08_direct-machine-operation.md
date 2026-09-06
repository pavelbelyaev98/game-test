# 1_08 — Direct machine operation

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make one physical batch require a satisfying, directly responsive machine operation before safe internal processing.

**Depends on:** [1_07 — Physical pepper batch comparison](1_07_physical-pepper-batch-comparison.md); preserve the full [1_03 delivery](1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026) and completed handoff. All earlier play gates must be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), dependency delivery/feedback records, and [core mechanics](../../core-loop-and-mechanics.md), [scope/validation](../../scope-and-validation.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [verification](../testing-and-performance.md) / [asset rules](../unity-and-assets.md) and [look/comfort](../../look-sound-and-comfort.md). Inspect actual source, scene/input wiring and pinned versions before implementation; the brief is not delivery evidence.

## Work

- Build one substantial handle/sliding rack in the current apparatus. Input directly controls useful mechanism motion with broad targeting and forgiving completion; a click followed only by a timer/model animation does not meet the task.
- Loading leaves accepted food queued. One completed operation validates available output reservation and starts the batch exactly once. Retain short automatic roast/rest/preparation/packing internally and safe waiting finished output. No fine doneness timing, repeated clicks or extra service chore.
- Keep meaningful whole-batch handling while automating repeated confirmations: no Start/Confirm per pepper, jar or internal stage, and no manual finished-jar arrangement. Preserve 1_04's recognizable receiving/handoff and nearby accumulation. A later automation example does not replace this task's directly responsive mechanism or add an endless supply mode.
- Define ready/operating/working/finished/blocked states, interrupted-stroke behavior and the transaction boundary. Stop/cancel/pause/recover without losing queued/active food or replaying the command; clear stale operation input on focus/resume.
- Own mechanism geometry, collision/constraint or controlled-motion choice, Input System controls, contextual UI and sound. Most yard/work locations are accessible; stage raw supplies near the apparatus. Preserve free carrier/prop placement and the broad finished-food handoff.
- Measure direct operation strokes/held time, pouring, travel, internal wait and output work through repeated complete and partial batches. Keep the modest apparatus pleasant; record baseline evidence for meaningful 2_01 improvements.

## Acceptance

- An ordinary player can load, directly operate, see food transform, unload and hand off a complete batch and the last partial one. Required actions remain readable with audio muted.
- Conservation/output reservation, interrupted operation, pause/focus, repeated input and recovery pass rules/integration/player checks. No stale gesture starts a batch after resume.
- M1 now contains the physical batch/operation loop and usable finished output. Technical readiness is separate from human enjoyment; 2_03 will evaluate it with purchases.

## Human playtest check

Operate several ordinary batches without a reward or reveal. Stop the handle midway, resume or recover, then process a partial last batch. Report whether moving the mechanism itself is enjoyable and which transfers or waits feel needless.

**Outside this task:** Coins/purchases (2_01), ten-step cooking, burns, fuel/jams/repairs, parts hunting, production art, final machine or second food activities.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Own scene/prefab, input, UI, assets and build wiring; record exact scene/build, controls, short checklist, checks, limitations and actual feedback. Update queue/milestone records and stop after this handoff.

Next in order: [2_01 — Coins and two equipment improvements](2_01_wheelbarrow-discovery-and-loader.md).

## Planning record — September 6, 2026

Added for the developer's revised processing direction. Todo / Not tested. 1_03 remains the technically delivered automatic backend; its old passes do not prove this new operation.
