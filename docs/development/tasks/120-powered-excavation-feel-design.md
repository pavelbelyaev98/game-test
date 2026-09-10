# Task 120 - Compare continuous removal for the powered shovel

Type: design/research; documentation only. Status: `planned`. Prerequisites: `100` excavation observations, resumed-and-completed `56` equipment structure and `39` representative material resistance. This task does not resume paused `56` or block the existing scoop-based production trip.

Feature: [shovel progression](../../features/backlog/shovel-progression.md). Related: `57` feedback, `11` existing shovel presentation, `58`/`39` terrain, `37` full-run pacing.

## User proposal and baseline

The user proposes late drilling/melting-like removal: terrain continuously erodes rapidly under the same upgraded primary tool instead of disappearing in large distinct bites. Hold/toggle input already works from the starting shovel. This proposal changes removal behavior and sensation, not access to comfortable controls; ice and a separate tool are not selected additions.

## Research and decision artifact

- Read the [Meltopia lessons](../../research/meltopia-lessons.md) on retained power and sensory feedback; use our current scoop profiles, material behavior and `100` evidence. Compare the existing response with the proposed result, distinguishing reference-game claims from observations of this build.
- Present early/middle/late contact and removal sequences for the same formation. Compare current strokes, smaller rapid cuts and continuous surface removal; state what meaningfully differs in control, shape, time and recognition rather than merely relabeling a shorter cooldown.
- Preserve one recognizable evolving shovel, acquired capabilities and easy old-ground revisits. Show whether continuous removal is a particular purchased milestone or whether ordinary rapid scoops already supply the desired power. Do not make every new material reset the player's strength.
- Compare useful removed volume, energy, aim control, response on resistant material, partial reveals and boundaries. Protect recognition and the selected direct-aim/0.6-second collection behavior; powerful digging cannot instantly collect unseen or off-aim finds. Include full bags and nearby physical objects.
- Inspect density/cutter, meshing/collision and save capture for feasible update bounds under `63`. Explain likely cost, temporal artifacts and conservative implementation limits; claims of smooth runtime behavior require later measurement. Do not restart `80`'s deferred benchmark or assume a physics/remeshing update every rendered frame.
- Coordinate visual/audio contact feedback and existing comfort controls through `57`. No new material, model, sound, camera effect or image-generated art is approved by a comparison. Record what a real playable test would need before selecting implementation scope.
- Recommend keep current, include a bounded powered milestone, or defer/omit, with concrete reasons and rejection criteria. Link findings and illustrated/timed examples from this task; questions alone are not the design artifact.

## Questions and acceptance

Review the desired late-tool sensation, control over shape, strength/speed relationship and whether the expected benefit warrants changing terrain removal. Numerical tuning remains implementation/playtesting after the behavior is selected.

Done when the user reviews the comparison and the shovel feature records the selected direction, rationale and unproven feel/performance assumptions. Amend affected milestone/material contracts and create a separate numbered implementation task only for an approved change, with MainGame recognition, frame/collision/save and Windows feel acceptance. Design completion does not claim continuous excavation is implemented or proven enjoyable.
