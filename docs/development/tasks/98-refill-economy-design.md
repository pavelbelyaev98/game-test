# Task 98 - Select paid surface refill policy

Type: design/research. Status: `done` for the user-selected surface-service scope. [Completion](../completed/98-refill-economy-design.md).

Feature: [battery and jetpack](../../features/backlog/battery-jetpack.md). Context: [finite recovery risk](../design-risks.md), `101` trip observations and scoped `56` capacity selection. The user explicitly requests paying for fuel now, before the later full production trip. Implementation: [139](139-paid-surface-refills.md).

## Decision artifact

- Selected: instant, explicit paid refill at the existing workshop, alongside selling/upgrades. One shared resource; capacity upgrades preserve current charge. Walking through the surface area, opening a menu and hovering never bill or refill.
- Current user-selected rate under `143`: **$1 per 100 fuel, rounded up to whole amounts with a $1 minimum**. A 25/85/100-fuel top-up costs $1; 150 costs $2. The user rejects `142` fractional prices after initially confirming them. Keep `142` reliable delivery and `141` lower operating cost. Show the exact quote before purchase; whole-money UI uses only `$`. These are initial tuning values, not measured final balance.
- If full refill is unaffordable, offer at most wallet × 100 fuel. No money: disabled “Need $1. Sell finds to refuel.” Full tank: disabled “Tank full.” Never spend without fuel or accept stale/repeated offers. Existing fractional saves round their balance upward once on session restore; future checkpoints have no fractional remainder.
- Preserve the existing emergency rescue: empty fuel returns the player, loses the bag and charges up to 10 wallet credits, then fills the owned tank. Empty wallet/bag remains recoverable with no debt, free ordinary refill zone, new respawns or mandatory passive upgrade.
- Rejected: fractional prices (`143` user correction), a flat fee independent of amount/capacity, and free ordinary service. The selected whole-unit minimum intentionally makes small top-ups cost $1. Limited basic refill, portable/found charges, teleporter and changed rescue penalties remain deferred.

## Illustrative trip ledgers after `143`

| Case | Before service | Purchase | Result |
| --- | --- | --- | --- |
| Early trip | $6–$8, starter tank empty | Full refill: 100 fuel / $1 | $5–$7 remain; $6 capacity needs 1–2 such trips, $10 shovel needs 2 |
| Half tank | $8, 50/100 fuel | +50 fuel / $1 | $7 remain |
| Low money | $1, 10/150 fuel | Partial: +100 fuel / $1 | 110/150 fuel, $0; may sell first to afford more |
| Larger tank | $20, 0/400 fuel | +400 fuel / $4 | $16 remain |
| Empty wallet/bag/fuel | $0 | Existing emergency rescue | Full owned tank at surface, no negative wallet |

## Acceptance and remaining limits

The concrete policy, UI/error states and examples above guide `139`; the implementation checks exact payment, depletion recovery and save integrity. Rescue remains more expensive than routine service when the player has money, and also loses carried finds. At zero money/bag its free refill can be exploited deliberately; preserving that existing recovery is an explicit limitation, not a hidden new subsidy. `59` owns any changed consequence.

These are arithmetic examples, not measured new trip yields. `141` also halves digging consumption, so the same tank permits more excavation.

After `140`, the finite test field has 672 new-game sale credits; all current upgrades total 562 (370 shovel + 96 backpack + 96 fuel), leaving 110 before service/rescue. Maxing every track is optional. The full game's ending/loot route is unfinished, so a finishable 2–3-hour economy is **not proven**; `37` must qualify necessary purchases and operating costs after full content. `99` consumes the same service assumptions if travel work resumes. Concept section 34 now describes `139` paid normal refills.

`141` controlled MainGame evidence at the same whole-amount rounding: two trips banked 7 and 11 credits after refills; shovel level 2 left 8 credits and a full tank. Fixed patches and aimed finishing retained 20 fuel per trip but did not simulate travel; [ledger/limitations](../completed/141-early-fuel-economy.md).
