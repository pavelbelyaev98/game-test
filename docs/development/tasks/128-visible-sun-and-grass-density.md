# Task 128 - Visible sun and consistent grass density

Type: implementation. Status: `done`. Prerequisites: 127 delivered; explicit sun-batch approval received before import. [Completion](../completed/128-visible-sun-and-grass-density.md).

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md). User-selected follow-up, 2026-09-12. Next remains 126.

## Selected scope

- Fix the visible thin-to-thick grass change reported while moving. The prior renderer switched entire 2 m patches to half the clumps and six instead of twelve blades at about 9 m; this was a cost reduction, not evidence of a user hardware fault.
- Keep the approved full clump/mesh density across the small 24×24 m site, preserving spatial view culling, anchored wind, seed layout and local excavation support. Measure added triangle/frame cost and inspect movement across the old boundary. No new grass art.
- Create one visible warm cartoon sun with a restrained halo, using an original Blender-rendered RGBA disc projected in the sky at the existing directional light's angle. Preserve blue background, daylight/shadows and world geometry.
- Approved source ownership: `art/sun/` recursively (blend, recipe, disc texture, preview, license). Runtime ownership: `unity/Assets/Content/Sun/` recursively and folder meta (disc, sky shader/material, license); editor setup and MainGame camera skybox references.
- License: original project-owned Blender MCP artwork, free commercial use/modification/redistribution/sale, no attribution. The user explicitly replied **"Approve this sun"** to the concrete preview and integration/removal proposal on 2026-09-12, before import; [ledger](../../asset-ledger.md#task-128---approved-visible-sun).
- Removal: remove the camera's local Skybox component, restore its SolidColor clear mode, remove sun setup/builder wiring and isolated Sun assets/source/metas, then rebuild. Keep the existing directional light, grass and ground unchanged.

## Acceptance

- Approve the concrete sun preview before import; ledger owns exact files/integration/removal.
- Grass renders the same supported clumps/blades on both sides of the old detail boundary; off-camera culling and excavation/reset/restore remain correct.
- Sun appears in the correct light direction, stays fixed as the player walks/turns, and remains behind terrain/objects. Preserve accepted sky/lighting away from the disc and inspect from the lawn and underground.
- Run proportionate deterministic/integration checks; inspect MainGame using official Unity CLI, record measured grass cost and deliver the Windows build with review images.

## Current result

- Approved sun and full-density grass are integrated; [Sun and grass validation record](../completed/128-visible-sun-and-grass-density.md). Grass and scene integration tests pass; original 20,451-clump placement hash is preserved. Sky pixels away from the disc and the translated sun/halo region match exactly; a soil ceiling occludes the sun from a validation-only air pocket.
- Widest lawn view: 128 batches / 3.06 million grass triangles versus 1.24 million previously. A 180-frame local Editor sample records 8.56 ms median / 12.31 ms p95 frame time and 0.16 ms median CPU submission. Native/minimum-hardware qualification stays 54.
- Windows build 2026-09-12 15:17 UTC succeeds with zero errors and one existing Pipeline warning; seven-second native startup is clean. [Detailed evidence](../../../unity/Logs/Task128/). Next: 126; preserve 127's gentler shovel baseline.
