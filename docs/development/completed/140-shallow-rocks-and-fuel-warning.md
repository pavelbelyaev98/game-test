# 140 - More shallow rocks and bottom-center fuel warning

- Why: the user wants slightly more collectible rocks near the top and a prominent yellow-to-red low-fuel warning.
- Result: New Game has 264 shallow rocks (+24, 10%) and 72 deeper rocks. Keep the 0.65–1.1 m shallow band, approved appearances and 2-credit value. Preserve preferred spacing, with a bounded 0.10 m soil-gap fallback for crowded placements.
- HUD: bottom-center LOW FUEL in yellow at 35%, FUEL CRITICAL in red at 15%, FUEL EMPTY at zero; percentages use owned capacity. Warning persists near the workshop, clears after refill and hides with menus/unlimited fuel. Pickup feedback remains above it.
- Evidence: fast compile, 119 relevant tests pass, 100-seed placement/coverage checks and four fixed small-patch excavation checks. Collection fixtures also verify the existing immediate aimed finishing pickup and bounded waits across resume. Saves retain population, appearance, pose and collected identity.
- Official CLI inspection at 960×540, 1920×1080 and 1600×1000 confirms color, placement, readable text and feedback separation; inspected upgraded capacity, menu suppression and paid refill.
- [Windows build](../../../builds/windows/SomethingDownThere.exe): 2026-09-13 06:46:18Z, zero errors, 1 existing disabled-Pipeline warning; clean seven-second native startup. All 39 user save/preference files preserved; temporary captures removed. [Validation](../../../unity/Logs/Task140/validation-summary.json).
- Limitation: extra rocks require New Game; Continue preserves the saved population. Human encounter pacing remains with `101`; return-effort semantics remain `93`/`94`.
