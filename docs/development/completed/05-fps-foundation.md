# Task 05 - FPS movement and controls

Status: production acceptance reopened by Task `18`; mechanics remain implemented and verified.

Why: provide first-person movement, targeting, input isolation and shared player state.

Integrated result: CharacterController movement, camera look, Input System bindings, dig/interaction contracts, battery, focus recovery and uGUI menus are integrated. Disposable station/find/recharge adapters stay in validation fixtures; Task `16` holds the pending jump/window increment.

Task 18 evidence: fresh 28/28 EditMode and 26/26 PlayMode checks passed, including movement, occlusion, held-input suppression, focus and menu navigation. Live main-scene inspection found one canvas/raycaster, one EventSystem with InputSystemUIInputModule, and correctly non-raycastable text. [Audit](../../../unity/Logs/Task18Audit/audit.md).

Open acceptance: the uGUI skill calls for TextMeshPro; the HUD has eight legacy Text components and code-built placeholder presentation. Task `08` must supply approved visuals and a polished HUD; a Windows control/feel review is still required before this task returns to `done`.
