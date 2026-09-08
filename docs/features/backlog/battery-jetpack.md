# Battery and jetpack economy

Task `13` supplies surface recharge and reserve warnings; [queue/completion](../../development/tasks.md). It uses the shared battery and existing surface anchor independently of transactions.

Idea coverage: sections 30-35, relevant parts of section 46, and tuning in section 53.

## Purpose

Use one readable resource to create return pressure while letting the player choose between more digging and easier ascent.

## Task 13 - recharge and warnings

- Scope: configure `SurfaceRecharge` on the existing `MainGame` surface anchor (the validation recharge adapter remains fixture-only); add polished `ReturnWarning` and HUD safe/risky/critical feedback from tunable charge fractions (initially risky <=35%, critical <=15%). Recharge free and instantly inside the surface-only zone. Use the existing HUD/font; new art/audio and the deferred TextMeshPro migration remain outside this task.
- The zone checks the player's feet within its footprint and a shallow band above the surface; a head/capsule overlapping from underground cannot recharge. Show the nearby recharge title and a restrained refill confirmation. Menus/focus loss suspend recharge and warning transitions; re-entry and resume inside the zone work without needing a fresh trigger event.
- Acceptance: real terrain charges only accepted digs; thrust shares that battery; walking/waiting/menu inspection are free. Warnings cross configured bands without promising a return cost; pause/depletion/re-entry behave correctly, and the zone cannot recharge a player underground.
- Next: Task `14` introduces the rescue wallet ahead of transactions in `12`. Tune the tempting-find route in Task `15`; battery/jetpack upgrades and disk persistence remain later work.

## Later expansion

Connect the existing shared battery to production digging and jetpack behavior, warnings, surface recharge, upgrades, and persistence.

## Required behavior

- Only accepted digs and active thrust consume energy.
- Walking, jumping, looking, waiting, and inventory inspection do not consume energy. Space jumps on press; sustained holding engages thrust after the FPS control contract's delay.
- Surface recharge is fast and does not become a management chore.
- Recharge is free or nearly instant; there is no sleep, day/night gate, or fuel purchase per trip.
- Return feedback uses coarse safe/risky/critical language, not an exact calculated energy requirement. Task `32` removes subtitles from every reserve, critical, empty-battery, recharge and refill notice: show their titles alone, without return instructions or secondary explanations.
- Jetpack upgrades progress from weak boosts to stronger, efficient, controllable sustained ascent so old shallow routes become easy.

## Done when

- Consumption/recharge remains correct across pause, depletion, stations, and save/load.
- Warning values are tunable and do not claim an exact guaranteed return cost.
