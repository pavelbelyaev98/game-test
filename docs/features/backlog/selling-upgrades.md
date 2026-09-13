# Selling and upgrade checkpoint

Status: Task `12` is [complete](../../development/completed/12-selling-and-shovel-upgrades.md), using the existing six-level shovel system and Task `14` session wallet. The approved Blender station pair is integrated; Task `11` visible shovel progression remains separate.

Idea coverage: sections 25-27.

Design: [56 - equipment progression structure](../../development/tasks/56-progression-design.md) decides tracks/milestones before purchases are implemented. Final numerical prices and run pacing remain `37`.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again. The player should understand what the trip banked and anticipate a useful purchase; acknowledgment must not add waiting, sorting or confirmation chores. `57`/`08` own presentation refinements to the completed stations, while `56`/`37` prove practical whole-trip value across purchase choices and voluntary excavation styles. See the [risk register](../../development/design-risks.md).

## Task 12 - production transactions

- Scope: reuse `SessionWallet` from `14`, implement a visually finished `SellStation` and separate `UpgradeStation` through `StationTarget`; sell selected inventory records and purchase one shovel level at a time through the existing six levels. Station visuals must use Blender MCP or free-to-use assets licensed for commercial use, after explicit approval.
- Acceptance: explicit Sell One/Sell All atomically remove selected IDs and credit value; explicit purchase checks affordability, charges 10 and applies level 2 once. Opening menus, Tab inspection, stale/repeated commands and insufficient funds cannot mutate items/money/levels incorrectly.
- Prices for levels 2-6: 10 / 25 / 55 / 100 / 180 credits, serialized on the player and copied into its session transaction owner. Display owned-level scoop width, reach, stroke time, price and remaining/missing credits. Developer overrides never change purchase eligibility or price.
- Sell rows bind exact item identities; all-sale offers bind the displayed bag. Changed inventory, money or owned levels require a refreshed offer. Opening selects Close; keyboard selection scrolls item rows into view. Focus loss, disabled stations, distance and occlusion prevent transactions.
- Evidence: transaction checks and station/menu integration in `MainGame`; wallet and tool level survive surface trips. Display cost, effect, affordability and result.
- Next: validate the complete trip under `15` once its remaining dependencies are met; `13`/`14` are already complete. Prices remain tunable. State persists across trips within the scene session; disk saving remains separate.

## Later expansion

Retain Task `12` transactions, separate approved stations and current-to-next shovel comparisons. `25` adds independent shovel purchases; `46`–`49` add battery/jetpack/capacity/detector tracks. Each extends this same atomic shop/save flow with concise effects, affordability and qualitative milestone benefits; no duplicate shop rebuild is needed by the [review findings](../../research/player-review-findings.md#progression-and-discovery).

## Current workshop increment (`138`, `46`, `48`, `139`)

- Keep the existing approved station pair. Workshop has one compact selectable row per real upgrade (shovel, backpack, fuel tank): name, owned level and next price/max state. Refill is a separate service row. Hover, click or keyboard focus shows one right-hand details pane; row activation never purchases.
- A single explicit purchase button applies the selected quote. Start focus on Close, retain selected row after purchase, show missing credits/max/full/partial states and one short result. No explanatory text repeated across rows and no inactive future equipment.
- [141](../../development/tasks/141-early-fuel-economy.md) lowers operating costs to 1 fuel per dig and 100 fuel per credit, so the starting tank supports twice as many digs and refills for 1 credit. Rock value and upgrade prices stay unchanged.
- Independent backpack/fuel prices start at $6; owning one is not a prerequisite for the other. Shared model transactions validate revisions, affordability and session/station identity before charging once; successful purchases request a checkpoint. Save v5 remains compatible with v1–v4 and retains capacities, charge, loot and excavation. `142` fixes fractional-fuel delivery; the user supersedes fractional prices in `143`: $1 per 100 fuel, rounded up to whole amounts with a $1 minimum. Balances/payments are whole numbers; legacy v5 fractions round upward once on restore and subsequent checkpoints write zero fraction. All player-facing money uses `$`, including HUD, inventory, sales, upgrades, service and rescue.

[145](../../development/tasks/145-depth-mineral-progression.md) adds eight repeatable mineral sale items with increasing whole-dollar values across overlapping depth bands. Its table/source catalog owns tuning; inventory, Sell One, Sell All and transaction checkpoints use the same exact per-item value. Existing saved prices remain historical. Mineral income does not add crafting, a currency or a new upgrade track.

## Required behavior

- Opening a station never sells, buys, or spends automatically.
- Underground inventory inspection cannot bank money.
- The selling location and separate upgrade location sit close to the excavation entrance.
- Prefer a fast physical selling machine with animated/noisy feedback and explicit Sell One/Sell All controls; never require dropping items individually.
- Normal finds provide reliable income. Rare sellable finds can fund several trips or a major upgrade but must not collapse the whole economy; [permanent upgrade discoveries (`36`)](buried-upgrades.md) never enter sale offers.
- Purchases show effect, cost, affordability, and a clear result. Basic input comfort is available from the start, never sold as an upgrade.
- Purchase priorities should change with route/equipment: capacity, battery, terrain resistance, mobility and detector quality each need a reason to matter. Avoid battery always being best or a forced upgrade sequence; old obstacles stay easier after purchases. `56` defines benefit hypotheses and `37` tests them.

## Done when

- Sale and purchase transactions are atomic and persist correctly.
- Invalid or repeated commands cannot duplicate money/items or skip upgrade levels.

## Task 37 - full-run upgrade pacing

See [numbered Task `37`](../../development/tasks/37-full-run-pacing.md) for scope, research, questions and acceptance.
