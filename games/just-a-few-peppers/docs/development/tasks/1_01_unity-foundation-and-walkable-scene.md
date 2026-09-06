# 1_01 — Unity foundation and walkable scene

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Give Pavel a new outdoor test scene he can open and walk around immediately.

**Depends on:** [0_01 — Repository and design baseline](0_01_repository-and-design-baseline.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [M1 contract](../first-playable-task.md) · [Architecture](../../../ARCHITECTURE.md) · [Repository audit](../repository-audit.md) · [Unity and assets](../unity-and-assets.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Inspect actual editor/package versions and current source. Add compatible Input System and Unity test tooling; record installed versions and verified commands. Reuse or retire Stage0 pieces after checking references.
- Create the v4 scene, camera, collision, keyboard/mouse movement, gameplay/UI input actions, and broad targeting foundation. Use placeholders for mound, crate, station, and rack in a short outdoor route.
- Wire basic pause, focus loss, cursor capture/release, and a reset to safe spawn. Keep new scene/build configuration reproducible and prevent old generators from overwriting it.

## Acceptance

- The supplied scene opens and runs without missing references or new unexplained errors; movement/look and pause/resume work with the new input setup.
- Compatible test assemblies can run a meaningful scene/input check; record actual evidence and limitations. Do not install a general gameplay framework.

## Pavel's check

Walk the route, turn, pause, switch focus, and resume; identify any movement or camera discomfort.

**Outside this task:** Pepper handling, wheelbarrow, saves, polished art, or a full property.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_02 — Scooping and crate carrying](1_02_scooping-and-crate-carrying.md). Stop after this task's handoff unless the user explicitly requested a larger range.
