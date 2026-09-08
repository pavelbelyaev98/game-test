# Architecture

- `unity/Assets/Runtime/Player` - input, movement, camera, battery, and player state
- `unity/Assets/Runtime/Interaction` - targeting, inventory identities, session wallet, seeded discovery placement and terrain-driven reveal/collection
- `unity/Assets/Runtime/Terrain` - finite signed density field, smooth surface-net meshes, dig adapter and permanent boundary contract
- `unity/Assets/Runtime/UI` - HUD and menu behavior
- `unity/Assets/Runtime/Validation` - disposable validation adapters, not production mechanics
- `unity/Assets/Scenes` - serialized scenes
- `unity/Assets/Editor` - editor-only tooling
- `unity/Assets/Tests` - repository-owned EditMode and PlayMode checks
- `unity/Packages/manifest.json` - direct dependency source of truth

Each scene owns one player/menu root and child camera. `FpsValidation` contains disposable control fixtures. `MainGameRoot` owns the evolving game scene: excavation, static surface/boundaries, scenery, and station/return anchors.

The existing recharge anchor owns `SurfaceRecharge`, which samples the player's feet after movement and refills the shared battery only inside its surface bounds. `FpsPlayer` owns tunable `ReturnWarning` charge bands; `FpsHud` presents those bands and recharge context without calculating a route cost. Recharge preserves inventory, owned upgrades and excavation state.

`FpsPlayer` owns `SessionWallet` and `RescueController`. Rescue snapshots exact carried records and the clamped fee, validates that snapshot before committing once, and leaves the collected registry untouched. The player owns landing clearance, teleport/reset of movement, refill and menu/input restoration. `FpsHud` presents the loss confirmation using the existing pause menu. Selling/purchasing will reuse the wallet; no disk persistence exists yet.

`TerrainVolume` owns one in-memory `ExcavationGrid`, replaces the edit-mode preview with runtime chunks, and synchronously updates matching render/collision meshes after accepted organic scoops. The grid removes disconnected soil after a scoop using incremental support searches; the same accepted stroke includes its volume and expanded dirty region. Chunks share density samples and gradient normals through a halo. Permanent boundary colliders remain separate from removable soil. After mesh/collision updates, changed world bounds notify discoveries, including detached islands and resets.

`DiscoveryField` owns one seeded placement/session population; each `BuriedFind` uses sampled terrain exposure plus actual visibility, then transfers its unique record to inventory. `FpsPlayer` routes held primary input to collection before digging and owns pickup recovery; station interaction remains separate. Collected identities remain absent across terrain resets, rescue and surface trips. The three simple prefabs in `Assets/Content/Finds` are development content awaiting final art; placement/collection code remains reusable. `FpsPlayer` owns `ShovelState` and admin flags; `FpsHud` projects X-ray markers without changing physics/eligibility. The build flag gates admin access. Selling/purchase services and disk persistence remain future owners.

Update this file only when folder ownership, major system boundaries, or scene ownership changes.
