# 8_02 — Performance and build cleanup

Milestone: M8 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make the chosen presentation run consistently on recorded target hardware.

**Depends on:** [8_01 — Full game reliability](8_01_full-game-reliability.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Testing and performance](../testing-and-performance.md) · [Architecture](../../../ARCHITECTURE.md) · [Unity and assets](../unity-and-assets.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Profile physical batches, directly operated/powered mechanisms, disturbed props, combined improvements, machine output, displays, bench UI and save/load on recorded hardware/configuration.
- Fix measured CPU/GPU/allocation or memory problems with bounded effects, suitable imported textures/meshes, reuse, and targeted code changes. Treat the 64 moving-proxy cap and 60 FPS/1080p aim as provisional until measured.
- Profile live gameplay peppers separately from decorative proxies, including single/bulk selection, container filling/pouring and spills. Preserve representative nearby selectable physical material and grouping response under optimization; compare resting/distant aggregation without silently replacing local handling with counters/effects.
- Profile physical carriers/props separately from decorative pepper proxies: resting arrangements, a disturbed stack, repeated drop/toss, and saving/restoring a busy workspace. Record active/sleeping body counts and physics cost. Use suitable colliders/sleeping and measured limits while retaining nearby grab/release behavior; performance fixes must not restore compulsory placement mats or silently freeze interactable objects.
- Check player build contents and references; remove unused prototype/debug assets and scaffolding where safely unreferenced. Retain necessary licenses and avoid broad unrelated cleanup.

## Acceptance

- Record hardware, resolution, build settings, frame timing, allocations, and observed limits; minimum hardware claims are supported or left undecided.
- Optimizations do not change harvest accounting, local depletion, save behavior, or action readability; relevant affected cases are checked.

## Human playtest check

Try a full dump and busy machine area with the profiled build; report hitching or weakened feedback.

**Outside this task:** Engine-wide optimization without a measured issue, hidden quality regressions, and guarantees on untested hardware.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [8_03 — Release candidate rehearsal](8_03_release-candidate-rehearsal.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
