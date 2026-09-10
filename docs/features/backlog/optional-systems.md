# Optional systems

Status: implementation remains deferred until the full loop feels good. [71 - C4 research](../../development/tasks/71-dynamite-design-research.md) evaluates one existing candidate; research completion does not select it. User-requested [idea assessment](../../research/player-idea-assessment.md) adds concrete design comparisons, not new delivered systems.

Idea coverage: sections 29, 38-41, and any tutorial alternative from section 50 not already owned by [FPS controls](fps-controls.md).

## Purpose

Hold worthwhile ideas that must not inflate the core implementation: item condition, C4, large discoveries, special progression objects, and location-specific story finds.

## Task rule

Promote only one selected optional system at a time into its own implementation task and feature file. A research task may conclude include, defer or omit, recording why before any implementation is scheduled. Define its benefit to excavation, integration boundary, and removal path before implementation.

## Acceptance gate

- The core loop is already playable and validated.
- The optional system improves discovery or excavation enough to justify its complexity.
- It does not introduce a second primary tool loop, excessive menus, or an unrelated progression economy.

## Preserved concepts

- Item condition is low priority and should be removed if it adds appraisal noise.
- **Selected name: C4**, replacing the earlier dynamite wording for this game's existing placed, remotely detonated explosive concept. This is the same proposed mechanic; `71` retains its stable filename and inclusion/usefulness decision. Charges cost money and remove a satisfying amount of terrain; upgrades may affect blast, strength, price or capacity. The name choice does not select bomb-only terrain or approve an asset.
- `71` investigates whether C4 adds value beside strong shovels, then defines forgiving visible-near-surface snapping with valid/invalid preview, including moving/airborne and wall/ceiling use. No throw/bounce, stationary/perfect-angle requirement or tiny hotspot; invalid placement costs nothing. If selected, test anchoring as supporting soil disappears, tunnelling/clipping, substantial predictable blasts, charge/save state and shared `26`/`34` cleanup. Ask the user to choose the purchase/carry/trigger proposal and approve its specific art/audio. Ordinary progress and the ending must remain possible without bombs or consumable stockpiling; the review report does not make this optional system mandatory.
- Remembered hard formations should offer meaningful access/progress when later overcome, though not guaranteed treasure every time.
- Large finds require substantial exposure and never enter ordinary slots; extraction may use an in-world cable while control stays with the player, or the object may remain underground.
- A tiny number of keys/components can live outside normal capacity, survive rescue, and be used automatically or at an obvious location without inventory puzzles.
- Optional short story reactions belong to `61`/`52`, with their decision and no-blocking/no-spam rules in [ending](ending.md). No report line chooses a final object or authorizes text/audio presentation.
- The conditional [HOME-direction experiment](return-rescue.md#conditional-home-direction-experiment) stays with return planning; `70` also investigates simple revisit markers, with no implementation until playtests and a reviewed decision justify it. [Buried passive upgrades (`36`)](buried-upgrades.md) have their own committed content scope, but are optional to collect and do not make C4 mandatory.
- [Research investigation](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation) belongs to `15`/`37`: distinguish navigation confusion from a boring commute and first review mobility, spacing and surface-trip length. No outposts/shortcuts are scheduled. Keep smelting/recipes, cargo-weight simulation, extra hazards, infinite-world expansion and a compulsory boss outside the design.
- New proposal owners: [96](../../development/tasks/96-discovery-physics-design.md) selective find motion, [97](../../development/tasks/97-special-find-interaction-design.md) special interactions/chests, [98](../../development/tasks/98-refill-economy-design.md) refill/field charges and [99](../../development/tasks/99-return-mobility-design.md) mobility/placed teleport. Concrete comparisons may proceed under the user's design request; selected delivery still gets its own numbered task and normal asset approval. `40` owns possible item-origin sound briefs.
