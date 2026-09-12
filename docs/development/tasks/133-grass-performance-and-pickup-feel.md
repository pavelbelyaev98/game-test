# Task 133 - Grass performance, patchy growth and quick pickup

Type: implementation. Status: `done`. [Completed result](../completed/133-grass-performance-and-pickup-feel.md). Prerequisites: 129/131/132 delivered. Explicitly selected by the user on 2026-09-12, ahead of 126.

Features: [art style](../../features/backlog/art-style.md), [collection](../../features/backlog/discovery-collection.md). Repository policy: [validation](../../scope-and-validation.md).

## Scope and research

- Profile the current grass and separate its contribution from other scene costs. Research established grass rendering approaches using primary sources; select the measured, maintainable improvement for this small editable site.
- Replace uniform growth with coherent bare-earth openings and fuller/taller grass areas. Share one stable world-space distribution between the approved ground and 3D grass, retain excavation support and avoid the rejected distance-thinning/pop-in behavior. Refine existing approved Blender grass if needed; no unrelated art.
- Make pickup a quicker direct pull toward the player, with little shrink and no ornamental arc/rotation. Preserve committed identity, collection authority, pause/cleanup and full-bag behavior.
- User requests removing the requirement/assumption of permanent review images. Update guidance, remove completed temporary art/reference/capture galleries and their links, preserve actual source/runtime assets and the pending reservoir approval preview. Use temporary captures for this task, then clean them up.

## Acceptance

- Record matched grass-on/off and before/after rendering measurements, resolution/hardware and limits. Explain the actual observed bottleneck without equating triangle counts with FPS.
- MainGame shows attractive irregular earth openings and varied taller grass, stable on approach and after local digging/reset/restore. Reduce grass rendering cost substantially while retaining rooted natural wind and coherent lighting.
- Pickup starts moving immediately, disappears quickly and avoids excessive shrink/twisting; relevant collection and cleanup checks pass.
- Fast compile plus relevant integration checks pass; inspect through official Unity CLI, deliver Windows build, clean temporary images and update task/status/asset ownership.

## Questions

The user asked us to measure rather than guess where the slowdown occurs. Native fixed-camera on/off captures confirm a grass GPU cost; retain the actual resolution and do not label a surface-only sample as digging qualification. No new asset approval is required for these explicitly requested refinements of existing approved grass/ground and find visuals.

## Research and selected approach

- [Unity RenderMeshInstanced](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Graphics.RenderMeshInstanced.html) batches up to 1023 uniformly scaled instances and culls each submission as a group. Keep small patch visibility checks, then compact visible matrices into a few reusable batches.
- [AMD grass rendering research](https://gpuopen.com/learn/mesh_shaders/mesh_shaders-procedural_grass_rendering/) identifies geometry/detail reduction and preserving apparent coverage as central grass optimizations. Here, retain all twelve blade roots/tips at both geometric detail levels; avoid the density loss already rejected by this user. Existing Blender meshes and normal instancing fit the supported Unity renderer.
- [Unity indirect rendering](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Graphics.RenderMeshIndirect.html) permits GPU-managed draw arguments and requires compute support. Native grass submission costs only about 0.11 ms, versus about 1.9 ms of added GPU frame time; indirect/mesh-shader infrastructure would not address the main measured cost. Reduce visible geometry/coverage first.
- [Frame Timing Manager](https://docs.unity3d.com/6000.0/Documentation/Manual/frame-timing-manager.html) supplies native CPU/GPU timing. Use a separate temporary validation scene and additive MainGame with no world-save session or preference edits, uncapped matching 2560x1440 DX12 runs, alternating on/off states and warm-up. The headless batch-mode attempt rendered nothing and was discarded; the recorded windowed captures have actual grass draw calls and GPU timings.
