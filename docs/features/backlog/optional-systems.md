# Optional systems

Status: implementation remains deferred until the full loop feels good. [71 - dynamite research](../../development/tasks/71-dynamite-design-research.md) evaluates one existing candidate; research completion does not select it.

Idea coverage: sections 29, 38-41, and any tutorial alternative from section 50 not already owned by [FPS controls](fps-controls.md).

## Purpose

Hold worthwhile ideas that must not inflate the core implementation: item condition, dynamite, large discoveries, special progression objects, and location-specific story finds.

## Task rule

Promote only one selected optional system at a time into its own implementation task and feature file. A research task may conclude include, defer or omit, recording why before any implementation is scheduled. Define its benefit to excavation, integration boundary, and removal path before implementation.

## Acceptance gate

- The core loop is already playable and validated.
- The optional system improves discovery or excavation enough to justify its complexity.
- It does not introduce a second primary tool loop, excessive menus, or an unrelated progression economy.

## Preserved concepts

- Item condition is low priority and should be removed if it adds appraisal noise.
- Dynamite is placed, remotely detonated, costs money, and removes a satisfying amount of terrain. Upgrades may affect blast, strength, price, or capacity.
- `71` investigates whether dynamite adds value beside strong shovels, then defines forgiving visible-near-surface snapping with valid/invalid preview, including moving/airborne and wall/ceiling use. No throw/bounce, stationary/perfect-angle requirement or tiny hotspot; invalid placement costs nothing. If selected, test anchoring as supporting soil disappears, tunnelling/clipping, substantial predictable blasts, charge/save state and shared `26`/`34` cleanup. Ask the user to choose the purchase/carry/trigger proposal and approve its specific art/audio. Ordinary progress and the ending must remain possible without bombs or consumable stockpiling; the review report does not make this optional system mandatory.
- Remembered hard formations should offer meaningful access/progress when later overcome, though not guaranteed treasure every time.
- Large finds require substantial exposure and never enter ordinary slots; extraction may use an in-world cable while control stays with the player, or the object may remain underground.
- A tiny number of keys/components can live outside normal capacity, survive rescue, and be used automatically or at an obvious location without inventory puzzles.
- Optional short story reactions belong to `61`/`52`, with their decision and no-blocking/no-spam rules in [ending](ending.md). No report line chooses a final object or authorizes text/audio presentation.
- The conditional [HOME-direction experiment](return-rescue.md#conditional-home-direction-experiment) stays with return planning; `70` also investigates simple revisit markers, with no implementation until playtests and a reviewed decision justify it. [Buried passive upgrades (`36`)](buried-upgrades.md) have their own committed content scope, but are optional to collect and do not make dynamite mandatory.
- [Research investigation](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation) belongs to `15`/`37`: distinguish navigation confusion from a boring commute and first review mobility, spacing and surface-trip length. No outposts/shortcuts are scheduled. Keep smelting/recipes, cargo-weight simulation, extra hazards, infinite-world expansion and a compulsory boss outside the design.
