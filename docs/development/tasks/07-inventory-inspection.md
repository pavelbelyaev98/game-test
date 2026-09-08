# Task 07 - Finish production inventory inspection acceptance

Type: validation. Status: `planned`. Prerequisites: 08.

Feature: [inventory](../../features/backlog/inventory.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Verified implementation: `SessionInventory` records have stable instance ID, display name and ordinary sale value, with capacity 10 and matching HUD/validation callers. Retain this working state; the remaining acceptance is production inspection UI.
- Acceptance: same-name finds retain distinct IDs; duplicate IDs and full-capacity additions fail without mutation; inspect and remove preserve identity/counts. Tab remains inspection-only and existing FPS checks pass.
- Production acceptance after `08`: inspect a full-capacity, scrollable TextMeshPro UI using approved presentation assets at supported review-window sizes; preserve the existing identity and input checks. This closes the inspection slice without claiming discovery/economy/persistence is complete.
- Before production acceptance: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect actual full-bag scrolling/legibility and preserve the quiet HUD. No new interaction question is required; `08` supplies approved presentation. Capacity upgrades belong to `48`, saving to `35` and passive exceptions to `36`.
