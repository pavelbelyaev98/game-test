# Task 126 - More finds in the shallow starting layer

Type: implementation. Status: `done`. User-selected rock-only revision delivered. [Completion/evidence](../completed/126-shallow-find-density.md).

Prerequisites: `110`/`114`/`116` delivered. Feature: [discovery generation](../../features/backlog/discovery-generation.md); content: [starter bottles](../../features/backlog/starter-find-batch.md), [Rock](../../../art/photo-rock/README.md). Research: [progression/discovery findings](../../research/player-review-findings.md#progression-and-discovery), [roster `40`](40-discovery-roster.md). Resume `106` afterward.

## Selected work

- Disable all three bottle variants for new-game spawning, retaining their assets, save keys and compatibility aliases for existing saves and reversible future reactivation. Use only the existing three Rock appearances; richer categories remain with `40`.
- New games contain 312 rocks: 240 at 0.65–1.1 m centre depth across the full upper site, including the starting rim, and 72 deeper. This is ten times the previous shallow rock allocation. Larger counts hit the real rock spacing limit; the previous 552-find bottle-heavy allocation is superseded.
- Use bounded seeded spacing/coverage selection to reduce empty stretches, using each approved prefab's bounding radius plus 0.15 m of soil between finds. Keep bag size, detector silence, recognition/handling and save identities intact.
- Rock-only playability tuning: new rocks sell for 2 credits (a full ten-slot bag remains worth 20, five rocks fund the first shovel). At 1 credit, all 312 rocks cannot fund the 370-credit shovel track; at 2, the shallow pool alone provides 480 credits. Historical saved values remain unchanged. Full-run economy/novelty still belong to `37`/`40`.
- Stronger damping of slow supported contact oscillations settles the new rock orientations within the existing strict drift/rotation bounds. Bodies stay dynamic, fall/throw normally and wake after digging; no wider sleep envelope or anchored substitute.
- Existing saves retain their exact population and excavation; do not inject finds, reroll or overwrite user profiles. The denser layout starts with New Game.

## Acceptance

- Source catalogs and MainGame agree on population/quotas. Sampled seeds reproduce buried, nonoverlapping placements, retain deeper content, and give dense top-layer coverage across the site, including the starting rim.
- Actual approved meshes remain fully buried. MainGame pickup/full-bag behavior and persistence pass; older saved populations remain unchanged after restore and resave.
- Use 127's gentler shovel baseline when resuming pacing validation; prior timing/stroke evidence does not establish the new tuning. Measure fixed small patches with the default shovel, without consulting find positions: around 1 m depth must reveal multiple finds within one battery. Inspect the result and collection through the official Unity CLI; distinguish scripted digging from a human pacing verdict.
- Across 100 seeds, shallow locations stay within 1.5 m horizontally of a rock centre (the larger rock envelopes replace the old small-bottle 1.25 m target). Save/restore the population exactly; preserve older layouts.
- Produce `builds/windows/SomethingDownThere.exe`, preserve saves/preferences, update concise records and promote the next eligible task. No commit.

Questions: none required for this bounded density tuning; new item categories/assets require their own review.
