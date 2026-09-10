# Inventory

Status: Task `07` session records are verified; Task `18` reopened production inspection UI acceptance pending `08`.

Idea coverage: sections 23-24.

Design: [56 - equipment progression structure](../../development/tasks/56-progression-design.md) sets capacity milestones before `48`, with numerical trip/price tuning in implementation and `37`.

## Purpose

Carry discoveries without forcing the player to move items individually or interrupt digging with inventory management.

## Task 07 - session item records

See [numbered Task `07`](../../development/tasks/07-inventory-inspection.md) for scope, research, questions and acceptance.

## Later expansion

Implement the production slot inventory, capacity upgrades, inspection UI, and persistence while retaining the existing interaction contract.

Current inspection is a names/value list opened with the Inventory binding (Tab by default). [118](../../development/tasks/118-bottom-action-bar.md) supplies the requested visible opening cue. [119](../../development/tasks/119-backpack-inspection-release-design.md) compares richer optional inspection and returning a stored item to the world; 3D viewing and drop-from-backpack remain unselected and unimplemented. `07` remains validation of the existing inventory, not delivery of those proposed mechanics.

## Required behavior

- The HUD already displays carried count / capacity and battery without opening inventory. Preserve these facts through collection, selling, rescue, load and capacity upgrades; they inform return planning, not discovery completion.
- Slots, not weight: filling the bag cannot slow flight or require dumping items to escape. No per-item carrying or cargo-weight simulation.
- Ordinary finds consume simple capacity; protected/special finds may use separate rules. [Permanent passive discoveries (`36`)](buried-upgrades.md) grant an effect outside the bag and cannot be sold or lost through rescue.
- Initial capacity progression can start around 10, 15, 20, 30, and 40 slots, but must be balanced so early trips are not mostly travel.
- A full inventory blocks ordinary loot collection without deleting or replacing the world item; it does not block passive-upgrade rewards.
- Inventory can be inspected anywhere but sold only at the surface station.
- Sold ordinary objects disappear from that save; the excavation contains a finite generated set rather than infinite loot respawns.

## Done when

- Add, reject, inspect, remove/sell, save, and load paths preserve item identity and counts.
- Capacity changes do not corrupt existing carried items.

## Task 48 - paid inventory capacity

See [numbered Task `48`](../../development/tasks/48-inventory-upgrades.md) for scope, research, questions and acceptance.
