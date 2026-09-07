# 10 - Battery and jetpack economy

Status: foundation implemented; production balance and upgrades are planned.

Idea coverage: sections 30-35, relevant parts of section 46, and tuning in section 53.

## Purpose

Use one readable resource to create return pressure while letting the player choose between more digging and easier ascent.

## First-playable task `10a`

- Scope after `08a`: replace the recharge adapter in FirstPlayable with `SurfaceRecharge`; add `ReturnWarning` and HUD safe/risky/critical bands from tunable charge fractions (initially risky <=35%, critical <=15%). Recharge free and instantly inside the surface-only zone.
- Acceptance: real terrain charges only accepted digs; thrust shares that battery; walking/waiting/menu inspection are free. Warnings cross configured bands without promising a return cost; pause/depletion/re-entry behave correctly, and the zone cannot recharge a player underground.
- Next: make `11a` ready. Tune the tempting-find route in `01a`; battery/jetpack upgrades and disk persistence remain in task `10` below.

## Full-feature implementation task `10`

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
