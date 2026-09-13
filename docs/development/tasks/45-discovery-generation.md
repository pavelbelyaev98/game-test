# Task 45 - Weighted discovery pools and related-item clusters

Type: implementation. Status: `planned`. Prerequisites for the first cluster increment: 39, 40, first content 42 and 58. Full weighted-pool completion additionally requires 41/43/44; do not wait for those models to test the first clusters.

Feature: [discovery generation](../../features/discoveries.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

Create reproducible discovery placement using weighted pools, loose depth influence, material/location rules, and optional clusters. Keep generation separate from reveal and collection.

Task `145` supplies 1,024 catalog finds across 32 m, including 312 shallow rocks/Coal, authored radii plus 0.10 m soil gaps and bounded seeded depth bands. The population ceiling is already full: budget replacements/counts explicitly instead of appending loot. Preserve catalog/save identities and old populations; [current collection contract](../../features/discoveries.md).

Start with five `40`-briefed related cluster templates with valid rotation/spacing envelopes. Compare recognizable relationships, voluntary lateral investigation and memory against roughly ten isolated finds before broad weighted generation. Use existing/approved content and the finite value budget; no pre-dug rooms, fixed fallback route or new object commissions. Partial cluster evidence does not mark the full task done.

## Before implementation

- Before implementing any first-encounter guarantee, consume [155](155-first-expedition-discovery-design.md). Its policy is unresolved: preserve the current no-fixed-fallback/no-reroll contract until the user reviews a change. Capture actual first-recognition time across route styles; seeded spacing alone cannot guarantee ten minutes.

- Read [progression/discovery findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current seed, placement failure handling and save compatibility. Research bounded placement of differently sized/clustered objects and measure representative seeds, including full intended pools rather than only successful small examples. Use `58`'s site/encounter bands and `40`'s novelty groups to define measurable layout checks, with thresholds validated by route playtests.
- Use the site/material plan from [58](58-site-and-terrain-design.md) and the `40` roster, including any tracking contract from `55`. Ask before a proposed change to the intended pool, footprint or depth; preserve the finite world and existing saves. Tune probabilities/cluster sizes through recorded runs instead of adding a rigid story sequence.

## Done when

- The same seed reproduces the same valid placement.
- Placement avoids boundaries and invalid overlaps.
- Validate the [recovery cases](../../research/steam-review-audit/progression-and-recovery.md#validate-recovery-not-only-spawn-validity), not only packing: ceiling/high-wall finds, oblique clusters, cramped recesses and near-boundary objects after nearby ground is widened/removed, a full-bag return and reload. Use authored exposure samples and actual interaction reach; demonstrate recovery with normal bought equipment, with `47` covering controlled airborne collection. Required finds cannot depend on optional dynamite, admin movement or an unselected construction tool.
- Before accepting a new population, validate noteworthy-find gaps, spacing of unrelated major discoveries and novelty spread across overlapping encounter bands. Reject or deterministically rearrange candidates that fail; preserve intentional clusters, varied orientations and non-linear routes. Never claim a spatial test guarantees encounter timing.
- Bound candidate attempts, repair work and generation time under `63`; define an explicit failure/retry state if no valid layout is found. Do not silently drop required finds, hang generation or use fixed hand-placed fallback routes. Persist only an accepted population.
- Deterministic game-owned tests include valid layouts plus adversarial empty stretches, major-find clumps, front-loaded novelty and impossible packing; assert rejection/repair and bounded failure. Test distributions across recorded seeds without testing random-library internals.
- Save/load preserves the already-generated population, consumed IDs and rotations when tables/content versions change; no silent reroll of an existing excavation.
- Review real early/middle/late and lateral routes for new recognizable discoveries and long gaps, using authored object bounds rather than a fixed sphere spacing assumption. Compare straight-down, lateral signal-following and mixed routes for tempting finds; depth influences possibilities without a dominant automatic price multiplier. Support useful revisits and varied self-shaped pits/chambers without story locks or a compulsory signal-to-signal route. Generation checks cannot grade one legitimate excavation shape as failure; useful route-making does not need guaranteed treasure behind every wall. Task `37` completes the full-run timing check.
