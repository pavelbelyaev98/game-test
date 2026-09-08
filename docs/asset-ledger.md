# Asset and audio ledger

Task `12` adds the explicitly approved pair of station models below. Other new art/audio remains deferred; the user owns the ground work. The removed presentation pass and TextMeshPro resources below remain inactive.

Tasks `13`/`14`/`32`/`33` add no visual/audio assets or dependencies. Recharge/rescue use existing scene anchors, runtime HUD/font and native UI layout; text scaling refreshes that same font at runtime. Asset ownership and rollback boundaries below are unchanged.

## Task 12 - approved surface stations

- Approval: user replied **"Approve these two station models"** to the specific batch: amber salvage buyer with weighing tray/intake flap/SELL sign, and teal workbench with drawers/vise/UPGRADES sign. No separate shovel, scenery or audio is approved.
- Source/license: original project art authored through Blender MCP; no third-party art, downloads or attribution. Retained source: `art/stations/Stations.blend`. Materials/textures are authored and baked in Blender as part of these models.
- Owned files: `art/stations/` recursively (source and any source backups); `unity/Assets/Content/Stations/` recursively plus `Stations.meta` (FBX exports, baked textures, extracted/imported materials, two prefabs and all companion `.meta`). No fonts, audio settings, external notices or runtime loaders.
- Integration: replace only `MainGameRoot/Surface/{SellStation,UpgradeStation}` pedestal children in `MainGame.unity`; `Assets/Editor/SurfaceStationSetup.cs` owns repeatable wiring, and `MainGameSceneBuilder.cs` calls it for new scenes. Shared `FpsPlayer.cs`, `FpsHud.cs`, `WorldActionContracts.cs` integrate reusable transactions/UI, independently of the art.
- Removal: remove the two imported model instances through the Editor; restore `Selling pedestal`/`Upgrade pedestal` at their anchors with local position `(0,0.6,0)`, scale `(1.2,1.2,1)` and existing `Assets/Settings/MainGame/{SellAnchor,UpgradeAnchor}.mat`. Remove station components/colliders added by setup, its builder call and `SurfaceStationSetup.cs`/`.meta`; delete only the owned source/content folders and `Stations.meta`. Preserve all user-owned terrain/materials, Finds, existing anchors/recharge and reusable transaction/UI code. No notices or external dependencies to remove.

## Task 27 - user-requested simple finds

- Approval: the user explicitly requests random small objects/circles to see excavation and collection. Use three colored sphere/disc forms (blue marble, copper token, amber bead) only; this scoped request supersedes the primitive-art restriction for these development finds. No external art/audio or unrelated scenery.
- Source/license: Unity built-in Sphere mesh, used under the project's Unity Editor license; project-authored material colors and prefabs, no third-party download or attribution. Intended files: recursive ownership of `unity/Assets/Content/Finds/` plus its `.meta` (three prefabs/materials and all companion `.meta`); new parent `Content.meta` if needed.
- Integration: `MainGame.unity` has a `DiscoveryField` referencing these prefabs; `MainGameSceneBuilder.cs` maintains that wiring. Runtime `DiscoveryField`/`BuriedFind`, terrain change notifications and existing player/HUD/input provide behavior independently of this art.
- Removal/replacement: replace the field's three prefab references through the Editor with approved content, or remove its `Discoveries` scene object and the discovery setup call in `MainGameSceneBuilder`. Then remove the owned Finds folder/metas (and empty Content parent/meta only if no other content exists). Preserve terrain, existing materials, user assets and reusable collection/admin code. No notices, loaders or external source files are introduced.
- Production TODO: replace the simple prefabs under Task `09`; they are enabled only for development through `DiscoveryField` until final-art acceptance. Keep the gameplay systems.

## Task 30 - resize approved finds

Task `30` modifies only the existing three approved find prefabs, following the user's explicit request for bigger valuables: `unity/Assets/Content/Finds/{Blue marble,Copper token,Amber bead}.prefab`. Their meshes, materials, source/license and `.meta` identities remain unchanged; no new asset/audio is added. Set dimensions to 0.8/0.8/0.8, 1.0/0.18/1.0 and 0.64/0.90/0.64 m, respectively, and exposure thresholds to 0.6. The existing `DiscoveryField` supplies spacing/cover; `BuriedFind`, `FpsPlayer` and `MainGameSceneBuilder` integrate threshold/held-input behavior. Revert just these scale/threshold fields through the Editor to 0.4/0.4/0.4, 0.5/0.09/0.5 and 0.32/0.45/0.32 with threshold 0.8, plus Task `30` changes in those four scripts and their matching tests/docs; preserve Task `13` changes, all `.meta` files and user-owned terrain. No shared scene references, notices, loaders or importer outputs are added by resizing.

Task `31` adds no art/audio. The user explicitly requested 50% exposure: the same three prefab files now use threshold 0.5; sizes/materials/metas remain unchanged. To undo only this tuning, restore threshold 0.6 through the Editor and in `BuriedFind.cs`/`MainGameSceneBuilder.cs`, plus matching checks/docs. Preserve Task `30` scaling and collection behavior. No new owned files, import outputs or references.

## Required inventory for future imports

Every new asset/audio addition needs explicit user approval before entering the project. Present the specific item or listed batch, purpose, source/license, preview/sample when available, files/integration and removal steps; ask and wait. Record the approved scope here. A general feature request or assumed necessity is not approval.

Before use, record purpose/user authorization, source URL or Blender `.blend`, exact commercial license, attribution, task and approval state. List every owned file or explicitly recursive folder, including its `.meta`, downloads, exports, importer outputs, materials, prefabs, font atlases, audio configuration and notice entries. List every shared scene/code/settings file changed, the baseline revision, and precise rollback steps. Import related content into an isolated folder; preserve subsequent user edits when undoing shared-file changes.

## Removed presentation pass: complete path inventory

All paths below are repository-relative. Folder ownership is **recursive**, including every file and `.meta` inside, plus the folder's adjacent `.meta`. The [file-by-file removal manifest](../unity/Logs/Task19/removed-files.md) records every original path and its inactive rollback location.

| Removed owned path | Contents / companion changes |
| --- | --- |
| `art/` | All source ZIPs, Lato font/OFL, bird recording and selected-file manifest |
| `unity/Assets/Art/` | Kenney Nature, Survival, Sky; ambientCG Ground048; models, textures and licenses |
| `unity/Assets/Resources/Presentation/` | Kenney UI sprites/audio; Lato source/font atlas/OFL; all presentation resources |
| `unity/Assets/Resources/TMP Settings.asset` + `.meta` | Assistant settings referencing the removed font; empty Resources parent and `.meta` removed |
| `unity/Assets/Settings/Presentation/` | Soil/sky materials and volume profile |
| `unity/Assets/StreamingAssets/ThirdPartyNotices.txt` + `.meta` | Kenney, ambientCG, Thimras and Lato notices; empty parent and `.meta` removed |
| `unity/Assets/Editor/CampPresentationSetup.cs` + `.meta` | Scene/import/lighting integration tool |
| `unity/Assets/Runtime/Player/FpsPresentation.cs` + `.meta` | Added shovel and audio presentation owner |
| `unity/Assets/TextMesh Pro/` + `.meta` | Standard TMP Essentials/Examples, shaders, sample fonts/materials/textures and demos; removal separately authorized by the user |

Shared files restored from baseline `6480694` before the separate rendering/rim cleanup:

- `unity/Assets/Scenes/MainGame.unity`
- `unity/Assets/Editor/MainGameSceneBuilder.cs`, `unity/Assets/Editor/SomethingDownThere.Editor.asmdef`
- `unity/Assets/Runtime/Player/FpsPlayer.cs`, `unity/Assets/Runtime/SomethingDownThere.Runtime.asmdef`
- `unity/Assets/Runtime/Terrain/TerrainChunkMesh.cs`, `unity/Assets/Runtime/UI/FpsHud.cs`
- `unity/Assets/Settings/SomethingDownThereURP.asset`, `unity/ProjectSettings/QualitySettings.asset`
- `unity/Assets/Tests/PlayMode/FpsUiInputTests.cs`, `unity/Assets/Tests/PlayMode/SomethingDownThere.PlayModeTests.asmdef`

The added DigSucceeded hook, texture UV changes, TMP references, scenery/viewmodel instances, ambience and footstep/dig sources are removed. The reimported TMP resource directory was removed only after the user separately requested that cleanup. The installed uGUI package itself remains.

Rollback copies live under ignored `unity/Logs/Task19/removed/`; they are excluded from Unity imports and builds. These are recovery copies, not approved content. Do not restore them without a new scoped user request. Source/license files remain with those inactive copies. Build output is regenerated from the restored scene.

## Current changes without new art

| Existing file changed by Task 19 | Reversal to baseline `6480694` (only these fields/hunks) |
| --- | --- |
| `unity/Assets/Scenes/MainGame.unity` | Restore Sun shadows, camera near/far planes and four wall bounds; no added art |
| `unity/Assets/Editor/MainGameSceneBuilder.cs` | Same Sun/camera/rim defaults as the authored scene |
| `unity/Assets/Runtime/UI/FpsHud.cs` | Restore status formatting, label sizes and text contrast effect |
| `unity/Assets/Tests/EditMode/MainGameSceneTests.cs` | Remove only the rim/wall overlap regression assertions if reverting the fix |
| `unity/Assets/Settings/SomethingDownThereURP.asset` | MSAA 4 -> 1; main-light shadow support false -> true |
| `unity/ProjectSettings/ProjectSettings.asset`, `unity/ProjectSettings/GraphicsSettings.asset` | Linear -> Gamma color space/intensity |
| `unity/ProjectSettings/QualitySettings.asset` | Restore per-level shadow settings and the active level's antialiasing |
| `unity/Assets/Settings/MainGame/{Bark,Bedrock,Foliage,SellAnchor,Soil,Surface,UpgradeAnchor,Water}.mat` | Unity rounded legacy `_Color` floats during color-space conversion; authored `_BaseColor` values remain unchanged |

Apply only the recorded changes when reverting; preserve subsequent user work. The [completion record](development/completed/19-presentation-rollback.md) records validation. Blender tool installation/configuration is separately inventoried in [the setup guide](../unity/readme.md#blender-mcp).
