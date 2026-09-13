# Task 105 - Living ground comparison

Accepted: **Sunny r8 ground and natural daylight**, 2026-09-12. The user called the current result “perfect” and authorized closing 105. Preserve its palette, sparse solid mineral faces, thin turf edge and gradual daylight falloff. Moving grass is separately delivered by 127; reference artwork is never extracted for game assets.

| View | Reference observation | Sunny r2 gap | Required revision |
| --- | --- | --- | --- |
| Lawn at eye level | Upright tapered blades break the silhouette; exposed short turf remains visible | Flat continuous carpet, no wind or depth | Short clumps with anchored roots, gentle coherent wind and varied density |
| Cut rim | Ragged grass fringe hangs over a shallow turf band | Abrupt slope cutoff exposes bare soil at the lip | Authored blade/root mask, narrow irregular height transition, no grass on deep walls |
| Close soil wall | Small mineral grains and recognizable scattered embedded stone faces | Earth and stones merge into uniform brown noise | Distinct sparse dirt-coated stone faces, bounded fine grains, stronger local value separation |
| Deeper excavation | Shadowed recesses preserve warm lit earth and cooler shade | Detail and shape separation weaken together | Inspect shading with actual MainGame lighting; retain readable underground surfaces |

## Moving-grass handoff

The original Blender grass batch was explicitly approved and delivered in [127](../tasks/127-moving-grass-and-shovel-strength.md); see its [Grass implementation record](../completed/127-moving-grass-and-shovel-strength.md). It preserves the separately accepted r8 ground/daylight.

## Technical sources

- [Unity RenderMeshInstanced](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Graphics.RenderMeshInstanced.html): instanced mesh submission and group bounds. Use small spatial groups and conservative per-draw counts; Unity culls a submitted group together.
- [Unity instancing shader support](https://docs.unity3d.com/6000.0/Documentation/Manual/gpu-instancing-shader.html): explicit instance macros and shader variants.
- [Unity SSAO controls](https://docs.unity.cn/6000.0/Documentation/Manual/urp/ssao-renderer-feature-reference.html): half-resolution processing, radius/falloff and sample-cost tradeoffs. R6 retains restrained contact shading at full resolution alongside four-cascade sunlight; no new package is added.
- [Official reference gallery](https://store.steampowered.com/app/3244220/A_Game_About_Digging_A_Hole/): visual comparison only. Internal renderer, source texture sizes and performance cannot be determined from stills.

Current result: r8 retains all 110 r7 stones/chips and changes only their albedo coating; the soil outside the stone mask is pixel-identical. R7 normal/mask and r6 turf maps remain unchanged. The shader uses coherent mineral selection through projection overlaps. Task `145` subsequently raises the ambient minimum from 0.14 to 0.45 for mineral readability at 32 m; connected-air falloff, sun occlusion and full surface daylight remain. The original review inspected twelve matched r7/r8 views and two closer mineral views; completed review captures have been removed. Seven targeted tests and a 7-second native headless startup check pass; Windows build 2026-09-12 14:01 UTC. Ground/style acceptance is complete. Moving grass is separately implemented in 127, final props remain with 08/09/11, and native graphics/performance qualification with 54.

## Image and source retention

- Selected runtime art remains unchanged: six PNGs plus their existing `.meta` files under `unity/Assets/Content/GroundTextures/`. Their six source exports remain in `art/ground-textures/trials/` (r8 albedo, r7 soil normal/mask and r6 turf).
- `GroundTextures.blend` retains editable source geometry/materials, all recipes and the six selected packed cartoon bakes. Forty superseded PNG exports and their old packed cartoon bakes were removed at the user's request; old treatments can be regenerated from the retained sources.
- `images/` now contains only six representative accepted r8 screenshots, used by the [Accepted art direction](../../features/backlog/art-style.md). Superseded/duplicate gallery images and all temporary image copies under `unity/Logs/Task105/` were removed. Validation reports and code/config baselines remain; old reports describe captures that were subsequently pruned.
