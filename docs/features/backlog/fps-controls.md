# FPS movement and controls

Status: Task `05` mechanics are verified; Task `18` reopened production HUD/feel acceptance pending `08`. [Task `16`](../../development/completed/16-window-and-jetpack.md) verifies window/jump/hold behavior in the main scene and Windows executable, including final-fuel depletion. [Task `21`](../../development/completed/21-review-controls-and-organic-digging.md) establishes immediate airborne restart; see the [queue](../../development/tasks.md).

Idea coverage: sections 50 and 53, plus controls required across the loop.

## Purpose

Provide simple first-person movement and one clear input path for digging, collection, jetpack use, inventory, and surface stations.

## Controls

| Input | Behavior |
| --- | --- |
| WASD | Camera-yaw-relative walking with normalized diagonal speed. |
| Mouse | Yaw and bounded pitch without camera roll. |
| LMB | Collect the eligible visible find on a fresh click; otherwise dig/repeat at tool cadence while held. Pickup consumes the click until released. |
| Space | Grounded jump; first hold engages jetpack after 0.22 seconds. After thrust in this flight, release to fall and press/hold again for immediate thrust. Landing restores the initial delay. |
| E | Perform the single eligible aimed station interaction once per press; finds use LMB. |
| Tab | Open inventory for inspection only. |
| Escape | Pause or close the topmost menu. |
| Mouse/LMB or arrows/Enter | Navigate menus without triggering world actions. |
| Ctrl+Shift+F10 (development) | Open/close Developer admin; [all admin controls](shovel-progression.md#task-23---reach-and-developer-admin). |

## Implemented contract

- A `CharacterController` player root owns yaw/state; its child camera owns pitch.
- Unity Input System supplies input. Menus pause gameplay, release the cursor, and block world actions.
- Focus loss pauses. Held Dig, Interact, and Jetpack must be released after resume before acting.
- Jumping is free, works with an empty battery, and cannot repeat in midair or automatically on landing. Releasing Space stops thrust and energy use. The last fraction of battery powers only its affordable thrust duration, then charge reaches zero; shorter frames cannot restart an exhausted pack. Task `21` preserves jetpack readiness until landing so re-pressing Space can arrest a fall immediately, provided battery remains.
- Desktop builds start in a bordered, resizable 16:9 window fitting 75% of the current display, capped at 1920x1080 and constrained by the work area. A 2560x1440 monitor starts at 1920x1080. Resizing remains player-controlled until the next launch.
- Center-view targeting chooses the nearest visible collider in range; occluders block targets behind them.
- `IDigTarget` commits valid hits, charging battery only when the target changes.
- `IInteractionTarget` supplies and revalidates station prompts. `BuriedFind` revalidates visibility, size eligibility and capacity before primary-click collection; a full inventory leaves finds in the world.
- Inventory cannot sell or upgrade; aimed surface stations open explicit station menus.

## Regression checks

- Production acceptance after `08`: retain the tested input contract, use the uGUI skill's TextMeshPro guidance and approved presentation assets, inspect pause/inspection UI at target window sizes, and record a Windows control/feel review.

- Movement remains collision-safe, horizontal relative to view, and speed-normalized.
- Digging respects reach, visibility, cadence, and accepted-hit energy cost.
- Interaction respects exposure, capacity, station context, and one press/one action.
- Menus and focus changes cannot leak input into gameplay or leave actions held.
- Task `16`: verify tap versus hold, release/depletion, ceiling collisions, empty-battery jump, and held Space across focus/menu transitions; inspect the main scene and bordered Windows build. Resolution detection sets window size, not a hardware quality benchmark.
- Task `21`: verify release into a fall, immediate airborne restart, repeated restarts, ground reset and depletion. Temporary refill/strength keys must not suppress held Space; focus/menu release safety remains intact.
- Task `23`: admin chords require Ctrl+Shift and a fresh action-key press. Plain keys cannot change gameplay; admin refill/strength preserve thrust, unlimited battery covers dig/flight, and restore normal rules removes overrides. Release builds cannot enable admin.

Deferred: detector, economy, saving, rescue, controller/rebinding support, graphics auto-benchmarking, and final tuning.

## HUD guidance

Tasks `28`/`29` remove automatic movement/digging/jump/inventory hints, "Move closer" coaching and per-stroke excavated-volume popups at the user's request. Keep a compact control reference in Pause, without the free-jump/shared-battery paragraph. X-ray is markers only; retain item names, pickup confirmations and meaningful errors. Movement speed, sensitivity, reach, cadence and comfort remain tunable through full-game playtests.
