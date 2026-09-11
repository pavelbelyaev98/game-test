# Shared menu components

Shared components from [123](../tasks/123-consistent-menu-components.md); [125](../tasks/125-subtle-menu-hover.md) refines the monochrome hover states and dropdown spacing. Keep the selected row layout.

| Component | Shared rule |
|---|---|
| Screen frame | Settings 920 × 620 reference pixels; shops 720 px wide; dialogs 480 px wide. 28 px inset, no divider lines; 1280 × 720 UI scaling. |
| Typography | One 30 px heading; body/control text 18 px; secondary text 14–16 px; startup title 42 px. Reuse title/body/caption classes. |
| Tabs | ToolkitTabs owns selection and ordered navigation for UXML buttons; five equal columns, shared selected surface. |
| Settings rows | ToolkitSettingsRows owns labels and built-in DropdownField, Toggle, SliderInt and binding buttons. 44 px rows, 12 px horizontal padding, 4 px gaps; one 260 px field column. |
| Controls | Native dropdown lists replace arrow pickers; selected rows are shaded, with no tick icons, 8 px item padding and a uniform 4 px popup inset behind a darker gray outline. Toggles share a 46 × 24 px switch treatment. Sliders share a 64 px value, 12 px gap and 184 px track. |
| Actions | ToolkitMenuComponents creates shared Button variants. Primary uses a dark neutral surface and white text; secondary is transparent/muted at rest, with a restrained dark-gray hover fill. No stretched footer buttons; compact actions align right. At most one primary. |
| Disabled | Dim surface/text, no hover response; unavailable dropdowns hide their arrow. VSync-controlled FPS reads right-aligned Automatic. |
| Settings footer | Back and Reset category together at bottom-left: identical 160 px buttons, 12 px gap. No Apply button. |
| Dialogs | ToolkitDialog reuses the same single heading/body and action region for new game, display, terrain reset and persistence states. No tabs or redundant branding. |
| Interaction | No automatic focus highlight or whole-row hover fill. Buttons and native inputs retain subtle grayscale hover feedback; disabled controls remain unchanged on hover. Light grayscale focus outline only during keyboard navigation. EventSystem panel selection is established before pointer use. |
| Colors | White text, neutral gray secondary text and charcoal surfaces; normal, hover, pressed and focused states must retain readable contrast. Mint only for shop data; all action buttons use the same neutral styles. Existing HUD warning semantics remain. |

Settings take effect immediately. Display mode/resolution require Keep in a standalone 15-second dialog. Escape closes an open dropdown first, then returns from settings or reverts a display preview. Controls scrolls as one list.

Continue remains first and neutral. Pause omits the last-saved timestamp and uses neutral actions. Graphics is a plain TBD label with no controls; internal maximum defaults remain. New/default VSync is Off with a 144 cap. Existing explicit preferences remain. Developer admin uses the normal Pause action style and is hidden in release. Production write failures quit silently; development retains diagnostics without Open save folder.
