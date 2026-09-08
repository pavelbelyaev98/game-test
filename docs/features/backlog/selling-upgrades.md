# Selling and upgrade checkpoint

Status: Task `12` is planned; validation adapters exist without production money.

Idea coverage: sections 25-27.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again.

## Task 12 - production transactions

- Scope after Task `11`: implement session `SessionWallet`, a visually finished `SellStation`, and a separate `UpgradeStation` through `StationTarget`; consume inventory records and purchase the next shovel level. Station visuals must use Blender MCP or free-to-use assets licensed for commercial use.
- Acceptance: explicit Sell One/Sell All atomically remove selected IDs and credit value; explicit purchase checks affordability, charges 10 and applies level 2 once. Opening menus, Tab inspection, stale/repeated commands and insufficient funds cannot mutate items/money/levels incorrectly.
- Evidence: transaction checks and station/menu integration in `MainGame`; wallet and tool level survive surface trips. Display cost, effect, affordability and result.
- Next: Task `13`. Values remain tunable, but the transaction flow, feedback, and player-facing stations are production-quality.

## Later expansion

Implement discovery values, explicit sell-one/sell-all actions, money, and one-level-at-a-time upgrades at separate surface stations. [Task `25`](shovel-progression.md#task-25---independent-speed-and-strength-upgrades-future) must later present digging speed and strength as two independent purchases; this is a future TODO.

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
