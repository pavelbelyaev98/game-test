# FPS movement and controls

Status: Task `05` mechanics are verified; Task `18` reopened production HUD/feel acceptance pending `08`. User-selected Task `16` adds jump/hold jetpack and an adaptive desktop window; see the [queue](../../development/tasks.md).

Idea coverage: sections 50 and 53, plus controls required across the loop.

## Purpose

Provide simple first-person movement and one clear input path for digging, collection, jetpack use, inventory, and surface stations.

## Controls

| Input | Behavior |
| --- | --- |
| WASD | Camera-yaw-relative walking with normalized diagonal speed. |
| Mouse | Yaw and bounded pitch without camera roll. |
| LMB | Dig once or repeat at tool cadence while held. |
| Space | Immediate grounded jump; keep holding for 0.22 seconds to engage powered jetpack thrust. |
| E | Perform the single eligible aimed interaction once per press. |
| Tab | Open inventory for inspection only. |
| Escape | Pause or close the topmost menu. |
| Mouse/LMB or arrows/Enter | Navigate menus without triggering world actions. |

## Implemented contract

- A `CharacterController` player root owns yaw/state; its child camera owns pitch.
- Unity Input System supplies input. Menus pause gameplay, release the cursor, and block world actions.
- Focus loss pauses. Held Dig, Interact, and Jetpack must be released after resume before acting.
- Jumping is free, works with an empty battery, and cannot repeat in midair or automatically on landing. Releasing Space stops thrust and resets its hold delay.
- Desktop builds start in a bordered, resizable 16:9 window fitting 75% of the current display, capped at 1920x1080 and constrained by the work area. A 2560x1440 monitor starts at 1920x1080. Resizing remains player-controlled until the next launch.
- Center-view targeting chooses the nearest visible collider in range; occluders block targets behind them.
- `IDigTarget` commits valid hits, charging battery only when the target changes.
- `IInteractionTarget` supplies and revalidates prompts. Full inventory leaves finds in the world.
- Inventory cannot sell or upgrade; aimed surface stations open explicit station menus.

## Regression checks

- Production acceptance after `08`: retain the tested input contract, use the uGUI skill's TextMeshPro guidance and approved presentation assets, inspect pause/inspection UI at target window sizes, and record a Windows control/feel review.

- Movement remains collision-safe, horizontal relative to view, and speed-normalized.
- Digging respects reach, visibility, cadence, and accepted-hit energy cost.
- Interaction respects exposure, capacity, station context, and one press/one action.
- Menus and focus changes cannot leak input into gameplay or leave actions held.
- Task `16`: verify tap versus hold, release/depletion, ceiling collisions, empty-battery jump, and held Space across focus/menu transitions; inspect the main scene and bordered Windows build. Resolution detection sets window size, not a hardware quality benchmark.

Deferred: detector, economy, saving, rescue, controller/rebinding support, graphics auto-benchmarking, and final tuning.

## First-use guidance

Prefer contextual `LMB Dig`, `Space Jump / hold Jetpack`, and `E Interact` hints or one very short first loop. Do not lock the final tutorial wording before observing a new player. Movement speed, camera sensitivity, reach, cadence, and comfort values should be tuned through full-game playtests rather than treated as fixed design.
