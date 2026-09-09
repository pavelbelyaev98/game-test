# Task 81 - Design brightness and reticle accessibility

Type: design/research. Status: `planned`. Prerequisites: `65`, `68`, `79`.

Feature: [camera comfort](../../features/backlog/camera-comfort.md). Delivery: [82](82-visual-accessibility.md). [Queue](../tasks.md). [Research](../../research/meltopia-lessons.md).

## Scope and proposal

- Retain implemented vertical FOV 55-90 degrees and Steady crosshair. Distinguish crosshair steadiness from the missing visibility/center-dot choice; avoid documenting existing controls as absent.
- Propose concrete reticle choices: current cross, fixed center dot and hidden, with current cross/steady default retained. Show sizing/contrast at 960x540 and larger windows and explain targeting with a hidden reticle.
- Propose brightness calibration range/default/reset and a real-world paused preview. Compare brightness/exposure adjustment with gamma terminology in the actual renderer; preserve approved terrain colors, surface highlights and underground legibility at zero battery.
- Define startup/Pause placement, keyboard/mouse adjustment, immediate application, saved preferences and malformed-value recovery using `65`/`79` conventions.
- Keep the current camera without shake/bob. Any separately introduced shake needs 0-100% strength, bob Off and independently disabled jetpack effects; do not manufacture effects or fake settings to match a checklist.

## Review and acceptance

- Present the actual control/range/default proposal and brightness/reticle comparisons for review; resolve choices together with `68`'s lighting contract.
- Record selected behavior and rejected alternatives in the feature so `82` can implement without product guesses. This task delivers a decision artifact, not settings or asset approval.
