# Task 06 - Finish production excavation presentation acceptance

Type: validation. Status: `planned`. Prerequisites: 08.

Feature: [excavation terrain](../../features/backlog/excavation-terrain.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Verified mechanic: `TerrainVolume` behind `IDigTarget`, one soil response, permanent bedrock/perimeter, local mesh/collider updates and in-memory removal state in the [main game scene](../../features/backlog/core-loop.md). Retain the existing player, surface/return anchors and user-owned terrain work.
- Acceptance: untouched start; downward, diagonal and lateral cuts produce traversable space with matching collision; repeated/rejected hits cannot breach boundaries or spend energy without changing terrain. Returning to the surface preserves cuts for this scene session.
- Evidence: focused terrain/state checks, player collision/dig integration and editor inspection of shell wiring; record representation choice, measured update cost and visual limits. Preserve the existing FPS scene and `.meta` references.
- Production acceptance after `08`: inspect the approved replacement presentation, traversal and boundary readability in the Windows build. Saving belongs to `35` and material variety to `39`; this task does not rebuild the verified terrain mechanic.

## Before implementation

Before production acceptance (`06`), review that research, inspect actual boundary/remnant traversal after `08` and confirm the approved terrain is clearly readable. No gameplay question is required; any changed user-owned terrain art needs a concrete proposal. Task `54` extends traversal/performance checks to the fully excavated game.
