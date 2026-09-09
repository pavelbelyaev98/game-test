# Task 75 — UI Toolkit HUD

- Why: the user selected migration now to share UI authoring and remove the remaining Canvas presentation.
- Integrated result: `GameUiDocument` owns one panel for `GameHudView` and existing menus. UXML/USS retain the centered steady/pulsing crosshair, battery/reserve/recharge warnings, finds/credits, targeting, feedback, equipment readings and projected developer markers. Obsolete Canvas/scaler code is removed; the existing EventSystem remains menu input infrastructure.
- Evidence: **69/69 PlayMode checks passed** in 56.28 s; official CLI inspected MainGame through an additive fixture with no save session, zero runtime Canvases and no USS warnings. Reviewed 960×540, 1080p, 16:10, critical reserve, X-ray and Pause (`unity/Logs/Task75/`).
- Delivery: Windows build succeeded `2026-09-09 07:54 UTC`, [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe).
- Limitation: this delivery has CLI/PlayMode evidence and a rebuilt player; no native-isolation pass is claimed. The user cancelled that setup in `76`. Final world art and sustained HUD/comfort acceptance remain `08`/`05`/`54`, using ordinary announced Windows reviews.
