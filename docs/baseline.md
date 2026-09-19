# Prototype Baseline Implementation

> **Notice:** This document is a factual inventory of systems currently functioning in the Unity prototype. **None of this implementation is considered signed-off or final.** Everything is subject to refactoring, rebalancing, or replacement to align with the new concept in `docs/concept/`.

## 1. Player & Controls (`unity/Assets/Runtime/Player/`)
- **Controller:** First-person `CharacterController` with smooth crouch (Left Ctrl, 35% speed) and modest sprint (Left Shift, 1.35x speed).
- **Tool ladder:** authored once in `ShovelProfile.Defaults()` and no longer serialized into `MainGame`; Developer admin → **Tool tuning** nudges bite/cadence/reach live and prints a paste-ready table (`Logs/tuning.txt`).
- **Jetpack & Rescue:** Vertical thrust and hover mechanics. Automatic zero-fuel rescue if stranded.
- **Input & Comfort:** Unity Input System with full runtime action rebinding. Camera FOV slider (55–90°), crosshair toggle, and preferences persistence (`Preferences/*.ini`).

## 2. Terrain & Excavation (`unity/Assets/Runtime/Terrain/`)
- **Voxel Engine:** Finite signed density field (24 × 24 × 100 m since 005, authored by `SiteLayout`) running synchronized surface-net meshing (0.125 m resolution) with collision generation. Chunks are materialized on demand: 7,200 possible keys, 144 built on a fresh site, so startup and load cost do not scale with depth.
- **Checkpoints:** dense density (~119 MB for the shipped site) validated and gzip-Fastest encoded on a worker thread, packed payload capped at 64 MB / 256 MB unpacked. A shallower checkpoint (12 m, 32 m) deepens in place on load: the dug hole and every find keep their world position.
- **Digging:** Spherical scoop cuts, organic cut variation, and detached soil cleanup within the stroke.
- **Admin Tools:** Session-only debug panel (`Ctrl+Shift+F10`) with shovel tier selection, refill, and buried find markers.

## 3. Finds & Physics (`unity/Assets/Runtime/Interaction/`)
- **Finds:** 2,578 depth-placed finds, every type at its authored **0.7 model scale**. **Plain rocks
  own the shallow layer**: 1,000 total, 460 of them hung just under the turf (0.47–0.75 m) — the
  packing ceiling for full-size rocks, since neighbours need one clearance envelope between them
  (520 saturates the placer, 600 fails outright). A wide starter scrape (~1.2 m across, 0.3 m deep)
  therefore turns up **~1 rock** (max 2); more per scrape needs smaller pieces or extra small object
  types. The ore ladder starts beneath them: 186 coal at $4 from 1 m, then copper $5 through diamond
  $45, each with a dense core band plus a thin scatter band, so the dig rate stays ~45–60 finds per
  metre below the shallow layer while the mix slides from coal at the top to gold/ruby/diamond below
  ~18 m. Mass stays authored. Retired junk types (bottles) remain in the catalog as zero-count
  entries: old saves resolve them, new games never spawn them.
- **Detection & Pickup:** Aim-assisted reveal, 60% voxel exposure threshold for collection, held aim instant pickup.
- **Handling:** Physical lift/drop (RMB) and throw (LMB). Carried finds track motion and settle physically on release; a slow creep on a slope counts as quiet, so finds stop instead of rolling away forever.

## 4. Hub & Economy (`unity/Assets/Runtime/Player/`, `Runtime/Interaction/`)
- **Surface Stations:** Sell Station (instant trade) and Upgrade Station (shovel, battery capacity 100–400, bag capacity 10–40 slots). Every track shares one tier price ladder (`EquipmentProgression.TierPrices` = 10/25/55/100/180, authored in code and never baked into the scene); the shovel runs one tier deeper than the bag and tank.
- **Refill Economy:** Paid battery recharge ($1 minimum, whole-dollar `$`).

## 5. UI & Presentation (`unity/Assets/Runtime/UI/`)
- **UI Toolkit:** Single UI Document (`FpsHud`) driving the HUD, Pause menu, Settings tabs, and Station trading interfaces with unified grayscale styling.
- **Focus loss:** the game still pauses when the window loses focus, but the dim overlay and pause card are hidden while focus is elsewhere, so external screenshot tools capture the game rather than the pause screen.
- **Station machines:** Workshop and Sell All are one fixed-size parts-board table (`Station.uss` + `ToolkitStationRows`) — money-only header, categories in their own columns, one clickable row per upgrade track, refill service or carried find. One click buys; nothing is selected first and nothing resizes.
- **Environment:** Triplanar soil/turf shader (`GardenGround`), procedural instanced wind-blown grass clumps, sun disc projection, cyan sky gradient.

## 6. Persistence & Lifecycle (`unity/Assets/Runtime/Persistence/`)
- **Saving:** Versioned whole-world snapshots (`WorldSaveController`), atomic disk write, 10 s background autosave, recovery from interruptions.
- **Windows Process:** Single-instance reservation (`DesktopInstance`) focusing existing window on relaunch.
