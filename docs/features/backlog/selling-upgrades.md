# Selling and upgrade checkpoint

Status: Task `11` is planned; validation adapters exist without production money.

Idea coverage: sections 25-27.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again.

## Task 11 - initial transactions

- Scope after Task `10`: implement session `SessionWallet`, `SellStation` and separate `UpgradeStation` through `StationTarget`; consume inventory records and purchase shovel level 2 once. Fixture values: A=10, B=20, upgrade=10 credits; start at zero.
- Acceptance: explicit Sell One/Sell All atomically remove selected IDs and credit value; explicit purchase checks affordability, charges 10 and applies level 2 once. Opening menus, Tab inspection, stale/repeated commands and insufficient funds cannot mutate items/money/levels incorrectly.
- Evidence: transaction checks and station/menu integration in `MainGame`; wallet and tool level survive surface trips. Display cost, effect, affordability and result.
- Next: Task `12`. Production balancing, animated selling feedback, other upgrade categories and disk persistence remain later work.

## Later expansion

Implement discovery values, explicit sell-one/sell-all actions, money, and one-level-at-a-time upgrades at separate surface stations.

## Required behavior

- Opening a station never sells, buys, or spends automatically.
- Underground inventory inspection cannot bank money.
- The selling location and separate upgrade location sit close to the excavation entrance.
- Prefer a fast physical selling machine with animated/noisy feedback and explicit Sell One/Sell All controls; never require dropping items individually.
- Normal finds provide reliable income. Rare finds can fund several trips or a major upgrade but must not collapse the whole economy.
- Purchases show effect, cost, affordability, and a clear result.

## Done when

- Sale and purchase transactions are atomic and persist correctly.
- Invalid or repeated commands cannot duplicate money/items or skip upgrade levels.
