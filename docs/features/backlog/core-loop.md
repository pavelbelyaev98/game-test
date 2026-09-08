# Core loop and main game scene

Status: Persistence `35` and terrain cleanup `34` are complete. Task `15` follows independent shovel purchases (`25`) and production acceptance (`05`–`14`).

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose

Prove one short trip: dig, detect, uncover, collect, decide whether to push farther, return, sell, recharge, upgrade, and repeat. Excavation/discovery dominate play time; the surface is a nearby checkpoint.

## Scene contract

- `MainGame.unity` is the evolving full-game scene. It starts at the south rim of an untouched finite dig volume with visible water, scenery, and distinct permanent boundaries.
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
- Keep the previous complete checkpoint. A damaged latest checkpoint offers recovery and preserves its original file; incompatible content/version or missing valid recovery blocks play with Retry/Open save folder/Quit. A write failure pauses with the live world retained and retry available; quitting without its unsaved changes requires confirmation. No routine reset/new-game button is added.
- Future reward/photo/ending state joins this same versioned boundary. Preserve older readers and content identities, or explicitly migrate them; never force a new excavation merely because content changes.
