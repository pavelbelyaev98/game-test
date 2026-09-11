# 124 - Monochrome control states and release save UX

Why: native widget hover/focus styles overrode the shared palette, and the user requested a simpler settings/release presentation.

- Preserved row dimensions; scoped native Button/DropdownField/Toggle/SliderInt styling to neutral grayscale in normal, hover, pressed, focused and disabled states. No ticks or blue outlines; light keyboard focus only.
- Right-aligned Automatic; centered slider value/track/thumb; matching 160 px Back/Reset buttons together. Graphics is only TBD; new/reset VSync is Off with 144 FPS cap. Explicit saved preferences remain.
- Developer admin uses the normal Pause button and remains development-only. Open save folder is removed. Release write failures quit silently with the last completed checkpoint preserved; development retains Retry/error/exit diagnostics.
- **207 relevant tests passed:** 158 EditMode; 25 UI, 12 save, 8 startup and 4 station PlayMode. Focus/hover/pressed contrast, slider endpoints and release failure routing have regression checks.
- CLI inspected small/wide categories, scrolled controls, conditional dialogs and shops. **125 enabled text samples ≥7.69:1; checked slider contrasts ≥3.69:1**. [Resolved-color report](../../../unity/Logs/Task124/contrast-report.json), [screens](../ui-review/common-settings.md).
- Native Windows checked actual pointer hover/press, dropdowns, no-tick switches, 144 FPS, volume change/persistence, Graphics TBD and footer navigation. All 24 save/preference file hashes preserved. [Native evidence](../../../unity/Logs/Task124/native-final-results.json).
- Development Windows build **2026-09-10 21:55 UTC**, zero errors, one existing Pipeline runtime-configuration warning. [Executable](../../../builds/windows/SomethingDownThere.exe). MainGame clean/stopped; no commit.
- Next: `106` resumes broader UI review. Individual graphics controls remain deferred; release failure routing was integration-tested with intercepted exit, not a deliberately failed native production save.
