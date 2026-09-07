# Architecture

- `unity/Assets/Runtime/Player` - input, movement, camera, battery, and player state
- `unity/Assets/Runtime/Interaction` - targeting and dig/interaction contracts
- `unity/Assets/Runtime/UI` - HUD and menu behavior
- `unity/Assets/Runtime/Validation` - disposable validation adapters, not production mechanics
- `unity/Assets/Scenes` - serialized scenes
- `unity/Assets/Editor` - editor-only tooling
- `unity/Assets/Tests` - repository-owned EditMode and PlayMode checks
- `unity/Packages/manifest.json` - direct dependency source of truth

The current scene owns one player/menu root and a child camera. Production excavation, discovery, economy, progression, and persistence should be added behind focused runtime contracts as their tasks begin.

Update this file only when folder ownership, major system boundaries, or scene ownership changes.
