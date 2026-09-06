# Architecture map

Status: task 1_01 implements scene composition, input, movement, targeting, and basic UI/pause; gameplay state and subsequent systems remain proposed. The [audit](docs/development/repository-audit.md) describes the actual Stage0 code. [Core mechanics](docs/core-loop-and-mechanics.md) remain the player-facing contract.

## Ownership

Use one gameplay scene and a small composition component. Menus and harvest completion can be states within it. Stage0 is disposable reference material; reuse suitable pieces or retire obsolete code during implementation after checking retained references. Maintaining its old loop is not a requirement.

| Responsibility | Authoritative boundary |
| --- | --- |
| Plain C# session rules | One runtime model owns pile quantities, carriers, station work, stored food, equipment, and completion. Transfers use validated commands. |
| Scene composition | Owns the model, explicit references, ticking, and pause; no second inventory. |
| Input, movement, targeting | Resolve broad targets and request actions; never edit quantities independently. |
| Pile, carrier, station views | Render the model and bounded cosmetic motion; visual peppers do not own food. |
| Discovery and access | Derive open paths from cleared pockets and record equipment activation without losing contents. |
| Household and completion views | Read one stored-food total and harvest-completion state; no recipient/shelf inventory, mandatory household actions, or required ending sequence. |
| Save adapter | Captures one snapshot and reconstructs views through stable IDs. |
| UI and audio | Read state and accepted actions; feedback never grants progress. |

These are responsibilities, not a manager class per row. Prefer a small model, scene driver, reusable views, direct calls, and local callbacks.

The planned **Finished Food Handoff Rack** is the only deposit target; its command moves carried finished units into stored food once. Cellar and family-box views update from that total without simulating a helper or food distribution. In M4, the final tier selects an authored fixed intake close to the final supply, feeding the same station model/input buffer. The installed tier also reconstructs this layout on load; it does not create another machine or UI subsystem.

The full-game final deposit commits harvest completion in the same transaction as its normal transfer, then drives quiet nonmodal feedback. Neither a table interaction nor a presentation callback owns completion. Restoring a completed snapshot shows the completed yard directly with normal movement/camera control; optional table/gift presentation owns no additional state. See the [state/completion contract](docs/development/state-and-saving.md#harvest-completion-contract-for-m4-onward). These are future responsibilities; the M1–M2 interaction prototype contains no Grandpa dialogue or household display system.

## Implemented foundation — 1_01

`Assets/JustAFewPeppers/Scenes/PepperYard.unity` explicitly wires `YardSession`, `YardPlayer`, `YardTargeting`, `YardHud`, the safe spawn, and the UI input module. `YardSession` owns pause/time/cursor state and ticks movement/targeting. `YardInput` owns a runtime clone of `Content/YardControls.inputactions`; Gameplay is disabled while paused, System/Pause stays enabled, and UI actions drive the menu. Focus return never resumes automatically. `YardTarget` only describes/highlights a placeholder; there is no mutable food model yet.

`YardPlayer` owns vertical velocity, grounded grace, and the pending jump timer; authored speed/height/gravity values remain configuration. Its CharacterController uses short collision steps, cancels ascent at ceilings, and restores the authored step offset after each update. `YardSession` accepts one Jump press after release, clears pending jumps on pause/reset, and stops movement ticks while paused. The current airborne arc resumes afterward. Gameplay and UI input are disabled while unfocused; focus return enables the menu without resuming play. See the [on-foot movement contract](docs/look-sound-and-comfort.md#on-foot-movement).

Reset returns position, yaw, pitch, vertical velocity, and jump timing to the authored gate spawn. It preserves paused state. Out-of-bounds recovery uses the same path. When carrier state arrives in 1_02, recovery must preserve contents under the state contract; this foundation reset does not establish a quantity reset rule. Builds use the saved scene. The create command refuses to overwrite it, and Stage0 generation/build is guarded against changing the current project's settings. The focused `ApplyMovementUpdate` editor command updates existing input/HUD/colliders without regenerating the scene. See [delivery evidence](docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026).

## Unity authoring

During implementation, put new content in `Assets/JustAFewPeppers/`. Add Runtime, Presentation, Editor, Content, Scenes, and Tests folders only as real work requires them. Stage0 can be removed as its useful pieces are replaced or reused; do not rebuild it as a prerequisite.

Author stable pile/discovery IDs, initial quantities, capacities, rates, targets, and visual references in simple configurations. Use ScriptableObjects when they improve reuse; mutable state stays in the runtime model. Free commercially usable models attach to existing view components without unique rule scripts. Follow the [Unity and asset policy](docs/development/unity-and-assets.md), using placeholders early and custom assets only where needed.

Start with the project's Built-in Render Pipeline. New gameplay uses a compatible Input System package, configured in M1; Unity's [6000.6 input manual](https://docs.unity3d.com/6000.6/Documentation/Manual/Input.html) identifies the old Input Manager as deprecated. A small input-action boundary supports configurable bindings and hold/toggle settings. Check official documentation for the pinned editor/package versions before API choices. Record a rendering change only when a concrete asset or feature requires it.

Task 1_01 added runtime/editor/test assembly definitions and verified compatible Unity test infrastructure. The standalone Stage0 harness's .NET target is not Unity's runtime target.

## Data flow and verification

Input requests → session validation/transfer → state update → views and feedback. The [state/save contract](docs/development/state-and-saving.md) defines the conserved quantities. Commit accepted amounts once; interrupted visual motion cannot undo or repeat them.

EditMode checks exercise pure rules. PlayMode checks exercise scene wiring, targeting, recovery, and reconstruction. Packaged builds exercise the actual player path. Human play establishes action enjoyment.

Use a fixed pool of decorative moving peppers, independent of harvest size, and measure before increasing simulation. No factory routing, general crafting, multiple recipient inventories, multiplayer, or NPC work systems are planned.

