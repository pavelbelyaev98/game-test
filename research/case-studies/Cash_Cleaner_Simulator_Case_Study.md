# Cash Cleaner Simulator — Design Case Study
## Why handling dirty money is so satisfying, where the game creates friction, what was fixed after launch, and what our blueprint should learn from it

**Research date:** September 5, 2026  
**Game:** *Cash Cleaner Simulator*  
**Developer:** Mind Control Games  
**Publishers:** Forklift Interactive, Gamersky Games  
**Release:** May 8, 2025

---

# 1. Executive conclusion

*Cash Cleaner Simulator* is one of the clearest examples of a modern “tiny physical job becomes addictive game” architecture.

Its official loop can be summarized as:

> **CHECK IT → CLEAN IT → SORT IT → COUNT IT → PACK IT → SHIP IT.**

The Steam page currently shows roughly **91% positive English reviews across about 3,800+ reviews**, with recent reviews also around the low 90s.

The game succeeds because **cash is unusually good physical game material**:

- everybody instantly understands what money is;
- piles visibly accumulate;
- denominations naturally create sorting;
- dirt/wetness/ink/goo create processing states;
- counterfeit and marked bills create inspection;
- currency creates value;
- wrapping creates neat completion units;
- money-counting machines have excellent sound/animation payoff;
- increasingly huge piles create a power fantasy.

The strongest high-level lesson is:

> **One ordinary object can support an entire game if that object has enough meaningful states.**

A banknote can be:

- dollar/euro/yen/pound;
- one of several denominations;
- clean or dirty;
- dry or wet;
- inked;
- covered in chemical goo;
- genuine or counterfeit;
- unmarked or marked;
- loose;
- stacked;
- strapped;
- packed;
- stored;
- requested by a job;
- collectible.

That creates enormous apparent variety while reusing the same core object and pipeline.

The biggest historic criticism of the game was that progression promised automation but kept forcing repetitive manual machine babysitting. Importantly, the developers **responded in 2026**: L/XXL money counters received Endless Operation and Auto Packing, and later updates added more QoL tools such as the LootVac.

That makes the game an especially useful case study because we can see:

> **the original friction → negative reviews → targeted QoL fix.**

---

# 2. Current verified game status

Official Steam page:
https://store.steampowered.com/app/2488370/Cash_Cleaner_Simulator/

At research time the page reports:

- release: May 8, 2025;
- very positive reception;
- first-person;
- single-player;
- cleaning;
- organizing;
- automation;
- physics;
- multiple endings;
- sandbox / management elements.

The official description:

> deliveries arrive in bags, boxes or other containers; identify problems such as dirt, blood or counterfeit bills; clean the money; count it; send it back; buy tools; scale the operation; handle other currencies and valuables; explore the lab and uncover secrets.

That is much richer than:

> “wash money.”

---

# 3. The primary gameplay pipeline

The core architecture is a **processing pipeline**.

```text
JOB ARRIVES
        ↓
DELIVERY DROPS INTO LAB
        ↓
OPEN BAG / BOX / PACKAGE
        ↓
IDENTIFY CONTENTS
        ↓
SEPARATE TRASH / COINS / CASH / VALUABLES
        ↓
INSPECT CONDITION
        ↓
WASH / CLEAN SPECIAL STAINS
        ↓
DRY
        ↓
DETECT COUNTERFEIT / MARKED NOTES
        ↓
SORT CURRENCY / DENOMINATION
        ↓
COUNT
        ↓
STRAP / PACK
        ↓
PLACE INTO REQUIRED CONTAINER
        ↓
SEND DELIVERY
        ↓
REPUTATION + CRYPTO / PROGRESSION
        ↓
BUY BETTER EQUIPMENT
        ↓
ACCEPT LARGER / STRANGER JOB
```

This is one of the strongest **workflow simulator** structures we have studied.

---

# 4. Why the object state-space is so powerful

The game does not require 1,000 mechanically unique objects.

It takes one object class:

# BANKNOTE

and gives it many dimensions.

## Currency

The game supports multiple currencies, with later updates adding more.

## Denomination

Bills have different face values.

That means identical physical actions produce different values and job requirements.

## Condition

Bills can require:
- washing;
- drying;
- special stain treatment.

## Authenticity

A bill can be:
- genuine;
- counterfeit.

## Mark state

A genuine bill may carry a UV-visible mark.

Different criminal/law-enforcement marks can matter to specific jobs/collections.

## Packaging state

A bill can exist as:
- loose bill;
- hand stack;
- strapped bundle;
- larger packed unit;
- container content.

## Ownership/purpose state

The bill may be:
- job input;
- required job output;
- bonus;
- collectible;
- stored for later;
- burned/played with.

### Blueprint lesson

> Before adding a new object type, ask whether the existing object can gain another meaningful state.

This is incredibly efficient for a solo game.

---

# 5. Dirty / wet / ink / goo creates a material taxonomy

The processing system works because different “problems” demand different treatments.

Community documentation describes condition categories including:

- dirt;
- wetness;
- ink;
- goo / chemical stains;
- markings.

This creates the simulator equivalent of material tiers.

```text
NORMAL
→ easy

DIRTY
→ wash

WET
→ dry

INKED
→ special cleaning

GOO
→ special cleaning

MARKED
→ inspect / separate / clean with specialized process

COUNTERFEIT
→ separate / use for specific jobs
```

This is exactly the type of taxonomy we wanted in our master blueprint.

### Generic abstraction

Any object-processing game can use:

| State | Meaning |
|---|---|
| Normal | baseline |
| Dirty | basic treatment |
| Wet | sequencing requirement |
| Special stain | advanced tool |
| Fake | identification challenge |
| Marked | hidden state detectable by scanner |
| Rare | collectible/jackpot |

---

# 6. Sequencing is doing important work

A wet bill cannot simply jump to the end of the pipeline.

It must be processed in the correct order.

Conceptually:

```text
DIRTY + WET
→ WASH
→ STILL WET
→ DRY
→ COUNT / PACKAGE
```

That creates **process dependency**.

This is more interesting than a single “cleanliness HP bar.”

### Blueprint rule

For processing simulators:

> Use 2–4 meaningful state transitions rather than one long progress bar.

The player should think:

> “What does this batch need next?”

---

# 7. Counterfeit detection creates attention

Counterfeit cash can be detected under ultraviolet inspection because it lacks the expected fluorescent security features.

Later machines can separate counterfeit notes automatically.

This creates a progression curve:

```text
EARLY
LOOK AT EACH BILL / STACK

MID
USE UV STATION

LATE
MACHINE FILTERS
```

This is excellent progression design because the player first **learns the rule manually**, then earns automation.

### Blueprint rule

> Automation feels most satisfying after the player personally understands the task it replaces.

If the machine does everything from minute one, the player never develops appreciation for it.

---

# 8. Marked bills create hidden information

Marked bills are especially interesting.

Under ordinary light they may appear normal.

Under UV they reveal hidden marks.

The game then lets the player:

- identify them;
- sort them;
- collect specific mark types;
- clean/remove marks with later tools/processes;
- save them for specific jobs.

This is effectively a **detector mechanic**.

It performs the same psychological job as the detector in *A Game About Digging a Hole*:

> ordinary-looking environment/object may secretly contain special information.

### Blueprint abstraction

Possible detector states in other games:

- UV mark;
- metal;
- hidden cavity;
- damage under paint;
- authenticity mark;
- wiring behind wall;
- rare material;
- biological contamination.

---

# 9. Job requirements turn sorting into a puzzle

Community quest documentation shows that jobs may request combinations such as:

- exact cash value;
- specific currency;
- specific denomination;
- clean/dry condition;
- specific container;
- strapped/wrapped state;
- no counterfeit bills;
- no marked bills;
- special types of marked/counterfeit bills.

This is crucial.

Without jobs, the player would simply optimize toward:

> clean everything identically.

Jobs turn the same inventory into different puzzles.

Example:

```text
JOB A
$20,000
clean + dry
no marks

JOB B
$16,000 legitimate
+
$16,000 counterfeit

JOB C
specific denomination
in specific container
```

### Blueprint rule

> Reuse the same objects by changing the **requested specification**, not by constantly adding new mechanics.

This is cheap content.

---

# 10. Optional quality bonuses create optimization

Quest systems can reward cleaner execution such as:

- no fake money;
- no marked money;
- no unwanted extra items;
- single delivery;
- extra money.

This creates a nice optional layer.

Casual player:

> complete job.

Optimizer:

> complete it perfectly.

### Blueprint rule

> Give players a baseline completion path and an optional “beautiful execution” bonus.

That is better than requiring everyone to play perfectly.

---

# 11. The physical money pile is a progression display

One of the strongest visual choices is that money physically exists in the room.

The player is not merely watching:

```text
BANK BALANCE: $734,500
```

They can see:

- loose bills;
- strapped stacks;
- boxes;
- pallets;
- piles.

This gives wealth a tangible form.

It is the money equivalent of:

- a giant hole;
- a clean backyard;
- a restored workshop;
- a completed bookshelf.

### Blueprint rule

> Whenever possible, make progression occupy physical space.

Possible examples:

- jars filling cellar shelves;
- restored tools appearing on wall;
- collectibles in cabinet;
- sorted boxes on racks;
- clean rooms;
- stacked salvage.

---

# 12. Money-counter audio is part of the mechanic

Community guides repeatedly praise the counting-machine sound.

This matters.

The player performs repetitive work, so the machine must produce:

- mechanical motion;
- rhythmic sound;
- a clear completion sound;
- neatly ejected stack.

The reward is not only numerical.

It is audiovisual.

### Game-feel rule

Every processing station needs:

1. intake sound;
2. operating rhythm;
3. visual movement;
4. progress state;
5. completion cue;
6. satisfying output object.

---

# 13. The lab is both workplace and playground

The official game does not treat the lab only as a menu hub.

The player can:

- arrange equipment;
- store cash;
- decorate;
- explore secrets;
- use physical cash in playful ways;
- build a visually ridiculous workspace.

The developers have continued adding:
- furniture;
- decorative items;
- collection frames/compendiums;
- rare collectible sets.

This keeps the same map interesting.

### Blueprint lesson

> A single map becomes much more valuable when it is both **workplace + trophy room + mystery space**.

---

# 14. Collection adds a second long-term desire

By 2026 the game includes more explicit collection systems, including banknote compendiums and rare collectible sets.

This is smart because money alone becomes abstract.

Collection asks:

> “What am I missing?”

That can survive even when the player already owns expensive machinery.

### Blueprint rule

Money buys progression.

Collection gives completion desire.

Use both when appropriate.

---

# 15. The phone centralizes complexity

The official description makes the phone the operational center.

Functions include:
- job access;
- ordering;
- photography;
- scanning/hints;
- equipment/build mode.

That means the player does not need to walk through a large town.

### This is extremely relevant to us.

Schedule I often externalizes economy through a city.

Cash Cleaner compresses a lot of those functions into:

> **one phone inside one location.**

### Blueprint rule

> If travel is not the fantasy, compress external systems into one interface.

A phone/computer can replace:
- shops;
- NPC vendors;
- job boards;
- delivery offices;
- property menus.

This dramatically reduces content and bugs.

---

# 16. Nested loops

## Second-to-second

```text
GRAB
→ LOOK
→ DROP / SORT
→ MACHINE
→ STACK
```

Tactile physical loop.

## 1–5 minute batch loop

```text
OPEN DELIVERY
→ PROCESS BATCH
→ COMPLETE REQUIRED CONDITION
→ PACK
```

## 10–30 minute economy loop

```text
COMPLETE JOBS
→ REPUTATION / CRYPTO
→ BUY MACHINE
→ REDESIGN WORKFLOW
```

## Whole-game loop

```text
BIGGER / WEIRDER JOBS
→ EXPAND LAB
→ DISCOVER SECRETS
→ BUILD COLLECTION
→ STORY / ENDINGS
```

This is exactly the four-layer architecture from our blueprint.

---

# 17. Why the game feels better than “just sorting”

It combines several engagement engines simultaneously.

## Order-from-chaos

Messy cash becomes neat bundles.

## Transformation

Dirty/wet/stained money becomes clean.

## Detection

Hidden fake/marked states.

## Automation

Manual processes become machine processes.

## Optimization

Arrange the workshop into an efficient flow.

## Collection

Rare bills/items/frames.

## Mystery

Why are you trapped here? What is hidden in the lab?

## Physical accumulation

Your workspace visibly fills with wealth.

That is a very dense design built from a narrow premise.

---

# 18. Launch-era negative reviews: automation frustration

This is the most important complaint cluster.

Early negative reviews repeatedly argued:

> the game looks like an automation game but actively resists automation.

Specific launch-era friction included:

- needing to babysit money counters;
- manually press pack;
- manually remove completed stacks;
- machines stopping until the output is handled;
- no satisfying end-to-end automated line;
- new currencies multiplying manual sorting.

This criticism is valuable because it identifies the **automation promise** created by the game itself.

Once the player sees:
- counters;
- washers;
- dryers;
- sorters;

they naturally expect:

> “Eventually I can design a smooth pipeline.”

If the game refuses, progression feels incomplete.

Sources:
- Steam negative reviews: https://steamcommunity.com/app/2488370/negativereviews/?browsefilter=toprated

---

# 19. Important update: some automation complaints were fixed

Do not repeat old reviews as if the current game is unchanged.

In April 2026 the developers reworked Money Counters L/XXL with:

- **Endless Operation**
- **Auto Packing**
- larger tape capacity
- improved counter UI

The Marked Money Sorter also gained endless operation.

Community response specifically celebrated auto packing.

Official/announcement sources:
- https://steamcommunity.com/app/2488370/announcements/
- SteamDB patch record: https://steamdb.info/patchnotes/22783814/

### Design lesson

The negative reviews were not merely whining.

The developers changed the system in the direction players requested.

That is strong evidence that:

> **automate repetitive execution once the player has mastered it.**

---

# 20. 2026 QoL expansion: Neat & Tidy

The August 12, 2026 update added:

- LootVac Multi-Cleaner;
- World Banknote Compendium;
- new rare collectibles;
- new furniture;
- highlighting for newly dropped objects;
- better interaction priority when several objects overlap;
- option to disable random rotation inside containers;
- bug fixes / story-trigger improvements.

Source:
https://steamcommunity.com/app/2488370/announcements/

This is interesting because many changes directly attack **physical clutter friction**.

---

# 21. Negative reviews: physics clutter

The game makes bills physical, which is part of the appeal.

But it also creates problems.

Reported complaints include:

- trying to grab one bill and grabbing nearby cash;
- money going everywhere;
- equipment getting bumped out of alignment;
- bills missing machine inputs;
- large quantities of cash hurting performance.

This is the exact double edge we saw in:
- Storage Hunter;
- Deconstruction Simulator;
- Chopping Trees.

### Universal rule

> **Physics should produce delight, not determine truth.**

For our games:
- free physics for local spectacle;
- snap/lock objects once placed;
- logical inventory/state underneath;
- easy recovery for lost objects.

---

# 22. Performance lesson: visible wealth has a cost

Players have reported that performance improves when cash/items are placed into closed containers rather than left as thousands of loose physical objects.

Even if implementation details vary by version/platform, the design problem is obvious:

> thousands of tiny simulated banknotes are expensive.

### Solo Unity version

Represent money in levels:

```text
0–100 notes
individual visual bills

100–1,000
pile mesh + small interactive sample

1,000+
logical count + bundled/pallet representation
```

Do not let wealth scale linearly with Rigidbody count.

---

# 23. Negative reviews: machine-layout frustration

Players complain that workshop equipment can be:

- hard to align;
- different output trajectories/heights;
- easy to bump;
- frustrating to position precisely.

This breaks the optimization fantasy.

### Blueprint rule

If layout matters:

> **placement must be extremely forgiving.**

Use:
- snap points;
- optional grid;
- “lock furniture”;
- visible input/output arrows;
- ghost trajectories;
- auto-align neighboring stations.

The player should optimize workflow, not fight transforms.

---

# 24. Negative reviews: tutorial / information problems

Some players report that the tutorial teaches only the basics and leaves important systems unclear.

Examples include:
- how to track exact job money;
- marked vs counterfeit distinction;
- what tools are required;
- when special conditions matter.

### Better onboarding

Teach through **one example of each state**.

Job 1:
clean + dry.

Job 2:
counterfeit.

Job 3:
marked.

Job 4:
special stain.

Then stop tutorials.

Do not dump a manual before the player knows why the information matters.

---

# 25. Negative reviews: resource/job softlock risk

Players have reported quests requiring rare marked/counterfeit categories when they have not retained enough matching bills.

Current community discussion advises players to store special bill types because future story jobs may require them.

That is dangerous because the player can unknowingly destroy/clean/sell a future-required resource.

### Blueprint rule

Never require a rare consumable that the player could permanently exhaust **without a recovery source**.

Solutions:
- job guarantees enough input;
- repeatable source becomes available;
- substitute payment;
- buy resource at premium;
- mission converts requirement;
- warning before destroying the last reserved item.

No “you should have known three hours ago.”

---

# 26. Negative reviews: endgame grind / weak long-term goals

Launch-era and later reviews complain that:

- very large money goals can become repetitive;
- the player already has the tools but still must process huge volume;
- some unlocks/areas feel less meaningful than expected;
- the game may become “processing for the sake of processing.”

This is the **solved-action problem** again.

### Blueprint rule

Once the workflow is solved:

Either:
- introduce a new decision;
- introduce a new object state;
- introduce a new optimization challenge;
- deliver the ending.

Do not ask for 20x more identical volume just because the final number is larger.

---

# 27. How I would improve Cash Cleaner structurally

## Improvement A — Workflow blueprints

Allow the player to save machine layout presets.

## Improvement B — Hard snap / machine locking

Equipment cannot move after “lock workstation.”

## Improvement C — Explicit throughput UI

Show:

```text
WASH: 600 bills/min
DRY: 450
SORT: 900
PACK: 500
```

Now the player can see the bottleneck.

## Improvement D — Job planner

Before accepting:

- required amount;
- required currency;
- condition;
- current compatible stock;
- missing stock.

This prevents accidental impossible-feeling jobs.

## Improvement E — reserve system

Mark a box as:

> DO NOT AUTO-CLEAN / RESERVED FOR SPECIAL JOBS.

## Improvement F — physics LOD

Loose cash visually collapses into efficient pile/bundle proxies at high count.

## Improvement G — late-game challenge through specifications, not volume

Instead of:
> process $5,000,000 more.

Use:
> create three simultaneous deliveries with different mark/currency/container constraints.

That keeps decisions alive.

---

# 28. The single biggest mechanic to borrow

Not money.

The **state-rich object pipeline**.

Create one everyday object with many meaningful conditions.

Examples:

## Pepper game

One pepper:
- raw;
- dirty;
- ripe;
- underripe;
- hot;
- roasted;
- burnt;
- steamed;
- peeled;
- seeded;
- dried;
- jar-ready.

## Cemetery game

One headstone:
- material;
- stable/unstable;
- mossy;
- dirty;
- cracked;
- faded inscription;
- photographed;
- catalogued;
- restored.

## Archive game

One document:
- wet;
- dry;
- dirty;
- torn;
- labeled;
- misfiled;
- duplicate;
- rare/historical;
- restored.

That is the Cash Cleaner lesson.

---

# 29. The second biggest mechanic: specification jobs

A job should not merely say:

> process 50 objects.

It can say:

> 20 objects of TYPE A  
> clean  
> no damage  
> packed in container X.

This creates content using existing systems.

For our future concept generator, add:

### JOB SPECIFICATION FIELDS

- quantity;
- class;
- state;
- forbidden state;
- packaging;
- quality threshold;
- optional bonus condition.

---

# 30. The third biggest mechanic: physical accumulation

Cash Cleaner understands the meme value of:

> ridiculous piles of money.

Our game should identify its equivalent.

Examples:

Hot peppers:
> mountain of peppers / wall of jars.

Panelka basement:
> growing shelf of weird recovered objects.

Cemetery:
> map filling with restored markers.

Archive:
> perfectly filled shelves.

Workshop:
> machines turning on one by one.

The “progress pile” should appear in trailers.

---

# 31. The fourth biggest mechanic: automation boundary

The game historically demonstrates both sides.

Early manual work is good because it teaches the process.

Permanent manual repetition is bad once the player has solved it.

The ideal curve:

```text
MANUAL
→ TOOL-ASSISTED
→ MACHINE-ASSISTED
→ SEMI-AUTOMATED
→ PLAYER OPTIMIZES EXCEPTIONS
```

The player should move from:

> worker

to:

> workflow designer.

But preserve some satisfying physical interaction.

---

# 32. The fifth biggest mechanic: one room is enough

Cash Cleaner creates a surprisingly deep game in one primary lab/workspace.

That is extremely important for our project.

You do not need:
- town;
- drivable vehicles;
- 20 NPCs;
- open world.

Depth can come from:

- deliveries changing;
- machines changing;
- room layout changing;
- stockpile changing;
- mystery areas opening;
- collection growing.

### Blueprint rule

> Change the **state of the room**, not necessarily the room itself.

---

# 33. Implementation architecture for a small Unity version of this pattern

Use data-driven objects.

Example:

```text
ProcessableItem
    ItemType
    Variant
    Value
    ConditionFlags
    HiddenFlags
    ProcessState
    PackagingState
    JobTags
    CollectibleID
```

Condition flags might be:

```text
Dirty
Wet
Marked
Fake
Damaged
Rare
```

Machines use rules:

```text
Washer:
removes Dirty
adds Wet

Dryer:
removes Wet

Scanner:
reveals HiddenFlags

Sorter:
moves by Variant

Packer:
changes PackagingState
```

This is extremely AI-code-friendly because each machine has a clear contract.

---

# 34. Save-system lesson

Do not serialize thousands of tiny physics transforms unless necessary.

Save logical state:

```text
Container A:
Currency USD
100x denomination 20
State Clean/Dry
10 marked
3 counterfeit
```

Then reconstruct a representative visual pile.

For manually placed special collectibles, save exact transform.

For ordinary bulk resources, save aggregated quantities.

This reduces:
- file size;
- corruption risk;
- load time;
- physics explosions.

---

# 35. Testing edge cases

For any Cash-Cleaner-style processing game, automate tests for:

- object is both dirty and wet;
- wrong process order;
- job accepts exact amount;
- job rejects wrong state;
- mixed categories;
- full machine;
- full output;
- machine loses power/state;
- save during processing;
- load during processing;
- object falls out of bounds;
- player sells/destroys last required rare type;
- collection object already registered;
- multiple jobs require same stock;
- auto-processing should not consume reserved objects;
- rounding/value totals;
- container closed/open;
- machine moved while output exists.

These are much more important than complicated AI tests because the game is mostly state machines.

---

# 36. What to borrow for our own game

Absolutely borrow the **roles** of:

- one iconic processable object;
- many states on that object;
- physical piles;
- clean/dirty transformation;
- hidden state revealed by detector;
- manual early inspection;
- machine progression;
- specification-based jobs;
- optional perfection bonuses;
- collection;
- one contained hub;
- equipment layout;
- ridiculous visible accumulation;
- light mystery;
- old chores increasingly automated.

Do not copy:
- cash;
- criminal contacts;
- counterfeit bills;
- exact machine lineup;
- story/theme.

---

# 37. Final blueprint addition from Cash Cleaner

Add this formal pattern:

```text
ONE OBJECT CLASS
        ↓
MANY VISIBLE + HIDDEN STATES
        ↓
JOB SPECIFICATION
        ↓
INSPECT
        ↓
PROCESS IN CORRECT ORDER
        ↓
SEPARATE EXCEPTIONS
        ↓
PACKAGE INTO CLEAN COMPLETION UNIT
        ↓
SHIP / STORE / COLLECT
        ↓
UPGRADE ONE WORKFLOW BOTTLENECK
        ↓
OLD MANUAL STEP BECOMES AUTOMATED
        ↓
NEW STATE / SPECIFICATION APPEARS
        ↓
REPEAT
```

This is different from *Digging a Hole* but fits the same larger blueprint beautifully.

---

# 38. Why the concept is memeable

Cash Cleaner has a perfect visual escalation.

Start:

> a few dirty bills.

End:

> an industrial criminal money-processing laboratory covered in ridiculous quantities of cash.

That visual contradiction is funny.

The best simulator concepts often have:

> **ordinary task + absurd scale.**

Examples:

- roast peppers → industrial pepper yard;
- clean basement → museum-quality junk archive;
- untangle cables → NASA-looking patch closet;
- restore cemetery → immaculate historical grounds;
- clear leaves → leaf-industrial complex.

This should be a core idea filter.

---

# 39. Bottom line

*Cash Cleaner Simulator* is fun because it makes one object class unusually deep.

It combines:

> **sorting + cleaning + hidden information + physical accumulation + workflow optimization + machine progression + collection + mystery.**

Its negative reviews teach us that the most dangerous mistake is:

> giving the player machines that look like automation while preserving pointless manual babysitting.

Its 2026 updates reinforce the lesson by adding exactly the kind of QoL/automation players requested.

For our own game, the strongest thing to steal is not the theme.

It is:

> **Give one ordinary object enough states that handling it becomes a small production puzzle. Then let the player grow from touching every object manually to designing a beautiful, semi-automated workflow.**

---

# Sources

## Official

Steam store:
https://store.steampowered.com/app/2488370/Cash_Cleaner_Simulator/

Official Steam announcements / 2026 updates:
https://steamcommunity.com/app/2488370/announcements/

Steam community/all news:
https://steamcommunity.com/app/2488370/allnews/

DLC page:
https://store.steampowered.com/dlc/2488370/

## Negative/player reviews

Steam negative reviews:
https://steamcommunity.com/app/2488370/negativereviews/?browsefilter=toprated

Steam reviews:
https://steamcommunity.com/app/2488370/reviews/?browsefilter=toprated

## Detailed mechanics

Official/community wiki — bills:
https://cashcleanersimulator.wiki.gg/wiki/Bill

Quest/task requirements:
https://cashcleanersimulator.wiki.gg/wiki/Task
https://cashcleanersimulator.wiki.gg/wiki/Quest

Marked bills:
https://cashcleanersimulator.wiki.gg/wiki/Marked_bills

Steam community ultimate guide:
https://steamcommunity.com/sharedfiles/filedetails/?id=3572594053

## 2026 automation fix

SteamDB record of April 2026 counter rework:
https://steamdb.info/patchnotes/22783814/

The most important current changes include Endless Operation and Auto Packing for larger money counters. Older launch reviews should be interpreted in that historical context.

---

# Evidence note

Official sources are used for current advertised features and update history.

Community wikis/guides are used for detailed state-machine/job behavior that the store page does not enumerate.

Steam reviews and Reddit posts are anecdotal evidence. The report emphasizes recurring complaint patterns and highlights when developers later changed the relevant system.
