# Task 65 - Implement early camera comfort settings

Type: implementation. Status: `done`. Prerequisites: `35`, `63`, `64` (complete). [Completion](../completed/65-camera-comfort-settings.md).

Feature: [camera comfort](../../features/backlog/camera-comfort.md), with [FPS input rules](../../features/backlog/fps-controls.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md#physical-comfort).

## Task contract

- Implement the accepted `64` FOV slider and stable-center-reticle option in the existing pause/settings flow, with clear current values and reset. Apply preferences before the first playable frame and retain them across launch, resize, rescue and save/Continue changes; resetting preferences cannot reset excavation.
- Follow the owning contract's exact labels/defaults, vertical/aspect convention, Back/reset behavior, unchanged input sensitivity and future-effects policy after `64` review. Use a persistent panel with explicit keyboard focus; account for the additional Pause entry at 960×540. Do not replace existing UI resources or import TMP as part of this delivery.
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
- Verify preference-write failure keeps session values and offers local retry without affecting world saves. Capture/compare excavation and economy before/after preference reset; unchanged settings must not produce per-frame disk writes, unnecessary UI rebuilds or camera-only world checkpoints. Check steady-crosshair position/size/color across actual successful digs, flight and FOV changes.
- Measure stable-setting overhead and record gameplay/presentation evidence; deliver `builds/windows/SomethingDownThere.exe`. `05` rechecks production HUD integration and `54` performs the final long-session comfort review.

## Delivery and evidence

- `FpsPlayer` loads camera preferences before play; the settings view retains controls/focus and the existing font. [74](74-ui-toolkit-menus.md) subsequently owns its Toolkit presentation. Pause includes Camera comfort; settings own one level of Escape and retain selection on background clicks. No motion effects exist to disable; future owners retain the selected zero/off policy.
- A bounded version-1 device file under `Preferences/camera-v1.ini` (`EditorPreferences` in the Editor) uses independent field validation and atomic replacement at paused boundaries/quit. Unknown formats use defaults until explicit edits/reset; failed writes preserve session values and offer Retry locally.
- Official CLI: **13 preference checks and 68 PlayMode checks passed**. MainGame review compared 55/75/90° in a passage/pit at both aspects; center-hit distance varied by less than 0.00001 m. Successful digs at all three FOVs, pulse opt-out, flight, mouse/keyboard input and held-action isolation passed.
- Native review covered 960×540, 1280×720, 1280×800 and the 1080p initial window; first/default, saved, corrupt, non-finite and out-of-range preferences; reset, focus flush, rescue, normal quit/relaunch and real locked-file failure/Retry. Held digging and flight retained the centered white cross. Reset left native world-save bytes unchanged; a live 476.25 m³ excavation/economy snapshot also matched exactly (excluding capture timestamp).
- Stable-setting check: 100,000 unchanged setter/flush iterations took 1.27 ms with **0 allocated bytes**, writes or change events; unchanged UI values did not redraw controls. This microcheck is not hardware/FPS qualification. Evidence: `unity/Logs/Task65/`; native profiles archived and original Editor save preserved, MainGame clean.
- Windows build succeeded `2026-09-09 05:49 UTC`; native logs have no errors. Pipeline's expected warning leaves its runtime bridge disabled. Production held-tool clipping, final HUD integration and long-session comfort remain with `11`/`05`/`54`.
