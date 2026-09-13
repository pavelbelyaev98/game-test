# Task 142 - Reliable refills and amount-based pricing

Type: implementation. Status: `done`. Explicit user-selected refill bug and pricing correction. [Completion](../completed/142-refill-correctness-and-pricing.md).

Prerequisites: service `139`, economy tuning `141`. Features: [battery/fuel](../../features/backlog/battery-jetpack.md), [selling/upgrades](../../features/backlog/selling-upgrades.md). Policy context: [98](98-refill-economy-design.md); user payoff feedback [101](101-collection-return-playtest.md).

The user supersedes fractional currency/prices in [143](143-whole-currency-and-symbol.md). Keep the reliable-delivery fix and historical validation below.

## Report and reproduction

- The user reports critical fuel, 10 credits, and a refill that removes money without adding fuel. They also request higher payment for larger amounts of missing fuel.
- Reproduced in the actual Unity runtime: 46,213 of 100,000 fractional critical-charge cases across current tank sizes reported success and charged without filling. At 13.00586/100, the quote adds 86.99414; recomputing the remaining space rejects that rounded amount. The caller ignores the failure.

## Scope and acceptance

- Commit a validated target fuel level; fully affordable refills finish exactly at capacity, including fractional jetpack residues. Never charge for a rejected/no-op refill. Keep full, partial, unaffordable, changed and replayed offers safe.
- User-confirmed: retain 1 credit per 100 fuel, rounded up to hundredths of a credit. Thus 25/85/150 fuel costs 0.25/0.85/1.50 credits, and a full starter tank still costs 1. Show the exact amount and price before purchase.
- Store money as integer hundredths; keep whole-credit loot/upgrades and clamp rescue to the exact remaining balance. Save v5 adds the fractional remainder; v1–v4 saves retain their full purchasing power, capacity, charge and world. Validate the frozen v4 writer fixture and fractional checkpoint/reload.
- Reproduce the user's critical-fuel/10-credit case through the actual workshop, verify HUD and purchase feedback, resume, save and reload. Include starter/upgraded tanks, fractional consumption and exact quote/payment agreement.
- Run fast compile and focused transactions, station, fuel/save integration checks; inspect via official CLI and supply a Windows build. Preserve user profiles and remove temporary captures.

Questions: resolved. The user explicitly confirms proportional fractional credits.

Evidence: 109 relevant checks pass; critical-charge probe now has zero failures in 100,000 cases. CLI workshop purchase fills 13.00586/100 to 100 for 0.87 credits and clears the warning. Save v5 and frozen v4 compatibility pass. [Validation/build](../../../unity/Logs/Task142/validation-summary.json).
