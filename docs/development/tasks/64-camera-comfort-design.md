# Task 64 - Define early camera comfort settings

Type: design/research; documentation only. Status: `planned`. Prerequisites: `35`, `63`.

Feature: [FPS controls](../../features/backlog/fps-controls.md). Implementation: [65](65-camera-comfort-settings.md); later presentation: `57`/`11`/`47`. [Queue](../tasks.md).

## Research and proposal

- Read the [physical-comfort findings](../../research/player-review-findings.md#physical-comfort); inspect the actual camera, reticle, pause UI and window/aspect-ratio behaviour. Currently the camera has yaw/pitch only, the HUD reticle pulses while digging, and there are no player-facing FOV settings, shake, head bob or jetpack camera effects.
- Propose a compact settings contract: FOV slider with an explicit horizontal/vertical convention, tested range/default and aspect-ratio handling; a stable center reticle option; reset-to-default behaviour and durable preferences independent from excavation saves.
- Define shake strength down to zero, head-bob off and an independent jetpack-camera-effects off policy for effects if/when they exist. Keep the present motion-free baseline; do not commission effects or expose switches that currently do nothing. Every future motion-effect owner must honor the contract when introduced.
- Research current official Unity camera/UI guidance and compare readability, perceived movement and terrain/tool visibility at candidate FOVs. Prefer a steady-reticle, low-motion default; distinguish proposed settings from a promise to eliminate everyone's motion sickness.
- Specify settings access through the existing pause/menu flow, keyboard/mouse navigation and input isolation. Use existing approved UI content; do not reopen the deferred presentation scope, add assets, change hold-to-dig or introduce a minimap.

## Questions to resolve with the user

Present the concrete FOV/defaults and steady-reticle proposal with clear examples, then resolve their preferred defaults and any meaningful visibility/comfort tradeoff. Routine numerical limits follow measured readability. Do not ask whether to add head bob just to fill a settings menu.

## Done when

- Record the accepted controls, labels/defaults, persistence/reset rules and low-motion contract in the FPS feature; `65` can implement them without inventing the product settings while coding.
- `57`, later tool/jetpack work and `54` inherit the same comfort rules. No runtime settings or new effects are claimed implemented by this design task.
