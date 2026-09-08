# Core loop and main game scene

Status: Task `15` awaits its existing production dependencies. Task `35` is ready to add saving; terrain cleanup `34` is complete.

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

After Tasks `08`-`14`, integrate the production presentation, discoveries, passive feedback, shovel progression, selling, recharge, warnings, and rescue:

1. Collect the first find, physically return, sell it, recharge, and buy the upgrade.
2. Re-enter the unchanged excavation; the upgrade must visibly improve digging toward the second find.
3. Verify the alternative overextension/rescue path without resetting excavation, collected state, or upgrades.

Done when focused integration checks pass, the Windows build is updated, and a manual build review records the main feel/visual limitation. Include a useful lateral branch as well as downward progress; note whether returning through that excavation is confusing before considering the [HOME experiment](return-rescue.md#conditional-home-direction-experiment). Saving (`35`), expanded randomized placement, final presentation and long-run balance (`37`) remain separate.

## Task 35 - persistent excavation and progression

- Gap: terrain, collected identities, inventory, wallet and owned shovel levels currently live only in memory. The locked design rule that excavation never resets still needs save/load support; this is also the foundation for [permanent discoveries (`36`)](buried-upgrades.md).
- Scope after `34`: persist the authoritative excavation density and deterministic generation state, discovery identities/collected state, carried records, wallet, owned upgrades, battery and player return position as one consistent save. Rebuild derived meshes/colliders and exposure from restored state before play resumes; developer overrides are not progression.
- Use versioned saves and atomic replacement with last-valid recovery. Failed/interrupted writes or incompatible data must not silently overwrite progress or start a fresh excavation; provide a readable recovery state. Save during play and on normal exit with a measured cost suitable for continuous digging.
- Acceptance: dig downward and sideways, collect/sell/upgrade, return/rescue, quit and relaunch the Windows build. Restore the same traversable cuts, cleared remnants, absent collected finds and exact economy/upgrade state without duplication or a fresh seed. Check interrupted writes, recovery and repeated load; new reward state in `36` must use this same save boundary.
- Excavation never resets within a save through ordinary gameplay, rescue, reload or the ending. A separate new game and the existing explicitly confirmed development-only reset do not redefine that rule; preserve the release gate in `22`.
