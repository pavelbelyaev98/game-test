# 3_01 — Snapshots and in-scene restore

Milestone: M3 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Reconstruct the current playable loop from one coherent logical snapshot.

**Depends on:** [2_03 — Core feel playtest gate](2_03_core-feel-playtest-gate.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [State and saving](../state-and-saving.md) · [Architecture](../../../ARCHITECTURE.md) · [Core mechanics](../../core-loop-and-mechanics.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Represent stable content/pocket IDs, exact quantities/local depletion, carriers, station tier and pending activation, processing duration, stored food, and valid player pose in ordinary versioned data.
- Capture after committed actions and restore the model before rebuilding views. Provide a small development-only capture/restore action in the playable scene.
- Validate IDs, finite values, bounds, capacities, conservation, and content/schema versions; recover invalid transforms to safe authored points without adding food.

## Acceptance

- Round trips preserve partial piles, raw loads, active processing, accumulating output, carried finished loads, pending upgrades, and stored progress.
- Meaningful tests reject invalid snapshots; restoration does not replay deposits or make particles authoritative. This task claims in-memory restore only.

## Pavel's check

Capture during several load stages, change the scene state, restore, and check that quantities and usable equipment return correctly.

**Outside this task:** Disk writes, cloud, slot browser, or prototype-save compatibility.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [3_02 — Local save and continue](3_02_local-save-and-continue.md). Stop after this task's handoff unless the user explicitly requested a larger range.
