# Task 135 - Restore the grassy site and clear sky

Type: implementation. Status: `done`. [Completed result](../completed/135-restore-grassy-site.md). Prerequisites: 134 delivered. Explicitly selected by the user ahead of 126.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md). Context: [134](134-authored-surface-and-volume-clouds.md), [133 grass performance](133-grass-performance-and-pickup-feel.md).

## Scope

- Remove clouds, scenery trees and river/water, and decorative ground rocks including 134's small surface stone dressing from MainGame and its authoring paths.
- Restore continuous turf and moving grass across the entire existing excavation surface. Retire the clearing mask; preserve natural wind, excavation-aware support, compact instancing and twelve-blade geometric distance detail.
- Preserve the approved sun and bright cyan sky, existing terrain dimensions, boundaries, stations, collectible finds, saves, gentler shovels and quick collection.
- Remove unused cloud/stone runtime assets, setup and integration references. Retain editable art sources and licenses as inactive archives; clean temporary review captures. Preserve the staged reservoir batch without integrating it.

## Acceptance

- Official CLI inspection shows a continuous grassy land patch, clear sky/sun, and no clouds, trees, water or decorative surface rocks.
- Fast compile, relevant scene and grass excavation/reset/restore checks pass; optimized rendering remains active and no missing scripts or removed-asset references remain.
- Deliver one Windows build with clean startup; update current docs and asset ownership. Existing measurements do not establish zero frame drops on all hardware.

## Questions

None. The user's explicit removal and restoration request supersedes the surface/cloud composition selected in 134.
