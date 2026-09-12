# Task 126 - More finds in the shallow starting layer

Type: implementation. Status: `ready`, next after the accepted completion of `105`. Reopened after the user found the delivered layer still too sparse; resume the recorded work ahead of `106`.

Prerequisites: `110`/`114`/`116` delivered. Feature: [discovery generation](../../features/backlog/discovery-generation.md); content: [starter bottles](../../features/backlog/starter-find-batch.md), [Rock](../../../art/photo-rock/README.md). Research: [progression/discovery findings](../../research/player-review-findings.md#progression-and-discovery), [roster `40`](40-discovery-roster.md). Resume `106` afterward.

## Selected work

- Increase encounters with existing bottles/rocks around 1 m below the original surface, the user's clarified meaning of shallow, especially near the starting rim. This implements the user's density request; richer object categories remain with `40`.
- New games: 552 finds, including 480 shallow (456 bottles / 24 rocks) at 0.65–1.1 m centre depth and the existing 72-find deeper allocation. Cover the full upper site, including the starting rim; replace the sparse front/back quotas.
- Use bounded seeded spacing/coverage selection to reduce empty stretches, using each approved prefab's bounding radius plus 0.15 m of soil between finds. Keep values, bag size, detector silence, recognition/handling and save identities intact.
- The denser seeded mix exposed contact wobble in additional rock orientations. Damp small supported contact oscillations before the existing bounded settling window; retain dynamic bodies, falling/throwing and waking after digging. Keep the existing strict resting-motion regression.
- Existing saves retain their exact population and excavation; do not inject finds, reroll or overwrite user profiles. The denser layout starts with New Game.

## Acceptance

- Source catalogs and MainGame agree on population/quotas. Sampled seeds reproduce buried, nonoverlapping placements, retain deeper content, and give dense top-layer coverage across the site, including the starting rim.
- Actual approved meshes remain fully buried. MainGame pickup/full-bag behavior and persistence pass; older saved populations remain unchanged after restore and resave.
- Measure fixed small patches with the default shovel, without consulting find positions: around 1 m depth must reveal multiple finds within one battery. Inspect the result and collection through the official Unity CLI; distinguish scripted digging from a human pacing verdict.
- Across 100 seeds, shallow locations stay within 1.25 m horizontally of a find. Save/restore the larger population exactly; preserve older layouts.
- Produce `builds/windows/SomethingDownThere.exe`, preserve saves/preferences, update concise records and promote the next eligible task. No commit.

Questions: none required for this bounded density tuning; new item categories/assets require their own review.
