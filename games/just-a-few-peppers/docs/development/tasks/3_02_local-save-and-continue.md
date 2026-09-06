# 3_02 — Local save and continue

Milestone: M3 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make closing and resuming the game preserve real work.

**Depends on:** [3_01 — Snapshots and in-scene restore](3_01_snapshots-and-in-scene-restore.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [State and saving](../state-and-saving.md) · [Testing and performance](../testing-and-performance.md) · [Architecture](../../../ARCHITECTURE.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Connect snapshots to one local save slot in the application's persistent-data directory. Coalesce autosaves after meaningful progress and provide basic explicit save/continue controls.
- Write a temporary sibling, validate it, and replace the current save while retaining a previous valid backup where supported. Flush for explicit save/exit and report failure honestly.
- Persist currently implemented settings separately from game progress. A New Game/reset of progress must not silently reset preferences; later tasks extend the settings fields.

## Acceptance

- An actual player restart resumes carried, queued, processing, output, and stored quantities from committed state without duplicate rewards.
- Disk writes are not per visual pepper; a failed save cannot claim success or intentionally destroy the only valid existing save.

## Human playtest check

Save partway through a load, close and reopen the build, continue, and confirm the small settings already implemented remain applied.

**Outside this task:** Polished main menus, multiple save slots, offline progress, or migrations for the old roasting spike.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [3_03 — Save failure recovery](3_03_save-failure-recovery.md). Stop after this task's handoff unless the user explicitly requested a larger range.


