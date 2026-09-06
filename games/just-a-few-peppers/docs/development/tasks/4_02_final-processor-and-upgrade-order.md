# 4_02 — Final processor and upgrade order

Milestone: M4 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make Grandpa's final machine useful even when discovered before the wheelbarrow.

**Depends on:** [4_01 — Connected graybox yard](4_01_connected-graybox-yard.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Yard and progression](../../yard-and-progression.md) · [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Scope and validation](../../scope-and-validation.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Expose E's covered-machine stand-in before its demonstration mound is cleared. Install the single station's 96/96 tier at a safe boundary, preserving all contents.
- Keep the raw wheelbarrow at 48; combine two 48-unit outputs into one pickup or allow earlier partial pickup. The same input, carrier, and rack remain in use.
- Handle crate-to-final discovery followed by wheelbarrow/loader discovery without downgrading 96 capacity. Save pending/activated equipment correctly and compare the same 96-unit job at tiers 48 and 96.

## Acceptance

- Tests and scene checks cover final-before-wheelbarrow order, loaded installation, save during pending upgrade, and output accumulation while the carrier is away.
- The measured whole job shows useful feeding/output improvement; no additional queue, recipe, raw-carrier upgrade, or forced waiting is introduced.

## Pavel's check

Discover the final machine first, later take the wheelbarrow, and try collecting two loads together versus collecting early.

**Outside this task:** Custom polished machinery yet, conveyors, machine assembly, a 96-unit wheelbarrow, or separate production lines.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [4_03 — Harvest completion and ending state](4_03_harvest-completion-and-ending-state.md). Stop after this task's handoff unless the user explicitly requested a larger range.
