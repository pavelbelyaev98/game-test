# Discoveries

Owns the detector, seeded generation, roster/content, reveal and collection, find handling/physics, personal display and approved find assets. Task IDs are plain numbers. Detector audio is not selected; common/minor finds stay detector-silent.

## Detector (`10`; upgrades `49`)
- Passive proximity feedback from nearby eligible finds; no scan button or tool switch. Signals suggest exploration, never waypoints/GPS, and never reveal identity, value or rarity.
- All minor/common finds are silent at every tier regardless of size, metal, value, depth or clusters. No common cluster aggregates; no paid or completion exception. Physical noteworthiness is authored separately from price inside non-minor content.
- Early upgrades improve short-range feedback; later ones may add range and broad direction. Foreground one signal with stable switching; no target lock, rapid hopping or detector audio.
- Owner split: `57` defines feel, `56` paid milestones, `10` first feedback after production targets (`42`), `49` paid progression, `84`/`85` conditional late assistance (never minors).

## Generation (`145` implemented; `45`, `155` open)
- New Game: 1,024 finds in 24 × 24 × 32 m — 96 ordinary rocks + 928 minerals in overlapping Coal → Copper → Iron → Silver → Gold → Emerald → Ruby → Diamond bands; 312 shallow rocks/Coal at 0.65–1.1 m. `145` owns exact counts/prices. Bottles no longer spawn in new games but old saves resolve them.
- Placement: bounded seeded best-of-64 candidates, enclosing radius + 0.10 m soil gaps, ≤4,000 candidates per find, explicit failure with seed; validated across seeds for gaps, band/quadrant coverage and multiple finds revealed per battery. Save identities never reroll.
- Continue always uses the persisted population/poses/collected state; New Game is the only place new distribution applies. `45` adds weighted pools and related clusters (first: five micro-scene templates using approved first-batch assets) without pre-dug rooms or detector eligibility.
- `155` (open): compare validated random layouts, a reserved nearby noteworthy encounter and adaptive drought protection; recommend one policy and record generation/save implications.

## Roster and content (`40`–`44`, `89`, `114`, `117`, `149`/`150`)
- Tiers: common/minor = routine finds repeated often (bottles, plain rocks); familiarity alone does not make something common. A familiar toy car is above common. Higher tiers carry stronger surprise; exact labels/frequencies belong to `40`.
- Eight minerals (`145`) are repeatable ordinary sellable finds, detector-silent. Rock `114`: one item, three appearances (hollow A, solid B/C), persistent appearance keys, $2.
- `117` (open): unique-find purpose, one-per-map vs multiple types/copies, sale vs retention. Sale-plus-photo and roster-count directions are baselines. Collectibles becoming battle equipment is rejected. `40` then owns named assignments/frequency/value; `97` owns special interactions/chests.
- Batches in queue order: `41` remaining ordinary (20–30 incl. starters), `42` first five distinctive, `43` middle, `44` late (30–50 total). `148` gates bulk production on actual recognition/lateral interest; `149`/`150` rare uncanny variants are proposed, reusing the finite budget.
- Every batch item must start buried with authored bounds and be recognizable, uncoverable, collectible, sellable and restorable without duplication or pixel cleaning, using approved assets and no substitute primitives. `83` optional sparse inspection flavor remains proposed.

## Reveal and collection (`110`/`112`/`131`/`133`/`136` implemented)
- Recognize and collect smoothly; no cleaning minigame or modal inspection.
- Requires ≥60% of 256 real exterior samples exposed, actual collider visibility and 3 m reach; covered finds cannot be named/collected through soil; developer X-ray never bypasses.
- `136`: active held/toggle Dig directly aimed at an eligible find collects immediately, including during cooldown. `131`: a completed stroke collects an already-aimed item, and grounded walking collects a fully uncovered floor find in the small foot area. Walls/floors, elevated items, held objects, loading, menus, a full bag and deliberately dropped items block walk-over.
- Off-aim/occluded/out-of-reach finds stay; soil-only strokes never collect neighbors. Collection costs no fuel and cannot trigger another dig. Full inventory preserves the world find and suppresses error spam.
- `133` pickup: immediate straight pull ~0.18 s, ≤15% shrink, no arc/bob; a nonphysical copy follows the camera; identity transfers immediately; no save state.
- E stays for stations; `97` may add deliberate special interaction/chest behavior.

## Handling and physics (`115`/`116` implemented; `96` open)
- RMB (rebindable) lifts/drops an aimed eligible object; a fresh Dig press throws it (bottle 8 m/s, Rock 4 m/s). The real body stays visible and collides; lifting works with a full bag; holding blocks dig/collection and clears held/toggle digging.
- Finds detach after terrain rebuild with convex colliders, fall/tip/settle with restrained friction and zero bounce; player collision is ignored. Held motion tracks the player. Pause/load freeze; saves store pose and released state; reload resumes from rest. Rescue/reset releases the held object without duplication.
- Invalid/out-of-site finds restore to their last clear pose; never rerolled, destroyed, auto-sold or duplicated. Selected only for bottles and the Rock; `96` owns remaining bulky/special/ceiling categories.

## Display and records (`51`, `60`; `117` open)
- Notable discoveries get a lasting visual record without an identification/museum chore. Entries show name + discovery depth only (never value, rarity or a counter); sold items may disappear while their surface snapshot remains.
- Capture before a recognized object disappears; repeated load/sale cannot replace the first snapshot; a failed image write must not lose the find or block progress.

## Approved find facts
- Starters (`89`): three bottles, common, one slot, detector-ineligible, trial art retained for old saves only, zero new spawns. Original can/brick IDs map to bottle IDs; aliases must never be removed.
- Bottles: muted green/amber/blue-green frosted glass, worn Bulgarian labels, side-lying spawns; separate Blender convex hulls for physics, visual exterior for exposure; ≥60% exposure threshold. `09` owns final recognition/art acceptance.
- Rock: one common item, three appearances, seeded orientation; 96 shallow spawns at $2 (`114`).
- Cans and bricks were removed and need new explicit approval to return. Starter models are trial art, not final acceptance; the eight-mineral batch is approved. New models require Blender MCP or free commercial licenses, user approval and a ledger entry.
