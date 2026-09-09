# Task 64 - Define early camera comfort settings

Type: design/research; documentation only. Status: `done`. Prerequisites: `35`, `63` (both complete). [Completion](../completed/64-camera-comfort-design.md).

Feature: [camera comfort](../../features/backlog/camera-comfort.md), linked from [FPS controls](../../features/backlog/fps-controls.md). Implementation: [65](65-camera-comfort-settings.md); later presentation: `57`/`11`/`47`. [Queue](../tasks.md).

## Research and proposal

- Read the [physical-comfort findings](../../research/player-review-findings.md#physical-comfort); inspect the actual camera, reticle, pause UI and window/aspect-ratio behaviour. At this task's research baseline, the camera had yaw/pitch only, the HUD reticle pulsed while digging, and there were no player-facing FOV settings, shake, head bob or jetpack camera effects. Settings were subsequently implemented in [65](65-camera-comfort-settings.md).
- Propose a compact settings contract: FOV slider with an explicit horizontal/vertical convention, tested range/default and aspect-ratio handling; a stable center reticle option; reset-to-default behaviour and durable preferences independent from excavation saves.
- Define shake strength down to zero, head-bob off and an independent jetpack-camera-effects off policy for effects if/when they exist. Keep the present motion-free baseline; do not commission effects or expose switches that currently do nothing. Every future motion-effect owner must honor the contract when introduced.
- Research current official Unity camera/UI guidance and compare readability, perceived movement and terrain/tool visibility at candidate FOVs. Prefer a steady-reticle, low-motion default; distinguish proposed settings from a promise to eliminate everyone's motion sickness.
- Specify settings access through the existing pause/menu flow, keyboard/mouse navigation and input isolation. Use existing approved UI content; do not reopen the deferred presentation scope, add assets, change hold-to-dig or introduce a minimap.

## Questions to resolve with the user

Present the concrete FOV/defaults and steady-reticle proposal with clear examples, then resolve their preferred defaults and any meaningful visibility/comfort tradeoff. Routine numerical limits follow measured readability. Do not ask whether to add head bob just to fill a settings menu.

Selected: retain today's view by default; add a narrower/wider slider and make the aiming cross steady by default, with the existing pulse available as an option. The user accepted proceeding to implementation after reviewing this recommendation. The [complete contract](../../features/backlog/camera-comfort.md) specifies labels, range, reset, persistence, navigation and errors; runtime delivery belongs to `65`.

## Research and comparison evidence

- At the Task `64` baseline, official CLI confirmed MainGame's perspective camera: **75° vertical**, 0.1 m near / 180 m far clip, no physical projection/lens shift. `FpsPlayer` applied yaw/pitch only, with sensitivity independent of FOV. The former uGUI HUD used a centered `+`, built-in LegacyRuntime font, a 1280×720 reference canvas and Input System menu navigation; Pause had no settings entry. [65](65-camera-comfort-settings.md) added settings; [74](74-ui-toolkit-menus.md)/[75](75-ui-toolkit-hud.md) subsequently migrated menus/HUD to Toolkit.
- An isolated Play Mode session loaded a copy of Task 35's validation excavation under `unity/Logs/Task64/ReviewProfile`, then used ordinary accepted terrain cuts for a lateral passage. An empty additive review scene prevented the normal EditorSave session. The user's native/Editor saves were not opened by this review; no validation geometry was saved into MainGame.
- `unity/Logs/Task64/fov-comparisons.json` and twelve CLI screenshots compare **55/75/90° vertical** in the passage/open pit at **1280×720 and 1280×800**. Center-ray direction remained aligned and hit distance identical within each pose. Narrower views enlarged distant finds; the widest view showed more nearby wall/periphery and stronger apparent stretching. Keep 75° as the default to preserve familiar scale, with 55–90° as an adjustable range; final native movement feel and future tool clipping remain `65`/`11` checks.
- One actual accepted `FpsPlayer.TryDig` produced `DigPulse=1`, crosshair scale **1.3**, gold `(1,.82,.35)`; source decays this back to normal. `reticle-sample.json` and `pause.png` confirm the existing response/menu. Recommend constant white/ordinary size for Steady crosshair On; Off preserves the old response without inventing extra per-stroke HUD feedback.
- [Unity camera documentation](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Camera-fieldOfView.html) defines vertical FOV and aspect-dependent horizontal coverage. The CLI conversion table supports preserving that convention over locking horizontal coverage or displaying an unexplained 16:9-equivalent number. A 90° **vertical** default was rejected because it makes finds substantially smaller than the current view; forcing one fixed FOV would remove the requested player choice.
- Unity's [Input System UI guidance](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/UISupport.html#distinguishing-between-ui-and-game-input) explains that UI interaction does not implicitly suppress gameplay actions. The public 1.20 UI page was unavailable; checked 1.17 guidance against the installed 1.20 module/project's explicit menu gating. Preserve one Escape owner and release barriers. [PlayerPrefs saving guidance](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerPrefs.Save.html) supports writes at paused boundaries; implementation storage remains `65`'s choice.
- This task retained the existing font resource under the repo's deferred asset scope; it did not select a future TextMeshPro migration. No camera-motion switches are presented before an actual effect is separately selected. Review ended with one clean MainGame scene, 75° FOV and original background behavior restored (`restored-editor.json`).

## Done when

- Record the accepted controls, labels/defaults, persistence/reset rules and low-motion contract in the FPS feature; `65` can implement them without inventing the product settings while coding.
- `57`, later tool/jetpack work and `54` inherit the same comfort rules. No runtime settings or new effects are claimed implemented by this design task.
