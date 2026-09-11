# Task 122 - Common PC settings and concise menus

Type: implementation. Status: `done`. Prerequisites: completed `107` theme, `78` bindings and `65` camera preferences. Explicitly selected by the user's follow-up request; `106` remains the broader audit owner.

Feature: [PC settings](../../features/backlog/pc-settings.md). Context: [theme research](../ui-review/theme-and-navigation.md), [menu presentation](../../features/backlog/menu-presentation.md), [camera comfort](../../features/backlog/camera-comfort.md), [input](../../features/backlog/fps-controls.md).

## Selected scope

- Five categories: Display, Graphics, Audio, Controls, Accessibility. Add real FPS cap/VSync, window mode/resolution, FPS readout, rendering controls, master/mute controls and mouse sensitivity/inversion. Retain bindings, Hold/Toggle, FOV and steady crosshair.
- Short labels and values; remove routine descriptions, slogan subtitles and redundant instructions. Retain concise failure, binding conflict and destructive-loss information.
- Persist device settings separately from world saves. Preserve existing camera/input files and defaults. Clamp/validate loaded values; unknown/corrupt formats stay on disk until explicit edit/reset. Retry failed writes.
- Stage mode/resolution; Apply previews with a 15-second Keep/Revert confirmation. Revert on timeout, Escape, focus loss or leaving Settings; persist only confirmed display values. Category resets preserve unrelated groups and world state.
- Implement rendering choices on a runtime clone of the existing URP asset; never mutate source art/pipeline assets. No empty effect/channel/language controls or new assets.

## Research and rationale

- [Unity frame pacing](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Application-targetFrameRate.html): VSync overrides targetFrameRate on desktop. Disable FPS-cap editing while synchronized and show “VSync” in its value.
- [Unity display API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Screen.SetResolution.html): display changes complete after the frame; exclusive fullscreen is Windows-only. Read supported modes and confirm actual applied resolution.
- [URP asset API](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@17.0/api/UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset.html): renderScale and MSAA are pipeline settings. Current scene has no shadows/effects/audio mix groups; expose relevant rendering and global listener controls only.
- [Xbox XAG 112](https://learn.microsoft.com/en-us/xbox/accessibility/xbox-accessibility-guidelines/112): accessible settings before gameplay, consistent navigation and grouped options. [God of War PC features](https://support.sms.playstation.com/hc/en-us/articles/27013720221069-God-of-War-Ragnar%C3%B6k-PC-Features) illustrates resolution/framerate choice; it does not select upscalers for this game.
- Detailed values/defaults are implementation tuning. Brightness/reticle variants remain with `81`/`82`; no new product question is required for this bounded request.

## Acceptance

- All offered values change their actual runtime owner; verify FPS/VSync, rendering, listener mute/volume, mouse direction/gain and display Keep/Revert (including timeout/focus loss) with deterministic and integration checks.
- Settings available from startup/Pause; direct Back, reset/retry, tab switching, keyboard/mouse and capture barriers remain sound. Browsing creates no world save or preference write.
- Inspect MainGame with the official CLI at supported small/wide sizes and native Windows display modes. Rebuild `builds/windows/SomethingDownThere.exe`; deliver concise research/capture links and remaining limitations. Do not commit.

Completed: [integrated result and validation](../completed/122-common-pc-settings.md).
