# Task 45 - Weighted discovery pools and related-item clusters

Type: implementation. Status: `planned`. Prerequisites: 39-44, 58.

Feature: [discovery generation](../../features/backlog/discovery-generation.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

Create reproducible discovery placement using weighted pools, loose depth influence, material/location rules, and optional clusters. Keep generation separate from reveal and collection.

Task `27` supplies an independent seed and 96 development finds; current spacing is 1.15 m after enlargement in `30`. The first 24 are shallow near the entrance, six near the player's approach. Preserve the reusable field/save identities while replacing the fixed three-prefab assumptions; [current collection contract](../../features/backlog/discovery-collection.md#task-27---buried-finds-and-admin-x-ray).

## Before implementation

- Read [progression/discovery findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current seed, placement failure handling and save compatibility. Research bounded placement of differently sized/clustered objects and measure representative seeds, including full intended pools rather than only successful small examples.
- Use the site/material plan from [58](58-site-and-terrain-design.md) and the `40` roster, including any tracking contract from `55`. Ask before a proposed change to the intended pool, footprint or depth; preserve the finite world and existing saves. Tune probabilities/cluster sizes through recorded runs instead of adding a rigid story sequence.

## Done when

- The same seed reproduces the same valid placement.
- Placement avoids boundaries and invalid overlaps.
- Distribution checks cover pools, depth influence, and clusters without testing random-library internals.
- Save/load preserves the already-generated population, consumed IDs and rotations when tables/content versions change; no silent reroll of an existing excavation.
- Review real early/middle/late and lateral routes for new recognizable discoveries and long gaps, using authored object bounds rather than a fixed sphere spacing assumption. Support useful revisits without story locks. Task `37` completes the full-run timing check.
