# Task 140 - More shallow rocks and a prominent fuel warning

Type: implementation. Status: `done`. Explicit user-selected batch after `138`/`139`.

Prerequisites: rock-only spawning `126`, shared fuel and paid refills `46`/`139`, existing Toolkit HUD. Features: [discovery generation](../../features/backlog/discovery-generation.md), [battery/fuel](../../features/backlog/battery-jetpack.md), [return warning](../../features/backlog/return-rescue.md). Context: `101` playtest and `93`/`94` future return-advice research.

## Selected scope

- Add a modest number of collectible rocks in the existing shallow band, reusing the three rock appearances and their current value/size/physics. Retain buried clearance and the deeper allocation: 264 shallow rocks (+24), for 336 total. Prefer the existing 0.15 m enclosing-radius soil gap, with a bounded 0.10 m fallback for crowded placements.
- Present a persistent bottom-center **LOW FUEL** title in yellow at the existing 35% reserve threshold, escalating to **FUEL CRITICAL** in red at 15%. Empty fuel is red; text distinguishes the states without relying only on color.
- Keep the warning clear of the reticle, target prompt and pickup/transaction feedback. It remains visible near the workshop while fuel is low, disappears after sufficient refill, and hides with menus/focus pause or unlimited-fuel admin mode. Thresholds use the currently owned capacity.
- Preserve the current shared consumption, paid refill, automatic rescue and saved populations. This is a selected charge-warning presentation change; it does not settle `93`'s separate question about estimating return effort or complete broader `94` work.

## Acceptance

- More shallow rocks in New Game, exact quotas, deterministic placement, no protruding/overlapping rocks, and bounded generation across 100 seeds. Saved finds/poses/collected state restore without additions or rerolls.
- Actual HUD transitions through safe/yellow/red/refilled states at starter and upgraded capacities, including pause and workshop proximity. Bottom-center warning fits at 960×540, 1920×1080 and 16:10 without overlapping the feedback/reticle.
- Fast compile, relevant catalog/HUD/save integration checks, official Unity CLI visual inspection and Windows build at the standard path. Preserve user saves/preferences and remove temporary captures.

Questions: none blocking. The user selects location, color escalation and more top-layer rocks; numerical count/threshold tuning stays with implementation.

## Tuning evidence

At a strict 0.15 m gap, even 248 shallow rocks exhausted the bounded search on some seeds. Preserve that preferred spread with 2,000 candidates, then allow a 0.10 m gap for at most 2,000 more. This fits 264 across 100 seeds without changing depth or size. The full catalog check verifies actual approved-mesh envelopes and coverage.

[Completion and validation](../completed/140-shallow-rocks-and-fuel-warning.md).
