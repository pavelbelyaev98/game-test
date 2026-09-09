# Original garden ground

Two seamless surfaces: warm granular soil with sparse, partly buried, dirt-coated stones (two-metre tile) and short olive-green turf (one-metre tile).
Original Blender MCP work for [Task 77](../../docs/development/tasks/77-ground-textures.md); commercially usable under [LICENSE.txt](LICENSE.txt), without attribution.

- `GroundTextures.blend` retains the editable soil matrix, wrapped distorted stone geometry, grass-blade geometry and packed texture bakes. Embedded texts contain both authoring recipes and this guide.
- `rocky_soil.py` owns the Soil exports. Run inside Blender: `bind_source()`, `create_revision()` only if absent, then `bake(channel)` for `Albedo`/`Normal`/`Roughness`, followed by `save_source()`. `rebuild()` regenerates only the owned rocky source after recipe edits. Set `ROOT` for another checkout.
- `create_ground.py` owns the existing turf and its underlay. Use `bind_existing()` in the retained file, then `bake_map('Turf', channel)` and `save_source()`. A fresh recreation starts with `initialize()` here, followed by `rocky_soil.py`. The obsolete dot-based soil bake is guarded against overwriting the rocky revision.
- Six 2048-square PNG exports live in `unity/Assets/Content/GroundTextures/`: sRGB albedo, tangent-space +Y normal and linear surface masks. `Soil_Roughness` packs roughness in R and contact occlusion in G (BC7); turf roughness uses R (BC4). Albedo uses BC7 and normals BC5; all use mipmaps, Repeat and anisotropic filtering.
- MainGame uses the shared `GardenGround` material. Triplanar soil mapping stays anchored to world space with two-metre coverage and baked stone relief/contact shading. Short turf keeps one-metre coverage and appears only near the original top surface. No stones, meshes or colliders are added at runtime.
- The asset ledger owns integration/removal boundaries. These textures do not imply a new geological resistance type.
