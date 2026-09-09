# Task 66 - Design precision movement for player-made ledges

Type: design/research; documentation only. Status: `done`. Prerequisites: `65` (complete). [Completion](../completed/66-precision-movement-design.md).

Feature: [precision movement](../../features/backlog/precision-movement.md). Implementation: [67](67-precision-movement.md). [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Scope and outcome

Compare held slow walking with true crouch against the actual controller, terrain and comfort settings. Select input, stance/clearance, camera transition, ground/air control and recovery rules; produce a testable `67` contract without implementing gameplay.

The user selected **true crouch**, **held Left Ctrl**, and reduced horizontal steering **on the ground and in the air**. The [feature](../../features/backlog/precision-movement.md) retains the comparison, rationale, rejected alternatives, blocked standing, smooth height response and save/recovery rules. There are no remaining product questions; numerical tuning and feel evidence belong to `67`.

## Research and evidence

- Inspected `FpsInput`, `FpsPlayer`, `TerrainVolume`, existing movement/terrain checks, `WorldSnapshot`/`WorldSaveCodec` and `65` comfort behavior. Movement uses immediate normalized 4 m/s horizontal control both on ground/in air. Input has no precision action; save format 1 stores position/vertical speed but no stance.
- Official CLI inspected clean MainGame: capsule height 1.8 m, radius 0.3 m, center Y 0.9 m; eye height 1.6 m; skin 0.08 m, step 0.3 m, slope limit 45°, minimum move distance 0. No crouch lowering exists in the current build.
- At simulated 60 Hz in an additive MainGame fixture, the unchanged controller traversed a 1.25 m supported ledge for 6 m, a 26.6° ramp for 7 m with about 3 m rise, and 0.2 m steps for 6 m. A 0.2-second lateral input traveled 0.8 m and left the ledge; this supports shorter corrections but does not establish comfort or player-error rates.
- A 2.3 m-high tunnel allowed approach and a right-angle turn; standing movement stopped before its 1.3 m-high continuation. At the low section, matching-radius capsule occupancy queries found standing 1.8 m blocked and candidate crouch 1.1 m clear. This is clearance research, not proof of resize/stand-up implementation.
- Fixture geometry was an analytic density snapshot through the existing MainGame terrain mesher/colliders, not a hand-dug playtest. CLI screenshot inspected the low-tunnel entrance. Evidence: `unity/Logs/Task66/baseline.json`, `observe-baseline.cs`, `tunnel-baseline.png`. The additive fixture had no save session; MainGame was restored clean in Edit Mode. No native input/comfort review was performed.
- [Unity 6.6 controller reference](https://docs.unity3d.com/6000.6/Documentation/Manual/class-CharacterController.html): changing height expands in both Y directions, so implementation must adjust center to preserve feet. Keep radius and normal step/slope behavior; stance changes cannot substitute for terrain cleanup.
- [CheckCapsule](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.CheckCapsule.html) checks volume occupancy with layer/trigger filtering; [CapsuleCast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.CapsuleCast.html) does not detect initial overlap. Therefore stand-up needs occupancy coverage, not only an upward ray/sweep, with game-owned terrain/skin-margin regressions in `67`.
- [Input System 1.20 initial-state checks](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputAction.html#UnityEngine_InputSystem_InputAction_wantsInitialStateCheck) support held modifiers after enable. `67` must sample Ctrl before gameplay movement while preserving release barriers for LMB/Space/E. No package change is required.

## Acceptance and remaining tuning

- Selected behavior, reasons, alternatives, input/clearance cases and safe saved stance are recorded in the feature; the concept and `67` agree. Relative links, whitespace and task-state consistency checks pass.
- `67` starts with 35% horizontal speed, 1.1 m crouch height / 0.95 m eye height and a 0.2-second smooth stance transition. These are tuning candidates, not user-approved final feel measurements. It must verify actual collision-safe resizing and compatible save migration before delivering its Windows build.
- This task completes design only. The latest gameplay build remains `75`'s; crouch is not implemented until `67` passes its acceptance.
