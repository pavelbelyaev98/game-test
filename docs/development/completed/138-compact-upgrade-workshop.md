# 138 - Compact upgrade workshop

- Why: the user finds the shop overexplained and requests one row per upgrade with hover details on the right.
- Result: Shovel, Backpack and Fuel tank rows show owned level and next price; a separate refill service uses the same explicit purchase. Hover, click selection and keyboard focus update a single details pane. Initial focus is Close; purchases retain selection and show concise results.
- Evidence: real MainGame station/UI tests for hover without payment, selection, affordability, duplicate/stale actions and keyboard flow. Official CLI composited views inspected at 960×540, 1920×1080 and 1600×1000, including partial/full/maxed states; rows/actions fit. Temporary captures removed.
- [Windows build and 71 checks](../../../unity/Logs/Task138/validation-summary.json); [task](../tasks/138-compact-upgrade-workshop.md). Build succeeded with zero errors and only the existing disabled-Pipeline runtime warning; clean seven-second native startup, user profiles unchanged.
- Limitation: user layout/price verdict is pending; `106` retains the broader UI audit and `118` the backpack action cue.
