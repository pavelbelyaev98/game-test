# Optional systems

Status: deferred until excavation and the full loop feel good.

Idea coverage: sections 29, 38-41, and any tutorial alternative from section 50 not already owned by [FPS controls](fps-controls.md).

## Purpose

Hold worthwhile ideas that must not inflate the core implementation: item condition, dynamite, large discoveries, special progression objects, and location-specific story finds.

## Task rule

Promote only one optional system at a time into its own task and feature file. Define its benefit to excavation, integration boundary, and removal path before implementation.

## Acceptance gate

- The core loop is already playable and validated.
- The optional system improves discovery or excavation enough to justify its complexity.
- It does not introduce a second primary tool loop, excessive menus, or an unrelated progression economy.

## Preserved concepts

- Item condition is low priority and should be removed if it adds appraisal noise.
- Dynamite is placed, remotely detonated, costs money, and removes a satisfying amount of terrain. Upgrades may affect blast, strength, price, or capacity.
- If dynamite is selected, first research/test anchored placement, tunnelling/clipping, predictable blast volume, charge/save state and shared `26`/`34` cleanup. Ask the user to choose the purchase/carry/trigger proposal and approve its specific art/audio. Ordinary progress and the ending must remain possible without bombs or consumable stockpiling; the review report does not make this optional system mandatory.
- Remembered hard formations should offer meaningful access/progress when later overcome, though not guaranteed treasure every time.
- Large finds require substantial exposure and never enter ordinary slots; extraction may use an in-world cable while control stays with the player, or the object may remain underground.
- A tiny number of keys/components can live outside normal capacity, survive rescue, and be used automatically or at an obvious location without inventory puzzles.
- The conditional [HOME-direction experiment](return-rescue.md#conditional-home-direction-experiment) stays with return planning; it has no implementation task until playtests justify it. [Buried passive upgrades (`36`)](buried-upgrades.md) have their own committed content scope, but are optional to collect and do not make dynamite mandatory.
- [Research investigation](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation) belongs to `15`/`37`: distinguish navigation confusion from a boring commute and first review mobility, spacing and surface-trip length. No outposts/shortcuts are scheduled. Keep smelting/recipes, cargo-weight simulation, extra hazards, infinite-world expansion and a compulsory boss outside the design.
