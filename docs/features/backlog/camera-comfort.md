# Camera comfort settings

Status: **implemented in [65](../../development/tasks/65-camera-comfort-settings.md), following the selected [64](../../development/tasks/64-camera-comfort-design.md) design**. [FPS controls](fps-controls.md) retains movement/input rules and [54](../../development/tasks/54-release-qualification.md) owns long-session qualification.

## Purpose and recommended defaults

Let players adjust how much of the world they see and remove the aiming cross's dig pulse, while preserving today's camera without added motion. Keep the existing view as the default so the update does not unexpectedly change familiar movement/scale. These controls provide choice; they do not guarantee comfort for every player.

| Control | Selected value and behavior |
|---|---|
| Field of view (vertical) | Slider **55–90°**, whole-degree steps, default **75°** (the current camera). Lower values enlarge objects and show less surroundings; higher values show more surroundings with smaller distant finds and stronger edge stretching. Show the current number in the shared settings row; the short visible label is Field of view. |
| Steady crosshair | **On** by default: retain the current centered `+` at its ordinary size and constant white color while digging/flying. Off restores the existing accepted-dig size/color pulse. No target marker, extra sound or stroke popup follows from this setting. |
| Reset category | Immediately restore 75° and Steady crosshair On. Reset only this settings group; retain world saves, owned upgrades, input bindings and other preferences. |

## Access and interaction

- **Settings** immediately follows Resume in Pause. Startup uses the same screen. `122` places these controls in **Accessibility**, alongside Display/Graphics/Audio/Controls; Settings initially opens Display. Developer access and Save and quit remain accessible; automatic rescue is governed by `86`; fit the production/development variants at the [supported window sizes](release-validation.md#windows-support-targets), including 960×540.
- The Accessibility page contains the two shared rows, quiet Reset category and the bottom-left Back button. The user requests short labels/values without routine descriptions (`122`). Keep the actual world visible around the panel so view changes are observable while paused; do not add a simulated preview world.
- Apply camera changes immediately while gameplay stays paused and the mouse is released. **Back/Escape keeps changes and returns to the originating startup/Pause menu, focusing Settings**. Another explicit Resume, New Game or Continue starts play. Camera changes have no separate Apply/Cancel or automatic resume; `123` immediately opens a standalone confirmation for display-mode/resolution changes. Initial Accessibility focus is FOV; reset retains focus without rebuilding the panel.
- Mouse click/drag controls the slider, Up/Down moves between controls, Left/Right adjusts FOV by 1° or selects the crosshair value, and Enter activates buttons/toggles. The UI owns those actions while open. Escape has one owner and backs out one level per press; Tab cannot open inventory from this panel. Focus loss keeps it paused with its current values and selection.
- Resume retains existing release-before-resume barriers for LMB, Space and E. Changing a setting, resetting or navigating cannot dig, collect, thrust, move, rotate the camera or consume charge behind a menu.

## Projection and feedback rules

- Store/apply **vertical** FOV. Keep it unchanged when the viewport changes aspect: wider windows reveal more horizontally, with no stretched image, camera shift or forced letterboxing. At 16:9, 55/75/90° vertical gives about 85.6/107.5/121.3° horizontal; at 16:10 it gives 79.6/101.7/116.0°. Use Unity's [vertical camera convention](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Camera-fieldOfView.html); conversion values were checked through the official CLI.
- FOV never changes reach, movement speed, collision, mouse rotation per input unit or the center aiming ray. Avoid automatic FOV changes during digging, flight, upgrades or rescue. Future tool presentation must remain readable at the selected FOV, including the range extremes; no held production tool exists yet to certify its clipping.
- Steady crosshair fixes position, size and color within the current UI scale. Normal UI scaling on window resize remains; FOV/digging/flight do not resize it. Preserve existing menu visibility rules, actual terrain response, resource readings and meaningful pickup/error feedback. With Steady crosshair Off, retain today's 1.0–1.3 scale and white-to-gold accepted-dig pulse.
- Preserve yaw/pitch look without shake, head bob or jetpack motion, or switches for effects that do not exist. [66's selected crouch](precision-movement.md) permits a smooth physical stance-height change in `67`, coupled to safe capsule clearance; it preserves FOV/reticle preferences and adds no camera-effects settings group. If other effects are separately selected later, camera shake strength must reach zero, head bob must switch off, and jetpack camera effects must be independently disableable; all default to zero/off and obey saved preferences before play. Those optional effects affect presentation only, never paid capability or input response.

## Preferences and errors

- Preferences are local device/user settings independent of excavation and apply before the first playable frame. Retain them across launch, resize, rescue, world loading and same-save Continue. Changing/resetting the camera group never rewrites a world snapshot or resets excavation.
- Missing values use defaults; finite out-of-range FOV clamps to 55–90, malformed/non-finite values use 75, and invalid crosshair values use On. Invalid settings cannot block starting/loading a game. Unknown file versions use session defaults and preserve the file until an explicit edit/reset; future format owners must provide migration before changing this policy.
- Update values in memory while dragging; flush changed preferences on tab change, Back, reset, focus loss and normal quit, without per-frame disk writes or world autosaves for camera changes. If writing fails, keep the active session values and show a concise settings-local failure with Retry; avoid routine saving popups. Unity cautions that [saving preferences can stall](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerPrefs.Save.html), so persistence belongs at these paused boundaries.

## Owners and scope

`65` owns independent preference storage and behavior; [74](menu-presentation.md) moves the screen to persistent Toolkit controls without changing those rules. `57`/`11`/`47` must honor these preferences for future presentation, and `05` preserves them during HUD migration. `54` repeats them over full runs, final tools and Continue under `63` budgets. Existing UI resources are sufficient; TMP/art imports, new motion effects and other graphics options are outside these tasks. Input rebinding and optional accessibility dig-mode changes are being handled in [task `78`](../../development/tasks/78-input-accessibility.md). These settings do not complete production HUD/art acceptance.



## Planned visual accessibility extension

[81](../../development/tasks/81-visual-accessibility-design.md) reviews brightness calibration and current-cross/fixed-dot/hidden reticle choices; [82](../../development/tasks/82-visual-accessibility.md) delivers the accepted contract. These options are not implemented by `65`'s steady-crosshair setting. Preserve existing FOV/defaults and no-motion baseline; no new effect or asset is selected by this proposal.
