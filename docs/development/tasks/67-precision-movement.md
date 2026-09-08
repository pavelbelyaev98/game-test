# Task 67 - Implement held precision movement

Type: implementation. Status: `planned`. Prerequisites: `65`, `66`.

Feature: [precision movement](../../features/backlog/precision-movement.md). [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Task contract

- Implement the held precision interaction selected in `66` through the existing Input System/controller. Keep normal walking responsive, click-and-hold digging available from the start, and existing jump/jetpack behaviour.
- If a true crouch was selected, implement capsule/camera transitions and blocked stand-up; otherwise do not add a stance system. No stamina, stealth, ledge auto-stop or new art/audio.
- Preserve `65` comfort preferences, centered targeting, movement normalization and pause/focus barriers. Define safe restored stance under `35`; never load the player into a ceiling or silently modify terrain for clearance.
- Add the accepted input to the compact Pause reference, without restoring routine HUD coaching.

## Before implementation

Read the feature's recorded `66` decisions and examples, inspect current controller/save owners, and use official Unity CLI/physics/UI skills as relevant. Resolve technical collision details with focused tests; ask only if evidence requires changing the accepted interaction.

## Acceptance

- Compare normal/precision traversal on representative supported ledges, slopes, steps, tunnel corners and edited chunk seams at different frame rates.
- Verify held digging, jump/flight, release, menu/focus transitions and save/reload. If crouched, blocked stand-up cannot clip or trap the player; camera changes respect comfort settings.
- No movement mode bypasses solid terrain or changes ordinary speed when inactive. Preserve `26`/`34` cleanup rather than treating controller changes as a substitute for it.
- Inspect MainGame and deliver the Windows build. `05` rechecks production presentation and `50`/`54` retain the movement contract.
