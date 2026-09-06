# 1_08 — Direct machine operation

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make one physical batch respond to direct machine operation and one small player-controlled grouping action within finished-output collection.

**Depends on:** [1_07 — Physical pepper batch comparison](1_07_physical-pepper-batch-comparison.md); read and preserve the [1_03 delivery](1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026) and [1_04 delivery](1_04_finished-carrier-and-storage-rack.md#delivery-record--september-6-2026). Their automatic processing/receiving evidence remains the baseline. All earlier play gates must be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), dependency delivery/feedback records, and [core mechanics](../../core-loop-and-mechanics.md), [scope/validation](../../scope-and-validation.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [verification](../testing-and-performance.md) / [asset rules](../unity-and-assets.md) and [look/comfort](../../look-sound-and-comfort.md). Inspect actual source, scene/input wiring and pinned versions before implementation; the brief is not delivery evidence.

## Work

- Build one substantial handle/sliding rack in the current apparatus. Input directly controls useful mechanism motion with broad targeting and forgiving completion; a click followed only by a timer/model animation does not meet the task.
- Loading leaves accepted food queued. One completed operation validates available output reservation and starts the batch exactly once. Retain short automatic roast/rest/preparation internally and safe waiting prepared output; detailed filling/capping remains compressed within the grouping presentation. No fine doneness timing, repeated clicks or extra service chore.
- Replace passive finished-output collection with the [provisional grouping-guide candidate](../../core-loop-and-mechanics.md#make-the-finished-batch-worth-handling): directly slide a broad guide across the receiving tray to gather loose prepared material into a neat batch in the existing carrier. Input controls useful motion, with a forgiving endpoint and safe release; a click-to-watch animation alone is insufficient. Exact gesture, binding, distance and duration remain provisional for this test.
- Keep grouping within the existing output transfer, with one carrier and no additional inventory or chain. Automate detailed filling/capping and repeated confirmations: no Start/Confirm per pepper, jar or internal stage, individual peeling, lid shopping, precise alignment or repeated errands. Preserve 1_04's handoff and nearby accumulation; the last partial batch needs no full-carrier prerequisite.
- Define ready/operating/working/prepared-output/grouping/blocked states, interrupted-stroke behavior and both transaction boundaries. Grouping selects a subset of station output, transfers it into the sole docked carrier once on completion and preserves active output reservations. Cancel before commit leaves food at the output; interruption after commit keeps it in the carrier. New ready output cannot silently join a started selection. Stop/cancel/pause/recover without losing queued/active food or replaying the command; clear stale operation input on focus/resume.
- Own mechanism/guide geometry, collision/constraint or controlled-motion choice, Input System controls, contextual UI and sound. Teach operation and grouping one at a time with brief local guidance; preserve 1_07's distinct single/bulk, placement and pouring controls and hidden placement indicators. Most yard/work locations are accessible; stage raw supplies near the apparatus. Preserve free carrier/prop placement and the broad finished-food handoff.
- Measure direct operation strokes/held time, pouring, travel, internal wait and output work through repeated complete and partial batches. Keep the modest apparatus pleasant; record baseline evidence for meaningful 2_01 improvements.

## Acceptance

- An ordinary player can load, directly operate, help group loose prepared material into a neat finished batch, then hand it off, including the last partial amount. Required actions remain readable with audio muted. Packaging agency/feel is evaluated separately from appearance and machine operation.
- Conservation/output reservation, interrupted operation/grouping before and after commit, partial collection, pause/focus, repeated input and recovery pass rules/integration/player checks. No stale gesture starts a batch, recollects output or earns duplicate handoff credit after resume. Disk persistence remains M3.
- M1 now contains the physical batch/operation loop and usable finished output. Technical readiness is separate from human enjoyment; 2_03 will evaluate it with purchases.

## Human playtest check

Operate several ordinary batches without a reward or reveal. Stop the input handle and output-grouping guide midway, resume or recover, then finish a partial last batch and store it. Report machine operation, grouping/packaging, single pickup, bulk pickup and control clarity separately, including which transfers or waits feel needless.

**Outside this task:** Coins/purchases (2_01), batch-size settings or pepper classification (separate later decisions), detailed jar filling/capping, ten-step cooking, burns, fuel/jams/repairs, parts hunting, production art, final machine or second food activities.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Own scene/prefab, input, UI, assets and build wiring; record exact scene/build, controls, short checklist, checks, limitations and actual feedback. Update queue/milestone records and stop after this handoff.

Next in order: [2_01 — Coins and two equipment improvements](2_01_wheelbarrow-discovery-and-loader.md).

## Planning record — September 6, 2026

Added for the developer's revised processing direction. Todo / Not tested. 1_03 remains the technically delivered automatic backend; its old passes do not prove this new operation.
