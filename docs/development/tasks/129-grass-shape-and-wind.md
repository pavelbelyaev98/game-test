# Task 129 - Taller grass and natural wind

Type: implementation. Status: `done`. [Completion](../completed/129-grass-shape-and-wind.md). Prerequisites: 127/128 delivered. Explicitly selected by the user on 2026-09-12; next remains 126.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md).

## Selected scope

- Refine the existing approved grass source, export, atlas and wind material in place, following the user's request for taller, prettier grass and more natural motion. Their supplied image supplies the visual reference; a still image does not establish the reference game's exact animation.
- Longer curved blades, gentler folds and restrained blade-to-blade green variation, retaining the bright cartoon palette and accepted turf underneath. Preserve the twelve-blade topology and full density throughout the visible site.
- Spatial travelling gusts, varied blade response and soft tip movement, with fixed roots and normals following the bend. Expand culling bounds for the taller moving mesh.
- Preserve excavation support, seeded roots, reset/restore, saves, sun, soil, shovel reach and 127's gentler digging.
- Refine the already approved item through Blender MCP, retain the editable source and exports, preserve file names/metas/license, and update the existing ledger ownership.

## Acceptance

- Inspect player-height lawn, near blades, movement and excavation through official Unity CLI. Retain a matched before/after and animated wind review.
- Root positions stay fixed; nearby blades respond differently within a coherent travelling gust. No density switches, moving roots, culling of tall tips or grass hovering over a dig.
- Relevant scene/grass integration checks pass, including authored scale/motion data and conservative bounds. Record rendering cost without claiming native hardware qualification.
- Deliver the Windows build, concise evidence and updated status. Subjective prettiness remains available for user review.

## Current result

- Existing source refined through Blender MCP: 26–42 cm blades, 2.15 times the prior mean height, unchanged twelve-blade / 168-triangle topology and anchored roots. Full density remains, with travelling gusts, per-blade tip response and bent normals.
- Scene and grass integration tests pass, including motion data and tall-tip culling. [Grass validation record](../completed/129-grass-shape-and-wind.md); digging cleared 33 clumps. A 180-frame Editor sample records 132 draws / 3.15 million grass triangles, 7.95 ms median / 8.74 ms p95 frame time and 0.17 ms median grass CPU submission. Hardware qualification remains 54.
- Windows build 2026-09-12 15:37 UTC: zero errors, one existing Pipeline warning; seven-second native startup is clean. [Evidence](../../../unity/Logs/Task129/). Subjective feel remains available for user review; next is 126.
