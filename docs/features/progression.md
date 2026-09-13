# Progression and economy

Owns currency, selling/workshop, shovel tiers, fuel/battery, jetpack, inventory and permanent buried rewards. Task IDs are plain numbers.

## Currency and shop
- All money shows as `$` with whole numbers (HUD, inventory, sales, upgrades, refills, rescue). Legacy fractional balances round up once on restore. `143`.
- Money buys upgrades; no arbitrary depth gates. Purchase priorities must vary by route: capacity, battery, resistance, mobility and detection should each matter, with no mandatory order or battery always winning. `25`, `56`, `37`.
- Basic input comfort is free from the start. `12`.
- Developer overrides never change owned progression; restore normal rules removes them without resetting terrain/inventory. `20`/`23`.

## Selling and workshop
- Sell/Upgrade stations sit near the entrance. Opening never sells or buys; inventory inspection cannot bank money.
- Sell One/Sell All atomically remove bound identities and credit exact value; stale, repeated, unaffordable, out-of-range or occluded commands mutate nothing. `12`.
- Shovel purchase is one level at a time: 10 / 25 / 55 / 100 / 180 credits (tunable). `12`.
- Workshop: one compact selectable row per real upgrade (shovel, backpack, fuel) plus a separate refill row; one details pane showing owned width/reach/stroke time, price, missing credits, max/full/partial states; activation never purchases; one explicit purchase button; keep selection; no inactive future equipment. `138`, `46`, `48`, `139`.
- Backpack and fuel tracks are independent, each starting at $6. Transactions validate revision/affordability/station before charging once; purchases checkpoint. `139`, `142`.
- Eight repeatable minerals sell for increasing whole-dollar values across overlapping depth bands (Coal $2 → Diamond $45); all sell paths use the catalog value. No crafting or extra currency. `145`.

## Shovel
- Six ordered levels, each observably bigger; equal energy per stroke; reject non-increasing radius or reach. Implemented `20`/`12`.
- Reach (m): 3.0 / 3.2 / 3.4 / 3.6 / 3.8 / 4.0. Scoop width (m): 0.692 / 0.877 / 1.063 / 1.248 / 1.434 / 1.619 — about 40% less fresh-soil volume per stroke with cadence, variation and reach unchanged. `23`, `127`.
- Interaction/collection reach is independent of dig reach.
- One recognizable evolving shovel: basic → reinforced → powered → motorized → unreasonable homemade machine with visible attachments. Major milestones change shape, control, access or scale; small tiers may stay numeric. Visible art is `11`/`57`.
- Suction and a large breaker are omitted. Automatic remnant cleanup stays; RMB lifts/drops finds and Dig throws held finds.
- User decision: excavation guns are wanted, implementation deferred. Shave is liked; `120` owns its role and `147` owns deferred guns/migration. No tier redesign or saved-progress change before a reviewed migration.

## Fuel and battery
- Accepted dig stroke = 1 fuel (formerly 2); thrust = 8 fuel/s; walking, jumping, looking and inventory are free. Applies to new and continued games. `141`.
- Refill is explicit: $1 per 100 fuel rounded up, $1 minimum (25/85/100 fuel = $1, 150 = $2); the largest affordable amount is capped at missing capacity with an exact quote before payment; failed/stale offers never debit. `139`, `142`, `143`, `98`.
- Battery levels 100 / 150 / 200 / 300 / 400 at 6 / 14 / 28 / 48 credits, independent of shovel/backpack; buying preserves absolute charge. Older saves begin level 1 with their actual capacity plus increments. `46`.
- Warnings use charge/owned capacity: LOW FUEL yellow ≤35%, FUEL CRITICAL red ≤15%, FUEL EMPTY red at zero; titles only, hidden with the HUD. `140`, `32`.
- Return advice uses coarse safe/risky/critical language, never a guaranteed cost. `93`/`94` open.
- Jetpack progression (weak boosts → efficient sustained ascent) stays open in `56`; every paid level needs a matched-route benefit a larger battery cannot explain. `47`.

## Inventory
- Slots, not weight; filling never slows flight or forces dumping. Capacity 10 / 15 / 20 / 30 / 40 at 6 / 14 / 28 / 48 credits, independent of fuel. Buying preserves every carried item; older saves keep capacity and gain increments. `48`.
- HUD always shows carried count/capacity and battery without opening inventory.
- A full bag blocks ordinary loot collection without deleting the world item but never blocks passive rewards. Inspect anywhere, sell only at the surface. Inspection is a names/value list (Tab); `118` adds the opening cue; `119` richer inspection/drop-to-world is unselected. `07`.
- The excavation holds a finite generated set; loot does not respawn.

## Permanent discoveries
- `36` (planned after `35`, `45`, `46`–`49` and a verified `52`): 2–4 very rare buried permanent equipment upgrades, granted outside the bag, unsellable, rescue-proof and never required for the ending. `62` decides objects and effects first.

## Audio
- Selected: ambient nature plus digging/action feedback. Specific sounds need approval, a free commercial license and a ledger entry; `08` owns the production pass. Common finds stay detector-silent; no music or voice is selected.

## Open
- `56` equipment structure/milestones; `37` full-run pacing and final prices; `98` paid-service policy and `99` travel; `59` rescue consequences; `62`/`36` rare rewards; `119` inspection.
