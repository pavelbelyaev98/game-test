# 08 - Selling and upgrade checkpoint

Status: planned; validation adapters exist without production money.

Idea coverage: sections 25-27.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again.

## First-playable task `08a`

- Scope after `09a`: implement session `SessionWallet`, `SellStation` and separate `UpgradeStation` through `StationTarget`; consume `07a` records and purchase `09a` level 2 once. Fixture values: A=10, B=20, upgrade=10 credits; start at zero.
- Acceptance: explicit Sell One/Sell All atomically remove selected IDs and credit value; explicit purchase checks affordability, charges 10 and applies level 2 once. Opening menus, Tab inspection, stale/repeated commands and insufficient funds cannot mutate items/money/levels incorrectly.
- Evidence: transaction checks and station/menu integration in FirstPlayable; wallet and tool level survive surface trips. Display cost, effect, affordability and result.
- Next: make `10a` ready. Production balancing, animated selling feedback, other upgrade categories and disk persistence remain in task `08` below.

## Full-feature implementation task `08`

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
