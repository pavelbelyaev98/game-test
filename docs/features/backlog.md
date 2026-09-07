# Feature backlog (ask AI for one feature at a time)

Source: `docs/idea.md` and the core constraints in `docs/scope-and-validation.md`.

Use this list as one-click handoff prompts: pick the top unchecked item and ask an AI to fully implement only that feature set.

## Feature 1 — Core loop contract (foundation)

**Goal:** Lock the "dig → detect → uncover → collect → return → sell → upgrade → repeat" flow.

- Surface boundary and finite dig area rules
- One-shot loop per expedition
- No pre-dug hints
- Return condition and end-of-trip checkpoint behavior

**Ask AI for next:**  
“Implement the core loop contract as a design + lightweight prototype plan: how expedition start, dig session, return condition, sell checkpoint, and upgrade gating work. Include failure/edge cases and a test checklist.”

## Feature 2 — FPS camera and movement baseline

**Goal:** Establish player control contract before any content.

- First-person camera (mouse/look)
- Movement (forward/back/strafe/jump)
- Jetpack placeholder with battery drain integration
- Basic interaction raycast (`Dig`, `Interact`, `Sell`, `Upgrade`, `Inventory`)

**Ask AI for next:**  
“Create a deterministic FPS control/spec package with input mapping, movement behavior, and interaction bindings using Unity Input System. Provide `ScriptableObject`/data contract and test checklist.”

## Feature 3 — Excavation and terrain system

**Goal:** Player should be able to excavate freely (not strictly downward only).

- Excavation direction freedom: down/diagonal/sideways/lateral digging
- Terrain material variants (sand/soil/clay/gravel/rock)
- Hardness/factor that changes progress and can be revisited
- World state persistence when dirt is removed

**Ask AI for next:**  
“Design the excavation data model and terrain system so digging modifies persistent voxel/mesh state. Include material hardness, excavation depth profile, and replayable behavior constraints.”

## Feature 4 — Detector feedback system

**Goal:** Keep discovery meaningful and non-constant.

- Passive beep/signal logic (not a map/GPS)
- Signal strength/quality changes with distance/size/progression
- Cooldown/pacing to avoid constant noise fatigue
- Directional hints without revealing exact rarity

**Ask AI for next:**  
“Build detector behavior rules and tuning curves. Deliver pseudocode/state table for signal intervals, directionality, overlap handling, and false ambiguity behavior.”

## Feature 5 — Discovery/loot generation framework

**Goal:** Replayable and varied finding layout while preserving content progression identity.

- Procedural placement of discovery pool per run
- Separate ordinary and distinct discovery pools
- Positional + depth + rotation randomization
- Overlap and cluster support (wheel near axle, bones nearby, etc.)

**Ask AI for next:**  
“Design a discovery generator contract (seeded or deterministic enough for testing) with pools, weights, depth bands, and cluster rules. Include save/load representation.”

## Feature 6 — Item/loot reveal and collection

**Goal:** Keep reveal feel fast, clear, and satisfying.

- Small items: quick expose + instant collect
- Distinctive objects: staged reveal states
- Recognition progression before pickup (e.g., curved piece → handle → shape readable)
- Non-pixel-perfect, no full archaeological cleanup

**Ask AI for next:**  
“Define item reveal states and pickup rules. Provide object lifecycle, exposure thresholds, and transition logic from hidden → discovered → interactable → collected.”

## Feature 7 — Inventory and carry abstraction

**Goal:** Keep player momentum by avoiding cumbersome pickup burden.

- Simple abstract slot inventory
- Adjustable slot scale progression (10 → 15 → 20 → 30 → 40)
- Collect + sell flow without individual heavy item handling
- Clear full-inventory warning and recoverable flow

**Ask AI for next:**  
“Implement abstract inventory architecture and UI behavior contract (no per-item hand carrying). Include slot expansion rules and edge cases for full inventory.”

## Feature 8 — Selling and upgrade checkpoint

**Goal:** Make return trips meaningful and fast.

- Dedicated selling interaction at surface
- One-click `SELL ALL`
- Upgrade tree accessible at or near selling
- Money only secured on successful return + sale

**Ask AI for next:**  
“Create the checkpoint architecture for Return → Sell → Upgrade. Include money lock/unlock rules, sell feedback, and upgrade purchase flow.”

## Feature 9 — Shovel progression system

**Goal:** Escalating single-tool evolution from subtle to absurd.

- One primary tool evolves through 6–8 meaningful levels
- Milestones: faster, wider, more efficient, major tier jump, hard-material breaker
- Major upgrades feel obvious in play
- Prevent jump-level bypass (progress one level at a time)

**Ask AI for next:**  
“Design shovel upgrade schema with numeric tiers, costs, and explicit player-visible effects. Include balance tables and migration safety for save data.”

## Feature 10 — Battery and jetpack economy

**Goal:** Add decision tension without punitive UI spam.

- Shared battery between dig and jetpack
- Energy should matter only on meaningful actions
- Return warning (safe/risky/critical), not exact percentage
- Surface recharge is free/fast

**Ask AI for next:**  
“Define battery model and consumption equations; add return-warning thresholds and tuning strategy. Include no-penalty policy for idle/viewing/inspection.”

## Feature 11 — Return and risk system

**Goal:** Make come-up gameplay meaningful.

- Player must physically return through dug area
- Optional jetpack-assisted extraction
- Hard falls/stunts are risk events, not punishments loop
- Rescue fallback keeps excavation state (no full reset)

**Ask AI for next:**  
“Design return flow with risk levels and recovery states. Include fall consequences, rescue fallback with finite reset cost, and excavation persistence rules.”

## Feature 12 — Camera/ambience and tone delivery

**Goal:** Match “no music” atmosphere and physical feedback.

- Minimalist ambient audio palette (wind/water/earth/tools)
- Detector rhythm and excavation sound layer
- Sell/upgrade tactile/sound cues
- Visual style direction for readable dirt/material distinctions

**Ask AI for next:**  
“Define the audio/visual atmosphere system: SFX mix priorities, environmental loops, and feedback loops for discovery/dig/sell states.”

## Feature 13 — Progression discovery wall

**Goal:** Preserve “first-person finds” nostalgia and replay proof.

- Auto-capture discovery snapshot per first unique find
- Surface wall display with item name + depth only
- No sale value or condition in wall
- Stable ordering and save persistence

**Ask AI for next:**  
“Implement discovery wall data model and UI contract: snapshot metadata, dedupe rules, storage size limits, and save/versioning behavior.”

## Feature 14 — Final arc & ending gate

**Goal:** Define late-game payoff while preserving open-ended replay.

- Mid and late progression beats
- Final discovery decision + ending cutscene trigger
- Continue mode remains available after ending
- Keep core loop alive after cutscene

**Ask AI for next:**  
“Create endgame state machine (pre-ending, reveal, ending cutscene trigger, continue mode) and anti-dead-end safeguards.”

## Feature 15 — Optional systems (post-core, staged)

- **Dynamite expansion** (placement + remote detonation + area effects)
- **Special objects** (few non-inventory keys/components)
- **Location-specific weird discoveries** and lore micro beats

**Ask AI for next (if and only if core loop is stable):**  
“Design one optional system at a time from this section; include integration constraints with existing progression and no scope creep.”

---

## Backlog execution rule for AI handoffs

1. One feature at a time.
2. Before implementation request: define acceptance test and one regression check.
3. After implementation: update:
   - `docs/development/tasks.md`
   - `docs/development/status.md`
   - Related scope/design files
