We have now selected the game concept that will be built.

Before implementing the full game, I want to turn this repository into an **AI-first Unity development repository** where future coding agents can reliably implement individual features from written specifications, write tests, update documentation, and verify their work without needing the entire game explained again in every prompt.

I am a solo developer and expect AI coding agents to implement essentially all code.

The project therefore needs unusually clear:

- feature boundaries;
- data contracts;
- acceptance criteria;
- automated tests;
- save-state rules;
- implementation tracking;
- regression documentation;
- architecture documentation.

The goal is NOT enterprise architecture.

The goal is:

> Make a small Unity game extremely easy for coding agents to understand, modify, test and finish without creating architectural chaos.

---

# SOURCE MATERIAL

First read:

1. every game-design/research document I have provided for the selected concept;
2. the final concept-selection document;
3. the existing repository, if code/project files already exist;
4. the example documentation I provided from my other Rust game.

The Rust project is only a documentation/process reference.

Do NOT copy its complexity blindly.

It contains networking, procedural world generation, multiplayer authority, mod architecture and other systems that this game probably does not need.

Extract the useful patterns:

- concise persistent agent instructions;
- architecture decisions recorded explicitly;
- implementation checklists synchronized with reality;
- clear scope and non-goals;
- staged implementation plans;
- acceptance criteria;
- regression tests for every important bug;
- performance gates where actually relevant;
- explicit distinction between implemented / partial / todo;
- documentation updated in the same change as implementation.

Adapt those principles to a **small single-player Unity game**.

---

# FUNDAMENTAL PROJECT CONSTRAINTS

Unless the chosen concept genuinely requires otherwise:

- Unity;
- single-player;
- offline;
- one contained map or a very small number of contained areas;
- no multiplayer;
- no networking;
- no large NPC simulation;
- no ECS/DOTS unless a measured performance problem justifies it;
- no custom general-purpose engine/framework;
- no premature modding architecture;
- no open-world systems;
- no procedural generation unless explicitly necessary;
- no complicated dependency-injection framework;
- no unnecessary abstraction layers.

Prefer:

- simple C#;
- composition;
- explicit state machines;
- reusable MonoBehaviours/components;
- ScriptableObjects for authoring static content/configuration where appropriate;
- plain serializable runtime state for mutable state;
- stable IDs for persistent world objects;
- one authoritative save-game model;
- deterministic gameplay rules where possible;
- staged visual transformations rather than expensive real-time simulation;
- physics for presentation, NOT as the sole authority for progression-critical state.

Marketplace assets will be used heavily.

The code architecture must make marketplace prefabs/models easy to plug into reusable gameplay systems.

---

# CORE AI-DEVELOPMENT PRINCIPLE

The repository documentation is the **system of record**.

Future agents should not need to rediscover fundamental design decisions from source code.

However:

> Keep AGENTS.md short.

AGENTS.md should primarily tell agents:

- what this project is;
- where authoritative documentation lives;
- what files to read for different types of tasks;
- coding rules;
- testing commands;
- Definition of Done;
- documentation-update rules;
- prohibited architectural choices.

Do NOT put the entire game design inside AGENTS.md.

---

# PHASE 1 — AUDIT THE REPOSITORY

Before creating documents:

Inspect the repository.

Determine:

- Unity version;
- render pipeline;
- packages;
- folder layout;
- assembly definitions;
- Input System choice;
- test infrastructure;
- save implementation if any;
- existing scenes;
- existing prefabs;
- coding conventions;
- build configuration.

Do not invent details that can be discovered from the repository.

If this is a mostly empty Unity project, state that clearly.

---

# PHASE 2 — CREATE THE DOCUMENTATION ARCHITECTURE

Create approximately this structure, adapting names when necessary:

```text
AGENTS.md
ARCHITECTURE.md

docs/
    index.md

    product/
        game-vision.md
        design-pillars.md
        core-loop.md
        progression-and-economy.md
        content-catalog.md
        world-and-game-flow.md
        game-feel.md
        ux-accessibility.md

    features/
        <feature-specific documents>

    technical/
        unity-architecture.md
        data-model.md
        persistence.md
        testing-strategy.md
        performance-budgets.md

    implementation/
        mvp-checklist.md
        vertical-slice-plan.md
        active/
        completed/

    adr/
        <architectural decision records>

    testing/
        regression-fixtures.md
        playtest-checklist.md
```

Do not create empty documents merely to match this tree.

If a proposed document has no meaningful content yet, omit it or merge it with a related document.

Prefer approximately **12–25 useful documents** over 80 tiny documents.

---

# PHASE 3 — ROOT AGENTS.md

Create a concise root `AGENTS.md`.

Target roughly 80–150 lines unless there is a strong reason for more.

It should contain:

## Project summary

One short explanation of the game.

## Current development stage

For example:

- prototype;
- vertical slice;
- production;
- content complete;
- polishing.

## Documentation map

Tell agents exactly which documents to read for:

- game-design questions;
- feature implementation;
- architecture;
- save/load;
- testing;
- implementation status.

## Development principles

Include rules such as:

- prefer the simplest architecture that satisfies the documented requirement;
- do not introduce systems that are outside documented scope;
- do not preserve inferior prototype architecture merely for backwards compatibility unless requested;
- avoid duplicated gameplay truth;
- mutable runtime state must not live in ScriptableObject assets;
- progression-critical state must not rely exclusively on Rigidbody transforms;
- avoid hidden global mutable state;
- avoid `GameObject.Find`-style runtime dependencies where explicit references or registries are appropriate;
- do not silently add singleton managers for every feature;
- avoid reflection-heavy/general-purpose frameworks.

## Definition of Done

A feature is not complete merely because it compiles.

It is complete only when:

1. documented behavior exists;
2. code implements the full accepted scope;
3. automated tests appropriate to the feature pass;
4. relevant PlayMode integration behavior has been verified;
5. save/load behavior is tested if the feature is persistent;
6. visible presentation works where the player encounters it;
7. important edge cases pass;
8. relevant documentation/checklists are updated;
9. no known regression is being hidden;
10. packaged/build behavior is checked where appropriate.

## Test commands

Document exact commands once the repository supports them.

If Unity batch-mode testing is available, include the exact commands for:

- EditMode tests;
- PlayMode tests;
- build/smoke verification.

Do not fabricate commands that have not been verified.

---

# PHASE 4 — ARCHITECTURE.md

Create a concise architecture map.

This is NOT a 100-page architecture spec.

Document:

- major runtime systems;
- ownership of mutable state;
- data flow;
- scene boundaries;
- save boundary;
- content-authoring boundary;
- event/message boundaries;
- UI boundary;
- testing seams.

For each major system answer:

> Who owns the authoritative state?

Avoid having several MonoBehaviours independently represent the same gameplay truth.

A likely architecture for this class of game may contain concepts such as:

```text
GameSession
InteractionSystem
WorldObjectState
Inventory/Capacity
Processing
Progression/Upgrades
Economy or Completion
Discovery/Collection
SaveGame
Audio/VFX feedback
UI
```

Do not create a manager for something that can remain a normal component.

Adapt this to the actual chosen concept.

---

# PHASE 5 — PRODUCT DOCUMENTS

Create authoritative documents for:

## Game vision

Include:

- one-sentence pitch;
- player fantasy;
- target experience;
- approximate game length;
- target audience;
- commercial hook;
- Steam/trailer hook.

## Design pillars

Keep to approximately 4–7 pillars.

Include anti-pillars:

> Things this game deliberately is NOT.

## Core loop

Document:

- second-to-second loop;
- short task loop;
- economy/progression loop;
- whole-game loop.

## Progression

Fully specify:

- starting capability;
- bottlenecks;
- upgrade dimensions;
- unlock order;
- victory-lap/final-power timing;
- progression ending.

Do not leave progression as “balance later.”

Numbers can remain tuning data, but structural progression must be explicit.

## Content catalog

Define all player-facing content classes.

Examples depending on the selected game:

- target/object classes;
- materials;
- item states;
- tools;
- upgrades;
- discoveries;
- collectibles;
- containers;
- processing stations;
- areas;
- secrets.

For each content family distinguish:

- static authoring data;
- mutable runtime state;
- persistent state.

Prefer data-driven content where it meaningfully reduces bespoke scripts.

## World/game flow

Map:

- starting space;
- locked/visible future spaces;
- unlock order;
- major visual transformations;
- final objective;
- ending.

---

# PHASE 6 — FEATURE CONTRACTS

Create one feature specification for every **major gameplay system**, not every tiny class.

Each feature document should use approximately this structure:

```text
# Feature: <name>

Status:
Owner/source-of-truth:
Related docs:

## Player purpose
Why this feature exists.

## Scope
What is included.

## Non-goals
What explicitly is NOT included.

## Player-visible behavior
Exact behavior from the player's perspective.

## Rules and invariants
Rules that must always remain true.

## State model
Important states/transitions.

## Data model
Authoring data versus mutable runtime data.

## Unity implementation boundary
Likely components/assets and responsibilities.

## Interaction with other systems
Inputs/outputs/events.

## Persistence
What must save/load.

## Feedback
Animation/audio/VFX/UI requirements.

## Failure and recovery behavior
What happens if something goes wrong.

## Edge cases
Explicit list.

## Acceptance criteria
Binary testable conditions.

## Automated test plan
EditMode + PlayMode coverage.

## Manual test checklist
Only things automation cannot reasonably verify.

## Performance considerations
Only if relevant.

## Implementation status
Todo / Partial / Implemented.
```

The feature spec is a **contract**, not pseudocode dictating every private method.

Allow the implementation agent to choose clean implementation details as long as it satisfies the contract and project architecture.

---

# PHASE 7 — DATA-DRIVEN CONTENT

Design reusable content models appropriate to the selected game.

Do not hard-code every item/object as its own script.

For example, if the chosen game has processable objects, an architecture might conceptually support:

```text
ObjectDefinition
    stable ID
    display data
    category
    base properties
    allowed states
    interaction profile
    reward/value
    required tool
    presentation references
```

with separate mutable runtime state:

```text
ObjectRuntimeState
    stable instance/world ID
    current state
    progress
    collected/cleared flags
    discovered flags
    persistent variables
```

Adapt this to the actual game.

Use ScriptableObjects where they improve authoring.

Do NOT use ScriptableObjects as mutable save-game state.

---

# PHASE 8 — SAVE/PERSISTENCE CONTRACT

This deserves an explicit document even for a small game.

Document exactly what is authoritative and persistent.

Prefer:

```text
SaveGame
    schema version
    player/session state
    progression state
    currency
    upgrades
    collection state
    stable world-object states
    area completion
    relevant machine/process state
```

Requirements:

- stable IDs;
- one coherent snapshot;
- no rerolling random discoveries after load unless explicitly designed;
- no disagreement between world state and upgrade/progression state;
- temporary-write → validation → replace where practical;
- at least one recovery/backup strategy if full-game saves become important;
- explicit behavior for unsupported/corrupted data.

At prototype stage, backwards compatibility with old experimental saves is not required unless I explicitly request it.

Do not preserve poor schemas merely because they already exist.

---

# PHASE 9 — TESTING STRATEGY

Use Unity's appropriate automated-test infrastructure.

Separate:

## EditMode tests

Best for:

- deterministic state transitions;
- content validation;
- progression;
- economy;
- upgrade rules;
- loot/discovery tables;
- save serialization;
- calculations;
- state-machine edge cases.

## PlayMode tests

Best for:

- actual interactions;
- prefab/component wiring;
- scene transitions;
- world-object state;
- tool targeting;
- save → reload → world reconstruction;
- input-driven integration where practical;
- completion flow.

## Smoke/build tests

Where the environment supports them:

- open/bootstrap scene;
- start game;
- exercise minimal interaction;
- verify no exceptions;
- packaged/batch build verification.

Do not create brittle visual tests for things that can be tested as deterministic state.

---

# PHASE 10 — REGRESSION FIXTURE DOCUMENT

Create:

`docs/testing/regression-fixtures.md`

This should become a permanent history of important bugs.

Every meaningful bug should eventually become:

```text
Reported failure:
Root cause:
Acceptance contract:
Automated regression test:
Owner/system:
Status:
```

When fixing an important bug:

> reproduce with a failing test first when practical.

Then fix it.

Then keep the test forever.

The regression document should link the bug/failure to the test that prevents recurrence.

---

# PHASE 11 — MVP IMPLEMENTATION CHECKLIST

Create one living implementation-status document.

This is one of the most important documents in the repository.

Use:

```text
[x] Implemented
[ ] Partial — explanation
[ ] Todo — explanation
```

Never mark something complete because:

- some classes exist;
- only the happy path works;
- the editor demo looked right once.

A feature is complete only when the documented acceptance criteria for the current milestone are met.

Organize by milestone, for example:

```text
Foundation
Core Verb
First Complete Object
First Processing Loop
Progression
Save/Load
First Complete Area
Vertical Slice
Content Production
Ending
Polish
Release
```

Update this checklist in the SAME change that changes implementation status.

---

# PHASE 12 — EXECUTION PLANS

For features that are non-trivial, use:

`docs/implementation/active/<feature>-plan.md`

A plan should contain:

- problem;
- current state;
- desired behavior;
- scope;
- non-goals;
- architectural impact;
- implementation slices;
- tests/fixtures;
- performance requirements if relevant;
- acceptance gate;
- rollback/recovery considerations;
- documentation that must be updated.

Once completed, move it to:

`docs/implementation/completed/`

Do not create execution plans for trivial one-file changes.

---

# PHASE 13 — ADRs

Use Architecture Decision Records only for decisions that future agents are likely to revisit incorrectly.

Examples may include:

- authoritative interaction/state model;
- static content IDs;
- save architecture;
- whether gameplay bulk objects are logical versus physics-authoritative;
- scene ownership;
- event architecture;
- ScriptableObject/runtime-state boundary.

ADR format:

```text
# ADR NNNN: Title

Status:
Date:

## Context

## Decision

## Why

## Alternatives considered

## Consequences

## Revisit when
```

Do NOT create ADRs for obvious or reversible implementation details.

---

# PHASE 14 — PERFORMANCE BUDGETS

Do not overengineer performance.

Identify only risks actually relevant to this game.

For example:

- maximum simultaneously active interactive objects;
- Rigidbody budget;
- particle/debris limits;
- garbage allocations during the main interaction;
- save/load duration;
- scene startup;
- machine processing loops;
- expensive physics queries.

Write measurable budgets only where there is a reason.

If the game visually contains thousands of objects, explicitly define which are:

- full gameplay objects;
- sleeping/static;
- visual proxies;
- aggregated logical state.

The player may see 10,000 objects.

The CPU does not necessarily need 10,000 active simulations.

---

# PHASE 15 — UNITY-SPECIFIC AUTHORING RULES

Document the project's actual rules for:

- prefab ownership;
- scene ownership;
- ScriptableObject locations;
- runtime/generated objects;
- assembly definitions;
- editor tooling;
- tests.

Prefer reusable components that make marketplace assets easy to convert into game content.

If large numbers of assets need identical setup, prefer a small validated Editor tool/import workflow over manually modifying hundreds of prefabs.

Avoid fragile manual manipulation of Unity serialized YAML when a safe editor/runtime authoring path is available.

Do not create editor tooling for operations that will only happen once.

---

# PHASE 16 — GAME FEEL CONTRACT

The primary repeated action needs an explicit quality checklist.

Document:

- anticipation/windup;
- animation;
- hit/action timing;
- resistance;
- progress/state change;
- particles/debris;
- sound layers;
- completion burst;
- UI feedback;
- camera feedback;
- rarity feedback if relevant;
- before/after visual state.

A system is not “done” merely because its state changes numerically.

If it is a repeated core interaction, its presentation is part of its acceptance criteria.

---

# PHASE 17 — ACCESSIBILITY / QoL BASELINE

For a first-person PC game, establish at least the intended baseline for:

- mouse sensitivity;
- FOV;
- invert Y;
- key rebinding;
- master/music/SFX volume;
- camera shake;
- head bob if used;
- motion blur if used;
- hold/toggle interactions where appropriate;
- completion assistance for the final few targets.

Do not postpone every QoL feature until the final week.

---

# PHASE 18 — DOCUMENTATION SYNCHRONIZATION RULE

This rule is critical.

Whenever an agent changes behavior that affects:

- architecture;
- feature specification;
- content;
- persistence;
- implementation status;
- test contract;

the associated documentation must be updated **in the same task/change**.

Never let code knowingly invalidate the documentation and leave it for “later.”

If implementation reveals the documented design is wrong:

> update the design explicitly rather than silently implementing something else.

---

# PHASE 19 — FUTURE IMPLEMENTATION TASK CONTRACT

After this setup is complete, future prompts should be able to be short.

For example:

> Implement `docs/features/<feature>.md`. Read AGENTS.md and all linked docs. Implement the complete currently-scoped feature, add/update EditMode and PlayMode tests, add regression coverage for any bugs found, update the implementation checklist and affected documentation, run the relevant test/build commands, and report acceptance evidence. Do not mark the feature complete if any acceptance criterion remains unmet.

The repository should contain enough context for that prompt to work.

---

# PHASE 20 — DO NOT OVERDOCUMENT

Before creating any document ask:

> Would a future coding agent make a materially better decision because this file exists?

If no:

Do not create it.

Avoid:

- duplicate documentation;
- prose copies of source code;
- files that only repeat another file;
- giant speculative architecture documents;
- design documents for systems outside MVP;
- placeholder directories full of empty files.

Documentation should reduce ambiguity, not create it.

---

# FIRST DELIVERABLE

For this task, DO NOT implement the full game.

Produce the **documentation and repository-development scaffold**.

At the end give me:

1. the final documentation tree;
2. a short explanation of the purpose of every document created;
3. major architecture decisions;
4. missing decisions that genuinely require my input;
5. recommended first vertical-slice implementation task;
6. exact tests/verification that should exist before moving from prototype to production.

If a decision can reasonably be derived from the selected game's research and project constraints, make the decision rather than asking me unnecessary questions.

The result should leave the repository ready for future AI agents to implement one documented feature at a time with minimal ambiguity.