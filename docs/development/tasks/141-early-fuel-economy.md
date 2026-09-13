# Task 141 - Make early trips fund upgrades

Type: implementation. Status: `done`. Explicit user-selected fuel/economy tuning after `140`.

Prerequisites: paid service `139`, equipment `46`/`48`, current rock field `140`. Features: [battery/fuel](../../features/backlog/battery-jetpack.md), [selling/upgrades](../../features/backlog/selling-upgrades.md). Context: [98 refill policy](98-refill-economy-design.md), [101 user trip feedback](101-collection-return-playtest.md).

## Selected scope and rationale

- The user reports spending trips on fuel without reaching upgrades and authorizes higher rock payouts or longer-lasting starting fuel. Take the fuel route: halve accepted digging cost from 2 to 1 fuel and change service from 50 to 100 fuel per credit, rounded up. A starter tank supports 100 instead of 50 digs before flight; an empty refill costs 1 instead of 2 credits.
- This improves Continue immediately without changing saved charge, capacity, inventory values or equipment. Keep jetpack consumption at 8/second, current upgrade prices and rock value at 2. Preserve paid service, partial offers, warnings and rescue.
- Shop detail text derives its unit rate from the same balance constant as transactions. No new fuel system, art, equipment or save migration.
- Numerical tuning stays here; broader return consequences, full-run economy and user feel verdicts remain `59`/`37`/`101`.

## Acceptance

- Actual MainGame terrain digging supports 80 starter strokes with 20 fuel retained, versus the previous 40; first paid capacity supports 130 with the same reserve. Digging feel/geometry/cadence stays intact.
- Real shallow digging and collection followed by sale/refill leaves positive upgrade savings. Record a concrete trip ledger and demonstrate buying an upgrade while retaining refill money.
- Refill quotes charge 1 for an empty starter tank, 4 for 400 fuel, and round fractional amounts correctly. Partial/full/empty/stale/repeated quotes remain safe; UI shows the new rate and affordable amount.
- Verify Continue preserves exact state while using the new rates; shared thrust, warning and emergency rescue checks pass. Fast compile, proportionate integration checks, official CLI inspection and Windows build; preserve user profiles.

Questions: none blocking. Both levers tune the user-selected fuel alternative; human retest still determines whether the loop feels rewarding.

[Completion and concrete trip ledger](../completed/141-early-fuel-economy.md).
