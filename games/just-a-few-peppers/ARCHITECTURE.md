# Architecture map

Status: tasks 1_01–1_03 implement scene composition, input, movement, targeting, basic UI/pause, finite pile/crate handling, tipping, and automatic processing. Finished-food handling and subsequent systems remain proposed. The [audit](docs/development/repository-audit.md) describes the actual Stage0 code. [Core mechanics](docs/core-loop-and-mechanics.md) remain the player-facing contract.

## Ownership

Use one gameplay scene and a small composition component. Menus and harvest completion can be states within it. Stage0 is disposable reference material; reuse suitable pieces or retire obsolete code during implementation after checking retained references. Maintaining its old loop is not a requirement.

| Responsibility | Authoritative boundary |
| --- | --- |
| Plain C# session rules | One model owns raw/optional registered loose units, carriers, station/operation phases, stored food, Coins, paid/installed equipment and completion. Transfers/purchases use validated commands. |
| Scene composition | Owns the model, explicit references, ticking, and pause; no second inventory. |
| Input, movement, targeting | Resolve broad targets and request actions; never edit quantities independently. |
| Pile, carrier, station views | Render conserved contents and bounded cosmetic pepper motion; visual peppers do not own food. |
| Physical object handling | Own grab/release, placement queries, held collision, released-body motion, and recoverable poses for carriers/loose props. Physics may move objects; it cannot grant or lose food. |
| Equipment and budget | Validate earning/purchases, retain paid/installed IDs and derive combined apparatus capabilities. Yard access is mostly open; no required clearing/discovery progression. |
| Household and completion views | Read one stored-food total and harvest-completion state; no recipient/shelf inventory, mandatory household actions, or required ending sequence. |
| Save adapter | Captures one snapshot and reconstructs views through stable IDs. |
| UI and audio | Read state and accepted actions; feedback never grants progress. |

These are responsibilities, not a manager class per row. Prefer a small model, scene driver, reusable views, direct calls, and local callbacks.

The [free handling contract](docs/core-loop-and-mechanics.md#pick-up-place-and-play) permits placement based on actual geometry, rotation, stable stacking, dropping, and loose-prop tossing. Share a small handling component across portable objects; do not create a framework or a second inventory. Carrier identity/contents stay in the model, released motion can use physics, and coherent poses feed recovery and M3 saving. Pause/focus must also stop object simulation. An authored safe point is a recovery fallback, never a placement whitelist. This boundary is planned in the 1_02 revision and 1_06; the implementation map below still describes the existing mat-based code.

The **Finished Food Handoff Rack** is one broad deposit area near the output. After 2_01 its command commits finished units into stored food and proportional Coins once; cellar/family views read stored food without redistribution. The one upgrade bench displays both offers and submits validated purchases. Whole authored modules install at safe boundaries, preserving paid entitlement and prior benefits. The later powered apparatus is an equipment improvement at this work area, not an access unlock or separate machine queue.

2_01's [one physical attachment installation](docs/core-loop-and-mechanics.md#attach-a-purchased-improvement) reuses that purchase boundary and shared object handling. The model retains the offer/kit identity, paid-awaiting-installation or installed state, and accepted fitting while a batch finishes; the view supplies one nearby loose kit or mounted attachment. Recover the same entitlement, apply effects once at the safe boundary and preserve existing food. M3 saves the coherent lifecycle/pose. A large forgiving authored mount is a mechanical connection, not a general placement whitelist. No construction framework or global physics ban is needed.

The full-game final deposit commits harvest completion in the same transaction as its normal transfer, then drives quiet nonmodal feedback. Neither a table interaction nor a presentation callback owns completion. Restoring a completed snapshot shows the completed yard directly with normal movement/camera control; optional table/gift presentation owns no additional state. See the [state/completion contract](docs/development/state-and-saving.md#harvest-completion-contract-for-m4-onward). These are future responsibilities; the M1–M2 interaction prototype needs no Grandpa dialogue or household display system. At most one optional static M2 gag uses existing presentation. Later selected messages use original authored content and minimal reread/shown state in 5_04, without a mail service, real chatbot or random-event framework.

## Planned processing revision

1_07 tests manageable physical pepper batches against grouped representation. Physics may control important objects, but exact contents retain an explicit owner; if loose material becomes authoritative, register its units/identity in the model and recovery/snapshot rather than inferring them from arbitrary scene objects.

1_08 adds a directly controlled handle/rack and ready/operating/working/finished phases around the delivered processing backend. Completing an operation validates/reserves output and starts a batch once. Controlled motion or physical constraints are implementation choices to test against the pinned Unity version. Paused/interrupted input cannot repeat food transactions. 2_01 adds Coins and two independently purchasable improvements; no hidden upgrade discovery is required. These responsibilities are not implemented by the audit below.

## Implemented foundation — 1_01

`Assets/JustAFewPeppers/Scenes/PepperYard.unity` explicitly wires `YardSession`, `YardPlayer`, `YardTargeting`, `YardHud`, the safe spawn, and the UI input module. `YardSession` owns pause/time/cursor state and ticks movement/targeting. `YardInput` owns a runtime clone of `Content/YardControls.inputactions`; Gameplay is disabled while paused, System/Pause stays enabled, and UI actions drive the menu. Focus return never resumes automatically. `YardTarget` describes/highlights a scene target; quantity ownership belongs to the handling model below.

`YardPlayer` owns vertical velocity, grounded grace, and the pending jump timer; authored speed/height/gravity values remain configuration. Its CharacterController uses short collision steps, cancels ascent at ceilings, and restores the authored step offset after each update. `YardSession` accepts one Jump press after release, clears pending jumps on pause/reset, and stops movement ticks while paused. The current airborne arc resumes afterward. Gameplay and UI input are disabled while unfocused; focus return enables the menu without resuming play. See the [on-foot movement contract](docs/look-sound-and-comfort.md#on-foot-movement).

Recovery returns position, yaw, pitch, vertical velocity, and jump timing to the authored gate spawn. It preserves paused state. Out-of-bounds recovery uses the same path. Task 1_02 also recovers the crate with its contents; its separate prototype restart restores quantities. Builds use the saved scene. The create command refuses to overwrite it, and Stage0 generation/build is guarded against changing the current project's settings. The historical `ApplyMovementUpdate` editor command belongs to the foundation migration; do not reapply it over later HUD authoring. See [delivery evidence](docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026).

## Implemented handling — 1_02

**Revision pending:** user feedback rejected fixed-mat parking and absent physical release. Preserve the delivered scoop and processing integration while replacing those restrictions; see [1_02's feedback/revision record](docs/development/tasks/1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026). The following is an audit of the current implementation, not a requirement to retain its mat index or disabled released-body physics.

`YardSession` explicitly owns a `YardHandling` component. That component creates one plain C# `HarvestState` from copied authored region IDs/quantities and crate capacity. The model alone owns remaining units, raw load, held/parked state, and the index of a known resting point. Commands cap accepted amounts by both source and destination and move them atomically. `YardTargeting` resolves the first solid hit and its local `PileRegion`; neither input nor presentation can grant units.

`PileRegion` renders local silhouette, aligned mesh collision, exposed ground, and a readable final clump. `RawCarrierView` reconstructs one crate from the held pose or a validated authored mat; held colliders are disabled, nearby scenery tucks its visual inward, and parking requires ground support, reach, clearance, and an unobstructed approach. The player/controller remains the movement owner. `ScoopPresentation` has three reusable moving proxies and separate action/soft-feedback audio sources; it owns no harvest. Pause and recovery interrupt the action without undoing committed food.

`HandlingSceneAuthoring.Apply` migrated the existing saved scene once through editor APIs and refuses to overwrite existing handling. Input IDs/metas are retained. Runtime views reconstruct from the model, including after the explicit prototype restart. No disk snapshot is implemented yet. See the [handling contract](docs/core-loop-and-mechanics.md#current-crate-prototype-controls) and [task evidence](docs/development/tasks/1_02_scooping-and-crate-carrying.md#delivery-record--september-6-2026).

## Implemented tipping and processing — 1_03

`HarvestState` also owns queued input, one active batch/remaining duration, and accumulating output. `Tip` commits the accepted raw amount once and reserves available output room by moving queued food into the active batch. `AdvanceProcessing` finishes that batch and starts the next partial amount that fits. Authored capacities and duration are copied from the explicitly wired `StationView`; the view owns no inventory.

`YardHandling` resolves E against the broad intake before pickup/parking, calls the model, and starts `TipPresentation` only after an accepted transfer. Its nine pooled proxies and transient crate tilt have no progress callback. Looking away, pause/focus, or recovery cancels the motion; input must be released before another action. The unpaused session ticks station time independently of targeting, scooping, or carrying. `StationView` reconstructs queued peppers, stage markers/feeder motion, and exact partial jar fills. Recovery preserves every quantity and timer; prototype restart clears the whole station as well as restoring the mound.

`ProcessingSceneAuthoring.Apply` is a guarded, one-time addition to the existing scene. Normal builds use the saved scene. The new intake is the only station interaction target; its body and output remain scenery until collection is implemented. See the [processing contract](docs/core-loop-and-mechanics.md#current-tipping-and-processing-prototype) and [task evidence](docs/development/tasks/1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026).

## Unity authoring

During implementation, put new content in `Assets/JustAFewPeppers/`. Add Runtime, Presentation, Editor, Content, Scenes, and Tests folders only as real work requires them. Stage0 can be removed as its useful pieces are replaced or reused; do not rebuild it as a prerequisite.

Author stable pile/discovery IDs, initial quantities, capacities, rates, targets, and visual references in simple configurations. Use ScriptableObjects when they improve reuse; mutable state stays in the runtime model. Free commercially usable models attach to existing view components without unique rule scripts. Follow the [Unity and asset policy](docs/development/unity-and-assets.md), using placeholders early and custom assets only where needed.

Start with the project's Built-in Render Pipeline. New gameplay uses a compatible Input System package, configured in M1; Unity's [6000.6 input manual](https://docs.unity3d.com/6000.6/Documentation/Manual/Input.html) identifies the old Input Manager as deprecated. A small input-action boundary supports configurable bindings; gathering reads the held scoop action directly and has no latched mode. Check official documentation for the pinned editor/package versions before API choices. Record a rendering change only when a concrete asset or feature requires it.

Task 1_01 added runtime/editor/test assembly definitions and verified compatible Unity test infrastructure. The standalone Stage0 harness's .NET target is not Unity's runtime target.

## Data flow and verification

Input requests → session validation/transfer → state update → views and feedback. The [state/save contract](docs/development/state-and-saving.md) defines the conserved quantities. Commit accepted amounts once; interrupted visual motion cannot undo or repeat them.

EditMode checks exercise pure rules. PlayMode checks exercise scene wiring, targeting, recovery, and reconstruction. Packaged builds exercise the actual player path. Human play establishes action enjoyment.

Use a fixed pool of decorative moving peppers, independent of harvest size, and measure before increasing simulation. Physical carriers, props and the chosen pepper representation need suitable collision, sleeping/reuse where useful, recovery tests and measured active-body limits. Do not use a fixed decorative-proxy budget to prohibit important physical gameplay objects. No factory routing, general crafting, multiple recipient inventories, multiplayer, or NPC work systems are planned.
