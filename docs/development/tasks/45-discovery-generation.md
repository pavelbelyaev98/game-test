# Task 45 - Weighted discovery pools and related-item clusters

Type: implementation. Status: `planned`. Prerequisites: 39-44, 58.

Feature: [discovery generation](../../features/backlog/discovery-generation.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

Create reproducible discovery placement using weighted pools, loose depth influence, material/location rules, and optional clusters. Keep generation separate from reveal and collection.

Task `27` supplies an independent seed and 96 development finds; current spacing is 1.15 m after enlargement in `30`. The first 24 are shallow near the entrance, six near the player's approach. Preserve the reusable field/save identities while replacing the fixed three-prefab assumptions; [current collection contract](../../features/backlog/discovery-collection.md#task-27---buried-finds-and-admin-x-ray).

## Before implementation

- Read [progression/discovery findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current seed, placement failure handling and save compatibility. Research bounded placement of differently sized/clustered objects and measure representative seeds, including full intended pools rather than only successful small examples. Use `58`'s site/encounter bands and `40`'s novelty groups to define measurable layout checks, with thresholds validated by route playtests.
- Use the site/material plan from [58](58-site-and-terrain-design.md) and the `40` roster, including any tracking contract from `55`. Ask before a proposed change to the intended pool, footprint or depth; preserve the finite world and existing saves. Tune probabilities/cluster sizes through recorded runs instead of adding a rigid story sequence.

## Done when

- The same seed reproduces the same valid placement.
- Placement avoids boundaries and invalid overlaps.
- Before accepting a new population, validate noteworthy-find gaps, spacing of unrelated major discoveries and novelty spread across overlapping encounter bands. Reject or deterministically rearrange candidates that fail; preserve intentional clusters, varied orientations and non-linear routes. Never claim a spatial test guarantees encounter timing.
- Bound candidate attempts, repair work and generation time under `63`; define an explicit failure/retry state if no valid layout is found. Do not silently drop required finds, hang generation or use fixed hand-placed fallback routes. Persist only an accepted population.
- Deterministic game-owned tests include valid layouts plus adversarial empty stretches, major-find clumps, front-loaded novelty and impossible packing; assert rejection/repair and bounded failure. Test distributions across recorded seeds without testing random-library internals.
- Save/load preserves the already-generated population, consumed IDs and rotations when tables/content versions change; no silent reroll of an existing excavation.
- Review real early/middle/late and lateral routes for new recognizable discoveries and long gaps, using authored object bounds rather than a fixed sphere spacing assumption. Compare straight-down, lateral signal-following and mixed routes for tempting finds; depth influences possibilities without a dominant automatic price multiplier. Support useful revisits and varied self-shaped pits/chambers without story locks or a compulsory signal-to-signal route. Generation checks cannot grade one legitimate excavation shape as failure; useful route-making does not need guaranteed treasure behind every wall. Task `37` completes the full-run timing check.
