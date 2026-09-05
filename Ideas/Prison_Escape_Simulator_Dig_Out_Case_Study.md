# Prison Escape Simulator: Dig Out — Case Study Addendum

## Purpose

This document adds **Prison Escape Simulator: Dig Out** to the reusable “tiny tactile simulator” blueprint.

The goal is **not** to copy a prison-escape theme. The useful question is:

> What additional design roles does this game add to the *A Game About Digging a Hole* formula, which of them improve the loop, which create unnecessary friction, and how should a solo Unity developer simplify them?

There is also a separate 2025 game titled *Prison Escape Simulator* by PEGAX Studios that is primarily a parkour/action escape game. This addendum analyzes **Prison Escape Simulator: Dig Out** by Digital Melody / PlayWay because it is the mechanically relevant comparison.

---

# 1. What the game actually is

**Prison Escape Simulator: Dig Out** released on Steam on July 25, 2025.

The official premise is very compact:

- hide the entrance to a tunnel under the cell carpet;
- start digging with a weak tool;
- dig during safe periods;
- avoid guard inspections;
- remove/dispose of excavated dirt;
- find junk and valuables underground;
- sell/trade what you find;
- trade toilet paper with prisoners;
- upgrade tools and player stats;
- improve the cell;
- use glow sticks and dynamite;
- read clues about the correct digging direction/depth;
- eventually escape.

The official store page also describes food, shoes, larger shovels, strength and energy improvements, backpacks, toilets that remove sand faster, furniture/cell upgrades, and gambling involving money and reputation.

## Why this is interesting

The game tries to add a **social/time-pressure layer** on top of the basic digging loop.

*A Game About Digging a Hole* mainly creates pressure through battery, inventory, depth and traversal back to the surface.

*Prison Escape Simulator: Dig Out* instead adds:

- **time windows**;
- **inspection risk**;
- **evidence concealment**;
- **material disposal**;
- **mandatory routines**;
- **trading/economy outside the tunnel**.

This creates an important blueprint lesson:

> A repetitive work game can interrupt the main verb with **external schedule pressure** rather than only an energy bar.

But the reviews also show why this can easily become worse than the simpler system.

---

# 2. Core gameplay loop

```text
WAIT FOR SAFE WINDOW
        ↓
OPEN / HIDE TUNNEL ENTRANCE
        ↓
DIG
        ↓
GENERATE DIRT + DISCOVER OBJECTS
        ↓
COLLECT VALUABLES
        ↓
WATCH TIME / GUARD RISK
        ↓
RETURN TO CELL
        ↓
HIDE EVIDENCE + DISPOSE OF DIRT
        ↓
SELL / TRADE / GAMBLE
        ↓
BUY TOOL / CAPACITY / STAT / CELL UPGRADES
        ↓
READ ESCAPE CLUES
        ↓
DIG FARTHER
        ↓
EVENTUAL ESCAPE
```

The architecture is therefore broader than *Digging a Hole*:

> **WORK → DISCOVER → CONCEAL → PROCESS → UPGRADE → WORK AGAIN**

The **conceal/process** phases are the defining addition.

---

# 3. Nested loops

## Second-to-second loop

- swing digging tool;
- tunnel material disappears;
- tool feedback;
- movement through tunnel;
- object becomes exposed;
- pick up object;
- dirt/resource count changes.

## 1–5 minute loop

- dig during an available time window;
- decide when to stop;
- return before inspection/routine;
- conceal tunnel;
- dispose of accumulated dirt;
- sell/find/use recovered objects.

This is the **schedule-pressure loop**.

## 10–30 minute loop

- accumulate enough money/resources;
- upgrade shovel/tool;
- improve inventory/backpack;
- improve energy/strength;
- improve dirt disposal;
- obtain useful cell items;
- reach a new tunnel section.

## Whole-game loop

- interpret escape hints;
- choose direction;
- gradually extend the escape tunnel;
- acquire stronger equipment;
- reach the correct escape route;
- complete the escape.

---

# 4. What works well

## 4.1 The guard countdown creates suspense cheaply

Positive reviews specifically mention the countdown until the guard arrives as a major source of tension.

This proves that a game does **not** need combat, constant chases or complex AI to make a repetitive verb tense.

A simple timer can create:

> “One more shovel hit… or should I stop now?”

### Blueprint abstraction

**INTERRUPTION PRESSURE**

Possible implementations:

- guard inspection;
- shift ending;
- incoming storm;
- machine overheat;
- oxygen;
- customer deadline;
- security patrol;
- daylight;
- tide;
- train arrival;
- scheduled power outage.

---

## 4.2 Evidence concealment gives the return phase a purpose

The player is not returning merely because “the game says go home.”

The player must:

- cover the tunnel;
- remove evidence;
- dispose of dirt.

That is useful design.

### Blueprint abstraction

A return loop is stronger when returning lets the player:

- bank rewards;
- erase danger;
- reset a resource;
- process waste;
- repair equipment;
- prepare for the next run.

The return point should do **multiple useful things at once**.

---

## 4.3 Digging creates both reward and waste

This is a very good systems interaction.

Digging produces:

### Positive output
- tunnel progress;
- discovered valuables.

### Negative output
- dirt/sand that must be handled.

### Blueprint rule

> The primary verb should ideally create at least one **useful output** and one **management problem**.

| Main verb | Reward | Secondary problem |
|---|---|---|
| Dig | valuables / access | dirt |
| Cut scrap | components | heat / debris |
| Vacuum | cleared area | full container/filter |
| Wash | clean surface | water/chemical use |
| Chop | wood | logs blocking routes |
| Pump | exposed area | machine heat |
| Process goods | money | waste / storage |

---

## 4.4 Tool upgrades are visibly satisfying

Even negative reviews repeatedly mention that the satisfying part is upgrading the shovel and seeing more material removed per action.

### Blueprint rule

> Upgrades should visibly change the main action.

```text
weak tool
→ noticeably better tool
→ much larger work area
→ previously annoying material becomes trivial
```

---

# 5. Negative-review analysis

The recurring complaints fall into several clusters.

## 5.1 Repetition without enough escalation

Players repeatedly complain that:

- the same digging continues too long;
- later chapters feel too similar;
- mandatory routines create repetition rather than decisions;
- waiting for lunch/events is tedious;
- exploration is limited;
- later difficulty often changes numbers more than mechanics.

### Blueprint rule

> **Do not confuse more chores with more depth.**

Before adding another routine, ask:

**Does this produce a new decision?**

If no, remove or automate it.

---

# 6. Waiting is dangerous design

The guard timer is useful because it creates pressure.

Waiting for a clock to become available is different.

### Good timer

> “I need to finish before 13:00.”

Creates urgency.

### Bad timer

> “I cannot do anything useful until 13:00.”

Creates dead time.

### Blueprint rule

> **Time systems should pressure action, not prevent action.**

If a schedule exists, the player should always have something useful to do.

---

# 7. Economy problems

Negative reviews report:

- prices versus earnings feeling badly scaled;
- cash becoming irrelevant;
- many upgrades not being necessary;
- progression finishing before the economy fully matters.

### Blueprint rule

Every currency should matter across the full intended game length.

Early: money is scarce.  
Mid: upgrades compete.  
Late: money becomes abundant but final purchases are exciting.

If a currency becomes meaningless too early, either reduce payouts, add meaningful sinks, or shorten the game.

---

# 8. Upgrades need to be necessary AND interesting

An upgrade should do at least one of these:

1. change speed dramatically;
2. unlock a material;
3. unlock an area;
4. reduce a bottleneck;
5. increase expedition duration;
6. reveal hidden information;
7. create a new option.

Weak:

> “10% more energy.”

Strong:

> “You can now finish two work sessions before needing recovery.”

Excellent:

> “You can now break reinforced material and enter a new branch.”

---

# 9. The chapter problem

Players complain that later content can feel like a copy/paste of the first chapter.

### Blueprint rule

Do **not** think:

> “I need three maps.”

Think:

> “I need three escalation phases.”

A new phase should introduce at least one:

- new material;
- new reward;
- new danger;
- new tool behavior;
- new route;
- new secret;
- new visible state.

---

# 10. Save-state failures

This is one of the most serious complaint clusters.

Reported problems include:

- upgrades/abilities resetting while tunnel progress remains;
- chapter selection deleting progress;
- getting caught after loading;
- finishing/resetting saves unexpectedly;
- losing hours of progress;
- tunnel state and player/economy state disagreeing.

### Blueprint rule

> Persistent world state, upgrades, inventory, economy and chapter progress must serialize as **one coherent game state**.

Do not allow independent systems to disagree about what happened.

---

# 11. Unreachable objects and bad geometry

Reviews report junk spawning:

- inside toilets;
- through walls;
- through floors;
- at unreachable positions;
- behind tunnel geometry;
- in spaces too small for the player to collect the reward.

### Blueprint rule

> **Progression-critical objects may never depend on uncontrolled physics positioning.**

Use:

- validated spawn sockets;
- reachability checks;
- snap-to-safe-point;
- automatic pickup radius;
- recovery system;
- “retrieve lost item” fallback;
- scanner/highlight;
- world-bounds recovery.

For random junk, cosmetic physics is fine.

For required loot, physics should never be authoritative.

---

# 12. Movement and collision jank

Reviews mention getting stuck in tunnels, awkward hitboxes, junk obstructing movement, and softlocks in later vehicle sections.

### Blueprint rule

A tiny game with a simple loop depends disproportionately on **movement quality**.

Use:

- generous collision;
- few small clutter colliders;
- good step-up handling;
- unstuck/recovery;
- automatic pickup for tiny objects;
- no progression dependent on squeezing through awkward geometry.

---

# 13. Content density matters more than length

Some users call the game too short; others say it already becomes repetitive.

Those are compatible complaints.

The issue is **content density**, not raw hours.

A two-hour game can feel excellent if new things happen constantly.

A five-hour game can feel long if hours 2–5 repeat hour 1.

### Blueprint rule

Every 15–30 minutes should ideally introduce a meaningful change:

- material;
- tool behavior;
- region;
- hazard;
- discovery;
- economy decision;
- clue;
- mystery escalation.

---

# 14. New design roles this game adds to the master blueprint

## A. Schedule pressure
A recurring event forces the player to stop before they want to.

## B. Concealment
Temporary progress creates evidence that must be hidden.

## C. Waste output
The primary verb creates both useful progress and undesirable byproduct.

## D. Safe/unsafe state switching
The same location can change between safe and risky states.

## E. Clue-guided progression
The player gets hints about direction/depth instead of simply moving forward blindly.

## F. Optional gambling
A secondary risk/reward system can alter the economy.

---

# 15. How I would improve the design

## Keep guard pressure, remove dead waiting
Keep countdown urgency. Remove periods where the optimal action is simply to wait.

## Make chores conquerable through upgrades
Early-game manual disposal can later become faster or automated.

## Make underground discovery richer
Add side pockets, caches, special materials, pipes, rare branches, hazards and hidden rooms.

## Make wrong routes interesting
A wrong direction should contain compensation: loot, collectible, clue, shortcut or upgrade component.

## Make upgrades transform routines
Power, capacity, endurance, detection, disposal and concealment should materially alter play.

## Replace free-physics loot with validated discovery nodes
Gameplay state should spawn rewards in safe pickup positions; cosmetic debris can use physics separately.

## Use one coherent save
Tunnel, upgrades, inventory, economy, clues and chapter state should load from one authoritative snapshot.

## End with the main mechanic
Do not spend the whole game teaching digging and then make the finale depend on a poorly polished unrelated vehicle/parkour sequence.

---

# 16. Solo Unity difficulty rating

These ratings estimate a **small original game using this architecture**, not the commercial game's production difficulty.

Assumptions: solo beginner, Unity, heavy AI coding help, marketplace assets, single-player.

| Area | Difficulty (1–10) | Why |
|---|---:|---|
| Core digging interaction | 5 | Easy if staged/grid-based, harder if real terrain deformation |
| Economy/upgrades | 3 | Straightforward data-driven systems |
| Inventory/trading | 4 | Simple if slot/weight based |
| Schedule/time | 4 | Conceptually easy, edge cases matter |
| Guard inspection | 5 | Fine if scripted, harder with free AI |
| Cell upgrades/placement | 6 | Placement + persistence creates QA |
| Persistent tunnel/save | 8 | Highest-risk system |
| Loot spawning | 4 | Easy with safe sockets, risky with physics |
| Chapter/progression | 5 | Save transitions need testing |
| Vehicle/escape sequences | 7 | Unnecessary scope for first game |
| Content/art | 4 | Marketplace assets can cover much of it |
| QA/bug risk | 7 | Many persistent systems interact |
| **Overall if copied closely** | **6.5/10** | Too many systems for a first tiny project |
| **Overall if simplified** | **4/10** | Very feasible |

---

# 17. The simplified architecture I would actually build

```text
ONE CELL / HUB
ONE SMALL OUTSIDE AREA
ONE WORK AREA / TUNNEL
ONE SCRIPTED THREAT
ONE PRIMARY TOOL
ONE CAPACITY SYSTEM
ONE WASTE SYSTEM
ONE CURRENCY
FOUR UPGRADE CATEGORIES
ONE DETECTOR / CLUE SYSTEM
ONE FINAL GOAL
```

The threat should use a simple state machine rather than full AI:

```text
OffDuty
→ Warning
→ Approaching
→ Inspecting
→ Leaving
```

No free-form NPC schedule planning.

No combat.

No complex NPC inventories.

No open prison.

No vehicles.

---

# 18. Systems worth borrowing

## Strongly worth borrowing
- timed interruption pressure;
- useful progress + waste from one action;
- concealment before inspection;
- visible tool-power upgrades;
- clues about where to progress;
- discoveries inside repetitive work;
- one safe hub;
- capacity;
- short escalating progression;
- obvious final objective.

## Borrow only if needed
- food;
- reputation;
- gambling;
- furniture;
- multiple currencies;
- multiple chapters.

## Avoid for first project
- free NPC simulation;
- vehicles;
- physics-critical loot;
- large schedule simulation;
- complex placeable furniture persistence;
- copied chapters with only numerical changes;
- unrelated minigames with different controls;
- save deletion/reset as ordinary punishment.

---

# 19. New blueprint rules learned from this case

### Pressure should create urgency, not downtime
Good: “The guard comes in 60 seconds.”  
Bad: “Wait 60 seconds until the game lets you work.”

### Busywork should be conquerable through upgrades
Let the player eventually defeat repetitive maintenance.

### The main verb can create a negative byproduct
```text
WORK
→ PROGRESS
→ REWARD
→ WASTE / HEAT / NOISE / EVIDENCE
```

### Wrong routes should contain compensation
Exploration feels better when mistakes can still produce discovery.

### Chapters need mechanical escalation
Do not rely on copied geometry.

### Never let persistence systems disagree
World progress, upgrades, economy and chapter state must load coherently.

---

# 20. How it fits the master formula

```text
ONE VERB
DIG
        ↓
VISIBLE PERMANENT CHANGE
TUNNEL EXTENDS
        ↓
UNCERTAIN REWARD
JUNK / MONEY / USEFUL ITEMS
        ↓
NEGATIVE OUTPUT
DIRT / EVIDENCE
        ↓
EXTERNAL PRESSURE
GUARD / SCHEDULE
        ↓
PUSH YOUR LUCK
DIG A LITTLE LONGER?
        ↓
RETURN / SAFETY
COVER TUNNEL + CLEAN UP
        ↓
PROCESS
DISPOSE / TRADE / SELL
        ↓
UPGRADE
TOOL / ENERGY / CAPACITY / CELL
        ↓
INFORMATION GATE
ESCAPE CLUES
        ↓
NEW ROUTE
LONGER / BETTER TUNNEL
        ↓
FINAL PAYOFF
ESCAPE
```

The most useful new reusable pattern is:

> **PRIMARY ACTION → PROGRESS + BYPRODUCT → EXTERNAL DEADLINE → CONCEAL/PROCESS → UPGRADE → TRY AGAIN**

That pattern can produce many original games without involving prisons or digging.

---

# 21. Sources

Primary:
- Steam — *Prison Escape Simulator: Dig Out* (Digital Melody / PlayWay S.A.)  
  https://store.steampowered.com/app/3672720/Prison_Escape_Simulator_Dig_Out/

Community/review evidence:
- Steam negative reviews  
  https://steamcommunity.com/app/3672720/negativereviews/?browsefilter=toprated&l=english

- Steam all reviews  
  https://steamcommunity.com/app/3672720/reviews/?browsefilter=toprated

- Developer feedback / bug thread  
  https://steamcommunity.com/app/3672720/discussions/0/601910715108001993/

Player reviews are anecdotal evidence. Recurring themes are more useful than any single report.
