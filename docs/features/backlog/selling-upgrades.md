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

Retain Task `12` transactions and separate stations. [Task `25`](shovel-progression.md#task-25---independent-speed-and-strength-upgrades-future) later presents digging speed and strength as independent purchases; Task `37` owns full-run economy tuning across the intended upgrade categories.

## Required behavior

- Opening a station never sells, buys, or spends automatically.
- Underground inventory inspection cannot bank money.
- The selling location and separate upgrade location sit close to the excavation entrance.
- Prefer a fast physical selling machine with animated/noisy feedback and explicit Sell One/Sell All controls; never require dropping items individually.
- Normal finds provide reliable income. Rare sellable finds can fund several trips or a major upgrade but must not collapse the whole economy; [permanent upgrade discoveries (`36`)](buried-upgrades.md) never enter sale offers.
- Purchases show effect, cost, affordability, and a clear result.

## Done when

- Sale and purchase transactions are atomic and persist correctly.
- Invalid or repeated commands cannot duplicate money/items or skip upgrade levels.

## Task 37 - full-run upgrade pacing

- Gap: `12` validates transactions and five combined shovel prices; neither those prices nor the current 96 development finds establish a balanced 2–3 hour game. Keep `12` done. This pass follows `15`, independent tracks in `25`, and the intended purchasable systems/content and playable ending being available.
- Tune income, prices and useful upgrade choices across the short run so players can still afford and benefit from purchases late in the game. Do not require all tracks to be maxed for the ending or postpone every meaningful purchase until after it. Preserve money-based sequential purchases and avoid artificial depth/time gates or grind.
- Major tool upgrades must make the same previously difficult terrain genuinely easy, with useful access in downward and sideways branches. Do not immediately offset bought power with proportional terrain resistance; late purchases need enough remaining excavation to demonstrate their benefit.
- Acceptance: record representative complete runs with differing purchase priorities and layouts, including no passive rewards and an early lucky reward. Note time to first ending, income/spend and meaningful early/middle/late purchases. Verify that players do not exhaust all useful upgrades early, miss the 2–3 hour target through forced grinding, or need a rare reward to finish.
- Confirm remaining upgrades can still be bought and used in Continue Playing. Exact prices, track lengths and late-purchase timings remain open until these runs.
