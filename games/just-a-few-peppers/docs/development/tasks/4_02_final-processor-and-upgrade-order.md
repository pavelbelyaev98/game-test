# 4_02 — Final processor and upgrade order

Milestone: M4 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make Grandpa's final machine a major upgrade to the complete remaining job, using the same controls even when found before the wheelbarrow.

**Depends on:** [4_01 — Connected graybox yard](4_01_connected-graybox-yard.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Yard and progression](../../yard-and-progression.md) · [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Scope and validation](../../scope-and-validation.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Expose the covered-machine stand-in through the chosen compact layout, before the final supply is exhausted. Record remaining units and repeated complete cycles available after reveal. Install the single station's 96/96 tier at a safe boundary, preserving all contents.
- Keep the raw wheelbarrow at 48; combine two 48-unit outputs into one pickup or allow earlier partial pickup. Retain the one reusable finished carrier, output dock, and Finished Food Handoff Rack.
- Integrate a fixed loading chute/intake near the final supply as authored station geometry. It becomes the one active broad dump target into the existing input buffer. Preserve scoop/carry/dump controls; no extra carrier, powered clearing tool, queue, UI subsystem, construction, repair, or player verb.
- Handle crate-to-final discovery followed by wheelbarrow/loader discovery without downgrading capacity or removing the intake route. Pending installation retains the old intake; installed tier restores the new one. Save/restore must preserve loads and select exactly one active dump target.
- Run the [combined final-upgrade comparison](../../scope-and-validation.md#later-checks-for-the-complete-game): same 96 units from the same final supply, same wheelbarrow and handoff rack, original route/tier 48 versus nearby intake/tier 96. Store matched repeated-trial timings, loaded distances, output trips, waiting, empty walking, total scoop-to-storage time, and absolute/percentage changes in this task's delivery record. An isolated same-path capacity check is only supporting evidence.

## Acceptance

- Tests and scene checks cover final-before-wheelbarrow order, loaded installation, pending/installed save restoration of the active intake, output accumulation while the carrier is away, and the same permanent handoff after all upgrades.
- Loaded hauling is substantially shorter, combined output needs fewer collections, and total workflow time improves beyond ordinary trial variability. Increased capacity or spectacle alone does not meet acceptance; output travel/waiting cannot erase the gain.
- The reveal leaves repeated useful cycles on the intended approach, using existing supply and the same wheelbarrow. Keep insufficient measurement or a failed improvement recorded as partial work rather than expanding scope.

## Pavel's check

Compare the final-supply haul before/after the reveal, then run repeated two-load/output-handoff cycles. Also discover the final machine first, later take the wheelbarrow, and try collecting early.

**Outside this task:** Custom polished machinery yet, conveyors, machine assembly, a 96-unit wheelbarrow, or separate production lines.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [4_03 — Harvest completion and ending state](4_03_harvest-completion-and-ending-state.md). Stop after this task's handoff unless the user explicitly requested a larger range.
