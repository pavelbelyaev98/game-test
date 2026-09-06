Yes — and based on the research, I would **not choose “digging vs chopping vs washing” first**.

I would choose the **game structure**, then pick the fantasy.

My strongest recommendation for you is:

# 🥇 Make a **cleanout / clearing / salvage game**

Something like:

> **You buy/get access to one absolutely disgusting abandoned property and clear it out. Hidden inside the junk are valuables, collectibles, strange objects, locked areas, and one mystery.**

Not _My Wife Threw Out My Card Collection_ exactly. That game actually validates the underlying concept: **trash → uncertain discovery → collection → visible cleanup** is appealing. Its negative reviews are much more about execution—jank, missing cards, weak upgrades, items becoming unreachable, lack of completion information, and basically no satisfying ending—than the core fantasy itself. ([Steam Community][1])

## Why I think this is better for you than pure digging

It lets us combine the best pieces of several games:

**From A Game About Digging a Hole**
→ uncertain finds
→ sell valuables
→ upgrades
→ restricted location
→ progressively better discoveries
→ scanner/detector
→ mystery

**From PowerWash Simulator**
→ enormous visual before/after transformation
→ relaxing repetitive verb
→ sections/checklists
→ “I cleaned THIS” satisfaction

**From My Wife**
→ rummaging through junk
→ rare collectibles
→ one small property
→ obvious completion fantasy

**From Crime Scene Cleaner**
→ valuables hidden inside mundane work
→ environmental discoveries
→ cleaning + searching

And unlike actual digging, you don't need a difficult persistent voxel-terrain system. _Digging a Hole_ itself markets the compact loop of collecting resources, selling them, upgrading, and pursuing a mystery; it's still sitting around 90% positive across 11k+ English Steam reviews. ([Steam Store][2])

---

# What I would actually build

Imagine:

## **A Game About Clearing an Abandoned Property**

You arrive at this horrible lot.

At first you can barely move because there is:

garbage bags
boxes
broken furniture
appliances
branches
scrap metal
old tires
piles of junk

You start clearing it.

But you're not simply:

> click trash → trash disappears.

Instead:

### Trash piles behave like “dirt”

You remove junk from a pile piece by piece.

Eventually you expose things underneath.

Usually:

**worthless garbage**

Sometimes:

**$10 tool**

Sometimes:

**old coin**

Sometimes:

**electronics**

Sometimes:

### ✨ RARE COLLECTIBLE FOUND

And very occasionally:

**something weird relating to the property's mystery.**

That's your equivalent of hitting ore.

---

# The main loop

**CLEAR**

→ junk disappears

→ property visibly improves

→ hidden objects are exposed

→ collect valuables

→ cart/bag fills

→ take load to dumpster/van

→ sell valuable salvage

→ buy upgrade

→ clear harder/larger junk

→ gain access to new section of property

→ find better/weirder things

→ eventually uncover final secret.

This is basically our blueprint compressed into one extremely understandable game.

---

# Your “ore system”

Don't literally use ore.

Use **hidden-find tiers**.

For example:

| Type     | Example                   |
| -------- | ------------------------- |
| Common   | bottles, cans, scrap      |
| Uncommon | tools, electronics        |
| Valuable | jewelry, old cameras      |
| Rare     | collectible object        |
| Jackpot  | watch, antique, rare item |
| Secret   | story/mystery object      |

That gives every pile the:

> **“Maybe THIS one has something good.”**

feeling you liked in _Digging a Hole_.

---

# Your “different materials”

This is where the progression comes from.

### Level 1 — loose trash

Pick up normally.

### Level 2 — trash piles

Need shovel/rake/grabber.

### Level 3 — heavy junk

TVs, furniture, appliances.

Requires improved capacity/tool.

### Level 4 — scrap objects

Need cutter.

### Level 5 — locked objects

Safes, cabinets, shed.

Need upgraded tool/key.

### Level 6 — special obstacle

Final mystery access.

Notice what's happening:

You keep getting **new responses to roughly the same work**, instead of implementing five unrelated games.

---

# Four upgrades

I would keep it brutally simple.

### 1. TOOL

Clear objects faster / handle harder junk.

### 2. CART CAPACITY

Carry more before returning.

### 3. MOVEMENT / HANDLING

Heavy things become less annoying.

### 4. DETECTOR

Find valuables/secrets more easily.

That's enough.

Maybe detector starts as:

**beep**

Then later:

**BEEP BEEP**

Final upgrade:

> Valuable object — approximately 4 m away.

That also solves the terrible “last collectible” problem seen in _My Wife_ and the “last tiny dirty spot” frustration that exists in cleaning games.

---

# The return loop

You were worried about this earlier.

For this particular game:

### Do NOT force a dangerous return.

It isn't necessary.

Instead:

**your cart fills.**

So you're cleaning:

`11 / 12 capacity`

You find a $5 item.

Cart becomes full.

But your scanner suddenly goes:

### **BEEP BEEP BEEP**

behind the refrigerator.

You can:

**go dump/sell now**

or

**throw out something cheap and investigate.**

That's enough decision-making.

We don't need to awkwardly invent:

> “If you stay in the backyard for 80 seconds, you die.”

The blueprint doesn't require every mechanic from _Digging a Hole_.

It requires that **something interrupts endless optimal grinding**.

---

# And make the property itself the progress bar

This is huge.

Beginning screenshot:

# 🤢

Piles everywhere.

Blocked shed.

Overgrown corner.

Trash covering half the yard.

Broken furniture.

At 50%:

The ground is becoming visible.

Paths have opened.

You can reach the garage.

At 90%:

Almost pristine.

Now you notice something you couldn't see before.

At 100%:

The property looks **completely transformed**.

That before/after is your PowerWash Simulator payoff.

---

# Then add ONE mystery

This is where I'd improve massively on _My Wife_.

Maybe from the beginning there's:

**a locked shed.**

Or:

**an old safe.**

Or:

**a concrete hatch underneath one enormous junk pile.**

Or:

**an abandoned car that isn't registered to anyone.**

You gradually find objects hinting at it.

And your final upgrade lets you open/reach it.

Then:

### CLEAR ENDING.

Big payoff.

Credits.

Completion screen.

`PROPERTY CLEARED — 100%`

`COLLECTION — 37/40`

`SECRETS — 5/5`

Maybe Free Play unlocks afterward.

The negative reviews of _My Wife_ explicitly complain about doing all the work and receiving effectively no satisfying “you finished” confirmation. ([Steam Community][1])

We fix that aggressively.

---

# My ranking of the activity types you mentioned

For **YOU + Unity + AI coding + marketplace assets + minimum bugs**:

| Idea                                          |       Hook |  Tech ease | Content ease |            Overall |
| --------------------------------------------- | ---------: | ---------: | -----------: | -----------------: |
| **Property/junk cleanout + treasure hunting** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |   ⭐⭐⭐⭐⭐ |         **9.5/10** |
| **Abandoned-house stripping/cleanup**         | ⭐⭐⭐⭐⭐ |   ⭐⭐⭐⭐ |     ⭐⭐⭐⭐ |           **9/10** |
| **Drain/sewer unclogging**                    | ⭐⭐⭐⭐⭐ |   ⭐⭐⭐⭐ |     ⭐⭐⭐⭐ |         **8.5/10** |
| **Trash/junk dismantling**                    | ⭐⭐⭐⭐⭐ |   ⭐⭐⭐⭐ |     ⭐⭐⭐⭐ |         **8.5/10** |
| **Chopping/forest clearing**                  |   ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |     ⭐⭐⭐⭐ |           **8/10** |
| **Power washing**                             |   ⭐⭐⭐⭐ |   ⭐⭐⭐⭐ |       ⭐⭐⭐ |         **7.5/10** |
| **Mining without terrain deformation**        |   ⭐⭐⭐⭐ |   ⭐⭐⭐⭐ |     ⭐⭐⭐⭐ |         **7.5/10** |
| **Actual voxel digging**                      | ⭐⭐⭐⭐⭐ |       ⭐⭐ |     ⭐⭐⭐⭐ | **6.5/10 for you** |

The last one isn't because digging is bad.

It's because you're voluntarily choosing one of the harder persistence technologies when you don't need to.

---

# Chopping could work too

If you really like chopping:

Don't make:

> **Cut tree → get $10 → cut another tree.**

Too deterministic.

Make:

# **Clear an Overgrown Property**

Cut bushes.

Cut branches.

Cut trees.

Remove stumps.

Reveal stuff underneath.

Maybe:

**old bike**

**coins**

**buried tools**

**animal nest**

**valuable object**

**old foundation**

**mysterious bunker entrance**

Now chopping is merely the **main verb**, while discovery carries the game.

That's much stronger.

---

# Washing can also work, but I'd hybridize it

Don't make:

> generic PowerWash Simulator but smaller.

Instead:

# **Restore Something**

For example:

**clean an abandoned swimming pool**

or

**restore an abandoned gas station**

or

**clean an old workshop**

Washing exposes:

graffiti
numbers
hidden markings
valuable objects
damaged panels
secret compartments.

So again:

> **The repetitive work reveals uncertainty.**

That's the recurring pattern I think matters most.

---

# Another concept I really like

## **Storage Unit Cleanout**

You buy/access abandoned storage units.

Each unit is a tiny mystery box.

Open unit.

Boxes everywhere.

Remove junk.

Search boxes.

Find:

cheap garbage
electronics
collectibles
antiques
rare jackpot

Sell.

Upgrade cart/tools.

Unlock increasingly strange/high-value storage units.

Eventually one unit has a mystery.

### Technical difficulty: very low.

Because every unit can just be a small room built from marketplace assets.

You don't even need one big map.

This may actually be **even easier than the backyard property**.

The downside is weaker permanent-world transformation because you're moving between units rather than watching one location transform.

---

# Dumpster / landfill searching

Also viable.

But I would avoid massive piles of hundreds of Rigidbody trash objects.

Instead make each pile a deterministic **searchable volume**:

Trash pile:

`100%`

Hold/use tool.

`87%`

A few cosmetic trash items move.

`61%`

**object exposed**

`24%`

new visual mesh/state.

`0%`

pile disappears.

That gives you the fantasy of rummaging without reproducing the exact physics problems that hurt _My Wife_.

---

# My top 5 concepts for you right now

### 🥇 **Abandoned Property Cleanout**

Clear disgusting property → find valuables/collectibles → upgrade → uncover hidden mystery.

### 🥈 **Abandoned House Strip-Out**

Remove furniture, junk, wallpaper, boards, fixtures → reveal valuables/secrets → restore empty house.

### 🥉 **Storm Drain Unclogger**

Remove disgusting blockages → reveal lost objects → filter/battery fills → upgrade equipment → penetrate deeper tunnel network.

### 4. **Storage Unit Cleanout**

Search boxes/junk → rare jackpots → sell → buy access to better units → mysterious final unit.

### 5. **Overgrown Property Clearing**

Cut weeds/bushes/trees → expose forgotten objects/structures → upgrade cutting equipment → uncover final hidden area.

---

## The thing I'd prototype first

**Abandoned Property Cleanout.**

But make the prototype absurdly tiny:

**one backyard**

**3 junk pile types**

**6 possible finds**

**1 rare collectible**

**1 scanner**

**1 cart**

**1 sell station**

**4 upgrades**

**1 locked shed**

**20-minute completion**

If THAT 20-minute build feels addictive, expand it.

If picking up/clearing the junk itself isn't fun after 20 minutes, abandon it before spending months adding content.

The core design test is:

> **Is destroying/cleaning/removing the ordinary stuff satisfying even before I get the rare reward?**

If **yes**, then the random treasures, upgrades and mystery can turn it into a real Steam game.

[1]: https://steamcommunity.com/app/3868310/negativereviews/?l=german&utm_source=chatgpt.com "Steam-Community :: My Wife Threw Out My Card Collection (So I Bought a Dump to Find Them All) ⭐"
[2]: https://store.steampowered.com/app/3244220/A_Game_About_Digging_A_Hole/?utm_source=chatgpt.com "A Game About Digging A Hole™ on Steam"
