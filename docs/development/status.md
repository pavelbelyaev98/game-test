# Current status

- Active: none. Next ready: [66 — Precision movement design](tasks/66-precision-movement-design.md), prerequisite `65` complete. Current milestone: camera comfort and shared Toolkit menus/HUD implemented; precision movement next.
- Latest gameplay: [75 — UI Toolkit HUD](completed/75-ui-toolkit-hud.md). All HUD/menu presentation now shares Toolkit; the remaining uGUI EventSystem supplies menu input only. [UI authoring](ui-authoring.md).
- Windows build: [SomethingDownThere.exe](../../builds/windows/SomethingDownThere.exe), built `2026-09-09 07:54 UTC`.
- Evidence: **69/69 PlayMode checks passed** in 56.28 s; official CLI inspected MainGame HUD/menu/scaling/critical reserve/X-ray at 960×540, 1080p and 16:10 with zero runtime Canvases (`unity/Logs/Task75/`). MainGame restored; review fixture had no save session. Native isolation was not validated.
- Workflow: the user [cancelled Sandbox task 76](completed/76-isolated-windows-review.md); its tooling and setup guide are removed. Follow the [validation policy](../scope-and-validation.md#validation-policy): simulated-input regression tests and brief announced Windows checks, retrying observed input/focus interruptions. No Windows repair prerequisite or blocker for `66` remains.
- Release reminder: [63's internal Windows/performance targets](../features/backlog/release-validation.md#windows-support-targets) guide implementation. Advertised hardware/full-run qualification waits for the finished game in [54](tasks/54-release-qualification.md); edit spikes remain `25`, Steam `53`.
- Remaining presentation: final world art and sustained HUD/comfort acceptance remain `08`/`05`/`57`/`54`; future equipment must honor existing camera preferences.
