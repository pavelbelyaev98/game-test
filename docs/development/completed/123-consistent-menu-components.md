# 123 - Consistent settings and reusable menu components

Why: the user requested one reusable UI system, calmer actions, compact dialogs and aligned native settings controls.

- Integrated shared buttons, tabs, dialog heading/body and setting rows using Unity Toolkit DropdownField, Toggle and SliderInt. [Component contract](../ui-review/design-system.md).
- Five categories expose 16 settings plus 12 bindings; bottom Back, quiet Reset, immediate changes and standalone safe display confirmation. Master volume only; preset selector hidden, new/reset graphics use maximum supported values.
- Neutral Continue/Resume, no pause save timestamp, compact 480 px dialogs and 720 px shops with small right-aligned actions; one neutral primary maximum and visibly disabled purchase actions.
- Fixed initial EventSystem focus so keyboard navigation works before a mouse click; Escape closes an open dropdown before leaving Settings.
- Validation: **158 EditMode + 47 PlayMode passed** (24 UI, 8 startup, 4 station, 11 saving). CLI reviewed all categories at 960×540 and 1920×1080, scrolled bindings, Retry and modal/shop states. [Views and research](../ui-review/common-settings.md).
- Actual camera targets verified 50%/100% rendering and MSAA changes. Native Windows confirmed native Borderless, 144 FPS fallback, switches, dropdown dismissal, bottom Back and keyboard-only startup; display Keep/relaunch and rollback checks also passed in this task.
- Windows build **2026-09-10 21:32 UTC**, zero errors, one existing Pipeline runtime-configuration warning. [Executable](../../../builds/windows/SomethingDownThere.exe), [evidence](../../../unity/Logs/Task123/).
- Save/preference hashes preserved; MainGame clean/stopped. No commit.
- Next: `106` resumes broader UI audit and user review, including individual graphics choices; hardware qualification remains `54`.
