# Architecture

- `unity/Assets/Runtime/Player` - input, movement, camera, battery, and player state
- `unity/Assets/Runtime/Interaction` - targeting, inventory identities, seeded discovery placement and terrain-driven reveal/collection
- `unity/Assets/Runtime/Terrain` - finite signed density field, smooth surface-net meshes, dig adapter and permanent boundary contract
- `unity/Assets/Runtime/UI` - HUD and menu behavior
- `unity/Assets/Runtime/Validation` - disposable validation adapters, not production mechanics
- `unity/Assets/Scenes` - serialized scenes
- `unity/Assets/Editor` - editor-only tooling
- `unity/Assets/Tests` - repository-owned EditMode and PlayMode checks
- `unity/Packages/manifest.json` - direct dependency source of truth

Each scene owns one player/menu root and child camera. `FpsValidation` contains disposable control fixtures. `MainGameRoot` owns the evolving game scene: excavation, static surface/boundaries, scenery, and station/return anchors.

`TerrainVolume` owns one in-memory `ExcavationGrid`, replaces the edit-mode preview with runtime chunks, and synchronously updates matching render/collision meshes after accepted organic scoops. The grid removes disconnected soil after a scoop using incremental support searches; the same accepted stroke includes its volume and expanded dirty region. Chunks share density samples and gradient normals through a halo. Permanent boundary colliders remain separate from removable soil. After mesh/collision updates, changed world bounds notify discoveries, including detached islands and resets.

`DiscoveryField` owns one seeded placement/session population; each `BuriedFind` uses actual visibility for small finds and sampled terrain exposure for large finds, then transfers its unique record to inventory. `FpsPlayer` routes a fresh primary click to collection before digging; station interaction remains separate. Collected identities remain absent across terrain resets and surface trips. The three simple prefabs in `Assets/Content/Finds` are development content awaiting final art; placement/collection code remains reusable. `FpsPlayer` owns `ShovelState` and admin flags; `FpsHud` projects X-ray markers without changing physics/eligibility. The build flag gates admin access. Paid economy and disk persistence remain future owners.

Update this file only when folder ownership, major system boundaries, or scene ownership changes.
