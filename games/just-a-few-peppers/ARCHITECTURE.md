# Architecture map

Status: proposed for v4, not implemented. The [audit](docs/development/repository-audit.md) describes the actual Stage0 code. [Core mechanics](docs/core-loop-and-mechanics.md) remain the player-facing contract.

## Ownership

Use one gameplay scene and a small composition component. Menus and the ending can be states within it. Stage0 is disposable reference material; reuse suitable pieces or retire obsolete code during implementation after checking retained references. Maintaining its old loop is not a requirement.

| Responsibility | Authoritative boundary |
| --- | --- |
| Plain C# session rules | One runtime model owns pile quantities, carriers, station work, stored food, equipment, and completion. Transfers use validated commands. |
| Scene composition | Owns the model, explicit references, ticking, and pause; no second inventory. |
| Input, movement, targeting | Resolve broad targets and request actions; never edit quantities independently. |
| Pile, carrier, station views | Render the model and bounded cosmetic motion; visual peppers do not own food. |
| Discovery and access | Derive open paths from cleared pockets and record equipment activation without losing contents. |
| Household and ending views | Read stored food and completed-day state; no recipient or shelf inventory. |
| Save adapter | Captures one snapshot and reconstructs views through stable IDs. |
| UI and audio | Read state and accepted actions; feedback never grants progress. |

These are responsibilities, not a manager class per row. Prefer a small model, scene driver, reusable views, direct calls, and local callbacks.

## Unity authoring

During implementation, put new content in `Assets/JustAFewPeppers/`. Add Runtime, Presentation, Editor, Content, Scenes, and Tests folders only as real work requires them. Stage0 can be removed as its useful pieces are replaced or reused; do not rebuild it as a prerequisite.

Author stable pile/discovery IDs, initial quantities, capacities, rates, targets, and visual references in simple configurations. Use ScriptableObjects when they improve reuse; mutable state stays in the runtime model. Free commercially usable models attach to existing view components without unique rule scripts. Follow the [Unity and asset policy](docs/development/unity-and-assets.md), using placeholders early and custom assets only where needed.

Start with the project's Built-in Render Pipeline. New gameplay uses a compatible Input System package, configured in M1; Unity's [6000.6 input manual](https://docs.unity3d.com/6000.6/Documentation/Manual/Input.html) identifies the old Input Manager as deprecated. A small input-action boundary supports configurable bindings and hold/toggle settings. Check official documentation for the pinned editor/package versions before API choices. Record a rendering change only when a concrete asset or feature requires it.

Add runtime/test assembly definitions and compatible Unity test infrastructure during the first implementation milestone. The standalone Stage0 harness's .NET target is not Unity's runtime target.

## Data flow and verification

Input requests → session validation/transfer → state update → views and feedback. The [state/save contract](docs/development/state-and-saving.md) defines the conserved quantities. Commit accepted amounts once; interrupted visual motion cannot undo or repeat them.

EditMode checks exercise pure rules. PlayMode checks exercise scene wiring, targeting, recovery, and reconstruction. Packaged builds exercise the actual player path. Human play establishes action enjoyment.

Use a fixed pool of decorative moving peppers, independent of harvest size, and measure before increasing simulation. No factory routing, general crafting, multiple recipient inventories, multiplayer, or NPC work systems are planned.
