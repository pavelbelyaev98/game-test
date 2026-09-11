# Editing the menus and HUD

All screen menus and the gameplay HUD use **UI Toolkit**. The uGUI package remains for the existing menu EventSystem, with no Canvas presentation. [Selected approach](../features/backlog/menu-presentation.md).

## Sources

All screen UI sources are under `unity/Assets/Runtime/UI/Toolkit/`:

| Source | Owns |
|---|---|
| `Resources/GameMenus/GameMenus.uxml` | Shared frame, settings tabs, Pause reference, fixed trade summary, scrolling content and footer slots |
| `Resources/GameMenus/GameHud.uxml` / `Hud.uss` | Gameplay resource panel, quiet readings, reticle, target/feedback and marker layout |
| `Resources/GameMenus/Theme.uss` | Colors, typography, spacing, sections and responsive layout |
| `Resources/GameMenus/Controls.uss` | Shared button, native dropdown, switch, slider and scrollbar styling |
| `Resources/GameMenus/MenuTheme.tss` | Unity default widget structure, shared theme imports and full-screen document sizing |
| `Resources/GameMenus/MenuPanel.asset` | Panel scale/sorting and Unity's required text data references; edit through the Editor/official CLI |
| `GameUiDocument.cs` | Shared panel/document/font lifetime and HUD/menu tree composition |
| `GameHudView.cs` | Cached HUD readings, resource bands, optional FPS readout and camera-projected markers |
| `GameMenuView.cs` | Screen state, dynamic inventory/offers/errors, existing commands and navigation |
| `ToolkitMenuComponents.cs` | Shared button factory, tab selection/navigation and dialog heading/body; dimensions/variants in USS |
| `ToolkitCameraSettings.cs` | Persistent camera controls and change/error callbacks |
| `ToolkitInputSettings.cs` | Mouse options, binding capture, hold/toggle digging and concise conditional notices |
| `ToolkitDeviceSettings.cs` / `ToolkitSettingsRows.cs` | Display/graphics/audio categories and shared rows, toggles, sliders, choices and bindings across every settings page |

UI Builder opens `GameMenus.uxml` or `GameHud.uxml` for visual layout/style editing. Toggle the relevant page's `hidden` class for authoring, then restore the saved visibility classes. Dynamic inventory and save data appear during play. Follow the [component rules](ui-review/design-system.md). Edit shared USS classes rather than copying inline styling into each control; preserve element names used by C# bindings. Restart Play after source edits: Editor live reload can replace the document tree and invalidate runtime callback references.

## Runtime rules

[106](tasks/106-ui-ux-audit-design.md) owns the [screen/state/text review and catalog](ui-review/index.md); this source map is not that inventory. [107](tasks/107-ui-ux-cleanup.md) delivers the requested white theme, mint shops and settings hierarchy. The audit also follows text/conditions outside this folder, including `FpsPlayer`, input models, stations, content names and persistence. Future visible text, conditional actions and navigation changes update the catalog alongside code.

- The game clones the authored panel at a 1280×720 reference scale and loads its UXML resources. Keeping the panel configuration as an asset makes Unity include its required text segmentation data in builds. TextCore reuses the existing Unity built-in font across both trees. HUD controls remain cached and non-pickable; only actual values, visibility, reticle pulse and projected marker positions update.
- The Input System's existing arrows/Enter/mouse bindings drive menus. `FpsHud` establishes EventSystem panel selection before pointer use. Escape belongs to `FpsPlayer` and closes an open dropdown before leaving Settings; Space never submits a menu command. Initial focus is unpainted; keyboard navigation reveals a subdued cue. Check mouse and keyboard after layout changes.
- Keep repeated rows in the content ScrollView and required actions in the footer. Keep camera controls alive across value changes; rebuild dynamic offers only when state changes. Never add save writes to an Update loop or style callback.
- Station actions capture the displayed offer revision. Bind future controls to the owning gameplay system rather than changing wallet, inventory or world state in the view.
- Future settings/front-end screens should reuse these sources once their behavior is implemented. New art, fonts, icons and sounds still require the specific approval described in `AGENTS.md`.
- Inspect the actual game through the official Unity CLI and a Windows build at 960×540, 1920×1080 and 16:10. UI Builder previews alone do not validate runtime focus, font rendering or error/long-list layouts.
