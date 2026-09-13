# Depth-progression minerals

Approved original Blender MCP batch for [Task 145](../../docs/development/tasks/145-depth-mineral-progression.md): Coal, Copper, Iron, Silver, Gold, Emerald, Ruby and Diamond. [License](LICENSE.txt) permits commercial use with no attribution. [Ownership/removal](../../docs/asset-ledger.md#task-145---depth-progression-minerals).

`catalog.json` owns stable `mineral_<name>` IDs, source filenames, Blender XYZ dimensions in metres, prices, counts, centre-depth ranges, exposure and throw speeds. One slot, 60% exposure and ordinary detector-silent pickup/handling apply to every entry. Existing saves keep their populations and historical prices; New Game uses this complete roster.

`Minerals.blend` retains the original editable scene, procedural material graphs and packed baked textures. `create_minerals.py` is the Blender MCP recipe; execute in Blender with `__file__` set to its source path. It creates an isolated `SDT_Minerals` authoring scene and preserves existing unrelated scenes. Use a fresh isolated authoring scene for regeneration; do not duplicate source datablocks in a user's working scene. An optional `--preview` argument renders a temporary review image; delete it after review.

Each lowercase mineral folder contains one visual FBX, a separate fitted convex collision FBX (49–69 vertices), and 512px BaseColor, tangent-space Normal and Masks PNGs. Masks pack metallic in R, occlusion in G and smoothness in A. Original source shaders, texture grain and shaped ore/crystal geometry supply the appearance. `manifest.json` records dimensions and geometry budgets. The preview labels, backdrop and studio lights stay in the Blender source only.

In the saved MainGame scene, run **Tools > Something Down There > Sync Mineral Models** through the official Unity CLI. The importer maintains isolated `Assets/Content/Minerals/` outputs and stable prefab keys, merges with retained rocks/bottles and validates the complete catalog. It never opens or rewrites player saves. Sync after intentional source/spec changes; preserve `.meta` and existing IDs when replacing models.
