# Selling and upgrade checkpoint

Status: Task `12` is [complete](../../development/completed/12-selling-and-shovel-upgrades.md), using the existing six-level shovel system and Task `14` session wallet. The approved Blender station pair is integrated; Task `11` visible shovel progression remains separate.

Idea coverage: sections 25-27.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again.

## Task 12 - production transactions

- Scope: reuse `SessionWallet` from `14`, implement a visually finished `SellStation` and separate `UpgradeStation` through `StationTarget`; sell selected inventory records and purchase one shovel level at a time through the existing six levels. Station visuals must use Blender MCP or free-to-use assets licensed for commercial use, after explicit approval.
- Acceptance: explicit Sell One/Sell All atomically remove selected IDs and credit value; explicit purchase checks affordability, charges 10 and applies level 2 once. Opening menus, Tab inspection, stale/repeated commands and insufficient funds cannot mutate items/money/levels incorrectly.
- Prices for levels 2-6: 10 / 25 / 55 / 100 / 180 credits, serialized on the player and copied into its session transaction owner. Display owned-level scoop width, reach, stroke time, price and remaining/missing credits. Developer overrides never change purchase eligibility or price.
- Sell rows bind exact item identities; all-sale offers bind the displayed bag. Changed inventory, money or owned levels require a refreshed offer. Opening selects Close; keyboard selection scrolls item rows into view. Focus loss, disabled stations, distance and occlusion prevent transactions.
- Evidence: transaction checks and station/menu integration in `MainGame`; wallet and tool level survive surface trips. Display cost, effect, affordability and result.
- Next: validate the complete trip under `15` once its remaining dependencies are met; `13`/`14` are already complete. Prices remain tunable. State persists across trips within the scene session; disk saving remains separate.

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
