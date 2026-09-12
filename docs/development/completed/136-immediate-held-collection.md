# Task 136 - Immediate held collection

- Why: held Dig appeared to wait for the shovel before collecting an uncovered target; the old eligible-observation timer imposed the extra delay.
- Result: held/toggle and fresh input share immediate eligible aimed collection, before the shovel cooldown. Obsolete recognition state/tuning is removed. Exposure, reach, obstruction, inventory, menu/focus, handling, short pickup recovery and post-pickup dig protection remain.
- Evidence: fast compile and 30 relevant physics/collection/input cases pass, including corrected bottle/rock cooldown regressions and weak/strong held/remapped-toggle flows. Official CLI MainGame inspection collects a fully uncovered bottle with 0.455 s of dig cooldown remaining, exactly one inventory identity and no extra stroke/fuel; the visual clears after its existing quick pull. [Validation scope](../../../unity/Logs/Task136/validation-summary.json).
- Delivery: [Windows executable](../../../builds/windows/SomethingDownThere.exe), **2026-09-12 20:29 UTC**, zero errors / one existing Pipeline warning; clean seven-second native startup. Existing saves are untouched; temporary captures cleaned; no commit.
- Next: user retest of pickup feel in 101; [137](../tasks/137-reservoir-first-section-review.md) awaits the first reservoir-section verdict. Its staged artwork is excluded from this build.
