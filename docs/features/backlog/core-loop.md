# Core loop and main game scene

Status: Persistence `35` and terrain cleanup `34` are complete. Task `15` follows independent shovel purchases (`25`) and production acceptance in `05`–`09`/`11`–`14`. Its three common types are detector-silent; `42` → `10` → `102` later validates production non-minor detection.

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose

Prove one short trip: dig, uncover, collect, decide whether to push farther, return, sell, recharge, upgrade, and repeat. Non-minor detector signals later enrich the full loop; the common-only starter trip can stand on recognition and excavation. The surface is a nearby checkpoint. [15](../../development/tasks/15-complete-trip.md) observes recognition, understandable return advice and the satisfaction of banking a trip, as well as correct transactions. The [risk register](../../development/design-risks.md) preserves these player-feel expectations.

## Scene contract

- `MainGame.unity` is the evolving full-game scene. [79's startup menu](../../development/tasks/79-startup-menu.md) gates entry with New Game, Load Game, Settings and Quit. New Game starts at the south rim of an untouched finite dig volume; Load Game restores the existing excavation. Visible water, scenery and distinct permanent boundaries retain their current ownership.
- Existing player/camera/HUD own FPS behavior. Terrain owns removed volume and geometry; discoveries own exposure/collection; inventory owns item records; detector reads eligible discoveries; separate surface stations own selling/upgrades; recharge/rescue use a clear surface anchor.
- The main build uses only approved Blender MCP visuals or free-to-use external visuals/audio licensed for commercial use. Primitives and debug presentation belong only in validation scenes.
- Excavation, discoveries and progression survive application restarts as one consistent snapshot. Dirty state autosaves after at most 10 seconds plus writer/frame latency; successful trades/rescue request an immediate checkpoint, and Save and quit/window close wait for the latest write. Never restore fresh terrain with retained purchases.
- The unupgraded battery/inventory must already support a satisfying first trip. Validate meaningful digging time and return cost before using purchases to extend it.
- Battery, terrain, falling and return planning are the complete pressure model; do not introduce lava, gas, oxygen, hunger, earthquakes or other environmental hazards.

## Task 15 - complete-trip integration

See [numbered Task `15`](../../development/tasks/15-complete-trip.md) for scope, research, questions and acceptance.

## Task 35 - persistent excavation and progression

See [numbered Task `35`](../../development/tasks/35-save-load.md) for scope, research, questions and acceptance.

- Saves include exact density and generation state, all discovery identities/placements/collected records, carried finds, credits, owned shovel, battery and player pose. Developer overrides remain session-only.
- Resume is unavailable until restored terrain meshes/colliders and discovery exposure agree. Pause shows saving status quietly; routine autosaves do not interrupt play or reset menu selection.
- Keep the previous complete checkpoint. A damaged latest checkpoint offers recovery and preserves its original file; incompatible content/version or missing valid recovery blocks play with Retry/Open save folder/Quit. A write failure pauses with the live world retained and retry available; quitting without its unsaved changes requires confirmation. New Game is confined to the explicit startup flow; profile contention preserves checkpoints and supports retry after the other owner closes.
- Future reward/photo/ending state joins this same versioned boundary. Preserve older readers and content identities, or explicitly migrate them; never force a new excavation merely because content changes.

## Planned persistence scaling

[80](../../development/tasks/80-incremental-terrain-saving.md) addresses changed-density capture cost and incremental late-game scaling around `35`'s implemented background writer. Preserve coherent versioned snapshots, bounded dirty/durability windows and recovery; ordinary saving must not freeze excavation. Stable identities, seeds and explicit serializable edits retain future co-op options without adding networking.
