# Task 134 - Deliberate surface layout, stones and 3D clouds

Current appearance is superseded by [135](../completed/135-restore-grassy-site.md): the user restores continuous grass and removes clouds and decorative surface stones. Retain this record as prior evidence only.

Type: implementation. Status: `done`. [Completed result](../completed/134-authored-surface-and-volume-clouds.md). Prerequisites: 132/133 delivered. Explicitly selected by the user ahead of 126.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md). Context: [133 measurements](133-grass-performance-and-pickup-feel.md), [132 cloud revision](132-brighter-sparse-clouds.md).

## Selected scope

- User rejects the noisy, largely bare grass-island layout. Make grass the continuous majority, with a deliberate winding stony opening and subordinate edge clearings; use modest edge irregularity instead of noise defining the overall composition. Keep the shared ground/clump field, rooted natural wind and stable visible blade coverage.
- Place small groups of the already approved solid rock B/C meshes in exposed upper ground. These small embedded surface stones are scenery, distinct in size from collectible rocks; no new loot, colliders or save identities. Remove unsupported dressing after digging and derive it again after reset/restore.
- User rejects flat clouds facing the player. Reuse/refine the approved editable Blender cloud forms into actual 3D volumes, horizontal at a common distant altitude with fixed world orientations and gentle translation. Retain sparse pale clouds, bright sky/sun and world lighting. Retire the sky atlas projection; no camera-follow/billboard rotation.
- Measure rendering while travelling/turning and digging in an isolated native fixture. Keep 133's fixed-view result as a baseline, and report spikes and remaining limits honestly; never promise all FPS drops are eliminated.
- Existing asset reuse/refinement is specifically requested and already approved; update exact derivative ownership/removal before integration. Preserve pending reservoir work, actual artwork, user scenes and saves; clean temporary captures.

## Acceptance

- MainGame player-height inspection shows a predominantly grassy surface, deliberate connected clearings and sparse embedded stones. World placement and visibility stay stable across camera movement and local excavation/reset/restore.
- Clouds are depth-bearing Blender meshes whose orientation is independent of player pose. Inspect from below and different positions; sparse horizontal forms stay pale without billboard behavior or unintended foreground overlap.
- Fast compilation, relevant scene/surface/cloud integration checks and native rendering/travel/dig timing pass; record genuine measured costs and any remaining spikes.
- Deliver the Windows build, update task/status/feature/ledger and clean completed temporary visual captures.

## Questions

No open product choice. Native rendered inspection measures about 84.5% of the site with grass coverage above the blade-placement threshold, 6,678 supported clumps and 49 small surface stones.  The previous performance pass reduced measured grass GPU cost by 88% in one fixed surface view; it did not establish zero frame drops while moving or digging. The requested revision accepts a measured rendering increase to restore more grass, while retaining batching and geometric detail savings.
