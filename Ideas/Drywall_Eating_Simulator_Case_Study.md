# Drywall Eating Simulator — Design Case Study
## What a ridiculous meme premise can teach us about tiny Steam games

**Research date:** September 5, 2026  
**Game:** *Drywall Eating Simulator*  
**Developer / Publisher:** Peripheral Playbox  
**Release:** December 10, 2025  
**Target of this study:** extract reusable design lessons for a small, single-player Unity game built by a solo developer using marketplace assets and AI coding assistance.

---

# 1. Executive conclusion

*Drywall Eating Simulator* is valuable to study because its title does almost all of the marketing work.

> **You eat drywall because ordinary life makes you angry.**

That premise is:
- instantly understandable;
- absurd enough to be memorable;
- easy to explain in one sentence;
- easy for a streamer to title a video around;
- cheap to represent visually.

The official game is a short first-person comedic adventure rather than a deep simulator. NPC conversations increase the player's stress. High stress reveals/glows certain wall sections, and the player bites/smashes through drywall to relieve stress, create shortcuts, and continue through several satirical scenarios.

At the time of this research Steam shows a **Mostly Positive** user rating, with roughly three quarters of reviews positive. Players who like it frequently praise:
- the absurd premise;
- the crunch/sound of eating drywall;
- the character art;
- the short format;
- the humor and stress-release fantasy.

Its problems are almost the perfect warning list for our project:
- quest/save-state softlocks;
- objectives firing out of order;
- unclear interactable walls;
- doors becoming nonfunctional;
- player physics issues;
- mouse/camera sensitivity and motion-sickness complaints;
- framerate/performance issues;
- overly simple gameplay that sometimes cannot carry the writing;
- polarizing satire;
- low long-term replay value.

The most important lesson is:

> **A meme premise can get attention, but the meme must sit on top of a reliable, satisfying game loop.**

And another:

> **Do not make the core physical interaction dependent on a fragile quest chain.**

---

# 2. Verified premise and structure

The official Steam description calls the game a “bite-sized comedic adventure” about relieving ordinary-life stress by biting through walls.

The game presents several everyday/social situations:
- apartment/neighborhood interactions;
- office/corporate environments;
- retail/social settings;
- an art-gala-like setting;
- other satirical modern-life encounters.

NPC conversations are deliberately irritating. As stress builds, wall sections become available to destroy/eat. The player uses that destructive act both as:
- stress relief;
- navigation;
- progression.

The official store also explicitly says:
- multiple levels;
- many characters;
- nosy NPCs;
- anger/stress management;
- keeping the drywall-eating secret hidden.

This is therefore **not** a sandbox where every wall is always edible.

It is a more linear structure:

```text
ENTER SOCIAL SITUATION
        ↓
TALK / INTERACT
        ↓
BECOME STRESSED
        ↓
WALL SECTIONS BECOME AVAILABLE
        ↓
EAT / BREAK WALL
        ↓
STRESS DECREASES
        +
NEW PATH OPENS
        ↓
CONTINUE OBJECTIVE
```

---

# 3. The core interaction

The moment-to-moment fantasy is extremely simple:

```text
LOOK AT EDIBLE WALL
→ BITE / SMASH
→ CHUNK BREAKS
→ CRUNCH
→ WALL OPENS
→ STRESS RELIEF
```

This is a strong **completion burst**.

The player has a target with an obvious before/after state:

```text
SOLID WALL
→ DAMAGED WALL
→ HOLE
```

That is mechanically related to:
- chopping a tree;
- washing a dirty panel;
- removing a trash pile;
- breaking rock;
- stripping wallpaper.

The action permanently changes navigation inside the level.

That is one reason the concept works better than a joke where pressing a button merely increments “drywall eaten: +1.”

---

# 4. Stress is both permission and motivation

The stress system is the game's most important abstraction.

NPC/social friction increases stress.

At sufficient stress, edible wall sections become highlighted or visible.

Eating drywall reduces that stress.

So the system creates:

```text
ANNOYANCE
→ PRESSURE
→ RELEASE
```

That is a very readable emotional loop.

## Why it is clever

The bar does more than serve as a timer.

It explains:
- why the character eats drywall;
- when the player is allowed/encouraged to destroy walls;
- why NPC dialogue matters mechanically;
- why the destructive act feels like relief.

One system ties story, mechanics and comedy together.

### Blueprint lesson

> **The best constraint should explain the fantasy, not feel pasted on.**

Examples:

- stress → destroy;
- dirt → clean;
- overheating → cool;
- capacity → unload;
- pressure → vent;
- moisture → return to water;
- contamination → isolate/clean.

---

# 5. “Rage vision” is a detector mechanic

When a wall becomes relevant, it glows red / is made visually obvious.

This is a cousin of:
- Digging a Hole's detector;
- PowerWash dirt highlighting;
- completion scanners;
- Librarian shelf guidance;
- Cash Cleaner's UV inspection.

The purpose is the same:

> **Tell the player that ordinary-looking space contains a gameplay target.**

The wall itself is obvious visually, but the *correct destructible section* is not always obvious without assistance.

This matters because the game learned the hard way that when the highlight fails, players can believe the game is broken or have no idea where to progress.

---

# 6. Wall destruction doubles as traversal

A bite does not only give tactile satisfaction.

It can:
- open a direct route;
- bypass a conventional doorway;
- let the player peek into another room;
- turn architecture into a toy.

This is a powerful design role:

> **MAIN VERB = PROGRESSION + NAVIGATION**

That is economical.

The game does not need a separate lockpicking system for every route.

The same wall-eating mechanic can change where the player can go.

---

# 7. Nested loops

## Second-to-second loop

```text
MOVE
→ LOOK
→ TALK / TRIGGER STRESS
→ FIND RED WALL
→ EAT WALL
```

## 2–10 minute scene loop

```text
MEET ANNOYING CHARACTERS
→ FILL STRESS
→ DESTROY PATH
→ COMPLETE LOCAL OBJECTIVE
```

## Level loop

```text
ENTER NEW SOCIAL SCENARIO
→ ENCOUNTER NEW JOKES / NPCS
→ REPEAT STRESS-RELEASE STRUCTURE
→ EXIT LEVEL
```

## Whole-game loop

```text
MODERN-LIFE SATIRE
→ INCREASINGLY ABSURD WALL EATING
→ KEEP SECRET / NAVIGATE LEVELS
→ REACH FINAL SCENARIO / ENDING
```

Unlike most games we studied, there is little economy or transformative tool progression.

That keeps scope small but also reduces long-term mechanical anticipation.

---

# 8. What players consistently like

## 8.1 The title is the hook

“Drywall Eating Simulator” is inherently a joke.

You do not need:
- lore;
- a paragraph;
- a trailer explanation.

The name creates curiosity.

This is enormously valuable for a low-budget Steam game.

### Blueprint rule

> **A meme game should be funny before the player knows any mechanics.**

Good titles create an image in the player's head.

---

## 8.2 The physical crunch matters

Positive Steam reviews explicitly praise the satisfying crunch.

That validates one of our strongest existing principles:

> **For a one-verb game, sound design is gameplay.**

Eating drywall needs:
- bite anticipation;
- crack;
- crumb/debris response;
- wall chunk disappearance;
- stress feedback.

Without weight and sound, it would feel like clicking a texture.

---

## 8.3 The action has emotional meaning

Breaking the wall is not random destruction.

The character does it because social life has become intolerable.

The loop is:

> irritation → destructive relief.

That makes the same animation more memorable.

### Reusable lesson

Attach the main verb to a tiny emotional fantasy.

Examples:
- “the mess is driving me insane” → organize;
- “I need winter supplies before snow” → preserve;
- “this basement has defeated the family for 30 years” → clean out;
- “baba will inspect these peppers” → roast perfectly.

---

## 8.4 Short length helps the joke

Several positive/neutral reviews describe it as short-but-sweet.

The Magic Rain reports roughly a 90-minute playtime for its run.

A joke premise often benefits from ending before the player has fully exhausted it.

### Blueprint rule

> **A small absurd game does not need 10 hours.**

The right question is:
> “Did the player get enough escalation and a proper ending?”

not:
> “Can we make them repeat the joke for eight hours?”

---

# 9. Negative-review cluster #1 — quest/save softlocks

This is the biggest technical failure.

Early reviews repeatedly report:
- quest list not updating;
- final objectives never appearing;
- NPCs remaining after their state should change;
- player unable to continue;
- restart failing to repair state;
- walls not respawning/activating;
- the game forgetting which task should be active.

The developers later explicitly confirmed a **major conflict between the quest system and save system**.

Patch 1.1.0 overhauled quest/save data and attempted to repair corrupted saves.

This is an unusually valuable piece of evidence because it identifies the architectural cause, not merely the symptom.

## Blueprint rule

> **Do not let quest state, world state and save state independently decide what should exist.**

For a tiny game, there should be one authoritative state.

Example:

```text
CurrentLevel
CurrentObjective
ObjectiveFlags
NPCFlags
WallFlags
PlayerProgress
```

The wall should not independently ask:
> “Am I active?”

It should derive that from authoritative objective state.

---

# 10. Negative-review cluster #2 — objectives fire out of order

A later hotfix explicitly fixed office quests firing out of order on new saves.

This is common when:
- trigger volumes;
- NPC conversations;
- wall destruction;
- level loading;

can all change progression independently.

### Better architecture

```text
Objective 4 cannot become active
unless Objective 3 == Complete
```

Use explicit state transitions:

```text
Inactive
→ Available
→ Active
→ Complete
```

Avoid “if player touched X, maybe activate Y” logic spread across many scripts.

---

# 11. Negative-review cluster #3 — target highlighting failures

Reviews mention a wall behind an NPC failing to light up.

Patch notes later include a fix for a wall behind the tired boss not lighting correctly.

This is important because:

> **If the intended interaction is unusual, the affordance must be reliable.**

Players already accept:
> “I can eat walls.”

They should not have to guess:
> “Which of these 40 wall panels is secretly interactive right now?”

### Rule

When an unusual verb is context-gated:
- outline target;
- change material;
- add sound;
- add reticle cue;
- add accessibility option for stronger highlight.

---

# 12. Negative-review cluster #4 — doors and traversal blockers

An update broke glass-door interactions, producing progression blocks until hotfixed.

Players also reported falling/floating around stairs.

Patch notes later fixed:
- glass-door interaction;
- areas allowing the player to fall through the map;
- floating on office stairs.

This demonstrates another small-game truth:

> **A linear game is only as stable as its least reliable doorway.**

If progression has one path, one broken door can make the entire product unfinishable.

### Blueprint rule

Always provide recovery:
- restart checkpoint;
- level select;
- teleport to safe point;
- reset current objective;
- “unstuck.”

The game eventually added **level select** in 1.3.0, which is useful both as QoL and as a recovery mechanism.

---

# 13. Negative-review cluster #5 — input and motion sickness

Launch feedback included mouse sensitivity problems.

A hotfix adjusted sensitivity behavior and controller menu selection.

Community discussion later still included users asking for lower sensitivity due to motion sickness.

Another review reported sluggish camera behavior on handheld hardware.

The game eventually received further UI/controller work and became Steam Deck Verified.

### Blueprint rule

Every first-person simulator should launch with:
- wide sensitivity range;
- FOV slider;
- motion blur toggle;
- head-bob toggle if used;
- camera-shake slider;
- controller sensitivity;
- invert Y;
- good resolution/UI scaling.

These are not “late polish.”

---

# 14. Negative-review cluster #6 — performance

Patches mention attempted framerate fixes.

Some reviews describe inconsistent performance.

The environments are not obviously gigantic, so performance problems are especially damaging to perception.

A tiny-looking game that runs badly feels less forgivable than an enormous simulation.

### Rule

> **Visual simplicity raises performance expectations.**

For our game:
- profile packaged builds early;
- test target low/mid GPUs;
- avoid expensive post-processing by default;
- pool debris;
- cap active physics pieces;
- avoid rebuilding collision meshes unnecessarily.

---

# 15. Negative-review cluster #7 — gameplay too simple

Several critic reviews say the actual mechanic is too shallow.

The pattern is basically:

```text
TALK
→ GET ANGRY
→ EAT WALL
→ REPEAT
```

For some players, writing carries this.

For others, once the joke is understood there is little mechanical anticipation.

This is the key difference from:
- Digging a Hole;
- Cash Cleaner;
- Leaf it Alone;
- Librarian.

Those games give the player:
- upgrades;
- transformation progression;
- object taxonomy;
- workflow optimization;
- collection;
- mystery;
- increasing power.

Drywall largely uses **new dialogue/scenarios** as its content progression.

### Blueprint rule

> If the story is not exceptional, the main verb needs mechanical escalation.

---

# 16. Negative-review cluster #8 — humor is risky content

The satire is polarizing.

Positive reviewers find:
- workplace frustration relatable;
- jokes charming;
- the absurdity therapeutic.

Negative critics call the writing:
- overly internet-brained;
- shallow;
- repetitive;
- cynical;
- disconnected.

This is not something a mechanical patch can fully solve.

### Blueprint lesson

> **Do not make “the writing is funny” the only reason to continue.**

Humor should decorate a good loop.

The safest meme structure is:

```text
FUNNY PREMISE
+
SATISFYING VERB
+
ESCALATING POWER
+
VISUAL COMEDY
```

not:

```text
FUNNY PREMISE
+
LOTS OF DIALOGUE
```

---

# 17. Streamer hook versus player hook

SteamDB data is directional rather than perfect, but it is revealing:
- very low all-time concurrent player peak;
- much larger recorded Twitch peak.

That pattern fits the nature of the title.

A creator can make:
> “I PLAYED DRYWALL EATING SIMULATOR”

and viewers instantly understand the joke.

But watching the joke may satisfy some potential buyers without making them want to personally repeat the loop.

### Blueprint rule

A meme game needs two separate answers:

## Why would I WATCH this?
Funny premise.

## Why would I BUY this?
The interaction/progression feels better in my hands than on video.

Our ideas should satisfy both.

---

# 18. Patch history as a QA case study

## December 2025 launch

Major reports:
- progression blockers;
- save issues;
- sensitivity;
- softlocks.

## Hotfix 1.0.0f — December 12

- sensitivity behavior;
- controller option selection.

## Version 1.1.0 — December 21

Major quest/save overhaul:
- fixed conflict between quest system and save system;
- save repair attempt;
- falling-through-map fixes;
- focus on blockers/softlocks.

## Hotfixes 1.1.x

- credits/main-menu bug;
- framerate work;
- quest order fixes;
- glass-door interaction fixes.

## Version 1.2.0 — January 8, 2026

- player physics pass;
- VFX scaling;
- UI scaling;
- quest fixes;
- wall highlight fix.

## Version 1.3.0 — February 21

- level select;
- additional fixes/features;
- described as likely last major content update.

## Version 1.3.1 — March 2

- Steam Deck Verified support;
- menu/controller/resolution improvements.

### Key lesson

The developers were responsive.

But a short game lost disproportionate value because many users could not reliably finish it at launch.

---

# 19. How I would redesign the architecture, not the theme

I would preserve:

- ridiculous title;
- one tactile destructive verb;
- stress → release;
- walls changing traversal;
- short runtime;
- stylized art;
- environmental satire.

But I would add **mechanical escalation**.

## Phase 1 — basic drywall

One bite type.

## Phase 2 — layered walls

Some wall sections require repeated bites / different angle.

## Phase 3 — hidden contents

Walls occasionally contain:
- cables;
- pipes;
- collectibles;
- strange objects;
- shortcuts.

## Phase 4 — choice

Some walls are safe to eat.

Some create:
- alarms;
- flooding;
- electrical problem;
- awkward route.

## Phase 5 — absurd power

Final ability lets the player rip through huge sections.

Now:

```text
WALL EATING
```

is still the joke,

but the *game* keeps evolving.

---

# 20. Better use of stress

The existing game often uses stress as a gating mechanism.

A stronger structure could make stress a **power resource**.

Example:

```text
LOW STRESS
normal bite

MEDIUM
larger bite

HIGH
rage bite / wall smash

MAX
dangerous loss of control
```

Now the player may intentionally tolerate annoying NPCs to charge a stronger destruction ability.

That creates a choice.

### Reusable rule

> A resource is more interesting when it changes **how** you act, not only whether you may act.

---

# 21. The meme-simulator blueprint learned here

```text
ABSURD ONE-SENTENCE PREMISE
        ↓
ONE PHYSICAL VERB
        ↓
EXCELLENT SOUND / IMPACT
        ↓
VISIBLE PERMANENT CHANGE
        ↓
THEME-EXPLAINED RESOURCE
        ↓
VERB CHANGES NAVIGATION / ENVIRONMENT
        ↓
MECHANICAL ESCALATION
        ↓
NEW TARGET TYPES
        ↓
VISUAL COMEDY GETS BIGGER
        ↓
SHORT FINAL POWER FANTASY
        ↓
CLEAR ENDING
```

---

# 22. What to borrow

## Strongly borrow

- title communicates the entire joke;
- main verb is visually obvious;
- physical destruction provides immediate feedback;
- one emotional resource ties fiction to mechanics;
- short runtime;
- stylized low-cost presentation;
- level/area variation;
- destruction changes traversal.

## Borrow carefully

- NPC-triggered progression;
- dialogue-heavy comedy;
- tightly scripted linear levels.

## Avoid

- quest logic distributed across many scene objects;
- save system that can disagree with quest state;
- progression-critical interactables without fallback;
- one broken door blocking the entire level;
- relying on humor to replace progression;
- no recovery tools;
- launching without basic camera/accessibility settings.

---

# 23. Solo Unity architecture assessment

For a **small original game using this architecture**, not a literal clone:

| System | Complexity |
|---|---|
| First-person movement | low |
| One destructible interaction | low |
| Staged wall states | low |
| Debris/particles | low-medium |
| Stress meter | low |
| Highlight system | low |
| Multiple short levels | medium content workload |
| Dialogue/NPC presentation | medium |
| Quest chain | medium-high QA risk |
| Persistent arbitrary destruction | medium |
| Save + quest recovery | medium-high |
| Free dynamic destruction | high — avoid |

The key simplification is:

> use staged wall panels / mesh swaps / predetermined chunks.

Do **not** implement arbitrary real-time structural destruction.

---

# 24. AI-coding rules for this type of game

Use one central progression state machine.

```text
LevelState
ObjectiveState
StressState
InteractableState
```

Each wall:
- has an ID;
- has a required objective/state;
- reports completion;
- never directly decides the next quest.

Each NPC:
- has an ID;
- reads state;
- sends one event;
- cannot mutate unrelated world state.

Automated tests:

- every objective can transition once;
- save/load at every objective;
- destroy wall before/after intended trigger;
- leave/re-enter level;
- restart during dialogue;
- restart after wall destruction;
- wall target always has highlight;
- level has reachable exit;
- last objective always completes;
- no required wall can permanently disappear without state completion.

---

# 25. Final verdict

*Drywall Eating Simulator* is **more important as a marketing/design case study than as a simulator blueprint**.

Its success signal is:

> A ludicrous, instantly legible title can create attention with almost no explanation.

Its weakness signal is:

> Attention is not enough if the loop has little progression or the scripted state can break.

For our project, the ideal synthesis is:

> **Drywall's meme clarity + Digging's progression/discovery + PowerWash/Leaf transformation + Cash Cleaner's state-rich workflow.**

That creates something funny enough to click and substantial enough to buy.

---

# Sources

## Official

Steam — Drywall Eating Simulator  
https://store.steampowered.com/app/3749110/Drywall_Eating_Simulator/

Steam Community hub  
https://steamcommunity.com/app/3749110/

## Player reviews / community reports

Steam most-helpful reviews  
https://steamcommunity.com/app/3749110/reviews/?browsefilter=toprated

Softlock discussion  
https://steamcommunity.com/app/3749110/discussions/0/846243771540598410/

## Patch history

Version 1.1.0 — quest/save overhaul  
https://steamdb.info/patchnotes/21273019/

Hotfix 1.1.3 — glass doors  
https://steamdb.info/patchnotes/21372974/

Version 1.2.0 — physics/VFX/UI/quest fixes  
https://steamdb.info/patchnotes/21428743/

Version 1.3.0 — level select / final major update  
https://steamdb.info/patchnotes/22030166/

Version 1.3.1 — Steam Deck Verified  
https://steamdb.info/patchnotes/22084557/

## Independent reviews

Siliconera — Drywall Eating Simulator Gets Frustrating  
https://www.siliconera.com/drywall-eating-simulator-gets-frustrating/

The Magic Rain — stress-management review  
https://themagicrain.com/2026/03/drywall-eating-simulator-is-a-bite-sized-game-about-stress-management-game-review/

Gamereactor — highly negative review  
https://www.gamereactor.eu/drywall-eating-simulator-1654473/

Movies, Games and Tech review  
https://moviesgamesandtech.com/2026/01/12/review-drywall-eating-simulator/

## Market / visibility reference

SteamDB charts  
https://steamdb.info/app/3749110/charts/

SteamDB/third-party traffic estimates should be treated as directional, not audited sales data.
