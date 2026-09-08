# Task 65 - Implement early camera comfort settings

Type: implementation. Status: `planned`. Prerequisites: `35`, `63`, `64`.

Feature: [FPS controls](../../features/backlog/fps-controls.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md#physical-comfort).

## Task contract

- Implement the accepted `64` FOV slider and stable-center-reticle option in the existing pause/settings flow, with clear current values and reset. Apply preferences before the first playable frame and retain them across launch, resize, rescue and save/Continue changes; resetting preferences cannot reset excavation.
- Preserve the present camera without added shake, head bob or jetpack effects. If such effects exist by implementation time, wire their actual controls under `64` and verify zero/off; otherwise document the requirement for future effect owners, without fake switches or new motion.
- A steady reticle keeps its screen position/size stable during continuous digging and flight. Preserve targeting accuracy and useful feedback in existing non-motion channels; no new art/audio is authorized.
- Keep click-and-hold digging, release-before-resume, paused-menu input barriers and the user's quiet HUD. Changes use existing approved UI assets and need no terrain/presentation import; production restyling remains `08`/`05`.

## Before implementation

- Inspect current camera/HUD/settings/save ownership and the accepted `64` decisions. Use the relevant official Unity CLI and UI skills; consult official documentation for camera projection or UI behaviour that needs verification.
- Separate device preferences from world progression, validate stored values and apply a safe default if missing/invalid. Check allocations/update work: unchanged values must not rebuild UI or rewrite preferences every frame.
- No new product question is required once `64` is accepted. Return only for an evidence-backed change to its contract or any specific newly necessary asset batch.

## Acceptance

- Inspect low/default/high FOV in narrow lateral passages and open shafts at `63` target aspect ratios; center-ray digging/collection still agrees with the reticle, with no tool clipping that obscures interaction.
- Verify steady-reticle behaviour during held digging/flight; check every implemented motion effect can reach zero/off without weakening movement or paid equipment.
- Exercise first launch, saved preferences, corrupt/out-of-range settings, reset, resizing and pause/focus input isolation in the actual Windows build. Settings cannot dig, collect or trigger thrust through menus.
- Measure stable-setting overhead and record gameplay/presentation evidence; deliver `builds/windows/SomethingDownThere.exe`. `05` rechecks production HUD integration and `54` performs the final long-session comfort review.
