# 78 — Input accessibility

- Why: make long digging sessions accessible before final presentation acceptance.
- Integrated: startup Settings and Pause Controls rebind all ten keyboard/mouse actions; optional Toggle preserves ordinary digging/collection rules. Capture waits for release, blocks input leakage, supports explicit conflict swaps/cancel and retains fixed menu navigation.
- Preferences: independent versioned input file, complete defaults for malformed maps, session-preserving failure/retry and input-only reset. Menu/focus/rescue/load/New Game changes clear digging intent; active bindings drive Pause and station labels.
- Evidence: **120/120 EditMode**, **106/106 full PlayMode**, plus the added Pause-on-LMB/Enter regression. Includes remapped collection/full bag, zero fuel/rescue, restore, capture conflicts/cancel, failed storage and startup New Game preservation.
- Official CLI: production Controls/capture/conflict/Pause inspection. Native Windows: keyboard navigation, swapping, Q toggle binding, scrolling, reset and retained preferences after relaunch at 960×540 and 1280×800. Initial hidden-window focus required a click before keyboard review; subsequent keyboard checks passed. User save hashes unchanged; original preferences restored.
- Build: `builds/windows/SomethingDownThere.exe`, **2026-09-09 15:52 UTC**, zero errors. Results, build report and screenshots: `unity/Logs/Task78/`.
- Limitation: sustained comfort/final art acceptance remains `05`/`54`; controller support remains uncommitted.
