# Discovery generation

Status: [145](../../development/tasks/145-depth-mineral-progression.md) extends basic seeded placement with per-item mineral depth ranges across the 32 m site. Task `45` retains full weighted/clustered roster generation after `39`/`40`–`44`.

Idea coverage: sections 12-15 and relevant tuning in section 53.

## Purpose

Populate each excavation with a mix of ordinary finds, memorable objects, clusters, and increasingly strange discoveries.

## Shallow encounters and mineral depth bands (`145`)

- User-selected shallow means around 1 m below the original surface. New Game keeps 312 centres at 0.65–1.1 m: 96 ordinary rocks and 216 Coal. Another 712 minerals span overlapping depth ranges in the 32 m volume, in the selected Coal → Copper → Iron → Silver → Gold → Emerald → Ruby → Diamond order. [145](../../development/tasks/145-depth-mineral-progression.md) owns exact ranges/counts/prices; source catalogs own editable values. Bottles remain disabled for new spawning, with save resolution intact.
- Cover the full upper site and deeper mineral bands using bounded seeded best-of-64 candidates. Reserve mesh enclosing radii plus 0.10 m soil gaps from the first placement; spending extra clearance early can strand the last finds in the dense shallow layer. Allow at most 4,000 candidates per find, failing explicitly with the seed/placement if the site cannot fit the population. Deep bands choose candidates by 3D separation; shallow candidates retain horizontal coverage. Validate top-layer gaps/separation across 100 seeds and mineral band/quadrant coverage across 20 seeds. No fixed route or reward for every soil voxel.
- Applies to New Game. Restore always uses the persisted population/poses/collected state, including older 72/96/192-find saves.
- Spatial coverage and fixed small-patch excavation checks must both pass. The patches use the default shovel without hidden-position targeting and reveal multiple finds within one battery. Human encounter timing, economy and broader novelty still require `101`/`37`/`40`/`45`.

## Task 45 - weighted pools and related-item clusters

See [numbered Task `45`](../../development/tasks/45-discovery-generation.md) for scope, research, questions and acceptance.

## Required behavior

- Depth changes discovery possibilities, not a blanket value multiplier that makes a straight-shaft rush overwhelmingly optimal. Keep lateral clusters tempting and common finds financially relevant.
- Use the roughly 20–30 ordinary and 30–50 distinctive types defined in [Tasks `40`–`44`](discovery-content.md); this task does not duplicate their art/content production.
- Broad tendencies move from recent household finds toward machinery, bones, fossils, and stranger objects, but ranges overlap in both directions.
- Generate candidates, then validate novelty gaps, unrelated major-find clumps and excessive early concentration before acceptance. Use `58` encounter bands and `40` novelty groups with bounded deterministic repair/rejection; related clusters and free routes remain. Spatial checks need real-route timing evidence.
- Seeds reproduce accepted placement for debugging and tests. Persist the accepted population; revised rules never reroll an existing excavation.
- Each playthrough draws from the intended distinctive-item pool while positions, sensible depth ranges, rotations, and surroundings vary.
- Related bones, vehicle parts, household objects, or machinery fragments may cluster and naturally encourage lateral exploration.
- Distinctive finds are rarer without forcing a fixed sequence. Preserve enjoyable self-shaped excavations; neither detector chains nor generation checks require a prescribed tunnel shape or guaranteed loot behind every wall.
- [Task `36`](buried-upgrades.md) adds 2–4 very rare permanent-upgrade discoveries to the finite population, with stable identities and valid downward/lateral placement. Their effects and collection are owned by that feature; they are never necessary for the ending.

## Done when

See [numbered Task `45`](../../development/tasks/45-discovery-generation.md) for scope, research, questions and acceptance.

## Before implementation

See [numbered Task `45`](../../development/tasks/45-discovery-generation.md) for scope, research, questions and acceptance.
