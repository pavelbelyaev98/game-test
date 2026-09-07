# FPS movement and controls

Status: Task `05` is done. Unity compilation plus 7 EditMode and 14 PlayMode integration checks passed; manual feel review remains.

Idea coverage: sections 50 and 53, plus controls required across the loop.

## Purpose

Provide simple first-person movement and one clear input path for digging, collection, jetpack use, inventory, and surface stations.

## Controls

| Input | Behavior |
| --- | --- |
| WASD | Camera-yaw-relative walking with normalized diagonal speed. |
| Mouse | Yaw and bounded pitch without camera roll. |
| LMB | Dig once or repeat at tool cadence while held. |
| Space | Jetpack thrust while held and powered; not jump. |
| E | Perform the single eligible aimed interaction once per press. |
| Tab | Open inventory for inspection only. |
| Escape | Pause or close the topmost menu. |
| Mouse/LMB or arrows/Enter | Navigate menus without triggering world actions. |

## Implemented contract

- A `CharacterController` player root owns yaw/state; its child camera owns pitch.
- Unity Input System supplies input. Menus pause gameplay, release the cursor, and block world actions.
- Focus loss pauses. Held Dig, Interact, and Jetpack must be released after resume before acting.
- Center-view targeting chooses the nearest visible collider in range; occluders block targets behind them.
- `IDigTarget` commits valid hits, charging battery only when the target changes.
- `IInteractionTarget` supplies and revalidates prompts. Full inventory leaves finds in the world.
- Inventory cannot sell or upgrade; aimed surface stations open explicit station menus.

## Regression checks

- Movement remains collision-safe, horizontal relative to view, and speed-normalized.
- Digging respects reach, visibility, cadence, and accepted-hit energy cost.
- Interaction respects exposure, capacity, station context, and one press/one action.
- Menus and focus changes cannot leak input into gameplay or leave actions held.

Deferred: production terrain, detector, economy, saving, rescue, controller/rebinding support, jump, and final tuning.

## First-use guidance

Prefer contextual `LMB Dig`, `Space Jetpack`, and `E Interact` hints or one very short first loop. Do not lock the final tutorial wording before observing a new player. Movement speed, camera sensitivity, reach, cadence, and comfort values should be tuned through full-game playtests rather than treated as fixed design.
