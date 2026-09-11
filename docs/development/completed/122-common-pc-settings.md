# 122 - Common PC settings and concise menus

Why: the user requested more common PC options, useful categories and much shorter menu copy.

- Integrated 19 settings plus 12 bindings across Display, Graphics, Audio, Controls and Accessibility in MainGame; retained the white/charcoal theme and mint shops.
- Added functional frame pacing, display confirmation/rollback, runtime rendering choices, master/mute and mouse controls; category resets and atomic device preferences stay independent of excavation saves.
- Removed routine menu descriptions and slogans; retained short loss/error/conflict prompts. [Research, values and captures](../ui-review/common-settings.md).
- Validation: **156 EditMode + 44 PlayMode passed** (22 UI, 7 startup, 15 crouch), including delayed native-window rollback regression. [Results](../../../unity/Logs/Task122/).
- CLI inspected all five categories at 960×540 and 1920×1080, display confirmation, long bindings and failed-write Retry. Native Windows confirmed 30 FPS, display mode changes, Keep, Escape/timeout rollback and Back. Restored labels match the reverted window.
- Windows build **2026-09-10 20:03 UTC**, zero errors; one existing Pipeline runtime-configuration warning. [Executable](../../../builds/windows/SomethingDownThere.exe).
- Existing save/preference hashes unchanged after native review; MainGame clean/stopped. No commit.
- Remaining: `106` owns full catalog verdicts and broader UI coverage; separate audio channels await actual audio/routing, brightness/reticle choices remain `81`/`82`.
