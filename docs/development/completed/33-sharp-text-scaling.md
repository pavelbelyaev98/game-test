# 33 - Sharp text after canvas scaling

- Why: unchanged HUD labels retained glyphs generated at the previous canvas scale, becoming sharp only after their content changed.
- Integrated result: `FpsHud` uses `HudCanvasScaler` to refresh existing labels after scale calculation and before UI rebuilding. Works during Pause and scaler enable/disable; unchanged scale causes no text invalidation or hierarchy scan.
- Evidence: the new regression failed before the fix and passed afterward across four scales with unchanged content and no idle redraws. Final PlayMode suite: 56/56 passed; two initial unrelated integration failures passed individually and in the final full run.
- Presentation: official CLI verified HUD and Pause glyphs already matched a forced fresh render after scaling; native Windows HUD/menu review passed at 960x540 and 1920x1080. Evidence: `unity/Logs/Task33/`.
- Delivery: `builds/windows/SomethingDownThere.exe` rebuilt at `2026-09-08 14:45 UTC`, zero errors; expected Pipeline-disabled-in-player warning only. No game exceptions during native review.
- Limitation: existing font styling is retained; final HUD/art acceptance remains deferred. No asset/audio additions or commit.
