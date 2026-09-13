# Task 46 - Paid battery capacity progression

Type: implementation. Status: `done` for the user's explicitly requested playable capacity increment. Prerequisites: `35` and the scoped capacity/charge decision under `56`; full roster `45`, independent shovel `25` and jetpack research do not block this request.

Feature: [battery jetpack](../../features/backlog/battery-jetpack.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Add sequential money purchases with clear current-to-next capacity/effect and durable owned levels through the existing shop/save boundary. Accepted digging and affordable active thrust still share one battery; standing/looking/jumping remain free. Use [138](138-compact-upgrade-workshop.md)'s compact shop and coordinate paid service `139`.
- Before implementation: read [progression findings](../../research/player-review-findings.md#progression-and-discovery), measure real dig/return budgets and inspect the current capacity owner. Use the accepted capacity milestones and purchase-time charge policy from [56](56-progression-design.md); actual numerical tuning and final prices follow measured play and `37`.
- Acceptance: purchase, refill, warnings, final-fuel thrust, rescue and save/relaunch preserve exact capacity/charge, with no duplicate purchase or free-energy exploit. Compare starter and purchased trips: the starter supports meaningful digging/return and the purchase increases useful time underground, with measured dig/return ratios. Deliver the build.

## Delivery evidence

[Completion](../completed/46-battery-upgrades.md). Windows build: `builds/windows/SomethingDownThere.exe`, 2026-09-13 06:00:28Z. [Validation](../../../unity/Logs/Task138/validation-summary.json): 71 checks pass, official CLI visual inspection, clean native startup and preserved user profiles. Initial price/usefulness tuning remains `37`/`103`; this delivery does not claim a balanced full game.
