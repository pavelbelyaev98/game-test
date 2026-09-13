# Task 139 - Paid surface fuel refills

Type: implementation. Current rate is owned by [143](143-whole-currency-and-symbol.md): $1 per 100 fuel, rounded up to whole amounts with a $1 minimum, retaining `142` corrected fractional-charge delivery. Status: `done`. Prerequisites: selected service policy in [98](98-refill-economy-design.md), existing stations/save transactions and coordinated fuel capacity `46`.

Feature: [battery/fuel economy](../../features/backlog/battery-jetpack.md). Delivery UI: [138](138-compact-upgrade-workshop.md). The user explicitly requests paying for fuel after reporting 6–8 credits per trip.

## Scope and acceptance

- Replace automatic free zone refill with a fast explicit service at an existing surface station; retain shared digging/jetpack fuel and existing station artwork.
- Implement the selected price/amount, partial-fill, full-tank and insufficient-money rules from `98`; show a concrete offer before payment. Atomic payment and exact charge delivery must survive checkpoint/reload.
- No payment on hover, menu opening, walking through the old refill zone or replaying stale commands. Capacity purchases cannot silently refill the tank.
- Preserve existing rescue/loss rules unless separately selected; verify zero-money/empty-fuel recovery and avoid a new soft lock with finite loot. No portable packs, debt or second fuel resource.
- Validate starter and upgraded tanks, partial/full/unaffordable/stale service, pause/focus/input barriers, station distance, save/rescue and the completed Windows loop. Update concept section 34 and owning contracts when the selected replacement is integrated.

Questions: `98` owns price structure and recovery policy; numerical prices remain implementation tuning against the reported trip yield.

## Delivery evidence

[Completion](../completed/139-paid-surface-refills.md). Windows build: `builds/windows/SomethingDownThere.exe`, 2026-09-13 06:00:28Z. [Validation](../../../unity/Logs/Task138/validation-summary.json): 71 checks pass, official CLI visual inspection, clean native startup and preserved user profiles. Initial price/usefulness tuning remains `37`/`103`; this delivery does not claim a balanced full game.
