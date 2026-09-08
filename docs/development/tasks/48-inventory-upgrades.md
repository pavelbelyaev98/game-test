# Task 48 - Paid inventory capacity progression

Type: implementation. Status: `planned`. Prerequisites: 47, 56.

Feature: [inventory](../../features/backlog/inventory.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- After `35`, `25`, `45` and battery/jetpack purchases (`46`/`47`), add a sequential money-purchased capacity track to the existing shop/save boundary. Retain the initial 10 slots; the 10/15/20/30/40 curve is a starting proposal, not a final balance mandate.
- Before implementation: read [progression findings](../../research/player-review-findings.md#progression-and-discovery), measure filled-bag trip/return times and inspect identity/transaction handling. Use the capacity milestones from [56](56-progression-design.md); exact values/prices are validated through play and `37`, with any structural change brought back as a concrete proposal.
- Show the current-to-next capacity and concrete benefit. No cargo weight, per-item transport, extra currency or automatic sale. Passive rewards remain outside slots under `36`.
- Acceptance: valid purchases immediately increase usable slots, survive save/relaunch and preserve every existing item; duplicate/stale/unaffordable offers cannot alter state. Full-bag finds remain collectable after upgrading, with correct scrollable inspection and no identity duplication. Compare unupgraded and upgraded trips against `15`: starter capacity must already support a satisfying expedition and later slots must give a substantial practical benefit. Review the Windows build.
