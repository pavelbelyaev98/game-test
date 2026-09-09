# FPS movement and controls

Status: Task `05` mechanics are verified; Task `18` reopened production HUD/feel acceptance pending `08`. [Task `16`](../../development/completed/16-window-and-jetpack.md) verifies window/jump/hold behavior in the main scene and Windows executable, including final-fuel depletion. [Task `21`](../../development/completed/21-review-controls-and-organic-digging.md) establishes immediate airborne restart; see the [queue](../../development/tasks.md).

Idea coverage: sections 50 and 53, plus controls required across the loop.

## Purpose

Provide simple first-person movement and one clear input path for digging, collection, jetpack use, inventory, and surface stations. [Precision movement](precision-movement.md) has separate `66`/`67` design/delivery; it is not yet implemented.

## Controls

| Input | Behavior |
| --- | --- |
| WASD | Camera-yaw-relative walking with normalized diagonal speed. |
| Mouse | Yaw and bounded pitch without camera roll. |
| LMB | Hold to dig/repeat and collect the aimed, sufficiently uncovered find. Pickup has a short recovery before the same hold continues; one action per frame. |
| Optional toggle dig (accessibility) | Pending from [Task `78`](../../development/tasks/78-input-accessibility.md): default remains hold-to-dig, and players may enable a toggle-to-dig mode if selected in controls settings. |
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
- `IInteractionTarget` supplies and revalidates station prompts. `BuriedFind` revalidates visibility, authored exposure and capacity before held-primary collection; a full inventory leaves finds in the world. Task `30` retains release-before-resume safety across menus/focus loss.
- Inventory cannot sell or upgrade; aimed surface stations open explicit station menus.
- Task `86`: zero fuel automatically rescues to the surface, refills the battery and reports the existing loot loss/fee. Pause has no rescue action. Rescue suppresses held digging/thrust and preserves excavation, owned upgrades and collected identities.

## Regression checks

- Production acceptance after `08`: retain the tested input contract and [74/75's shared UI Toolkit presentation](menu-presentation.md), inspect the Toolkit gameplay HUD and screen menus at target window sizes, and record a Windows control/feel review. Any future font/presentation import still needs specific approval.

- Movement remains collision-safe, horizontal relative to view, and speed-normalized.
- Digging respects reach, visibility, cadence, and accepted-hit energy cost.
- Interaction respects exposure, capacity, station context, and one press/one action.
- Menus and focus changes cannot leak input into gameplay or leave actions held.
- Task `16`: verify tap versus hold, release/depletion, ceiling collisions, empty-battery jump, and held Space across focus/menu transitions; inspect the main scene and bordered Windows build. Resolution detection sets window size, not a hardware quality benchmark.
- Task `21`: verify release into a fall, immediate airborne restart, repeated restarts, ground reset and depletion. Temporary refill/strength keys must not suppress held Space; focus/menu release safety remains intact.
- Task `23`: admin chords require Ctrl+Shift and a fresh action-key press. Plain keys cannot change gameplay; admin refill/strength preserve thrust, unlimited battery covers dig/flight, and restore normal rules removes overrides. Release builds cannot enable admin.

The [camera comfort contract](camera-comfort.md) owns the implemented FOV slider, steady-crosshair defaults, preference/reset behavior and future motion-effect rules. [64](../../development/tasks/64-camera-comfort-design.md) selected the design and [65](../../development/tasks/65-camera-comfort-settings.md) delivered it through Pause. Back/Escape keeps changes and returns to Pause; `54` validates sustained comfort. Camera preferences remain separate from excavation state.

Digging default behavior is click-and-hold; release LMB to stop. A dedicated accessibility option can optionally switch to toggle-to-dig through a controlled settings path. Complete keyboard/mouse rebinding and optional toggle digging are planned in [Task `78`](../../development/tasks/78-input-accessibility.md), accessible from startup and Pause; controller support remains uncommitted; graphics auto-benchmarking remains uncommitted. Final comfort review belongs to `54`. Task `05` production acceptance, research and remaining questions are in [its numbered file](../../development/tasks/05-fps-controls.md).

## HUD guidance

Tasks `28`/`29` remove automatic movement/digging/jump/inventory hints, "Move closer" coaching and per-stroke excavated-volume popups at the user's request. Keep a compact control reference in Pause, without the free-jump/shared-battery paragraph. `FpsHud` already shows finds count/capacity and battery; preserve these return-decision facts and clear sell/upgrade/save meaning without adding routine coaching. X-ray is markers only; retain item names, pickup confirmations and meaningful errors. Movement speed, sensitivity, reach, cadence and comfort remain tunable through full-game playtests.

Task `32` removes secondary explanations from all battery/recharge notices and the "Normal gameplay rules" subtitle. Keep notice titles and meaningful override state on one line. Confirmation menus still explain actual consequences.

Tasks `74`/`75` use a shared Toolkit panel for menu and HUD scaling, replacing `33`'s legacy Canvas workaround. First display and window changes must not require content updates to become sharp; stable scale/content must not force repeated redraws.
