# Battery and jetpack economy

Baseline consumption/warnings come from `13`. The user's selected shared-capacity and paid-service increment is implemented under [46](../../development/tasks/46-battery-upgrades.md), [139](../../development/tasks/139-paid-surface-refills.md) and [138](../../development/tasks/138-compact-upgrade-workshop.md). Jetpack power/efficiency remains `47` after the remaining `56` decisions.

Idea coverage: sections 30–35 and 46. [56](../../development/tasks/56-progression-design.md) records the user's shared-capacity/no-free-fill selection; [98](../../development/tasks/98-refill-economy-design.md) owns paid-service decisions and ledgers.

## Purpose

Use one readable resource to create return pressure while letting the player choose between more digging and easier ascent.

## Required behavior

- Starting battery and slots must support meaningful excavation before the first purchase; stronger equipment extends an already satisfying loop. Compare dig/return ratios and useful work per trip at starter and paid levels.
- Only accepted digs and active thrust consume energy. [141](../../development/tasks/141-early-fuel-economy.md) sets digging to 1 fuel per accepted stroke (formerly 2); thrust remains 8 fuel/second. This applies to new and continued games without changing saved charge/capacity.
- Walking, jumping, looking, waiting, and inventory inspection do not consume energy. Space jumps on press; sustained holding engages thrust after the FPS control contract's delay.
- Surface recharge is fast and does not become a management chore.
- Normal surface service is an explicit workshop purchase: $1 per 100 fuel added, rounded up to whole amounts with a $1 minimum (`143`, user correction superseding fractional prices). Examples: 25/85/100 fuel costs $1; 150 costs $2. Offer the largest affordable refill, capped at missing capacity, with exact amount/cost before purchase; insufficient/full states disable payment. Surface arrival and menu/hover never refill or bill. No sleeping/day gate.
- [142](../../development/tasks/142-refill-correctness-and-pricing.md) commits the quoted final fuel level before payment, avoiding floating-point subtraction mismatches at fractional charge. Fully affordable service ends exactly at capacity; failed/no-op/stale offers never debit. Wallets and payments use whole amounts with `$` presentation. Legacy fractional saves round their balance upward once on session restore; subsequent checkpoints have no fraction (`143`).
- Capacity levels: 100 / 150 / 200 / 300 / 400, independent of shovel/backpack. Prices: 6 / 14 / 28 / 48 credits. Preserve absolute charge when buying capacity; persist both level and capacity. Older saves keep their actual capacity/charge, begin at level 1 and receive positive increments.
- Existing automatic emergency rescue still returns/refills with bag loss and up to 10-credit fee; zero-wallet recovery remains available. No portable charges, debt or new loot regeneration. Rescue exploitation/final finite-run budget remain `59`/`37`.
- Return feedback uses coarse safe/risky/critical language, not an exact calculated energy requirement. Task `32` removes subtitles from every reserve, critical, empty-battery, recharge and refill notice: show their titles alone, without return instructions or secondary explanations.
- [140](../../development/tasks/140-shallow-rocks-and-fuel-warning.md) makes reserve warnings persistent at bottom center: **LOW FUEL** in yellow at <=35%, **FUEL CRITICAL** in red at <=15%, **FUEL EMPTY** in red at zero. They use charge / owned capacity, remain visible near the workshop, clear after enough refill, and hide with the HUD during menus/focus pause or unlimited fuel. Keep pickup/transaction feedback above them; no flashing, sound or route guarantee.
- Jetpack upgrades progress from weak boosts to stronger, efficient, controllable sustained ascent so old shallow routes become easy. Every paid level needs a noticeable matched-route benefit, with major capability milestones; a larger battery alone cannot explain all improvement. Exact attributes/track length remain `56` decisions, not automatic new startup costs.

## Done when

- Consumption/recharge remains correct across pause, depletion, stations, and save/load.
- Warning values are tunable and do not claim an exact guaranteed return cost.

## Task 46 - paid battery capacity

See [numbered Task `46`](../../development/tasks/46-battery-upgrades.md) for scope, research, questions and acceptance.

## Task 47 - paid jetpack progression

See [numbered Task `47`](../../development/tasks/47-jetpack-upgrades.md) for scope, research, questions and acceptance.

`98` owns the selected surface policy; portable charges remain deferred; `59` retains rescue consequences and `37` final balance. A new policy must explain exact payment/use, full/empty states and finishable low-money recovery, and update the concept's free-refill rule if changed. [99](../../development/tasks/99-return-mobility-design.md) compares travel/teleporter effects on fuel and equipment value; none is selected implementation.
