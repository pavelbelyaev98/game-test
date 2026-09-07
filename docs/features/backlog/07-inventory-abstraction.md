# 07 - Inventory

Status: planned; a session-only foundation exists.

Idea coverage: sections 23-24.

## Purpose

Carry discoveries without forcing the player to move items individually or interrupt digging with inventory management.

## First-playable task `07a`

- Scope after `03a`: extend `SessionInventory` from names to records with stable instance ID, display name and ordinary sale value; retain capacity 10 and adapt HUD/validation callers.
- Acceptance: same-name finds retain distinct IDs; duplicate IDs and full-capacity additions fail without mutation; inspect and remove preserve identity/counts. Tab remains inspection-only and existing FPS checks pass.
- Next: make `06a` ready. Capacity upgrades, protected-item rules and disk persistence remain in task `07` below.

## Full-feature implementation task `07`

Implement the production slot inventory, capacity upgrades, inspection UI, and persistence while retaining the existing interaction contract.

## Required behavior

- Ordinary finds consume simple capacity; protected/special finds may use separate rules.
- Initial capacity progression can start around 10, 15, 20, 30, and 40 slots, but must be balanced so early trips are not mostly travel.
- A full inventory blocks collection without deleting or replacing the world item.
- Inventory can be inspected anywhere but sold only at the surface station.
- Sold ordinary objects disappear from that save; the excavation contains a finite generated set rather than infinite loot respawns.

## Done when

- Add, reject, inspect, remove/sell, save, and load paths preserve item identity and counts.
- Capacity changes do not corrupt existing carried items.
