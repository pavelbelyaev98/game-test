# Task 143 - Whole currency and $ presentation

Type: implementation. Status: `done`. Explicit user-selected correction to `142`. [Completion](../completed/143-whole-currency-and-symbol.md).

Prerequisites: reliable refill delivery `142`. Features: [fuel](../../features/backlog/battery-jetpack.md), [selling/upgrades](../../features/backlog/selling-upgrades.md), [menu presentation](../../features/backlog/menu-presentation.md). Policy: [98](98-refill-economy-design.md).

## Selected behavior and scope

- The user rejects fractions and requests a minimum cost of one, using only the `$` symbol in the game instead of “credits,” “cr,” or “dollars.” This supersedes the earlier fractional-price confirmation.
- Refill price is the missing fuel divided by 100, rounded up to a whole amount: 25/85/100 fuel costs $1, 150 costs $2. Unaffordable/full/no-op offers never charge. A smaller wallet still buys its affordable fuel at 100 per $1.
- Wallet balances and transactions are whole amounts. Existing v5 fractional balances round up once on session restoration, preserving purchasing power; subsequent checkpoints contain no fractional remainder. Retain old save compatibility and the validated-target refill fix.
- Apply `$` consistently to HUD, inventory values, selling, upgrades, refills and rescue feedback using the approved font's existing glyph. Keep current gameplay tuning and layout.

## Acceptance and validation

- Cover minimum/boundary/scaled prices, partial/full/stale refills, the critical fractional-fuel regression, whole-money guards and legacy balance conversion without repeat gains after save/load.
- Update relevant UI/integration expectations and verify actual workshop purchase, balance, disabled states and save restoration. Inspect main scene UI through official CLI.
- Fast compile, focused tests, updated Windows build and native startup; preserve user profiles, remove temporary captures and update current contracts/status.

Questions: resolved by the user's explicit correction; migration rounds upward to avoid removing saved money.

Evidence: 119 focused checks pass, including whole-money guards, minimum/scaled prices, pointer purchase, legacy fractional-file migration and repeated saves. CLI inspection confirms `$` throughout the changed screens. [Validation/build](../../../unity/Logs/Task143/validation-summary.json).
