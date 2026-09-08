# Architecture

- `unity/Assets/Runtime/Player` - input, movement, camera, battery, and player state
- `unity/Assets/Runtime/Interaction` - targeting, inventory identities, session wallet, seeded discovery placement and terrain-driven reveal/collection
- `unity/Assets/Runtime/Terrain` - finite signed density field, smooth surface-net meshes, dig adapter and permanent boundary contract
- `unity/Assets/Runtime/UI` - HUD and menu behavior
- `unity/Assets/Runtime/Persistence` - versioned whole-world snapshots, atomic disk storage, recovery and autosave coordination
- `unity/Assets/Runtime/Validation` - disposable validation adapters, not production mechanics
- `unity/Assets/Scenes` - serialized scenes
- `unity/Assets/Editor` - editor-only tooling
- `unity/Assets/Tests` - repository-owned EditMode and PlayMode checks
- `unity/Packages/manifest.json` - direct dependency source of truth

Each scene owns one player/menu root and child camera. `FpsValidation` contains disposable control fixtures. `MainGameRoot` owns the evolving game scene: excavation, static surface/boundaries, scenery, and station/return anchors.

The existing recharge anchor owns `SurfaceRecharge`, which samples the player's feet after movement and refills the shared battery only inside its surface bounds. `FpsPlayer` owns tunable `ReturnWarning` charge bands; `FpsHud` presents those bands and recharge context without calculating a route cost. Recharge preserves inventory, owned upgrades and excavation state.

`FpsPlayer` owns `SessionWallet`, `StationTrade` and `RescueController`. Rescue snapshots carried records and the clamped fee, validates before committing once, and leaves the collected registry untouched. The player owns landing clearance, movement/refill and menu/input restoration; successful rescue requests the same whole-world checkpoint as trades.

`WorldSaveController` on the main player owns the local save session. It observes small state revisions/pose values, captures a consistent world on the main thread, and serializes/compresses/checksums/flushes on one background writer. Unchanged terrain reuses an immutable density snapshot; changes during writes remain dirty and queued transaction requests coalesce. `WorldSaveStore` locks the profile against a second writer and replaces `world.sav` atomically with `world.previous.sav` retained. Incompatible/unreadable data blocks play; recovery is acknowledged before continuing, with damaged originals archived.

Saves live in `Application.persistentDataPath/Save`; Editor play uses `EditorSave`, and additive validation scenes have no profile unless explicitly started with an isolated directory. Version 1 stores exact density, terrain seeds/revision/support-search floor, accepted find population/poses/content keys/collected state, carried records, credits, owned shovel, battery and player pose/vertical speed. Loading rebuilds terrain render/collision meshes in frame slices, restores discoveries/exposure and only then enables Resume. Admin overrides are excluded. Future tasks extend this boundary with explicit readers/migrations for existing versions and retain discovery content keys (or supply a compatibility map); unknown content must never trigger fresh generation.

`SellStation` and `UpgradeStation` own offers for their current player interaction; `StationTrade` binds them to inventory/wallet revisions and owned shovel level, then prevalidates and commits without callbacks. The player rechecks station focus, distance and visibility and rejects old UI revisions. `StationMenuView` renders explicit item rows and purchase comparisons in the existing HUD canvas; `StationRowFocus` keeps keyboard selection visible. `StationMotion` animates only authored flap/drawer transforms. `SurfaceStationSetup` imports the approved Blender models/materials and wires their two existing anchors, independently of transaction logic.

`TerrainVolume` owns one in-memory `ExcavationGrid`, replaces the edit-mode preview with runtime chunks, and synchronously updates matching render/collision meshes after accepted organic scoops. The grid removes disconnected soil after a scoop using incremental support searches; the same accepted stroke includes its volume and expanded dirty region. Chunks share density samples and gradient normals through a halo. Permanent boundary colliders remain separate from removable soil. After mesh/collision updates, changed world bounds notify discoveries, including detached islands and resets.

`DiscoveryField` owns one seeded placement/session population; each `BuriedFind` uses sampled terrain exposure plus actual visibility, then transfers its unique record to inventory. `FpsPlayer` routes held primary input to collection before digging and owns pickup recovery; station interaction remains separate. Collected identities remain absent across sales, terrain resets, rescue and surface trips. The three simple prefabs in `Assets/Content/Finds` are development content awaiting final art; placement/collection code remains reusable. `FpsPlayer` owns `ShovelState` and admin flags; `FpsHud` projects X-ray markers without changing physics/eligibility. The build flag gates admin access; purchases always use owned progression.

Update this file only when folder ownership, major system boundaries, or scene ownership changes.
