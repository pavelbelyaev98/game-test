# Battery and jetpack economy

Task `13` recharge/warnings are complete; [queue/completion](../../development/tasks.md). Paid battery (`46`) and jetpack (`47`) tracks are planned after saving, the complete trip and expanded generation (`35`/`15`/`45`).

Idea coverage: sections 30-35, relevant parts of section 46, and tuning in section 53.

Design: [56 - equipment progression structure](../../development/tasks/56-progression-design.md) decides capacity/charge policy and jetpack track/milestones before `46`/`47`; actual numerical benefits remain tested during implementation.

## Purpose

Use one readable resource to create return pressure while letting the player choose between more digging and easier ascent.

## Task 13 - recharge and warnings

- Scope: configure `SurfaceRecharge` on the existing `MainGame` surface anchor (the validation recharge adapter remains fixture-only); add polished `ReturnWarning` and HUD safe/risky/critical feedback from tunable charge fractions (initially risky <=35%, critical <=15%). Recharge free and instantly inside the surface-only zone. Use the existing HUD/font; new art, fonts and audio remain outside this task.
- The zone checks the player's feet within its footprint and a shallow band above the surface; a head/capsule overlapping from underground cannot recharge. Show the nearby recharge title and a restrained refill confirmation. Menus/focus loss suspend recharge and warning transitions; re-entry and resume inside the zone work without needing a fresh trigger event.
- Acceptance: real terrain charges only accepted digs; thrust shares that battery; walking/waiting/menu inspection are free. Warnings cross configured bands without promising a return cost; pause/depletion/re-entry behave correctly, and the zone cannot recharge a player underground.
- Next: retain completed `12`/`14` transactions/rescue and tune the tempting-find route in `15`. Saving belongs to `35`; purchases are defined below.

## Later expansion

Extend the existing shared battery, recharge and movement through paid progression; do not rebuild working consumption or warnings.

## Required behavior

- Starting battery and slots must support meaningful excavation before the first purchase; stronger equipment extends an already satisfying loop. Compare dig/return ratios and useful work per trip at starter and paid levels.
- Only accepted digs and active thrust consume energy.
- Walking, jumping, looking, waiting, and inventory inspection do not consume energy. Space jumps on press; sustained holding engages thrust after the FPS control contract's delay.
- Surface recharge is fast and does not become a management chore.
- Recharge is free or nearly instant; there is no sleep, day/night gate, or fuel purchase per trip.
- Return feedback uses coarse safe/risky/critical language, not an exact calculated energy requirement. Task `32` removes subtitles from every reserve, critical, empty-battery, recharge and refill notice: show their titles alone, without return instructions or secondary explanations.
- Jetpack upgrades progress from weak boosts to stronger, efficient, controllable sustained ascent so old shallow routes become easy. Every paid level needs a noticeable matched-route benefit, with major capability milestones; a larger battery alone cannot explain all improvement. Exact attributes/track length remain `56` decisions, not automatic new startup costs.

## Done when

- Consumption/recharge remains correct across pause, depletion, stations, and save/load.
- Warning values are tunable and do not claim an exact guaranteed return cost.

## Task 46 - paid battery capacity

See [numbered Task `46`](../../development/tasks/46-battery-upgrades.md) for scope, research, questions and acceptance.

## Task 47 - paid jetpack progression

See [numbered Task `47`](../../development/tasks/47-jetpack-upgrades.md) for scope, research, questions and acceptance.
