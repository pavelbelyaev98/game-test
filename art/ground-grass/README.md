# Sunny surface grass

Approved in 127 and refined at the user's request in 129 and 133.

- `GroundGrass.blend` retains editable grass geometry/materials and source staging. Near and far meshes both retain twelve blades, their original roots/tips, UV palette and motion data. Near detail is 168 triangles; distant detail is 72. Authored heights are 26-42 cm; seeded uniform clump scale varies the game blades to roughly 25-63 cm.
- `create_grass.py` creates the original foundation in a fresh owned scene. `refine_grass.py` authors the taller curved near mesh and atlas; `optimize_grass.py` follows it to rebuild the current twelve-blade far mesh and export both. Use Blender MCP and set `__file__` to the recipe's absolute path. Temporary source previews may be rendered for inspection, then cleaned up.
- `GrassClumps.fbx` exports both metre-scale meshes. UV0 samples the existing 128x256 `Grass_Albedo.png` and supplies bend weights; UV1 retains each blade's phase and height. The near mesh and atlas are unchanged by 133.
- Runtime: `Assets/Content/GroundGrass/`; **Configure Approved Surface Grass** updates references/import settings and the eight-cell-per-2m-patch candidate grid without replacing GUIDs. 135 restores even coverage over the whole supported surface with seeded spacing and height variation; the clearing mask is removed.
- Grass grows only on supported original surface; scaled footprint checks clear blades near excavated lips. Local dirt edits rebuild only touched patches. Reset/restore and camera movement never reroll growth or alter its coverage.
- Small spatial patches are culled and compacted into up to 1023 instances per draw. Beyond 16 m from a patch's expanded bounds, the simpler mesh retains all twelve blades. No distance density thinning. Travelling gusts, per-blade response, bent normals, sunlight/shadow reception and fog remain; grass casts no shadows.
- `previous-approved.zip` retains the pre-129 source/runtime baseline. `previous-129.zip` retains pre-133 source/exports and affected renderer/setup/ground shader. Exact restoration/removal is in the [asset ledger](../../docs/asset-ledger.md); preserve independent saves, ground art, sun and shovel changes.

135 retires the bare islands/strips from 133/134. `previous-133.zip` retains the earlier layout for source history only; do not restore clearings without a new request.
