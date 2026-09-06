# 7_02 — Input and camera options

Milestone: M7 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make the supported controls comfortable and persistent.

**Depends on:** [7_01 — New continue and exit flow](7_01_new-continue-and-exit-flow.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Look, sound, and comfort](../../look-sound-and-comfort.md) · [State and saving](../state-and-saving.md) · [Unity and assets](../unity-and-assets.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Complete configurable input bindings, sensitivity, FOV, invert Y, hold-to-scoop input, and reduced-motion controls with sensible defaults.
- Use the Input System's compatible supported APIs, keep menu/gameplay action maps usable, and provide reset-to-default/recovery for unusable bindings.
- Apply and persist options separately from game progress. Keep camera shake/head bob off by default and the loaded wheelbarrow view unobstructed.

## Acceptance

- Changes apply predictably, survive restart and New Game, and do not duplicate input or alter conserved gameplay state.
- Rebinding/cancelling/resetting cannot strand the player without a usable way to open or navigate controls; actual pause/focus and movement behavior still works.

## Human playtest check

Change a binding, invert look, change FOV/sensitivity, verify holding/releasing the scoop binding, restart, then restore defaults.

**Outside this task:** Unrequested controller-platform support, camera effects that change authoritative interactions, or a generic input framework.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [7_03 — Audio display and guidance](7_03_audio-display-and-guidance.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


