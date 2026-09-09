# Task 05 - FPS movement and controls

Status: production acceptance reopened by Task `18`; mechanics remain implemented and verified.

Why: provide first-person movement, targeting, input isolation and shared player state.

Integrated result: CharacterController movement, camera look, Input System bindings, dig/interaction contracts, battery and focus recovery. The original uGUI presentation was replaced by UI Toolkit in [74](74-ui-toolkit-menus.md)/[75](75-ui-toolkit-hud.md). Disposable station/find/recharge adapters stay in validation fixtures.

Task 18 baseline evidence: 28/28 EditMode and 26/26 PlayMode checks passed, including movement, occlusion, held-input suppression, focus and menu navigation. The then-current main scene had one canvas/raycaster, one EventSystem with InputSystemUIInputModule, and correctly non-raycastable text. [Audit](../../../unity/Logs/Task18Audit/audit.md).

Open acceptance: Task `08` must supply final approved world presentation and refine the existing Toolkit HUD; a Windows control/feel review is still required before this task returns to `done`. The legacy Text/HUD migration is complete in `75`.
