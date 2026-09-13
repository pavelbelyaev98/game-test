# Core gameplay and controls

Owns the loop, FPS controls, crouch, camera comfort, PC/input settings, menus/HUD and world persistence. Tags: implemented / selected / planned / deferred / open. Task IDs are plain numbers.

## Core loop and scene
- `MainGame.unity` is the full game. Startup menu: Continue, New Game, Settings, Quit. New Game starts at the south rim of an untouched finite site; Continue loads the single slot.
- Loop: dig → uncover → collect → push or return → sell → refill → upgrade → repeat. The surface is a short checkpoint. Starting battery/inventory must already support a satisfying first trip.
- Pressure comes only from battery, terrain, falls and return planning. No lava, gas, oxygen, hunger, earthquakes or similar hazards.
- The user owns terrain art; primitives, debug labels and placeholder presentation are not acceptable in the main game.

## Persistence (`35` implemented, `80` planned)
- One versioned whole-world snapshot: density/seed state, discovery identities/placement/collected records, carried finds, credits, owned levels, battery and pose. Admin overrides stay session-only.
- Autosave after ≤10 s of dirty state; trades and rescue checkpoint immediately; Save and quit/window close wait for the latest write. Never pair fresh terrain with retained purchases; play blocks until terrain and discovery state agree.
- Keep the previous complete checkpoint; a damaged latest offers recovery. Incompatible/missing saves block play with Retry/Quit and never force a new excavation; older readers migrate (v1–v6 supported).
- `124`: release write failure quits silently keeping the last checkpoint; development keeps the world with Retry and exit confirmation. `80` adds incremental density capture without blocking ordinary saving.
- No menu opens the save folder; New Game only from startup. Single Windows instance; profile contention preserves checkpoints and offers Retry/Back/Quit.

## Controls (implemented; rebindable `78`)
- WASD camera-relative, normalized; mouse yaw + bounded pitch. Shift holds 1.35× sprint (4 → 5.4 m/s), no FOV/stamina change. Ctrl holds crouch (priority over sprint).
- LMB holds to dig/repeat and collects sufficiently exposed finds under the crosshair; pickup and terrain strokes are separate actions.
- Space jumps; first hold engages the jetpack after 0.22 s, release/repress restarts thrust immediately, landing restores the delay. Jump is free on an empty battery; no midair re-jump.
- E interacts with one aimed station; finds use LMB. Tab inspects inventory. Escape pauses/closes the top menu. Ctrl+Shift+F10 opens Developer admin in development builds only (`22` verifies release exclusion).
- Toggle digging is an option; Hold is default. Menus release the cursor, pause play and clear held intent; focus loss pauses and requires release after resume. A full bag leaves finds in the world.
- Targeting picks the nearest visible collider in range; occluders block. Open: `97` may add E interaction for distinctive finds/chests; `99` evaluates return mobility.

## Crouch (`67` implemented)
- Hold Ctrl: true crouch, slower camera-relative steering, grounded and airborne, free from the start. Continuous modifier, not a latch; save the physical stance including mid-transition, never the held key.
- Speed drops on the request and returns only when fully standing with Ctrl released. Capsule and viewpoint move together smoothly with feet fixed; release under a low roof keeps crouch, auto-stands when headroom appears, never pushes terrain.
- Keeps radius, reach, cadence, sensitivity, gravity, jump height and thrust; crouching cannot grant extra jumps or lift. Rejected: slow-walk stance, ground-only precision, Shift crouch, toggle/prone/stealth/equipment gating.

## Camera comfort (`65` implemented)
- FOV slider 55–90°, whole degrees, default 75; steady crosshair default On (Off restores the accepted-dig pulse); Reset touches only this category.
- FOV never changes reach, speed, collision or aiming. No shake, head bob or jetpack camera motion; future effects default off, are independently disableable and presentation-only.
- Preferences apply before the first frame and flush on tab change/Back/reset/focus/quit; failures keep session values with local Retry. Brightness/reticle options remain proposed (`81`/`82`).

## PC settings (`122`, refined `123`/`124`)
- Categories: Display, Graphics, Audio, Controls, Accessibility. Values apply immediately (no Apply button); Reset only the current category.
- Display: borderless native default, resizable Windowed, VSync Off, 144 cap, readout Off. VSync disables the soft cap (dropdown reads "Automatic"); mode/resolution changes show a 15 s Keep/Revert confirmation and only Keep saves.
- Graphics: TBD only (150% scene resolution, MSAA 8×, full textures). Audio: master volume; more channels may follow the selected audio direction. Rendering overrides are runtime objects, never source-asset edits.
- Write failures keep session behavior with Retry. `GamePreferences` is the sole runtime display owner.

## Input accessibility (`78` implemented)
- Sensitivity 0.10–3.00× (default 1.00), independent invert axes, Digging mode and 12 Movement/Actions bindings.
- Capture takes one physical key/mouse button, ignores motion/scroll; Escape cancels. Conflicts offer Cancel or Replace (swap, never unassigned). Reset restores defaults.
- Versioned `Preferences/input-v1.ini`, separate from camera/world saves; invalid maps fall back complete; unknown files are preserved until an explicit edit. Older maps gain Shift without rewrite.
- Toggle continues digging/collection until a stop press; a stop press never digs or collects; intent clears on menus, rescue, load and preference changes and is never saved.

## Menus and HUD (`74`/`75` implemented)
- UI Toolkit/UXML/USS, existing font, one EventSystem. No external widget library, Canvas HUD, decorative world assets or fake settings.
- Theme `123`: white typography, neutral grayscale panels, subdued focus/hover, at most one primary action, mint shop data, shared sizes.
- Startup: Continue is the keyboard default when available, otherwise disabled with an explanation. Pause: Resume, Settings, Save and quit, plus Developer admin in development. Escape has one owner; browsing never touches the world.
- New Game confirms replacement, focuses Cancel, archives the old slot after the new checkpoint succeeds. Continue never silently generates a world.
- HUD is quiet: finds count/capacity, battery, `$` whole-number money, return warnings and station menus; no routine coaching or per-stroke popups. `118` adds a bottom backpack icon/current binding cue; no unimplemented C4.
- `106` owns the whole-UI review/copy catalog; `119` optional richer inspection. Controller support and graphics benchmarking remain open.
