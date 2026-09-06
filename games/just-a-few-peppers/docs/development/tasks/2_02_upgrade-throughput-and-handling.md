# 2_02 — Upgrade throughput and handling

Milestone: M2 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make the wheelbarrow improve the whole job rather than only the capacity number.

**Depends on:** [2_01 — Wheelbarrow discovery and loader](2_01_wheelbarrow-discovery-and-loader.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [Yard and progression](../../yard-and-progression.md) · [Scope and validation](../../scope-and-validation.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Provide a reproducible equal-quantity comparison: 48 units by four crate loads versus one wheelbarrow load, on the same route. Use test configuration, not new player-facing modes.
- Tune capacity, scoop cadence/width, movement, intake/output placement, process rate, buffers, and feedback from observed bottlenecks. Count gather time, loaded travel, dump interaction, unavoidable waiting, output handling, and empty return travel. Never slow a satisfying scoop or add delays merely to improve the ratio.
- Keep route changes separate when comparing capacity; preserve the 12/48 tool and 12/48 station semantics.

## Acceptance

- Record actual comparison conditions and timings/observations; do not infer whole-job speed from capacity alone.
- The larger tool remains comfortable to turn/reverse, and station/output work does not erase the practical benefit or introduce blockers.

## Human playtest check

Complete the matched job with each tool and describe which ordinary actions you would choose to repeat.

**Outside this task:** Extra mandatory supply to cancel upgrades, longer waits for pacing, automation branches, or full-yard dressing.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [2_03 — Core feel playtest gate](2_03_core-feel-playtest-gate.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


