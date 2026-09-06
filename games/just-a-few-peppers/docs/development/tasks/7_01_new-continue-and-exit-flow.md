# 7_01 — New continue and exit flow

Milestone: M7 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Turn the playable scene into a clear start, resume, and leave experience.

**Depends on:** [6_02 — Campaign pacing and dialogue pass](6_02_campaign-pacing-and-dialogue-pass.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [State and saving](../state-and-saving.md) · [Testing and performance](../testing-and-performance.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Finish the title/pause flow for New Game, Continue when valid, save/exit, and returning after completion. Reuse the existing save/recovery rules.
- Make starting over an explicit choice that cannot accidentally erase progress; keep settings separate. Show save failure and backup options honestly.
- Wire mouse/keyboard UI navigation, cursor capture, focus, and transitions so no click leaks into a gameplay action.

## Acceptance

- Fresh install, existing save, ended save, and invalid-save cases open the appropriate usable flow.
- Saving/exiting/relaunching preserves committed progress and settings; menus pause gameplay and cannot duplicate a transfer or ending.

## Human playtest check

Start, save, leave, continue, finish, and open the menu again; try keyboard-only menu navigation.

**Outside this task:** Online accounts, cloud saves, multiple slots, a level selector, or new gameplay after the meal.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [7_02 — Input and camera options](7_02_input-and-camera-options.md). Stop after this task's handoff unless the user explicitly requested a larger range.


