# FPS movement and controls

Status: Task `05` mechanics are verified; Task `18` reopened production HUD/feel acceptance pending `08`. [Task `16`](../../development/completed/16-window-and-jetpack.md) verifies window/jump/hold behavior in the main scene and Windows executable, including final-fuel depletion. [Task `21`](../../development/completed/21-review-controls-and-organic-digging.md) establishes immediate airborne restart; see the [queue](../../development/tasks.md).

Idea coverage: sections 50 and 53, plus controls required across the loop.

## Purpose

Provide simple first-person movement and one clear input path for digging, collection, jetpack use, inventory, and surface stations. [Precision movement](precision-movement.md) owns the crouch delivered by `67`.

[97](../../development/tasks/97-special-find-interaction-design.md) reviews the user's proposed E interaction for distinctive finds/chests; this does not change today's LMB collection or E station controls. [99](../../development/tasks/99-return-mobility-design.md) evaluates mobility only after local observations: the user's horizontal-travel complaint was about Meltopia, not this implementation.

## Controls

| Default input | Behavior (keyboard/mouse buttons rebindable in `78`) |
| --- | --- |
| WASD | Camera-yaw-relative walking with normalized diagonal speed. |
| Left Shift | Hold to sprint at 1.35 times walking speed (4 to 5.4 m/s); crouch takes priority. |
| Mouse | Yaw and bounded pitch without camera roll. |
| LMB | Hold to dig/repeat and collect sufficiently exposed finds directly under the crosshair (`112`); no release/repress. Pickup and a terrain stroke are separate actions. Existing recovery permits the same hold to continue. |
| Optional toggle dig (accessibility) | [Task `78`](../../development/tasks/78-input-accessibility.md) implemented: enable Toggle in Controls; a fresh Dig press starts, the next stops. Hold remains the default. |
| Space | Grounded jump; first hold engages jetpack after 0.22 seconds. After thrust in this flight, release to fall and press/hold again for immediate thrust. Landing restores the initial delay. |
| E | Perform the single eligible aimed station interaction once per press; finds use LMB. |
| Tab | Open inventory for inspection only. |
| Escape | Pause or close the topmost menu. |
| Mouse/LMB or arrows/Enter | Navigate menus without triggering world actions. |
| Ctrl+Shift+F10 (development) | Open/close Developer admin; [all admin controls](shovel-progression.md#task-23---reach-and-developer-admin). |

## Implemented contract

- A `CharacterController` player root owns yaw/state; its child camera owns pitch.
- [Task `87`](../../development/tasks/87-modest-sprint.md): Sprint is a continuous held horizontal modifier on the ground and in the air, with immediate stopping and normalized diagonals. Crouch, its transition and blocked standing retain precision speed. Sprint changes neither vertical movement nor battery/FOV and adds no stamina or camera effects.
- Unity Input System supplies input. Menus pause gameplay, release the cursor, and block world actions.
- Focus loss pauses. Held Dig, Interact, and Jetpack must be released after resume before acting.
- Jumping is free, works with an empty battery, and cannot repeat in midair or automatically on landing. Releasing Space stops thrust and energy use. The last fraction of battery powers only its affordable thrust duration, then charge reaches zero; shorter frames cannot restart an exhausted pack. Task `21` preserves jetpack readiness until landing so re-pressing Space can arrest a fall immediately, provided battery remains.
- Desktop defaults use native-resolution Borderless (`123`), supporting desktop switching. Confirmed saved window/resolution choices take precedence. `GamePreferences` is the sole runtime display owner; Windowed remains resizable.
- Center-view targeting chooses the nearest visible collider in range; occluders block targets behind them.
- `IDigTarget` commits valid hits, charging battery only when the target changes.
- `IInteractionTarget` supplies and revalidates station prompts. `BuriedFind` revalidates visibility, authored exposure and capacity before held collection under the crosshair; a full inventory leaves finds in the world. Task `30` retains release-before-resume safety across menus/focus loss.
- Inventory cannot sell or upgrade; aimed surface stations open explicit station menus.
- Task `86`: zero fuel automatically rescues to the surface, refills the battery and reports the existing loot loss/fee. Pause has no rescue action. Rescue suppresses held digging/thrust and preserves excavation, owned upgrades and collected identities.

## Regression checks

- Production acceptance after `08`: retain the tested input contract and [74/75's shared UI Toolkit presentation](menu-presentation.md), inspect the Toolkit gameplay HUD and screen menus at target window sizes, and record a Windows control/feel review. Any future font/presentation import still needs specific approval.

- Movement remains collision-safe, horizontal relative to view, and speed-normalized.
- Digging respects reach, visibility, cadence, and accepted-hit energy cost.
- Interaction respects exposure, capacity, station context, and one action per frame; held ordinary pickup and pressed station interaction.
- Menus and focus changes cannot leak input into gameplay or leave actions held.
- Task `16`: verify tap versus hold, release/depletion, ceiling collisions, empty-battery jump, and held Space across focus/menu transitions; inspect the main scene and Windows build. Native resolution detection does not qualify hardware performance.
- Task `21`: verify release into a fall, immediate airborne restart, repeated restarts, ground reset and depletion. Temporary refill/strength keys must not suppress held Space; focus/menu release safety remains intact.
- Task `23`: admin chords require Ctrl+Shift and a fresh action-key press. Plain keys cannot change gameplay; admin refill/strength preserve thrust, unlimited battery covers dig/flight, and restore normal rules removes overrides. Release builds cannot enable admin.

The [camera comfort contract](camera-comfort.md) owns the implemented FOV slider, steady-crosshair defaults, preference/reset behavior and future motion-effect rules. [64](../../development/tasks/64-camera-comfort-design.md) selected the design and [65](../../development/tasks/65-camera-comfort-settings.md) delivered it through Pause. Back/Escape keeps changes and returns to Pause; `54` validates sustained comfort. Camera preferences remain separate from excavation state.

## Input accessibility contract (`78`)

- Startup and Pause expose **Settings → Controls**, alongside Display/Graphics/Audio/Accessibility (`123` shared rows). Controls includes mouse sensitivity (0.10–3.00×, default 1.00×) and independent horizontal/vertical inversion (Off), and digging mode in the same scrolling list as Movement/Actions bindings. All four directions, Dig/collect/throw, Lift/drop, Jump/jetpack, held Crouch/Sprint, Interact, Inventory and Pause remain available. Back returns to the originating menu and focuses Settings; tab switches persist changes. Fixed menu arrows/Enter and developer chords remain independent of gameplay bindings. Pause reflects current bindings and digging mode.
- Capture begins after opening buttons are released, accepts one physical keyboard/mouse button, ignores motion/scroll, and keeps gameplay/UI submission blocked through capture-button release. Escape cancels capture and always goes back within menus, even with Pause rebound. Gameplay bindings cannot steal menu Enter/Space/arrows/LMB; use Escape/Back when Pause uses those controls. Reset restores the default map, including Escape for Pause.
- A conflicting binding offers **Cancel** or **Replace**; Replace exchanges the two actions' bindings so neither becomes unassigned. The confirmation names the displaced action and its replacement key. Focus loss cancels capture without accepting an input.
- Hold releases to stop; Toggle continues digging/aimed collection until a fresh stop press. Both collect directly hovered eligible finds without another press (`112`), independently of scoop radius. Pickup does not stop toggle intent. A stop press cannot dig or collect. Menus, focus changes, rescue, load/New Game and preference changes clear active intent; release and a fresh press are required to restart. No active latch is saved, and refill never restarts it.
- Versioned `Preferences/input-v1.ini` (`EditorPreferences` in the Editor) stores bindings/mode separately from camera preferences and world saves. Invalid/duplicate maps fall back to a complete default map; unknown files are preserved until an explicit edit/reset. Failed writes retain session values and expose Retry in Controls. Reset controls changes neither camera settings nor excavation/progression.
- Older maps without Sprint preserve existing bindings/mode and gain Left Shift, then Right Shift or an unused ordinary key if occupied; loading alone never rewrites the file. Reset includes default Left Shift. Sprint resumes from current held input only after gameplay resumes and stores no active latch.

Controller support and graphics auto-benchmarking remain uncommitted. Final comfort review belongs to `54`; [05](../../development/tasks/05-fps-controls.md) checks the controls with final production presentation.

## HUD guidance

Tasks `28`/`29` remove automatic movement/digging/jump/inventory hints, "Move closer" coaching and per-stroke excavated-volume popups at the user's request. Keep a compact control reference in Pause, without the free-jump/shared-battery paragraph. `FpsHud` already shows finds count/capacity and battery; preserve these return-decision facts and clear sell/upgrade/save meaning without adding routine coaching. X-ray is markers only; retain item names, pickup confirmations and meaningful errors. Movement speed, sensitivity, reach, cadence and comfort remain tunable through full-game playtests.

**Selected exception, pending [118](../../development/tasks/118-bottom-action-bar.md):** show a backpack icon with the current Inventory binding at the bottom during gameplay. `106` reviews the compact action-bar layout; the label follows rebinding and uses the existing action. Hide with menus and preserve release/focus barriers and non-picking HUD behavior. Future implemented equipment may add selection cues under its own contract; this does not restore general movement/digging coaching or expose unimplemented C4.

Task `32` removes secondary explanations from all battery/recharge notices and the "Normal gameplay rules" subtitle. Keep notice titles and meaningful override state on one line. Confirmation menus still explain actual consequences.

Tasks `74`/`75` use a shared Toolkit panel for menu and HUD scaling, replacing `33`'s legacy Canvas workaround. First display and window changes must not require content updates to become sharp; stable scale/content must not force repeated redraws.
