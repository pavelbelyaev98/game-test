# Task 78 - Keyboard/mouse rebinding and optional toggle digging

Type: implementation. Status: `planned`. Prerequisites: `65`, `74`, `75`, `79`.

Feature: [FPS controls](../../features/backlog/fps-controls.md). [Queue](../tasks.md). [Meltopia implications](../../research/meltopia-lessons.md).

## Scope and selected behavior

- Deliver input accessibility early using the existing Unity Input System and Toolkit Settings in startup and Pause. Do not wait for `05`'s final production-art acceptance; `05` later checks integration.
- Hold is the default. Optional Toggle starts continuous digging/eligible collection on a fresh bound Dig press and stops on the next. Preserve reach, cadence, exposure, capacity and energy rules; accessibility is free at the starting shovel.
- Clear toggle intent on menus, focus loss, rescue, load, New Game and input-mode changes. Resume requires release and a fresh press; never save an active digging latch. Zero battery cannot cause autonomous digging after recharge.
- Rebind the complete player keyboard/mouse button action set: movement, dig/collect, interact, inventory, jump/jetpack, crouch and Pause. Mouse buttons must work for applicable actions; controller support is outside this task.
- Capture one binding without firing gameplay/menu actions. Show conflicts and offer replace/cancel, preserve an accessible cancel/back route, support reset-to-defaults and update displayed controls from active bindings. Do not expose developer chords as ordinary player actions.
- Persist device/user preferences independently of world saves. Startup Settings must permit rebinding before play; world reset retains it. Preserve existing camera preferences and menu focus/back behavior.

## Research and questions

- Inspect current action ownership and release barriers before implementation. Use `35`/`79` world lifecycle boundaries and `65` preference error handling.
- The user requested optional toggle digging; do not re-ask whether to add it or offer an incomplete minimal action subset. Bring back only an actual conflict with an established control contract.

## Acceptance

- Exercise default hold and toggle with remapped mouse/keyboard Dig through collection, full bag, empty battery, pause, focus, rescue, reload and settings capture. Each press produces at most one state change; inactive menus never consume energy.
- Verify conflicts, cancel, reset, malformed preferences, failed write/retry and relaunch. Keyboard-only menu navigation remains possible and labels match current bindings.
- Inspect production startup/Pause controls and deliver a Windows build with regression evidence. `05` and `54` consume this implemented baseline; completion does not depend on new art.
