# Task 48 - Paid inventory capacity progression

Type: implementation. Status: `done` for the user's explicitly requested playable capacity increment. Prerequisites: saving `35` and the scoped inventory decision under `56`; jetpack progression `47` does not block this request.

Feature: [inventory](../../features/backlog/inventory.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Add a sequential money-purchased capacity track to the existing shop/save boundary and [138](138-compact-upgrade-workshop.md)'s compact rows. Retain the initial 10 slots; numerical prices and capacities are tuning against the reported 6–8 credits per trip. Preserve independent purchasing from shovel/fuel upgrades.
- Before implementation: read [progression findings](../../research/player-review-findings.md#progression-and-discovery), measure filled-bag trip/return times and inspect identity/transaction handling. Use the capacity milestones from [56](56-progression-design.md); exact values/prices are validated through play and `37`, with any structural change brought back as a concrete proposal.
- Show the current-to-next capacity and concrete benefit. No cargo weight, per-item transport, extra currency or automatic sale. Passive rewards remain outside slots under `36`.
- Acceptance: valid purchases immediately increase usable slots, survive save/relaunch and preserve every existing item; duplicate/stale/unaffordable offers cannot alter state. Full-bag finds remain collectable after upgrading, with correct scrollable inspection and no identity duplication. Compare unupgraded and upgraded trips against `15`: starter capacity must already support a satisfying expedition and later slots must give a substantial practical benefit. Review the Windows build.

## Delivery evidence

[Completion](../completed/48-inventory-upgrades.md). Windows build: `builds/windows/SomethingDownThere.exe`, 2026-09-13 06:00:28Z. [Validation](../../../unity/Logs/Task138/validation-summary.json): 71 checks pass, official CLI visual inspection, clean native startup and preserved user profiles. Initial price/usefulness tuning remains `37`/`103`; this delivery does not claim a balanced full game.
