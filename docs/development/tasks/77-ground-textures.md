# Task 77 - Original ground textures

Type: implementation. Status: `done`. Prerequisites: completed terrain shell.

Feature: [terrain materials](../../features/backlog/terrain-materials.md). User-selected task, ahead of the normal queue.

Delivery record: [77](../completed/77-ground-textures.md), including the user's rocky-soil refinement.

## Scope and decisions

- The user requested ground textures similar to A Game About Digging a Hole and explicitly authorized creating them directly; online asset imports still require separate approval.
- Create original seamless garden soil and short turf through Blender MCP, retaining editable `.blend` source and texture exports. Use the reference game's official screenshots for broad visual direction only; do not copy its artwork.
- Integrate the textures on existing excavation/rim surfaces in MainGame with consistent scale and mapping on fresh vertical/diagonal cuts. Preserve existing terrain ownership, geometry, saves, resistance, boundaries and other scenery.
- Keep this surface-art delivery separate from the geological vocabulary/resistance decisions in `58`/`39` and the broader presentation/lighting work.
- User review rejects the round, uniform mineral dots. The supplied close-up selects warm granular earth with irregular embedded grey/brown stones, chipped silhouettes, varied sizes and visible rocky relief. Replace the soil bake with original Blender stone/soil detail; keep turf and excavation geometry unchanged. This is refinement of the already authorized texture batch.
- Further user review selects fewer, less conspicuous and dirt-covered stones. Reduce large/gravel density and exposed height, blend stone coloration into granular earth and increase roughness; retain fine soil texture. Delivered alongside `79`.

## Research and acceptance

- Inspect existing materials and runtime mesh mapping before integration; validate tiling, surface-to-soil transition, close detail and distant repetition in the live Editor through official Unity CLI.
- Inspect recognizable irregular stones at normal digging distance and fine grit close up; reject circles, uniform flecks, flat stone silhouettes and conspicuous repetition. Reference pixels are not copied into the texture.
- Retain commercially usable original source, exports, ownership/license and exact removal instructions in the asset ledger.
- Run shader/compile checks, relevant terrain/scene integration checks, inspect fresh cuts and build `builds/windows/SomethingDownThere.exe` for user review.
- Update current evidence and promote the next eligible task after acceptance. No unanswered product question blocks this explicitly requested texture batch.
