# Task 75 — Migrate the remaining HUD to UI Toolkit

Type: implementation. Status: `done`. Prerequisites: `74` complete. User-selected ahead of `66`, alongside `76`. [Completion](../completed/75-ui-toolkit-hud.md).

Feature: [menu presentation](../../features/backlog/menu-presentation.md). Preserve [FPS controls](../../features/backlog/fps-controls.md), [camera comfort](../../features/backlog/camera-comfort.md) and [physical-comfort findings](../../research/player-review-findings.md#physical-comfort).

## Scope and decisions

- Move the gameplay reticle, battery/reserve warnings, finds/credits, shovel/depth, target/feedback and developer X-ray markers to editable UXML/USS. Reuse the current font and shared menu theme; do not add art or change mechanics.
- Remove the obsolete Canvas HUD and its text-rescaling workaround. Retain the existing EventSystem/InputSystemUIInputModule as menu input infrastructure; uninstalling that package is not required to remove all Canvas presentation.
- Unity 6.6 [comparison](https://docs.unity3d.com/6000.6/Documentation/Manual/UI-system-compare.html) supports Toolkit HUDs. Shared authoring and eliminating duplicate layout ownership justify migration; uGUI age alone does not. Follow [runtime event routing](https://docs.unity3d.com/6000.6/Documentation/Manual/UIE-Runtime-Event-System.html).

## Acceptance

- MainGame uses Toolkit for all screen presentation, with no runtime Canvas/Text/Image HUD. Retain quiet, readable hierarchy at 960×540, 1920×1080 and 16:10; menu visibility, centered crosshair and marker projection remain correct.
- Preserve steady/default and optional accepted-dig pulse, battery bands/recharge/depletion feedback, resource counts and target/error messages. Cache controls; do not rebuild the HUD every frame.
- Adapt existing behavior tests to the new view and replace Canvas-internal tests with HUD scaling/visibility/projection integration checks. Run the full relevant PlayMode suite and inspect through official CLI.
- Rebuild and provide `builds/windows/SomethingDownThere.exe`. Native desktop checks follow the [validation policy](../../scope-and-validation.md#validation-policy), including brief announced input-control periods.
- Update concise records, authoring/ownership and future presentation tasks. Final world art and sustained HUD acceptance remain `08`/`05`/`54`.
