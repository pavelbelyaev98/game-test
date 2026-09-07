# Architecture

- `unity/Assets/Runtime/Player` - input, movement, camera, battery, and player state
- `unity/Assets/Runtime/Interaction` - targeting and dig/interaction contracts
- `unity/Assets/Runtime/Terrain` - finite occupancy state, exposed-face chunk meshes, dig adapter and permanent boundary contract
- `unity/Assets/Runtime/UI` - HUD and menu behavior
- `unity/Assets/Runtime/Validation` - disposable validation adapters, not production mechanics
- `unity/Assets/Scenes` - serialized scenes
- `unity/Assets/Editor` - editor-only tooling
- `unity/Assets/Tests` - repository-owned EditMode and PlayMode checks
- `unity/Packages/manifest.json` - direct dependency source of truth

Each scene owns one player/menu root and child camera. `FpsValidation` contains disposable control fixtures. `MainGameRoot` owns the evolving game scene: excavation, static surface/boundaries, scenery, and station/return anchors.

`TerrainVolume` initializes its in-memory `ExcavationGrid` once per scene session, replaces the authored primitive preview with runtime chunks, and updates matching render/collision meshes after accepted digs. Static boundary colliders are separate from removable soil. Discovery, economy, progression and disk persistence remain future owners.

Update this file only when folder ownership, major system boundaries, or scene ownership changes.
