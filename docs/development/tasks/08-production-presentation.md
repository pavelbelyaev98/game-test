# Task 08 - Production visual and audio foundation

Type: implementation. Status: `planned`. Prerequisites: 57, the accepted style from 105 and the existing user-selected art scope/specific batch approvals. `105`'s trials do not automatically activate this broader production pass.

Feature: [presentation audio](../../features/backlog/presentation-audio.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Consume [105's shared cartoon guide](../../features/backlog/art-style.md): bright color, broad shapes and clear authored detail. The user accepted Sunny r8 soil/turf and natural daylight on 2026-09-12; preserve that baseline while implementing the remaining presentation.

- Before any new asset/audio enters the project, explain the specific item or listed batch, purpose, source/license, files/integration and removal steps; show a preview/sample when available, ask and wait for explicit user approval. Record the approved scope in the ledger. A feature request or assumed necessity is not approval.
- Replace remaining player-visible primitive/flat placeholder presentation in `MainGame.unity` with the specifically approved coherent asset set. Preserve the completed `12` amber SELL/teal UPGRADES station pair and the user's terrain ownership; do not replace already-finished stations as if they were still pedestals.
- Create visual assets through Blender MCP or download free-to-use assets with an explicitly verified commercial-use license. Retain Blender sources/exports or source/license links and record everything in `docs/asset-ledger.md`.
- Add commercially licensed audio only when explicitly requested for a specific interaction. Use no music or voice acting.
- Runtime-generated excavation geometry may remain because it is a mechanic, but it must receive approved materials and fit [105's accepted style](../../features/backlog/art-style.md). Reuse its accepted assets/evidence where applicable; do not choose a conflicting texture pack or redo successful trials without an actual behavior/integration change.
- Improve lighting, composition, scale, boundaries, water/scenery, HUD readability, and camera comfort as one coherent pass; preserve the working settings from `65` through presentation refinement.
- Refine the existing UI Toolkit HUD and menus from [74](74-ui-toolkit-menus.md)/[75](75-ui-toolkit-hud.md) using the official UI Toolkit skill and shared UXML/USS. Retain Input System navigation and verify focus, scrolling, clipping and layout at supported window sizes. No TextMeshPro migration is pending. This supplies the final presentation required to revalidate Tasks `05`-`07` before `09`.
- Reproduce and remove flicker at the surface/excavation boundary after repeated digging; verify from moving player viewpoints while preserving collision boundaries.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect the current approved/user-owned scene and use the accepted [57 presentation brief](57-presentation-design.md). Research implementation-specific contact/reveal readability and sample fatigue. Prepare and obtain explicit approval for the actual asset/audio batches; the brief does not bypass the existing deferred art scope.
- Any approved frequent digging/motor/jetpack/station sounds need restrained levels, appropriate variation and clean start/stop/layering. Compare short repeated sessions before accepting samples; `10` owns detector feedback and `54` reviews full-session fatigue. No extra ambience pack, music or voice acting is authorized by the research.

Do not create substitute assets with Unity primitives, generated meshes/materials, code, or image generation. If Blender MCP is unavailable and suitable licensed assets cannot be obtained, mark the task blocked and ask the user.

## Moving-grass handoff from 105

Future implementation, not delivered by the completed style-selection task. The user accepted Sunny r8 on 2026-09-12. Specific asset-batch approval is still pending; completing 105 does not approve this addition. Retain the [comparison/reference brief](../art-review-105/ground-brief.md) and preserve the accepted soil/turf and daylight.

- Original Blender-authored short grass clump: 12 tapered, bent blades, approximately 10-24 cm high, plus a reduced six-blade distance mesh. Blade roots are darker and tips lighter within Sunny's palette. No flowers, props, audio or third-party artwork.
- A small Blender-baked color/roughness atlas supplies authored color variation. Opaque two-sided blade geometry avoids large overlapping transparent cards. Vertex wind weights leave roots anchored.
- Source/license: original Blender MCP authoring, project-owned; the existing original-ground commercial-use license applies without attribution. Sources under `art/ground-grass/` (recipe, `GroundGrass.blend`, exports, atlas, license); runtime ownership under `unity/Assets/Content/GroundGrass/` plus folder/importer `.meta` files (FBX meshes, atlas, material, wind shader, license).
- Integration: `SurfaceGrassRenderer.cs` beside TerrainVolume, editor setup, and MainGame scene references. Draw instanced spatial patches; cull off-camera/distant patches, thin at distance, animate in the vertex shader. Re-evaluate only patches touched by excavation; no blade GameObjects/colliders or per-frame terrain raycasts. Suppress unsupported roots and rebuild from restored terrain state.
- Removal: remove the SurfaceGrassRenderer component from MainGame and its setup hook, then delete only the two isolated owned folders/metas and the renderer/setup scripts/metas. Restore the saved scene through the Editor and rebuild Windows. Soil/turf textures, player saves and pre-existing content remain intact.
- Visual target: the user's third screenshot supplies the upright grassy silhouette; the first/fourth supply the lip treatment. Keep blades short enough to see exposed finds. Deliver current-vs-revised matched images and a Windows build, with measured patch/triangle/draw costs rather than an unsupported optimization claim.

## Acceptance

- Nothing visibly presented as final content is an unlabeled primitive, debug object, flat placeholder material, or validation adapter.
- The excavation entrance, permanent boundaries, surface stations/anchors, environment, and HUD read clearly in the Windows build.
- Apply `57`'s accepted affordance and feedback hierarchy to the existing stations/HUD. Observe whether an uncoached player understands banked proceeds, recharge and purchase benefit without extra service steps; test adjacent high-priority messages and decorative props that could be mistaken for usable ones. Keep `12`'s transactions and finished station ownership intact.
- Repeated sounds are restrained and varied; important interaction/detector space remains audible without music.
- Asset-ledger entries contain source, exact commercial license or Blender source, attribution, imported files, and approval state.
- When the grass batch is approved, verify anchored wind, distance thinning, off-camera culling, unsupported-root removal after digging and exact regeneration after terrain restore; inspect find visibility and record draw/triangle/frame costs.
- Inspect the result through the official Unity CLI when available, rebuild `SomethingDownThere.exe`, and provide screenshots plus the executable for user review.
