# Discovery generation

Status: Task `27` basic seeded placement is complete. [140](../../development/tasks/140-shallow-rocks-and-fuel-warning.md) increases the rock-only top-layer density from `126`. Task `45` owns weighted generation after materials (`39`) and the named production content batches (`40`–`44`).

Idea coverage: sections 12-15 and relevant tuning in section 53.

## Purpose

Populate each excavation with a mix of ordinary finds, memorable objects, clusters, and increasingly strange discoveries.

## Starter shallow encounters (`126`, `140`)

- User-selected meaning: **shallow is around 1 m below the original surface**. The rock-only selection from `126` now contains 336 rocks: 264 have centres at 0.65-1.1 m depth, with 72 deeper. Bottles have zero spawn counts; their keys/assets remain available to existing saves. New rocks sell for 2 credits so the shallow population can fund the existing shovel track; historical saved values, slots and detector silence remain unchanged. The [bottle contract](starter-find-batch.md) and [rock catalog](../../../art/photo-rock/catalog.json) own quotas.
- Cover the full upper site with bounded seeded candidate selection. Prefer each approved mesh's enclosing radius plus 0.15 m of soil between objects; after 2,000 unsuccessful candidates, allow a 0.10 m gap for at most 2,000 more. This preserves the established spread and lets 24 additional shallow rocks fill remaining gaps. Check a 1.5 m maximum sampled horizontal centre gap across 100 seeds and buried, nonoverlapping mesh envelopes. These remain buried pickups with freely chosen excavation routes. The bottle-heavy 480-find shallow layer is superseded by the user's rock-only request.
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
