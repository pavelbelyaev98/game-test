# Asset and audio ledger

Approved content and its removal boundaries are recorded below. The user owns the terrain work, including the explicitly requested original texture batch below. The removed presentation pass and TextMeshPro resources remain inactive.

## Tasks 113/114 - three appearances of one common rock

- Approval: the user explicitly requested this photo-referenced rock, then corrections, three appearances, random spawn rotations and game integration. **Only A retains one central hollow; B/C have solid uneven tops.** Narrow irregular undersides replace the rejected broad chest-like base. Trial integration is approved; final style/player-feel acceptance remains open.
- Source/license: original Blender MCP geometry/materials, using user photographs as visual reference without importing their pixels. `art/photo-rock/LICENSE.txt` permits commercial use, modification and redistribution without attribution. [Source, specs, previews and replacement](../art/photo-rock/README.md); [artifact checks](../art/photo-rock/validation.json).
- Source ownership (recursive): `art/photo-rock/`, including `.blend` sources/backups, recipe/catalog/manifest, `variants/{a,b,c}/` visual/collision FBX, textures, previews, validation/license and shared comparison previews. Blender data uses `SDT_PhotoRock` prefixes. Old single-model root exports/maps are superseded by the three canonical variant folders. Preserve existing Scene/SDT_Starter data and unrelated user work.
- Unity ownership (recursive): `unity/Assets/Content/PhotoRocks/` plus `PhotoRocks.meta`, including six FBX imports, nine maps, three URP materials, six centred meshes, three appearance prefabs, `RockContact.physicMaterial`, copied license and all importer/folder metas. Owned setup: `unity/Assets/Editor/PhotoRockSetup.cs` and `.meta`.
- Shared integration: `unity/Assets/Editor/StarterFindSetup.cs`, `Runtime/Interaction/{DiscoveryCatalog.cs,DiscoveryField.cs,BuriedFind.cs}`, `Content/StarterFinds/StarterFindCatalog.asset`, `Scenes/MainGame.unity`; `Tests/EditMode/StarterFindCatalogTests.cs`, `Tests/PlayMode/{FindPhysicsIntegrationTests.cs,DiscoveryIntegrationTests.cs,SaveIntegrationTests.cs,StartupMenuTests.cs}`. Shared docs: ledger, source/spec replacement guide, architecture, idea/starter/content/collection contracts, affected task/playtest files and status/queue.
- Handling updates `115`/`116`: source `catalog.json` owns `throw_speed`, synced to `FindPhysics`; carry/rest tuning belongs to runtime physics. Shared `FpsPlayer`/`FpsInput`/`InputPreferences`, `FindHandling.cs` (and meta), existing HUD/Controls files and input/physics/save tests own handling integration. `Tests/PlayMode/FindHandlingStabilityTests.cs` plus its `.meta` extends the shared physics test fixture. Preserve these generic systems when removing only the rock art; remove both partial physics test files together only when retiring that entire system.
- Identity/replacement: one catalog item `common_rock`; saved appearances `common_rock_a`, `common_rock_b`, `common_rock_c`. Keep these keys and existing metas while replacing geometry/maps. `art/photo-rock/catalog.json` owns price/slots/counts/dimensions/exposure/mass and export references; sync converts textures and regenerates centred meshes/hulls/exposure samples. Preserve source/license and update specs/validation together.
- Integration removal: first retain or explicitly migrate saved rock keys. Remove the `PhotoRockSetup.AppendToCatalog` hook from `StarterFindSetup`, remove the Rock catalog entry through the Editor, re-sync the bottle catalog and save MainGame. Then remove only the owned PhotoRocks folder/meta and setup script/meta. Keep generic appearance support, bottle entries/assets, terrain and saves; never delete a save to conceal missing content.
- Source removal: remove only `art/photo-rock/` (and its meta if created), owned `SDT_PhotoRock` Blender data and current source links. Preserve numbered/completed evidence, original user photos, terrain, bottle sources/imports and other open Blender work. No separate project notices or runtime loader was added.

## Tasks 108/109/110 - replaceable starter bottles

- Current approval: the user explicitly requests **only the three bottles, slightly larger, 60% exposure and physical falling when detached**, and full removal of cans/bricks. Task `110` revises the previously approved seven-model trial: bottles are 25% larger; retained models/materials are still **trial art, not final style acceptance**. Better replacements remain expected later.
- Source/license: original Blender MCP geometry/materials and original label layouts using Blender built-in outlines. No downloaded artwork or font file. `art/starter-finds/LICENSE.txt` permits commercial use, modification and redistribution without attribution; JulioVII's reference textures are not included. [Source/export guide](../art/starter-finds/README.md).
- Source ownership (recursive): `art/starter-finds/`, including `.blend` sources/backups, bottle/collision scripts, `collisions/` (3 convex FBX), collision manifest/validation, editable catalog, generated manifest, FBX, label-art masks, texture atlases, previews, validation and license. Owned Blender scenes/datablocks use `SDT_Starter`; preserve original Scene/Cube/Camera/Light and other user work.
- Unity ownership (recursive): `unity/Assets/Content/StarterFinds/` plus `StarterFinds.meta`. Includes `Models/` (3 visual bottle FBX plus 3 `_collision.fbx` hulls), `Textures/` (3 bottle PNGs), `Materials/Bottles.mat`, `Meshes/` (3 centred visual meshes plus 3 `_collision.asset` hulls), `Prefabs/` (3 stable bottle prefabs), `StarterFindCatalog.asset`, `BottleContact.physicMaterial`, license, and every folder/importer `.meta`. Source-only labels/fonts/preview staging are excluded. The physical contact material controls friction/bounce and adds no visible art.
- Owned setup: `unity/Assets/Editor/StarterFindSetup.cs` plus `.meta`; sync reads `art/starter-finds/catalog.json` and maintains the above generated assets without replacing their GUIDs. There are no audio, Resources loaders, additional fonts or ThirdPartyNotices entries in this batch.
- Shared integration (relative to `unity/Assets/`): `Scenes/MainGame.unity`; `Editor/MainGameSceneBuilder.cs`; `Runtime/Interaction/{DiscoveryField.cs,BuriedFind.cs,DiscoveryCatalog.cs}`; `Runtime/Terrain/TerrainVolume.cs`; `Runtime/Persistence/{WorldSnapshot.cs,WorldSaveCodec.cs,WorldSaveController.cs}`; `Tests/EditMode/{StarterFindCatalogTests.cs,WorldSaveTests.cs}`; `Tests/PlayMode/{DiscoveryIntegrationTests.cs,SaveIntegrationTests.cs,StartupMenuTests.cs}`. Reusable new `Runtime/Interaction/FindPhysics.cs` and `Tests/PlayMode/FindPhysicsIntegrationTests.cs`, both with `.meta`. Preserve new-file metas for catalog/tests from `109`. Save version 3 adds released state and retains v1/v2 readers; content aliases preserve retired identities.
- Removed by `110`: all four can/brick model sources/exports, their owned Blender geometry/material/label/image datablocks, six can/brick texture maps, two previews and three label masks; corresponding four Unity models/meshes/prefabs, two materials, six maps and all their metas. Recipes and current previews contain bottles only. Retired content strings remain exclusively for save compatibility, tests and concise removal/history records; they are not asset references or permission to restore removed art.
- Preserved separately: old `unity/Assets/Content/Finds/` prefabs/materials/metas, all user terrain/stations and user saves. Pre-session crash backups were kept as `unity/Assets/_Recovery/0.unity` with its `.meta` and `_Recovery.meta`; these are recovered user work, not starter assets and must not be removed as part of this batch.
- Replacement: follow the [complete reference/spec replacement guide](development/replacing-find-models.md). Edit source paths/maps/dimensions in the catalog, keep IDs, run sync, review partial burial and reload, and update owning docs. Models remain replaceable even after this integration task completes.
- Removal: prefer a compatible visual swap. For full removal restore the original three legacy prefab references, clear the catalog, restore 96/development gating and builder wiring through the Editor; remove the scoped physics component/test and revert field/find/motion-observation/terrain-restore hunks together, then remove the owned StarterFinds folder/meta and setup script/meta and rebuild. **Keep v3 save reading and content mappings** unless a tested downgrade is supplied; otherwise newer saves become unreadable. Remove source/owned Blender datablocks only if requested. Preserve all user work and saves; never reset a save to undo art.

## Task 77 - original soil and short turf

- Approval: user explicitly requested ground textures similar to A Game About Digging a Hole and said **"if creating u are allowed t ocreate it directly"**. This covers original soil/turf textures and their ground integration; online imports require a separate approval.
- Approved refinement: the user supplied a close-up and requested rocky texture instead of the uniform dots. Replace the existing soil maps with Blender-authored irregular embedded stones/mineral grit inside the same owned source/content folders; retain turf, geometry and existing asset GUIDs. Private provenance stays in development records; no authoring-tool credit is added to game UI.
- Subsequent approved tuning: fewer and less conspicuous stones with adhering dirt; rebake the same three soil maps from the retained recipe/source. Ownership and removal boundaries remain the same.
- Source/license: original Blender MCP authoring; no third-party artwork. `art/ground-textures/LICENSE.txt` grants free commercial use, modification and redistribution without attribution. The [official screenshots](https://store.steampowered.com/app/3244220/A_Game_About_Digging_A_Hole/) are visual reference only, never game content.
- Owned paths (recursive): `art/ground-textures/` (editable `GroundTextures.blend`, `create_ground.py` turf recipe, `rocky_soil.py` soil recipe, license and previews); `unity/Assets/Content/GroundTextures/` plus `GroundTextures.meta` (soil/turf albedo, normal and roughness PNGs, shared ground material, triplanar shader, license and all importer/folder `.meta`). Soil roughness packs contact occlusion in G; soil/turf cover 2 m/1 m respectively. Owned integration tool: `unity/Assets/Editor/GroundTextureSetup.cs` plus `.meta`.
- Shared integration: `unity/Assets/Scenes/MainGame.unity` changes `TerrainVolume.soilMaterial`, its edit-mode preview and four rim renderer material references (Unity also serializes the existing crouch defaults); `unity/Assets/Editor/MainGameSceneBuilder.cs` calls the setup for newly created scenes; `unity/Assets/Tests/EditMode/MainGameSceneTests.cs` recognizes the approved terrain shader and checks its assigned textures. Original `Settings/MainGame/Soil.mat` and `Surface.mat` remain user-owned.
- Removal: through the Editor restore the terrain/preview to `Assets/Settings/MainGame/Soil.mat` and four `Surface/{North,South,East,West} rim` renderers to `Surface.mat`; save MainGame. Remove the builder's `GroundTextureSetup.Configure()` call and the corresponding ground-specific test assertions; then remove the owned integration tool/metas and both owned folders/metas. Rebuild Windows to remove packaged textures. No notices, runtime loaders, extra scene objects or other config changes are owned by this batch. Preserve existing user terrain/materials, saved excavation and all other content.

## Task 12 - approved surface stations

- Approval: user replied **"Approve these two station models"** to the specific batch: amber salvage buyer with weighing tray/intake flap/SELL sign, and teal workbench with drawers/vise/UPGRADES sign. No separate shovel, scenery or audio is approved.
- Source/license: original project art authored through Blender MCP; no third-party art, downloads or attribution. Retained source: `art/stations/Stations.blend`. Materials/textures are authored and baked in Blender as part of these models.
- Owned files: `art/stations/` recursively (source and any source backups); `unity/Assets/Content/Stations/` recursively plus `Stations.meta` (FBX exports, baked textures, extracted/imported materials, two prefabs and all companion `.meta`).
- Integration: replace only `MainGameRoot/Surface/{SellStation,UpgradeStation}` pedestal children in `MainGame.unity`; `Assets/Editor/SurfaceStationSetup.cs` owns repeatable wiring, and `MainGameSceneBuilder.cs` calls it for new scenes. Shared `FpsPlayer.cs`, `FpsHud.cs`, `WorldActionContracts.cs` integrate reusable transactions/UI, independently of the art.
- Removal: remove the two imported model instances through the Editor; restore `Selling pedestal`/`Upgrade pedestal` at their anchors with local position `(0,0.6,0)`, scale `(1.2,1.2,1)` and existing `Assets/Settings/MainGame/{SellAnchor,UpgradeAnchor}.mat`. Remove station components/colliders added by setup, its builder call and `SurfaceStationSetup.cs`/`.meta`; delete only the owned source/content folders and `Stations.meta`. Preserve all user-owned terrain/materials, Finds, existing anchors/recharge and reusable transaction/UI code.

## Task 27 - user-requested simple finds

- Approval: the user explicitly requests random small objects/circles to see excavation and collection. Use three colored sphere/disc forms (blue marble, copper token, amber bead) only; this scoped request supersedes the primitive-art restriction for these development finds. No external art/audio or unrelated scenery.
- Source/license: Unity built-in Sphere mesh, used under the project's Unity Editor license; project-authored material colors and prefabs, no third-party download or attribution. Intended files: recursive ownership of `unity/Assets/Content/Finds/` plus its `.meta` (three prefabs/materials and all companion `.meta`); new parent `Content.meta` if needed.
- Integration: `MainGame.unity` has a `DiscoveryField` referencing these prefabs; `MainGameSceneBuilder.cs` maintains that wiring. Runtime `DiscoveryField`/`BuriedFind`, terrain change notifications and existing player/HUD/input provide behavior independently of this art.
- Removal/replacement: replace the field's three prefab references through the Editor with approved content, or remove its `Discoveries` scene object and the discovery setup call in `MainGameSceneBuilder`. Then remove the owned Finds folder/metas (and empty Content parent/meta only if no other content exists). Preserve terrain, existing materials, user assets and reusable collection/admin code. Replacement art must retain each prefab's `saveContentId` (copied from its `.meta` GUID) or provide a compatibility map so existing saves still resolve collected and buried finds.
- Approved resize (`30`): marble 0.8/0.8/0.8 m, token 1.0/0.18/1.0 m, bead 0.64/0.90/0.64 m. To undo only the resize, restore 0.4/0.4/0.4, 0.5/0.09/0.5 and 0.32/0.45/0.32 through the Editor and restore the matching placement spacing in `DiscoveryField`/`MainGameSceneBuilder`. Pickup tuning belongs in the [collection contract](features/backlog/discovery-collection.md).
- Production TODO: replace the simple prefabs under Task `09`; they are enabled only for development through `DiscoveryField` until final-art acceptance. Keep the gameplay systems.

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
