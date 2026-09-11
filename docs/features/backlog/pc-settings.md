# PC settings

Status: **implemented in [122](../../development/tasks/122-common-pc-settings.md), refined by [123](../../development/tasks/123-consistent-menu-components.md) and [124](../../development/tasks/124-monochrome-control-states.md)**. [Shared menu components](../../development/ui-review/design-system.md), [camera](camera-comfort.md), [bindings](fps-controls.md).

Display → Graphics → Audio → Controls → Accessibility is the selected order. Settings opens Display from startup/Pause. The matching compact Back/Reset buttons sit together at bottom-left. Back and Escape return directly to the origin, with Settings focused. Unity DropdownField/Toggle/SliderInt controls share the same row components. Escape closes a dropdown before leaving Settings. Every category uses the same rows, value columns, type sizes and quiet Reset category action. Controls scrolls as one list, including digging mode.

All ordinary settings take effect on change, with no Apply button. Device preferences are independent of excavation saves and use atomic local storage. Flush on category/Back/reset/focus/quit boundaries; failed writes retain session behavior and offer Retry. Reset changes only the current category. Rendering overrides are runtime objects, never source-asset edits.

Display: mode, resolution, VSync, FPS cap and FPS readout. New/default preferences use the monitor's native resolution in Borderless mode, VSync Off, cap 144 and readout Off. Explicit existing choices remain. VSync follows monitor refresh and disables the soft cap, whose disabled dropdown reads right-aligned Automatic with its arrow hidden; disabling VSync restores the chosen cap. This is a pacing default, not a performance guarantee.

Changing mode/resolution immediately opens a compact **Keep display changes?** dialog with no tabs and a 15-second unscaled countdown. Revert has safe focus; Keep is its sole primary action. Escape, timeout, focus loss or leaving the page restores the previous display. Only Keep saves the actual result; returning labels use the known result rather than a stale native frame. Reset Display opens the same confirmation if its native-borderless defaults change the window.

Graphics: a plain **TBD** label only, as requested in `124`; no rendering controls or preset selector. Reset is disabled on this empty page. Internal new/default graphics remain at 150% scene resolution, MSAA 8×, full textures and forced filtering; saved values remain intact. Individual graphics controls await user review before returning.

Audio: **Master volume only**. Old mute fields are ignored and do not silence output. Additional channels await actual audio and routing.

Controls: mouse gain, independent inversion, Hold/Toggle digging and 12 bindings. Accessibility: existing vertical FOV and steady crosshair, rendered with the same rows. Broader brightness/reticle options remain `81`/`82`.
