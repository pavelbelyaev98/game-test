# 7_03 — Audio display and guidance

Milestone: M7 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Keep feedback, text, and the final remaining work clear at supported settings.

**Depends on:** [7_02 — Input and camera options](7_02_input-and-camera-options.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Look, sound, and comfort](../../look-sound-and-comfort.md) · [State and saving](../state-and-saving.md) · [Core mechanics](../../core-loop-and-mechanics.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Complete separate sound-volume controls, readable subtitles/text, supported resolution/window modes, and persisted display/audio settings. Full/invalid cues remain limited to one restrained sound per meaningful state transition; holding input does not spam denial sounds, and persistent visual status remains readable when muted.
- Add restrained destination symbols/optional hints and final-target assistance for remaining actionable food. Distinguish a remaining pile from food already in the station or carrier.
- Check contrast, text layout, color-independent rules, menu/input usability, and safe recovery from unsuitable display settings.

## Acceptance

- Muted dialogue does not obscure required actions; settings persist and UI remains usable at recorded supported display configurations.
- The last partial load can be found and finished without a scavenger hunt or false completion cue. Hints reveal existing work rather than spawning supply; harvest completion waits for the last deposit, acknowledges it without a Finish action, and leaves normal control available.

## Human playtest check

Mute speech, lower effects, resize/change display mode, and use the guidance to finish the last few units.

**Outside this task:** An elaborate quest journal, completion collectibles, telemetry, full localization expansion, or gratuitous attention markers on scenery.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [8_01 — Full game reliability](8_01_full-game-reliability.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


