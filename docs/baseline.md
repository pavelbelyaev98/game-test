# Prototype Baseline Implementation

> **Notice:** This document is a factual inventory of systems currently functioning in the Unity prototype. **None of this implementation is considered signed-off or final.** Everything is subject to refactoring, rebalancing, or replacement to align with the new concept in `docs/concept/`.

## 1. Player & Controls (`unity/Assets/Runtime/Player/`)
- **Controller:** First-person `CharacterController` with smooth crouch (Left Ctrl, 35% speed) and modest sprint (Left Shift, 1.35x speed).
- **Tool ladder:** authored once in `ShovelProfile.Defaults()` and no longer serialized into `MainGame`; Developer admin → **Tool tuning** nudges bite/cadence/reach live and prints a paste-ready table (`Logs/tuning.txt`).
- **Jetpack & Rescue:** Vertical thrust and hover mechanics. Automatic zero-fuel rescue if stranded.
- **Input & Comfort:** Unity Input System with full runtime action rebinding. Camera FOV slider (55–90°), crosshair toggle, and preferences persistence (`Preferences/*.ini`).

## 2. Terrain & Excavation (`unity/Assets/Runtime/Terrain/`)
- **Voxel Engine:** Finite signed density field (24 × 24 × 32 m) running synchronized surface-net meshing (0.125 m resolution) with collision generation.
- **Digging:** Spherical scoop cuts, organic cut variation, and detached soil cleanup within the stroke.
- **Admin Tools:** Session-only debug panel (`Ctrl+Shift+F10`) with shovel tier selection, refill, and buried find markers.

## 3. Finds & Physics (`unity/Assets/Runtime/Interaction/`)
- **Finds:** 1,996 depth-placed finds — 1,801 minerals (8 tiers: Coal through Diamond), 195 rocks
  (3 visual variants) and a 312-find entry layer at 0.65–1.1 m. Each type has a dense core band
  plus a thin scatter band, so the dig rate stays ~45–60 finds per metre of depth while the mix
  slides from junk at the top to gold/ruby/diamond below ~18 m. Placement uses a deterministic
  spatial grid (~50 ms for the full population).
- **Detection & Pickup:** Aim-assisted reveal, 60% voxel exposure threshold for collection, held aim instant pickup.
- **Handling:** Physical lift/drop (RMB) and throw (LMB). Carried finds track motion and settle physically on release.

## 4. Hub & Economy (`unity/Assets/Runtime/Player/`, `Runtime/Interaction/`)
- **Surface Stations:** Sell Station (instant trade) and Upgrade Station (shovel, battery capacity 100–400, bag capacity 10–40 slots).
- **Refill Economy:** Paid battery recharge ($1 minimum, whole-dollar `$`).

## 5. UI & Presentation (`unity/Assets/Runtime/UI/`)
- **UI Toolkit:** Single UI Document (`FpsHud`) driving the HUD, Pause menu, Settings tabs, and Station trading interfaces with unified grayscale styling.
- **Focus loss:** the game still pauses when the window loses focus, but the dim overlay and pause card are hidden while focus is elsewhere, so external screenshot tools capture the game rather than the pause screen.
- **Station machines:** Workshop and Sell All are one fixed-size parts-board table (`Station.uss` + `ToolkitStationRows`) — money-only header, categories in their own columns, one clickable row per upgrade track, refill service or carried find. One click buys; nothing is selected first and nothing resizes.
- **Environment:** Triplanar soil/turf shader (`GardenGround`), procedural instanced wind-blown grass clumps, sun disc projection, cyan sky gradient.

## 6. Persistence & Lifecycle (`unity/Assets/Runtime/Persistence/`)
- **Saving:** Versioned whole-world snapshots (`WorldSaveController`), atomic disk write, 10 s background autosave, recovery from interruptions.
- **Windows Process:** Single-instance reservation (`DesktopInstance`) focusing existing window on relaunch.
