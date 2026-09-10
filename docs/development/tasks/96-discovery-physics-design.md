# Task 96 - Design selective physical movement of finds

Type: design/research; documentation only. Status: `planned`. Prerequisites: baseline observations from `101`; reuse `89`/`09` content evidence when available without reopening their starter scope.

Feature: [discovery collection](../../features/backlog/discovery-collection.md). Context: [idea assessment](../../research/player-idea-assessment.md), [risk register](../design-risks.md). Coordinate special interactions with `97`; actual roster remains `40`.

## Concrete comparison

- Propose three readable before/after sequences for a freed small object, a bulky find on a shelf and a ceiling find: fixed pose; restrained local settling/tipping; unrestricted rigid-body motion. Recommend which categories, if any, benefit. Large cable extraction remains an existing separate optional concept, not automatic scope here.
- Separate **collectible exposure** from **loss of support**. The existing 40% threshold is not automatically a physics-release trigger. Show support remaining on one side, soil removed beneath, airborne exposure and multiple nearby finds; a collectible should not unexpectedly fall away while being recognized.
- Define collision with the player/other finds, maximum acceptable displacement, rest/reburial, reachability and a credible recovery when terrain changes beneath an object. No destruction, tiny rolling-loot chase, blocked return route, mandatory carrying or physics puzzle.
- Inspect the existing saved position/rotation and stable identities. Compare settling before a checkpoint with persisted moving state; decide reload, rescue, partial support and out-of-bounds recovery without resetting the excavation or duplicating/selling twice. Technical physics/query choices and performance budgets belong to later implementation, informed by official Unity guidance then.
- Include recognition and real first-discovery snapshot timing for `60`/`51`; moving an object must not turn the personal photograph into empty dirt. Account for held pickup and `97`'s proposed deliberate special interaction without selecting E here.
- Produce a category/state table, illustrated textual sequences, explicit keep/fixed/settle decisions and a bounded prospective asset/integration list. No new art is required to make the design review concrete.

## Review and acceptance

Review how much movement feels physical/funny, whether the player may push a freed object, and which finds must remain anchored. Recommend a restrained default with specific exceptions; explain rejected full-simulation cases.

Record the accepted behavior and exclusions in the collection feature. If selected, create the next numbered implementation task with support/release/collision/save/recovery and Windows feel checks; otherwise record why it is deferred/omitted. `40` consumes category requirements only after selection. A design decision never marks discovery physics implemented.
