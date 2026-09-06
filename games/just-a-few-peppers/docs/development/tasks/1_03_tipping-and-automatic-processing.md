# 1_03 — Tipping and automatic processing

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Turn a crate dump into visible automatic processing with reliable partial transfers.

**Depends on:** [1_02 — Scooping and crate carrying](1_02_scooping-and-crate-carrying.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Wire one broad intake target and a short tilt/cascade. Transfer only the accepted amount into the input buffer; interrupted motion cannot repeat or erase a committed transfer.
- Add one input queue, one active batch, and accumulating output at the starting 12/12 tier. Reserve output room before beginning work and start partial batches automatically.
- Show simple working/output-full feedback and placeholder roast/rest/preparation/packing stages. Pause processing with gameplay; waiting never spoils or burns food.
- Keep transfer pacing tuned so the player's main motivation is visible clearing and occasional handling bursts, not repeated blocked-input feedback.

## Acceptance

- Test full, limited-space, cancelled, and final partial transfers; conservation holds across pile, crate, queue, active batch, and output.
- Full output safely pauses the line; output plus reserved active work stays within capacity. No minimum load or full-jar rule strands the last peppers.

## Human playtest check

Tip a full crate, inspect the cascade and output, then try a partial load and pause during processing. Output collection is added in 1_04.

**Outside this task:** Manual peeling, separate intermediate inventories, quality timers, fuel, recipes, or helper AI.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_04 — Finished carrier and handoff rack](1_04_finished-carrier-and-storage-rack.md). Stop after this task's handoff unless the user explicitly requested a larger range.


