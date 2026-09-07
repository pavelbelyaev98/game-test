# Core loop and main game scene

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose

Prove one short trip: dig, detect, uncover, collect, decide whether to push farther, return, sell, recharge, upgrade, and repeat. Excavation/discovery dominate play time; the surface is a nearby checkpoint.

## Scene contract

- `MainGame.unity` is the evolving full-game scene. It starts at the south rim of an untouched finite dig volume with visible water, scenery, and distinct permanent boundaries.
- Existing player/camera/HUD own FPS behavior. Terrain owns removed volume and geometry; discoveries own exposure/collection; inventory owns item records; detector reads eligible discoveries; separate surface stations own selling/upgrades; recharge/rescue use a clear surface anchor.
- Initial tasks may use primitives and two authored ordinary finds, then replace them as their production features are implemented. Generated/imported art or audio requires an asset-ledger entry.
- Session state survives trips and rescue but currently resets when the application closes.

## Task 14 - complete-trip integration

After Tasks `07`-`13`, integrate two finds, passive feedback, two shovel levels, selling, recharge, warnings, and rescue:

1. Collect the first find, physically return, sell it, recharge, and buy the upgrade.
2. Re-enter the unchanged excavation; the upgrade must visibly improve digging toward the second find.
3. Verify the alternative overextension/rescue path without resetting excavation, collected state, or upgrades.

Done when focused integration checks pass, the Windows build is updated, and a manual build review records the main feel/visual limitation. Saving, randomized placement, final presentation, and long-run balance remain later tasks.
