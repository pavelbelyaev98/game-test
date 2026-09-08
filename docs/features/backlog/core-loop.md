# Core loop and main game scene

Status: Task `35` is ready; terrain cleanup `34` is complete. Task `15` follows saving, independent shovel purchases (`25`) and production acceptance (`05`–`14`).

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose

Prove one short trip: dig, detect, uncover, collect, decide whether to push farther, return, sell, recharge, upgrade, and repeat. Excavation/discovery dominate play time; the surface is a nearby checkpoint.

## Scene contract

- `MainGame.unity` is the evolving full-game scene. It starts at the south rim of an untouched finite dig volume with visible water, scenery, and distinct permanent boundaries.
- Existing player/camera/HUD own FPS behavior. Terrain owns removed volume and geometry; discoveries own exposure/collection; inventory owns item records; detector reads eligible discoveries; separate surface stations own selling/upgrades; recharge/rescue use a clear surface anchor.
- The main build uses only approved Blender MCP visuals or free-to-use external visuals/audio licensed for commercial use. Primitives and debug presentation belong only in validation scenes.
- Session state survives trips and rescue but currently resets when the application closes.
- Battery, terrain, falling and return planning are the complete pressure model; do not introduce lava, gas, oxygen, hunger, earthquakes or other environmental hazards.

## Task 15 - complete-trip integration

See [numbered Task `15`](../../development/tasks/15-complete-trip.md) for scope, research, questions and acceptance.

## Task 35 - persistent excavation and progression

See [numbered Task `35`](../../development/tasks/35-save-load.md) for scope, research, questions and acceptance.
