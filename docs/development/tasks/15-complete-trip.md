# Task 15 - Integrate and validate one complete production trip

Type: validation. Status: `planned`. Prerequisites: 35, 25 and production acceptance in 05-14.

Feature: [core loop](../../features/backlog/core-loop.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

After `35`, `25` and production acceptance in `05`–`14`, integrate presentation, discoveries, passive feedback, shovel progression, selling, recharge, warnings and rescue:

1. Collect the first find, physically return, sell it, recharge, and buy the upgrade.
2. Re-enter the unchanged excavation; the upgrade must visibly improve digging toward the second find.
3. Verify the alternative overextension/rescue path without resetting excavation, collected state, or upgrades.

- Before implementation: read the [review findings](../../research/player-review-findings.md), inspect the already-complete transactions/recharge/rescue and record the missing integration. Research by watching a first-time trip: can the player explain their purchase, notice the find and navigate back? No new system is required by a comparator review alone.
- Ask the user only if observed first-use confusion warrants an onboarding choice; the removed routine HUD hints/subtitles remain removed. Distinguish orientation trouble from repetitive commuting before considering [HOME](../../features/backlog/return-rescue.md#conditional-home-direction-experiment) or return convenience; do not add outposts/shortcuts without a separate decision.
- Done when focused integration checks pass, the Windows build is updated, and manual review records dig/discovery/return/station time, useful downward and lateral progress, and the main feel/visual limitation. Restore the trip through `35`; expanded content/generation (`40`–`45`) and full-run pacing (`37`) remain separate.
