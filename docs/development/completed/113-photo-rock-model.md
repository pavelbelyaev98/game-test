# Task 113 - Larger photo-referenced rock

Superseded source revision: [114](../tasks/114-rock-variants-integration.md) corrects the shape, creates three appearances (only A hollow), and integrates the Rock. Links below now resolve to the current replaceable source.

- Why: the user requested a larger Blender find based on four views of one angular grey-green rock.
- Result: [editable source and replacement guide](../../../art/photo-rock/README.md), estimated 70 x 55 x 45 cm, broad fractures, uneven top grooves, grain/pitting, intermittent cracks and sparse pale weathering. Original Blender MCP geometry/materials; all three 2K maps packed into the isolated source.
- Delivery: `PhotoRock.blend`, visual FBX (12,500 triangles), separate convex collider FBX (84 triangles), reproducible recipe, manifest/license and [four rendered views](../../../art/photo-rock/previews/hero.png). Existing Blender scenes were preserved.
- Evidence: [saved-source and FBX round-trip validation](../../../art/photo-rock/validation.json) passes for actual dimensions, closed finite geometry, outward normals, nondegenerate UVs, convex collision and packed maps. Final baked front/rear/top/hero views inspected; both recipes compile.
- Limitation: source asset complete; user visual acceptance and Unity population/material/physics integration remain pending. Price, slots and population are unset. The current Windows build still contains the three bottle variants from `110`/`112`.

[Numbered task](../tasks/113-photo-rock-model.md) | [Ownership](../../asset-ledger.md) | [Content contract](../../features/backlog/discovery-content.md)
