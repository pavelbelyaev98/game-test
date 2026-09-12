# Task 132 - Brighter sky and sparse light clouds

Type: implementation. Status: `done`. Prerequisites: approved clouds in 130. Explicitly selected by the user on 2026-09-12, ahead of 126; reservoir approval remains separate.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md).

## Scope and selected direction

- The user rejects crowded, apparently close clouds, arbitrary orientations/shapes and gray shading. Their attached A Game About Digging a Hole screenshot selects a brighter cyan sky, sparse simple upright cloud silhouettes and light coloring only. Interpret the reference; do not import its artwork.
- Refine the already approved original Blender cloud forms/atlas, retaining editable source and original commercial license. Replace random fourteen-cloud placement with a deliberate distant arrangement and gentle drift; retain ample empty sky.
- Brighten the camera-local sky with a restrained cyan gradient. Preserve the approved sun's appearance/alignment, actual world lighting, ground/grass, gameplay, save data and pending reservoir work.
- Remove visible rectangular cloud edges/seams. Keep this revision independently reversible within existing cloud ownership.

## Acceptance

- Official Unity CLI views show a bright cyan sky and only a few distant, consistently upright light clouds, without gray pillow shading or visible card boundaries.
- Walking causes no cloud parallax/pop-in; slow motion remains coherent, the sun remains visible and terrain still occludes the sky underground.
- Fast compile and relevant scene checks pass; inspect player-height, upward and multiple heading views. Deliver an updated Windows build and review evidence.

## Questions

No open product choice: the supplied screenshot and explicit refinement request define the direction. Existing cloud approval remains valid; reservoir batch approval is still pending separately.

## Result

[Completion and evidence](../completed/132-brighter-sparse-clouds.md). The four-cloud Blender refinement and brighter sky are integrated in MainGame; official CLI inspection, scene validation and Windows startup pass.
