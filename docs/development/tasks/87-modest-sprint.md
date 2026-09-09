# Task 87 - Modest held sprint

Type: implementation. Status: `done`. User-selected. Prerequisites: `67`, `78` (complete). [Completion and evidence](../completed/87-modest-sprint.md).

Feature: [FPS controls](../../features/backlog/fps-controls.md). Research: [physical comfort](../../research/player-review-findings.md#physical-comfort); preserve [precision movement](../../features/backlog/precision-movement.md).

## Scope and tuning

- The user requested held Shift sprint that is not too fast. Select Left Shift and a 1.35 multiplier: 4 m/s walking, 5.4 m/s sprinting. Keep immediate stopping and normalized diagonal movement; the held modifier applies to horizontal steering on the ground and in the air.
- Crouch, its transition and blocked standing retain precision speed. Sprint changes no vertical jump/thrust, battery cost, FOV or camera effects and adds no stamina system.
- Integrate Sprint into Input System bindings, Controls and the compact Pause reference. Preserve existing saved bindings/mode when adding the optional sprint preference; use an available key if an old custom map already owns Shift. Never persist an active sprint latch.

## Acceptance

- Verify the 1.35 speed ratio, release/stop, diagonals, crouch/low-roof precedence, focus/menu barriers and held/remapped input with short deterministic checks.
- Existing preference maps, conflict handling and reset retain all actions; displayed Sprint binding follows the saved map.
- Inspect MainGame through the official Unity CLI, build the normal Windows executable and leave the prior task 80 benchmark deferred as the user requested. No long benchmark is needed for this bounded movement change.

Questions: none; numerical tuning is delegated by the request.
