# Task 66 - Precision crouch design

- Why: give players careful ledge/landing control while preserving normal walking, and resolve whether precision also changes tunnel clearance.
- Selected: the user chose true crouch on held Left Ctrl with slower horizontal steering on ground/in air. The [feature](../../features/backlog/precision-movement.md) retains alternatives, smooth body/viewpoint transitions, safe blocked standing, input recovery and saved physical stance.
- Integrated result: [67](../tasks/67-precision-movement.md) has concrete tuning candidates and input/terrain/camera/save acceptance; concept, comfort/FPS contracts and future jetpack/return/release tasks agree. `67` is next ready.
- Evidence: official CLI inspected MainGame and observed baseline traversal in an additive terrain fixture at 60 Hz. A 0.2 s correction moved 0.8 m off a ledge; standing capsule blocked at a 1.3 m tunnel while a 1.1 m query fit. Screenshot/measurements: `unity/Logs/Task66/`. MainGame restored clean with no fixture save session; documentation links/state/whitespace checks passed.
- Limitation: design only; dynamic crouch, numerical feel, saved-stance migration and Windows review belong to `67`. The existing `75` gameplay build is unchanged.
