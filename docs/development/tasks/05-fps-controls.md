# Task 05 - Finish production FPS controls and HUD acceptance

Type: validation. Status: `planned`. Prerequisites: 08, 65, 67, 69.

Feature: [fps controls](../../features/backlog/fps-controls.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Finish the reopened production controls/HUD acceptance after `08`; retain the existing tested movement, jump/jetpack, targeting, menus and focus barriers.
- Digging always uses click-and-hold: hold LMB to dig/collect eligible aimed finds, release to stop. No toggle mode.

## Before implementation

Before production acceptance (`05`), read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect actual input/viewport behaviour and review sustained click-and-hold play after `08`. No new product question is required; preserve the chosen controls and quiet HUD. Ask only about a reproduced comfort issue requiring a changed interaction, not whether to reimplement working controls.

## Acceptance

- Revalidate the [existing controls and regression contract](../../features/backlog/fps-controls.md#regression-checks) using actual Input System input and the approved presentation from `08`.
- Preserve the already-visible `FINDS count / capacity` and battery status through collect/sell/rescue/load/capacity changes; opening inventory must not be needed to decide whether to return. Clear station/save/menu wording must coexist with the removed routine hints/subtitles.
- Recheck `67` precision movement and `69` lighting in the finished HUD/viewport.
- Recheck `65` FOV/steady-reticle preferences after production UI integration, including reset/relaunch and any implemented motion-effect controls.
- Inspect gameplay, pause and inventory at supported window sizes: stable sharp text, clipping/scrolling, readable controls and no leaked world actions.
- Inspect real click-and-hold digging, jumping, jetpack, collisions, focus loss/resume and menu navigation in the Windows build. Preserve the quiet HUD; this task does not rebuild working controls.
- Keep the [prior mechanics record](../../development/completed/05-fps-foundation.md) as evidence; complete only after the remaining production acceptance passes.
