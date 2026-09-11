# Task 123 - Consistent settings and reusable menu components

Type: implementation. Status: `done`. Selected by the user's UI corrections after `122`. [Result and evidence](../completed/123-consistent-menu-components.md); `106` resumes the broader audit.

Feature: [menu presentation](../../features/backlog/menu-presentation.md), [PC settings](../../features/backlog/pc-settings.md). Design: [shared components](../ui-review/design-system.md). Existing context: [theme](../ui-review/theme-and-navigation.md), [settings](../ui-review/common-settings.md).

## Selected work

- Replace dividers and inconsistent layouts with shaded, aligned rows; one spacing/type scale shared by every category, including Accessibility and bindings. Digging mode scrolls with Controls.
- Latest selected Back placement: bottom-left text button, superseding the header chevron. All ordinary options apply immediately; each display-mode/resolution change opens a standalone confirmation, without tabs or Apply.
- Subtle category reset; one primary action at most, using the same dark neutral surface as settings. Secondary actions are quiet; disabled actions are visibly dim. No automatic focus highlight or row hover fill; a subdued keyboard-only focus cue remains.
- New-game/reset/display/save dialogs use one compact 480 px frame, one title, short body and small right-aligned actions. Shops use 720 px width and the same actions. Remove redundant branding/subtitles, decorative button colors and the pause save timestamp.
- Native-resolution Borderless on first/default launch; VSync On for monitor-paced presentation; fallback cap 144 when VSync is off. Preserve explicit existing preferences. Audio exposes Master volume only; remove hidden mute effects too.
- Verify rendering controls against actual camera output. Keep supported controls, clarify Render scale as 3D resolution, and disable a setting with Unavailable if its required renderer capability is absent; do not pretend unavailable features work.
- Use Unity Toolkit DropdownField, Toggle and SliderInt through shared row components. Switches replace checkbox squares; disabled FPS reads Automatic with no arrows. Hide graphics presets; new/reset graphics use the maximum supported values (150%, MSAA 8×, full textures, forced filtering). Individual graphics review remains with the user.

## Research

- [Unity borderless mode](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/FullScreenMode.FullScreenWindow.html): native-size desktop window and task switching; selected default.
- [Unity pacing](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-targetFrameRate.html) and [VSync](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/QualitySettings-vSyncCount.html): desktop VSync overrides soft caps and tracks display refresh. 144 is a selected fallback ceiling, not a promise or universal optimum.
- [Xbox XAG 112](https://learn.microsoft.com/en-us/xbox/accessibility/xbox-accessibility-guidelines/112): predictable, accessible pre-game settings. User reference selects compact common rows; numerical tuning is delegated.
- [Unity runtime theme](https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-tss.html): inherit the default Toolkit theme for native widget structure; shared USS overrides appearance and keyboard-only focus.
- [Unity input/focus guidance](https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-faq-event-and-input-system.html): initialize element and EventSystem panel focus before mouse interaction; Escape closes a dropdown before leaving Settings.

## Acceptance

- Inspect all categories, small/wide windows, full Controls scroll, preference errors, startup and standalone new-game/display/reset/save dialogs. No visible divider lines or competing primary actions; common row columns/sizes align.
- Verify immediate behavior, native defaults, display Keep/Revert/timeout/focus safety, all mute removal, rendering effect, Back/focus/rebinding barriers and preservation of user data.
- Run proportionate deterministic/integration checks, inspect via official CLI, rebuild and review Windows executable. Update concise records/catalog and return the queue to `106`. Do not commit.
