# Task 06 - Finite excavation in the main game scene

Status: production acceptance reopened by Task `18`; terrain behavior remains implemented and verified.

Why: support player-shaped downward, diagonal and lateral excavation with permanent boundaries.

Integrated result: a finite 0.5 m occupancy grid, 108 chunks, 1.1 m brush and local render/collider updates retain cuts for the scene session. Procedural terrain geometry serves the mechanic; bedrock/perimeter are separate permanent colliders.

Task 18 evidence: fresh suites passed 28/28 EditMode and 26/26 PlayMode, including terrain traversal/ascent, seams, stale/rejected hits, boundaries and session persistence. Live scene has one terrain/player owner, no missing scripts and no validation components. [Audit](../../../unity/Logs/Task18Audit/audit.md).

Open acceptance: live captures and builder inspection confirm visible Unity primitives and eight generated, untextured materials with no approved ledger entries. Task `08` must replace that presentation and validate traversal/boundary readability in the Windows build. Session resets on application exit; disk saving and material variety remain later.
