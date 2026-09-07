# Battery and jetpack economy

Status: Task `13` is planned; the shared battery foundation is implemented.

Idea coverage: sections 30-35, relevant parts of section 46, and tuning in section 53.

## Purpose

Use one readable resource to create return pressure while letting the player choose between more digging and easier ascent.

## Task 13 - recharge and warnings

- Scope after Task `12`: replace the recharge adapter in `MainGame` with `SurfaceRecharge`; add polished `ReturnWarning` and HUD safe/risky/critical feedback from tunable charge fractions (initially risky <=35%, critical <=15%). Recharge free and instantly inside the surface-only zone.
- Acceptance: real terrain charges only accepted digs; thrust shares that battery; walking/waiting/menu inspection are free. Warnings cross configured bands without promising a return cost; pause/depletion/re-entry behave correctly, and the zone cannot recharge a player underground.
- Next: Task `14`. Tune the tempting-find route in Task `15`; battery/jetpack upgrades and disk persistence remain later work.

## Later expansion

Connect the existing shared battery to production digging and jetpack behavior, warnings, surface recharge, upgrades, and persistence.

## Required behavior

- Only accepted digs and active thrust consume energy.
- Walking, looking, waiting, and inventory inspection do not consume energy.
- Surface recharge is fast and does not become a management chore.
- Recharge is free or nearly instant; there is no sleep, day/night gate, or fuel purchase per trip.
- Return feedback uses coarse safe/risky/critical language, not an exact calculated energy requirement.
- Jetpack upgrades progress from weak boosts to stronger, efficient, controllable sustained ascent so old shallow routes become easy.

## Done when

- Consumption/recharge remains correct across pause, depletion, stations, and save/load.
- Warning values are tunable and do not claim an exact guaranteed return cost.
