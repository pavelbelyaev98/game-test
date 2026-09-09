Name: **Something Down There**

The [player-review research synthesis](research/player-review-findings.md) supplies focused implementation/playtest questions; it does not replace this concept or approve optional systems/assets. This document owns the full concept; [individual feature contracts](features/backlog.md) retain detailed decisions, reasons and exclusions. Numbered tasks link those rules, implementation work and unresolved questions. A research proposal is not a selected mechanic.

# 1. The fantasy

A short, funny **first-person 3D excavation game** about hearing that something extremely valuable has been uncovered after water levels dropped, then arriving with the completely reasonable conclusion:

> There must be more stuff down there. I am going to get rich.

The player starts with a basic shovel, a primitive setup and unrealistic expectations.

They gradually turn themselves into an absurdly over-equipped amateur excavator while uncovering:

- ordinary rubbish;
- surprisingly valuable household objects;
- coins and tools;
- machinery;
- bones and fossils;
- buried vehicles or parts;
- strange clusters of related objects;
- a handful of things that increasingly should not exist where they were found.

The game begins relatively believable and becomes increasingly absurd.

The protagonist may begin wanting money but ultimately become **famous because of a completely impossible final discovery**.

The exact ending object is intentionally undecided.

Current joke territory includes things like an impossibly ancient **chushkopek** or primitive ancient machine for making moonshine.

The fictional location should not explicitly name a real country.

---

# 2. Core design principle

The game should satisfy two feelings:

> **“I wonder what I’m going to find.”**

and

> **“I wonder what my next upgrade will let me do.”**

Everything should support those.

It is **not a puzzle game**.

The player should not regularly:

- solve environmental puzzles;
- memorize recipes;
- switch between many tools;
- manage multiple currencies;
- carry objects individually over long distances;
- clean every fossil with five specialised brushes;
- navigate complicated phones or menus;
- wait for analysis timers;
- sleep to restore resources.

The basic work remains deliberately simple.

---

# 3. Core loop

The intended loop is:

**Dig → detector reacts → investigate → expose object → collect it → decide whether to push farther → return using remaining energy → sell → upgrade → dig again.**

The player should spend most of their time actually excavating.

The surface exists to provide a short satisfying break:

**return → sell → recharge → upgrade → immediately go back.**

Not:

**return → walk through building → open phone → find correct menu → sort 20 objects → buy fuel → sleep → walk back.**

---

# 4. The excavation starts completely untouched

This is important.

There are **no pre-dug holes** showing the player where they are supposed to go.

The player arrives at untouched exposed ground and creates the excavation themselves.

They can:

- dig straight down;
- dig diagonally;
- dig sideways;
- widen a pit;
- create trenches;
- create strange shapes;
- potentially make tunnels or overhangs if the terrain system supports them.

There is no intended tunnel route.

Tunnelling may happen naturally, but the game should **not force the player into tunnels**.

The important thing is freedom.

---

# 5. Sideways digging must be genuinely worthwhile

_A Game About Digging a Hole_ naturally encourages going downward.

Our game should avoid making:

> DOWN = PROGRESS
> SIDEWAYS = WASTED TIME

Depth can influence progression, geology and what kinds of things tend to appear, but it should never completely determine value. Avoid depth-based price multipliers that make rushing one straight shaft overwhelmingly better than following signals and lateral clusters. Downward digging remains useful; sideways digging earns its place through discoveries and worthwhile returns, without forced detours.

A detector signal may lead:

- downward;
- sideways;
- diagonally;
- back toward an earlier layer.

A partially uncovered skeleton might continue horizontally.

One vehicle part may suggest that the rest of the vehicle is nearby.

A hard formation discovered early may become worth revisiting after an upgrade.

The player should sometimes think:

> “I wonder what was behind that wall I couldn't get through earlier.”

rather than always:

> “I need to go another 10 metres deeper.”

---

# 6. World structure

The exact location remains **UNDECIDED**.

Current possibilities include:

- exposed riverbed after exceptionally low water;
- largely drained reservoir;
- artificial waterworks/reservoir connected to a river.

The important requirements are already clearer than the exact setting.

The location should have:

- attractive scenery above the excavation;
- dry sand, gravel, soil, stone and vegetation;
- some damp areas without becoming endless brown sludge;
- visible water;
- believable boundaries;
- no requirement for swimming gameplay.

The player should understand why they cannot excavate infinitely across the whole planet.

Possible boundary language includes:

- solid bedrock;
- steep natural rock;
- retaining structures;
- deep inaccessible water;
- authored non-diggable geological boundaries.

Permanent boundaries must look categorically different from excavation materials. Anything presented as ordinary soil or hard diggable stone must eventually yield to suitable purchased equipment; do not use an identical grey rock for both a tough formation and an eternal boundary. Mass, structure, silhouette and clear presentation should communicate what lies outside the finite site. Choose the exact boundary language with the terrain art; example walls/water/bedrock are not new assets.

---

# 7. Terrain

The terrain is freely excavatable inside the valid excavation region.

The player's changing hole is itself a reward. Widening a pit, carving a spiral return, keeping useful steps, or making side tunnels and chambers should remain enjoyable even without chasing the next detector signal. Preserve substantial supported routes through cleanup and saving; no base-building or smoothing-tool system is implied.

There can be several geological materials.

However:

> different texture ≠ treasure marker.

The player should **not** see a special red patch and immediately know:

> “Skeleton here.”

Ground variation exists because the underground should look interesting and because tougher materials can affect digging.

Possible materials:

- loose sand;
- soil;
- clay;
- gravel;
- compact sediment;
- rock;
- occasional concrete or constructed material.

Exact materials come later.

Tiny leftover fragments and thin slivers disappear as part of excavation cleanup so they cannot snag movement. Remove disconnected soil and conservatively clean up tiny attached remnants while preserving substantial supported ledges, tunnels and overhangs. Visuals and collision clear together, without falling debris or another resource cost.

---

# 8. Tough ground

Current preferred direction:

**ground gets somewhat tougher with depth, but hard formations also occur locally at different depths.**

So there is some sense of going deeper into harder terrain without making all horizontal exploration obsolete.

An early hard formation might take many shovel hits.

Later, after improving the digging tool, the player returns and demolishes the same material easily.

That is important.

Upgrades should make the player feel powerful against old obstacles.

The game should avoid the frustrating pattern:

> Buy 2× stronger tool → immediately encounter terrain with 2× more health → feel exactly as weak as before.

---

# 9. The digging tool

There is **one primary excavation tool**.

The player should not constantly switch:

- shovel;
- pickaxe;
- drill;
- scanner;
- brush;
- another shovel;
- etc.

Left mouse remains the basic **DIG** action.

Digging defaults to click-and-hold: hold the bound Dig input to keep digging and collect eligible aimed finds, and release it to stop. Task `78` adds free optional toggle digging and full keyboard/mouse rebinding from startup and Pause; toggle mode starts/stops on successive fresh Dig presses. Menus, focus loss and rescue preserve the existing release-before-resume safety. Continuous held digging works from the starting shovel; never sell basic input comfort as a later drill upgrade.

The starting tool is essentially a normal shovel.

It then becomes increasingly ridiculous while remaining recognisably the same evolving tool.

Possible visual progression:

**basic shovel → reinforced shovel → battery-powered shovel → motorised monstrosity → completely unreasonable homemade excavation device**

Major upgrades physically modify it:

- motors;
- battery packs;
- larger blade;
- strange attachments;
- reinforcement;
- wiring.

By the ending, it should look like somebody has upgraded the same shovel in their garage far beyond what any manufacturer intended.

---

# 10. Digging upgrades must feel powerful

There will likely be around **6–8 upgrade levels** for major systems.

Not every level needs to transform the game.

Some can be incremental.

But certain expensive levels should create a major jump.

Example structure:

- Tool Lv2 — noticeably faster
- Lv3 — larger excavation area
- Lv4 — more efficient
- **Lv5 — expensive major powered upgrade**
- Lv6 — makes previously difficult formations easy

Important upgrades can cost significantly more so the player occasionally has something exciting to save toward. Preserve useful late power: test the final meaningful purchase around 75-85% of the first completion, leaving time to use it before the ending. This is a pacing hypothesis for `56`/`37`, not a time gate or a requirement to max every track. `57` reviews brief physical installation of major attachments onto the same shovel, with `11` delivering the selected staging and approved assets.

The player buys upgrades **one level at a time**.

Even if they find something valuable, they cannot jump directly from Level 1 to Level 8.

---

# 11. Pure money progression

Main progression is driven by money.

There is no need for arbitrary rules such as:

> Reach 20 m before Battery Lv4 becomes available.

The upgrade shop is simple enough that this is unnecessary.

The player chooses what limitation matters to them. Balance so that the most useful purchase changes with the player's route, equipment and priorities: capacity, battery, resistance, mobility and detector quality can each become meaningful constraints. Avoid one track, such as battery, being objectively best for the whole game.

This is changing pressure, not a forced inventory → battery → strength sequence or difficulty that automatically scales against each purchase. The starter expedition must already be enjoyable, and upgrades must leave earlier limitations genuinely easier. Optional dynamite/light purchases enter this balance only if selected.

Likely categories:

- Digging Tool
- Battery
- Jetpack
- Inventory
- Detector

Potential later additions:

- Dynamite
- one or two strange late-game upgrade categories

Approximately **6–8 levels per major category** is the current target.

Exact prices and balancing should be discovered through playtesting.

Every purchase should show a concise current-to-next effect, cost and practical benefit. Major milestones must communicate what became meaningfully easier, without unexplained locked icons, story permission gates or a spreadsheet-like shop.

---

# 12. Discoveries

The game should have **many more distinctive finds than A Game About Digging a Hole**.

Current rough content target:

### Ordinary repeated objects

Around **20–30 types**.

Examples:

- bottles;
- cheap tools;
- cans;
- coins;
- scrap objects;
- household rubbish;
- simple appliances;
- random old belongings.

These provide reliable income.

### Distinctive / unique discoveries

Around **30–50 types**.

Examples may include:

- unusual appliances;
- chushkopeks;
- homemade machinery;
- older weapons/tools;
- unusual fossils;
- strange mechanical objects;
- interesting vehicle parts;
- increasingly bizarre things.

The exact list comes later.

Author discovery novelty across early, middle and late play: random positions or higher-value copies of the same junk are not enough. Preserve new recognizable objects and related lateral discoveries close to the ending; avoid long stretches whose only change is more dirt.

### Very rare permanent upgrades

Include **2–4 very rare buried discoveries** that grant permanent passive improvements instead of being sold. Possible effects include a detector improvement, better jetpack efficiency, or a dynamite improvement if dynamite is included; the exact items and effects remain to be chosen.

Collecting one grants its effect once, outside ordinary inventory capacity. It cannot be sold or lost through rescue, and the benefit persists with the save. These are optional bonuses: none is required to reach or finish the ending, and normal money-based progression must work without finding them.

---

# 13. Procedural discovery placement

Discoveries should **not be fully hand-placed**.

Replayability comes partly from not knowing where things are.

Current direction:

- authored surface environment;
- procedural underground discovery placement;
- the same intended unique-item pool exists in every playthrough;
- positions change;
- depths change within sensible ranges;
- rotations/orientations change;
- surrounding objects may change.

The player should be able to replay the game and genuinely not know where a favourite discovery is.

Generate a candidate layout, then validate its pacing before accepting it. Reject or rearrange excessive gaps between noteworthy discoveries, unrelated major finds clumped together, and layouts that concentrate nearly all strong novelty early. Related-item clusters remain intentional. Use overlapping encounter bands and representative downward/lateral search patterns, not a prescribed route or guaranteed minute-by-minute reveals.

Keep generation and repair bounded and reproducible. Persist the accepted population; changed pacing rules must never reroll an existing save. Spatial checks guide distribution, while actual full runs establish whether discovery timing works.

---

# 14. Depth influences finds, but does not dictate them

There can be broad tendencies:

### Shallower

More recent rubbish, coins, household items.

### Middle

Older equipment, machinery, bones, vehicle parts.

### Deeper

Older material, fossils, increasingly bizarre discoveries.

But the ranges overlap.

A great discovery can still happen relatively shallow.

An ordinary object can appear surprisingly deep.

This prevents the player learning:

> “Ignore everything above 30 metres.”

---

# 15. Discovery clusters

Related objects should sometimes generate together.

Examples:

A bone may suggest additional bones nearby.

A wheel may be part of a buried vehicle.

Several household objects might surround some buried human-made structure.

Fragments of machinery may lead toward the larger machine.

This creates natural sideways exploration.

The game does not need a quest telling the player:

> DIG 6 METRES EAST.

The player finds something and becomes curious about the surrounding area.

---

# 16. The detector

The detector is one of the most important progression systems.

It is **passive equipment**.

The player never equips a metal detector.

They simply dig.

Then:

> beep...

The player stops.

> beep... beep...

They start investigating.

This should create excitement rather than constant noise. Signals suggest an investigation; they do not prescribe tunnel geometry or make every unsignalled widening/return path wasted work. A player can deliberately ignore a signal to shape their excavation.

Signals come from physically noteworthy buried objects, not a high-price or rarity filter. A large inexpensive object or recognizable assembly can signal while routine tiny rubbish usually does not. Author physical eligibility explicitly, including any small but noteworthy exception; cash value alone never qualifies a target.

---

# 17. Detector progression

Early detector:

- short range;
- simple proximity beeping.

Later upgrades can add:

- greater range;
- better proximity feedback;
- stereo left/right information;
- eventually better directional guidance.

It should become more useful as upgraded, without becoming a treasure GPS.

The detector does **not reveal value or rarity**.

The player should not learn:

> low beep = garbage
> special beep = ignore everything else and go directly to legendary item

because that would make ordinary discoveries obsolete.

A huge object may generate a stronger or broader signal because of its physical size, but the detector still does not say what it is.

---

# 18. Detector frequency matters

The detector should not beep constantly.

Foreground one nearby eligible target at a time, with stable switching as the player investigates. Do not blend several pulse streams or rapidly jump between targets. This is an internal audio focus, not a visible target lock or exact marker.

If it makes noise every twenty seconds regardless of context, the signal stops being exciting and becomes another annoying game sound.

This is particularly important because the game has **no music**.

The underground audio space needs enough silence for:

> beep...

to matter.

---

# 19. Uncovering small finds

Cheap/small objects should be quick.

The player digs, exposes one and collects it without performing archaeology on every bottle.

There should be clear pickup feedback so upgraded digging never creates the problem:

> “Apparently I collected something but I didn’t even see it.”

Ordinary objects can enter the inventory quickly.

---

# 20. Uncovering interesting objects

More interesting objects remain physically visible when exposed.

They do not instantly disappear because the shovel touched them.

The player excavates around them.

Gradually, they recognise the shape.

Once enough is exposed, the object becomes interactable.

Example:

At first:

> curved piece of metal

Then:

> handle

Then:

> rectangular body

Then:

> oh for fuck’s sake, it’s a chushkopek.

That recognition moment is part of the reward.

---

# 21. No tedious final cleaning

Current approved collection requires 40% sampled exposure plus actual visibility and reach. Holding dig directly on a visible small find clears nearby covering ground with normal shovel strokes until it can be collected; large finds retain deliberate surrounding excavation (Task `86`). Production shapes must remain recognizable during uncovering; any different authored threshold needs a concrete playtest-backed decision, rather than silently restoring the earlier higher exposure proposal.

The player should **not** have to remove the final invisible clump of soil underneath something.

No:

- pixel hunting;
- precision brushing;
- washing minigame;
- individual bone cleaning;
- excavation puzzle.

Once the game is confident the player has genuinely revealed it, they can interact.

---

# 22. No identification bureaucracy

Normal finds are simply recognised.

The game does not require:

- mailing objects;
- waiting for experts;
- identifying twenty items one by one;
- sending screenshots manually;
- waiting for analysis timers.

The object can simply display:

**Chushkopek**

and whatever selling information the UI eventually needs.

The humour can come from what it is and how the economy treats it.

For example, a chushkopek being worth considerably more than some unimpressive old bone can be funny without anybody explaining the joke.

---

# 23. Inventory

Inventory is abstract.

The player does **not** physically carry:

- buckets;
- crates;
- backpacks;
- sacks.

They do not move normal finds one at a time.

Simple slot inventory.

Current rough progression concept:

**10 → 15 → 20 → 30 → 40**

Exact numbers will be balanced later.

The starting inventory should not be so small that the player digs for five seconds and then spends thirty seconds travelling. Together with the starting battery, it must support a satisfying first expedition before any purchase. Upgrades substantially improve a loop that already feels good; they do not repair deliberately miserable starting capacity.

---

# 24. Inventory UI

From inventory the player can:

- inspect finds;
- see what they collected;

Sell One and Sell All are available only at the surface selling station, not from underground inventory inspection.

The player does not need to equip objects in order to inspect them.

Show carried count / capacity and battery status without requiring a menu. The current HUD already does this; retain it through production UI changes and upgrades. These are the facts needed to decide whether to continue, not discovery-completion percentages. Keep the rest of the HUD quiet.

Normal sold objects disappear permanently from that save. For selected distinctive objects, `40` reviews a single factual or deadpan inspection sentence; conditional `83` integrates it without forced reading, pickup delays or identification chores. Fridge captions remain name + depth. `84` separately reviews late/postgame assistance for undocumented distinctive finds, with conditional delivery in `85`; no exact treasure GPS, value reveal or numeric completion checklist is selected.

The excavation contains a finite set of generated items rather than infinitely spawning loot.

---

# 25. Selling

Money is not banked until the player safely returns to the surface and sells.

That creates tension.

Finding something worth a lot underground means:

> “I actually need to get this home.”

The surface has a dedicated selling location.

The upgrade location is separate.

Exact presentation remains open:

- yard;
- trailer;
- van;
- improvised worksite.

The player should not have to enter several rooms or interfaces each trip.

---

# 26. Selling machine

Preferred presentation is a physical, funny selling machine.

But the interaction remains fast.

The player presses:

**SELL ALL**

The inventory is processed automatically.

The machine can:

- shake;
- rattle;
- make industrial noises;
- violently swallow the loot;
- spit out money feedback.

The player does **not** manually drop 25 objects into it.

This keeps the physical humour without turning selling into repetitive labour.

---

# 27. Money from discoveries

Normal objects create reliable income throughout the game. Collecting an ordinary find incidentally exposed on a later trip should still be financially worthwhile; early/common objects do not become economic rubbish merely because the player reached a deeper layer.

Rare sellable finds are worth more; the permanent-upgrade discoveries in section 12 grant their benefit instead of money.

However, rare items should **not create enormous jackpots that destroy progression**.

A good rare find might equal several ordinary expeditions.

It should not instantly purchase half the upgrade tree.

The excitement is:

> “Nice, I can afford that expensive shovel upgrade now.”

not:

> “Apparently the economy is finished.”

---

# 28. Discovery fridge

The first time a distinctive item is discovered, the game automatically records a **snapshot of how it looked when the player found it**.

The actual object can then be sold normally if it is a sellable find; permanent-upgrade discoveries grant their effect instead.

The screenshot remains permanently at the surface.

Possible display:

- refrigerator;
- messy corkboard;
- wall;
- workbench area.

Each photograph displays only:

**Item name**
**Depth found**

No sale price.

No condition.

No rarity.

No `17 / 42`.

The wall simply grows naturally as the player plays.

Steam achievements can handle explicit completion goals. They should mostly reward discoveries, equipment milestones and actions players naturally want to perform. Avoid completion chores requiring enormous purpose-built shafts, exact terrain geometry or fragile uninterrupted-fall tricks; any unusual challenge needs a deliberate design decision and forgiving, clearly testable rules.

This makes every player's discovery wall slightly different because the screenshot captures their own excavation.

---

# 29. Item condition

A condition system is **OPTIONAL / LOW PRIORITY**.

It could make duplicate finds more interesting, but the game should not become about evaluating condition grades.

The concept works perfectly well without it.

If prototyping shows it adds unnecessary noise, remove it.

---

# 30. Battery

Digging and jetpack use **one shared battery**.

That relationship is important. Starting capacity must allow meaningful excavation and a reasonable return before the first upgrade; frequent tiny compulsory trips are not how we demonstrate later power.

The player sometimes has:

- free inventory slots;
- something interesting nearby;
- enough energy to continue digging;

but also needs enough power to climb back out.

The question becomes:

> “Do I keep digging?”

That is one of the main sources of light planning.

---

# 31. Battery does not punish curiosity

Energy is consumed by meaningful powered actions:

- digging;
- jetpack.

It should not drain because the player:

- reads an item;
- looks around;
- stands still;
- thinks;
- inspects something.

The game should not punish them for appreciating a discovery.

---

# 32. Returning

Returning is part of the expedition.

There is no free normal teleport.

The player climbs/flys back through the excavation they created.

The surface sell/upgrade area should be close to the excavation entrance once they reach the top.

The game should avoid:

> exciting 30-second excavation
> followed by 90 seconds of boring walking

The interesting return is **getting out of the hole**, not crossing an empty car park afterward.

Evaluate a **HOME-direction aid** and a small set of reusable, visually differentiated revisit markers early in task `70`, immediately after the first complete trip. Compare a concrete no-aid/HOME/marker proposal before waiting for late paid equipment. A rough bearing may point toward the surface/base, never a route, waypoints or GPS guidance. Inclusion, marker count and free-versus-purchased access require review; neither is a required upgrade or a commitment to navigation UI.

A separate optional idea is a simple player-placed marker/flag/light to remember a hard formation or promising branch. Test whether players naturally remember their own excavation before adding it. It would not find treasure, show a route or become a building menu; exact form, limits, reuse, saving and behaviour when its supporting soil is dug away remain unresolved.

Measure confusing navigation separately from repetitive commuting. Tasks `15`/`59`/`37` establish a return friction budget from active excavation/discovery, return, station time and outbound retracing, including why bag/energy limits caused each trip. The shared battery, inventory and return geometry are enough; do not add warmth, oxygen, food or another fuel meter. First review jetpack progression, route readability, discovery spacing and the short surface checkpoint. Underground outposts or return shortcuts are not committed additions; investigate them only if actual late-game travel remains dull and the user selects a change.

---

# 33. Return warning

The player can see their battery.

The game can also provide a passive **return-power warning**.

It should roughly account for how difficult the return seems to be rather than simply activating at an arbitrary battery percentage.

But it should not solve the game mathematically.

Not:

> REQUIRED RETURN ENERGY: 17.4%

More like:

> RETURN POWER LOW

or visual states such as:

**safe / risky / critical**

Exact implementation can be tested.

---

# 34. Surface recharging

Normal recharge is:

- instant or nearly instant;
- free.

No sleeping.

No day/night requirement.

No fuel purchasing every expedition.

The battery system exists to make underground decisions interesting, not to tax the player every time they come home.

---

# 35. Jetpack

Jetpack progression should improve dramatically.

Early:

- weak boosts;
- poor efficiency;
- limited ascent.

Later:

- stronger thrust;
- better efficiency;
- longer sustained flight;
- much better control.

Eventually returning from old shallow excavations should feel trivial.

This gives movement progression in addition to digging progression.

Every jetpack level needs a noticeable practical benefit, with major milestones changing capability: stronger controlled boosts, easier steering or sustained ascent rather than a string of negligible percentages. Validate each level against the same routes and battery budget; battery capacity alone must not account for every improvement. Exact levels/attributes remain design and playtest decisions, not permission to add startup charges or change the simple Space input.

Higher-tier mobility must remain controllable in narrow player-created shafts. Ordinary wall/ceiling bumps should not become damaging merely because the player bought more thrust; test braking and landings so the upgrade stays desirable.

---

# 36. Falling

No annoying chip-damage system.

Minor falls do nothing important.

Large falls can:

- stun;
- knock away some battery;
- create a moment of danger.

Extreme situations may lead to rescue.

There does not need to be a traditional health-management game layered on top.

---

# 37. Rescue

Excavation progress **NEVER RESETS**.

This is locked.

Once the player digs somewhere, the world stays changed.

Save the whole excavation and associated discoveries/economy/progression consistently: periodic autosaving during changed gameplay, checkpoints after successful selling/upgrading, and safe recovery from interrupted writes. Use a short measured maximum unsaved interval without interrupting digging or spamming saving notices. Task `80` extends the existing background writer with bounded incremental terrain capture where needed, coherent revisions and measured late-game allocation/frame costs. Preserve stable discovery IDs, seeds and explicit serializable world edits for possible future co-op; do not build networking for launch. Recovery restores the last complete snapshot, never fresh terrain paired with retained purchases.

When the shared digging/jetpack fuel reaches zero, emergency rescue triggers automatically, returning the player to the surface with a full battery. There is no manual rescue option or confirmation in the pause menu (Task `86`). The return feedback reports the actual ordinary-find loss and fee.

Current direction:

- player returns to surface;
- ordinary carried loot is lost;
- unique/special progression objects are protected;
- rescue also has a financial cost sufficient to discourage intentionally using rescue as a teleport;
- the fee should never permanently ruin a save or make finishing impossible.

Rescue must remain an emergency fallback even when rich or carrying an empty bag, not the cheapest routine shortcut. Compare the actual fee/loss and time saved across the economy; do not make stranded low-money players unable to recover. Exact values and any change to the current fuel-depletion fee policy belong to return design and playtesting.

---

# 38. Dynamite

Dynamite is potentially one later system.

If included:

The player does **not throw dynamite**.

They:

1. place a charge;
2. move away;
3. remotely detonate it.

Charges cost money.

Possible upgrades:

- larger blast;
- stronger blast;
- cheaper charges;
- more charges.

Dynamite should be properly powerful.

Saving for explosives and then discovering they barely break anything would repeat one of the disappointing parts of the reference game.

A placed explosive should create a satisfying:

> **BOOM — large amount of terrain gone.**

Placement should snap forgivingly to a valid visible nearby surface, with a clear valid/invalid preview. Moving, hovering, falling or aiming at a wall should not demand a tiny hotspot, perfect angle or standing still. Revalidate reach/occlusion at placement; invalid attempts do not spend a charge. Preview art and the exact controls still need their own design/approval.

If included, a placed charge must stay reliably anchored and produce a substantial predictable blast with matching terrain cleanup/collision. Explosives remain an optional accelerator, never a bomb-only ordinary route or an endgame stockpiling requirement.

---

# 39. Dynamite and remembered obstacles

Hard formations may encourage the player to remember locations.

Early:

> “I can barely damage this.”

Later:

> “I finally have the upgrade/dynamite for that.”

When they return, overcoming the formation should provide:

- meaningful excavation progress;
- new access;
- potentially interesting discoveries.

Not necessarily treasure every single time.

But repeatedly spending expensive resources only to uncover ordinary dirt would make the mechanic underwhelming.

---

# 40. Large discoveries

Large discoveries can exist, but they are **not the central focus**.

The game should not become primarily about excavating cars and giant skeletons.

If a large object appears, the player excavates most of it first.

A car should not disappear into an inventory slot.

Once properly uncovered, it may be sold/extracted.

Possible funny extraction:

A cable descends from the surface.

It attaches.

The object gets violently yanked out using physics.

No need for a forced cinematic camera.

The player remains in control.

Some enormous discoveries may instead stay permanently in the excavation.

---

# 41. Special objects

There can be a very small number of special objects outside normal inventory.

Examples:

- keys;
- components;
- strange objects connected to the final discovery.

They do not consume normal inventory slots.

They are never lost through ordinary rescue.

They may eventually be used automatically or inserted somewhere obvious.

No complicated inventory puzzle.

---

# 42. Tone

Priority:

**fun / absurdity / curiosity**

Relaxation is secondary.

The game can be peaceful sometimes, but it should not be designed as generic “cozy slop.”

The humour can be:

- visual;
- item-based;
- physics-based;
- economic;
- environmental;
- occasional text.

It does not need a joke every thirty seconds.

Some discoveries should be genuinely cool without immediately turning into a gag.

That contrast can make the ridiculous things funnier.

---

# 43. Audio

No music.

No voice acting.

Main soundscape:

- wind;
- distant water;
- birds/insects where appropriate;
- shovel/terrain impacts;
- falling dirt;
- equipment motors;
- jetpack;
- selling machinery;
- occasional explosive effects;
- restrained detector sounds.

Detector and error noises must not become irritating.

Digging sounds should vary coherently by material and tool milestone, with restrained variations within repeated actions. Define this alongside terrain/tool presentation rather than postponing it to final polish; every actual sound still requires approval.

Review approved repeated digging, motor, flight and detector sounds over the intended session length. Use restrained levels, appropriate variation, clean layering and quiet gaps; a short impressive sample can still become tiring after hundreds of repetitions.

No repeated loud:

> ERR! ERR! ERR!

because inventory is full.

---

# 44. Visual style

Exact style remains open.

But the game should avoid:

- endless realistic brown mud;
- visually identical soil for hours;
- excessive darkness;
- ugly procedural surface terrain.

A slightly stylised, readable environment may work better than trying to simulate wet sediment perfectly.

Underground appearance should gradually change through overlapping geological formations within one continuous excavation. Material mixtures and readable colour/texture changes convey progress without separate levels, biome unlocks or rigid visual bands. The exact palette and formations belong to the user's terrain direction; examples are not selected assets.

Provide passive headlamp/tool lighting for enclosed underground work without tool switching or mandatory placeable lamps. The baseline must remain usable with an empty battery; visibility is not a new resource chore or hazard. Mounting, beam/shadows and any later improvements need design/performance research. A proposed 2–3-level light progression is not yet a selected shop category.

The surface world should be authored.

The procedural replayability comes mainly from **what is underground**, not from generating an entirely new ugly landscape every save.

---

# 45. Assets

Use either:

- assets licensed for commercial use;
- assets made specifically for the game.

Custom models can be created when needed.

The project does not need every bottle or rock sculpted from scratch if suitable commercial-use assets exist.

More distinctive discoveries and the evolving tool deserve custom attention.

---

# 46. Main progression arc

The game should begin modestly.

### Early

Basic shovel.
Weak detector.
Small battery.
Weak jetpack.
Limited inventory.

Digging itself is fairly slow.

Discoveries are mostly believable.

### Middle

Tool becomes visibly modified.
Detector starts providing better spatial information.
Player can afford meaningful expeditions.
Hard formations become manageable.
Discoveries become stranger.

### Late

Tool is ridiculous.
Jetpack is substantially stronger.
Old terrain melts away.
Detector is useful without solving the game.
Dynamite may be available.
A few impossible discoveries begin suggesting something is seriously wrong with the history of this place.

Then the player finds whatever leads to the final discovery.

---

# 47. Mystery pacing

The mystery should **not dominate from minute one**.

Current preferred direction:

### Beginning

Almost everything can be explained.

### Middle

One or two finds are slightly suspicious.

Maybe they are simply jokes.

Maybe not.

### Late

Several things increasingly do not fit.

The player starts thinking:

> “Wait. Why is this here?”

Then the final discovery confirms something absurd/impossible.

This avoids an ending that feels completely unrelated to the previous game.

Optional proposal: perhaps 3–5 very short, non-blocking buyer reactions or surface headlines tied to strange finds. Review whether they improve foreshadowing before selecting them; no dialogue tree, voice acting, forced reading or repeated subtitle spam. Exact lines, trigger/replay behaviour and presentation remain with ending design, not automatic imports from report examples.

---

# 48. Ending

Current preferred direction:

The protagonist begins wanting to get rich.

Eventually they uncover something impossible enough to make them **famous**.

A few special keys/components may be involved in reaching/opening it.

Exact structure is unresolved.

The playable lead-up and ending continue the normal upgraded excavation systems. Keep the shovel/digging tool, detector, jetpack and any included dynamite available under their normal rules; never strip upgrades or disable equipment to force a different challenge. Do not switch to stealth, combat or puzzles. Any special keys/components remain automatic or obvious interactions under section 41, and no rare passive upgrade is required.

The final discovery triggers a proper ending cutscene.

The cutscene follows a meaningful normal excavation/discovery payoff; it is a presentation break, not a replacement gameplay mode. Continue Playing restores the same equipment, upgrades and excavation, without requiring every purchase track to be maxed first.

After the cutscene:

**Continue Playing**

becomes available.

The player can:

- keep excavating;
- buy remaining upgrades;
- find missed discoveries;
- complete Steam achievements;
- fill the fridge.

The ending should therefore conclude the story without deleting the player's reason to continue playing.

Completion emphasizes recovering interesting discoveries, not deleting 100.000% of terrain. Players may freely clear the whole diggable site for enjoyment, but neither ending nor achievements should require every last voxel, awkward empty border wedge or removal of their own useful routes. No dirt-completion meter is needed.

Ordinary completion achievements must remain attainable on that same save. Do not require wiping excavation, replaying an unrepeatable reveal or restarting because an already-sold discovery no longer counts; preserve durable discovery/photo/progression records.

---

# 49. Possible final discovery

Not locked.

Current flavour:

Something clearly human-made and strangely familiar...

...but buried somewhere implying it existed impossibly long ago.

Possible joke directions:

- ancient chushkopek;
- ancient moonshine machine;
- absurd household technology;
- some familiar modern invention reconstructed in primitive ancient materials.

The important point is not the exact appliance.

The payoff is:

> **This should absolutely not exist here.**

We should decide the actual object only after the rest of the game works.

---

# 50. Tutorial

Currently **UNDECIDED / LOW PRIORITY**.

Could be almost nothing:

- LMB Dig
- Space Jetpack
- E Interact

Or a very short first loop showing:

dig → collect → sell → upgrade.

This is better decided by watching somebody play the prototype. Basic operations should still be clear: sell versus upgrade stations, battery and bag readings, held digging and automatic saving. Mystery belongs to discoveries, not hidden UI conventions or an unexplained save machine. Use the compact Pause reference and clear menu labels without restoring removed routine HUD hints/subtitles.

### Precision movement

Selected design: held true crouch lowers the viewpoint and permits lower tunnels, with slower horizontal movement on the ground and in the air while normal walking stays responsive. This adds no stealth, stamina or automatic cliff protection. The [precision contract](features/backlog/precision-movement.md) owns the controls, clearance and recovery rules; task `67` implements them.

### First-person comfort

Provide camera comfort settings early, separately from tutorial decisions: an adjustable field of view and a stable center reticle option. FOV and steady crosshair already exist in `65`; `81`/`82` review and deliver brightness calibration and additional reticle choices. Detector feedback in `10` includes independent saved volume/mute and usable reviewed non-audio feedback. Camera shake must be adjustable down to zero, head bob switchable off and jetpack camera effects independently disableable wherever those effects exist. The current camera has none of these added motion effects; keep that baseline instead of introducing motion just to supply switches.

Persist preferences across launches, offer sensible defaults/reset, and carry them through stronger tools and the ending. Implement actual controls for present effects; any future effect must respect the comfort policy when introduced. This adds neither a minimap nor hardcoded movement shortcuts: the default remains click-and-hold unless an accessibility toggle mode is explicitly enabled in the controls task.

---

# 51. Target playtime

Approximately:

**2–3 hours for first completion.**

Useful, realistically purchasable upgrades should remain available into the later part of that run. Balance find income, prices and competing upgrade priorities so players do not max everything early, while leaving enough excavation after a late purchase to enjoy it. Validate this without rare passive bonuses; do not stretch the game with grind, arbitrary depth gates or terrain that immediately cancels each major upgrade.

This is deliberately a small game.

The goal is not to stretch it to 8–10 hours.

Instead it should feel **dense enough** that players finish saying:

> “That had more in it than I expected.”

rather than:

> “Cool idea, but it ran out of content immediately.”

Replayability comes from randomized excavation/discovery layouts and optional completion.

Measure first-recognition moments, distinctive-find gaps, meaningful purchases and dig/return/station time across representative full runs. Compare straight-down rushing, signal-led lateral exploration and mixed routes across several accepted seeds and purchase priorities; no route is compulsory. Check that common finds remain worth collecting and starter trips are satisfying before upgrades. Improve content distribution, feedback and progression before expanding the world merely to fill time.

---

# 52. What this game is NOT

Battery, terrain, falling and return planning provide enough pressure. Do not add environmental hazard systems such as lava, gas, oxygen, hunger or earthquakes.

Do not import smelting/recipe chains or cargo-weight simulation from the reference games; the shared battery and abstract inventory already provide return pressure.

This is not:

- an archaeology simulator;
- a survival game;
- a crafting game;
- a puzzle game;
- an inventory-management game;
- a museum-management game;
- a realistic geology simulator;
- a procedural infinite world;
- a walking simulator;
- a game about constantly swapping tools;
- a game where automation eventually performs the fun activity for you.

The player remains the person doing the digging.

Upgrades make that work more capable and satisfying.

They do not remove the reason to play.

---

# 53. What should be prototyped rather than designed on paper

We should **not** attempt to finalize these without playing:

- exact shovel radius;
- exact energy drain;
- detector range;
- detector beep frequency;
- inventory sizes;
- sell prices;
- upgrade costs;
- rescue fee;
- terrain hardness;
- dynamite blast radius;
- unique-find frequency;
- how much of an object must be exposed;
- exact excavation depth;
- how powerful late jetpack becomes;
- tutorial wording.

Those are balancing questions.

The design goal tells us what should feel good.

The prototype tells us the numbers. Prefer observations of this game's players over another broad comparator survey. Include an unguided roughly 30-minute session and inspect the resulting hole and the player's reasons for shaping it. A repeated narrow-shaft pattern can prompt investigation of economy/detector pressure; it is not proof a player chose incorrectly, nor a quota demanding everyone dig a particular shape.

---

# 54. The core test

If the prototype is working, the player should repeatedly experience moments like:

> “I should probably go back...”

**beep**

> “Fuck it, one more thing.”

They dig sideways.

A weird shape appears.

They expose more of it.

They realise what it is.

They collect it.

Now their inventory is almost full and their battery is getting uncomfortable.

They barely make it back.

They hit Sell All.

They can finally afford the ridiculous powered-shovel upgrade they've been saving for.

Then they remember:

> “There was that hard wall at 14 metres.”

And immediately go back down.

That is the game.

