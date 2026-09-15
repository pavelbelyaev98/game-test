# Roadmap & Tasks

## Status
- **Active Task:** None (Ready for `001`)
- **Build:** `builds/windows/SomethingDownThere.exe` (Working prototype baseline)
- **Direction:** Implementing the vertical slice per `docs/concept/`. Starting with transformative machine upgrades, the janky Sell-All dumpster machine, visual workbench, and 100m+ reservoir depth.

---

## Priority Queue
> **Workflow:** When starting a task, create a thorough spec at `docs/tasks/<ID>-<slug>.md` (Objective, live code analysis, architecture changes, edge cases, Acceptance Criteria). On completion: move spec to `docs/tasks/completed/`, update `docs/baseline.md` if baseline systems changed, and mark `[x]` here.

### Phase 1: Machine Upgrades & Surface Shop
- [ ] **`001` — Transformative Machine Upgrades & Bolt-On Tool Rig** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Overhaul `EquipmentProgression` into 4 powerful tiers (Tier 1 base scoop, Tier 2 motorized teeth 2x speed/wider bite, Tier 3 wide rotary cutter, Tier 4 blast cannon) with visual bolt-on attachments on the tool viewmodel.
- [ ] **`002` — Hold-to-Dig & Continuous Digging Assist** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `08_INTERFACE_AND_CONTROLS.md`): Make continuous hold-to-dig the responsive default experience with optional toggle-to-dig setting, eliminating click-per-bite fatigue.
- [ ] **`003` — Janky Sell-All Dumpster Machine** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Build physical hopper machine in the yard with an interactive lever, noisy mechanical grinding audio, digital readout, and single-press "Dump All" payout keeping unique exhibit finds.
- [ ] **`004` — Visual Upgrade Workbench UI** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Overhaul the upgrade bench in UI Toolkit: visual tool preview with bolt-on progression, transparent "Current → Next" stat comparisons across all 6 tracks, and punchy purchase feedback.
- [ ] **`005` — Fuel Point & Transparent Surface Recharging** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Dedicated surface fuel dispenser with transparent pricing, full/partial refill options, whole-dollar `$`, and preserving current fuel when tank capacity upgrades.

### Phase 2: Reservoir Depth, Zones & Ground Feel
- [ ] **`006` — Expand Reservoir Depth (100m+) & Site Boundaries** (`docs/concept/03_WORLD_AND_SITE.md`): Scale `TerrainVolume` to 100m+ depth with contained lateral footprint. Build industrial boundaries (cracked concrete dam walls, intake tower silhouette, bedrock floor).
- [ ] **`007` — Four Proportional Geological Depth Zones** (`docs/concept/03_WORLD_AND_SITE.md`): Partition vertical depth proportionally into 4 zones: Zone 1 Recent Fill (0–25m), Zone 2 Old Sediment (25–50m), Zone 3 Deep Clay & Stone (50–75m), Zone 4 Ancient Constructed (>75m).
- [ ] **`008` — Multi-Material Ground & Automatic Tool Adaptation** (`docs/concept/03_WORLD_AND_SITE.md`, `04_TOOL_AND_MOVEMENT.md`): Assign material IDs to voxels (Soil, Clay, Gravel, Rock, Diggable Concrete). Tool automatically adapts bite speed, sound, and resistance without manual mode switching.
- [ ] **`009` — Multi-Material Terrain Shaders & Chunk Meshing** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Chunk meshing passes material weights/indices into vertex data/shaders to render distinct triplanar textures and normal relief for Soil, Clay, Gravel, Rock, and Concrete.
- [ ] **`010` — Seam Cleaving Mechanics & Fracture Feedback** (`docs/concept/03_WORLD_AND_SITE.md`, `14_PROTOTYPE_PLAN.md`): Broad cuts along natural density/material seams trigger crack audio → physical slab shift → fracture break to clear sections efficiently.
- [ ] **`011` — Hard Pockets & Geological Obstructions** (`docs/concept/03_WORLD_AND_SITE.md`): Seed 5–8 authored hard obstacles (concrete plugs, boulder clusters) that resist early tools but offer multiple solutions (upgrades, C4, routing).
- [ ] **`012` — Tiny Remnant & Crumble Cleanup** (`docs/concept/03_WORLD_AND_SITE.md`): Automatically cull thin unsupported terrain slivers and floating specks while strictly preserving player-carved ledges, tunnels, and overhangs.

### Phase 3: Discoveries, Clusters & Detection
- [ ] **`013` — Discovery Tier Classification & Roster Rebalance** (`docs/concept/05_DISCOVERIES.md`): Replace uniform 1,024 mineral grind with ~80–120 authored finds across the 4 zones, classified strictly into Commons (sellable junk/ore), Distinctives (repeatable high-value), and Uniques (1 per save, unsellable).
- [ ] **`014` — Vertical Slice 5 Signature Finds & Silhouette Reveal** (`docs/concept/05_DISCOVERIES.md`, `14_PROTOTYPE_PLAN.md`): Integrate 5 key slice finds (*washing machine, hand drill, gearbox, mammoth bone, gramophone*) with 50–70% exposure thresholds for partial silhouette recognition.
- [ ] **`015` — Buried Themed Clusters** (`docs/concept/05_DISCOVERIES.md`): Seed clustered finds (buried workshop scene, mammoth bone bed, old campsite) rewarding lateral exploration off the main vertical shaft.
- [ ] **`016` — Finds Inside Containers** (`docs/concept/05_DISCOVERIES.md`): Add buried containers (suitcases, toolboxes) cracked open with the machine in the world to reveal nested discoveries.
- [ ] **`017` — Tool-Mounted Silent Visual Detector** (`docs/concept/05_DISCOVERIES.md`): Directional visual pulse cue on the machine indicating distance and broad direction to the nearest uncollected distinctive/unique find without audio beeps, radar maps, or value spoilers.
- [ ] **`018` — Subtle Hover Price on Exposed Finds** (`docs/concept/05_DISCOVERIES.md`, `13_OPEN_QUESTIONS.md`): Display subtle fixed sale price tag when reticle hovers over an exposed, collectible sellable find.
- [ ] **`019` — Physical Find Handling (Lift, Throw & Rest Stability)** (`docs/concept/05_DISCOVERIES.md`): Refine RMB lift/drop and LMB throw with realistic simulation weight; dropped finds settle stably into rough terrain crevices.

### Phase 4: Surface Yard & Trophy Display
- [ ] **`020` — Surface Trophy Exhibit Stands & Placement** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): Build physical display stands and shelves in the surface yard where players manually socket collected unique oddities into pre-placed spaces.
- [ ] **`021` — Unique Find Lore Cards & Inspection** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `05_DISCOVERIES.md`): Interacting with placed trophies displays name, depth found, and 1-sentence deadpan lore card with support for rereading anytime.
- [ ] **`022` — Surface Yard Props & Worksite Environment** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): Populate surface yard with authored props: protagonist's rusty pickup truck, utility trailer, generator, fuel hose, and floodlights within 10s of the shaft.
- [ ] **`023` — Yard Progression & Late Cosmetic Sinks** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `06_PROGRESSION_AND_ECONOMY.md`): Optional visual worksite evolutions (shelter over workbench, weather tarp over display wall, tool skins, decorative lamps).

### Phase 5: Movement, Battery & Explosives
- [ ] **`024` — Jetpack Flight & Hover Hold Progression** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Smooth Space/bumper thrust handling. Upgrades improve ascent speed, fuel efficiency, and add hover-hold assist in narrow shafts without wall-bump collision damage.
- [ ] **`025` — Precision Crouch & Shaft Shaping** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Held Ctrl lowers viewpoint and slows horizontal speed to 35%, allowing players to carve tight horizontal tunnels without stamina limits.
- [ ] **`026` — Battery Drain Balance & Return Warning** (`docs/concept/02_CORE_LOOP.md`, `06_PROGRESSION_AND_ECONOMY.md`): Link battery drain to digging strokes and flight. Adaptive Return Warning (Safe / Risky / Critical) based on depth and ascent energy required.
- [ ] **`027` — Harmless Falls & Forgiving Debt Rescue** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Ordinary falls deal harmless landing feedback (no health bar, no battery loss). At 0 battery underground, auto-rescue to surface: keep all finds, charge depth fee + apply interest-free debt if broke.
- [ ] **`028` — Consumable Sticky C4 Charges** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Placed sticky C4 charges with remote detonation. Removes large predictable voxel volume while ensuring buried finds survive intact.
- [ ] **`029` — Reusable Underground Work Lamps** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Purchasable portable work lamps placed on hole walls to illuminate deep shafts and interesting discoveries.

### Phase 6: Audio, Sensory Feel & Feedback
- [ ] **`030` — Material-Specific Digging Audio** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Material cutting sound loops: sand hiss, clay thump, gravel rattle, rock sharp crack, concrete grinding screech. No music.
- [ ] **`031` — Machine Motor Whine & Strain Audio** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Engine pitch responds to tool upgrade level; motor audibly strains when biting dense rock or concrete; puff release sound on cut completion.
- [ ] **`032` — Cavern Acoustics & Depth Reverb** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Depth-based low-pass audio filtering and cavernous acoustic reverb that deepens as the player descends into deep shafts.
- [ ] **`033` — Subterranean Daylight Falloff** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Open sky illumination reaches down shafts and fades smoothly into deep underground gloom without pitch-black blindness.
- [ ] **`034` — Dig Juice & Directional Particle Bursts** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Directional soil crumbs and dust puff particle bursts on stroke completion; subtle visual settling feedback on cuts.

### Phase 7: UI, Controls & Persistence
- [ ] **`035` — Minimal Diegetic HUD Overhaul** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Strip HUD to the 5 essentials: real-time Depth meter (`-42.5 m`), Bag gauge with full warning color (`12 / 15`), Battery bar, adaptive Return Warning, and clean reticle.
- [ ] **`036` — Pause Menu & Clean Navigation** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Resume, Save & Load, Settings, Exit to Title. Ensure ESC/B closes menus reliably without trapping input.
- [ ] **`037` — Full Gamepad Parity & Dynamic Glyphs** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`, `10_ACCESSIBILITY_AND_COMFORT.md`): Ensure 100% controller support across all gameplay, UI Toolkit menus, and trophy placement with automatic button glyph swapping.
- [ ] **`038` — Accessibility, Camera Comfort & Sound Captions** (`docs/concept/10_ACCESSIBILITY_AND_COMFORT.md`): FOV slider (55–90°), crosshair toggle, motion sickness mitigation, and directional sound captions for hearing accessibility.
- [ ] **`039` — World Inspection & Photo Mode** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Free-look camera tool accessible from pause menu to inspect unburied finds and capture photos of the carved hole.
- [ ] **`040` — Multi-Slot World Persistence & Save Integrity** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Versioned atomic saves persisting exact voxel deformations, placed trophy coordinates, inventory, and player state across multiple profile slots.

### Phase 8: Mystery Climax, Ending & Sandbox
- [ ] **`041` — Mystery Trail Anachronistic Finds** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Seed subtle anachronistic oddities in Zones 2 and 3 (soda cans far too deep, rustless manufactured parts) establishing curiosity.
- [ ] **`042` — Zone 4 Ancient Anomalous Structure** (`docs/concept/03_WORLD_AND_SITE.md`, `11_ENDING_AND_MYSTERY.md`): Embed anomalous constructed architecture at the reservoir floor (>75m) with smooth unnatural materials that differ clearly from bedrock walls.
- [ ] **`043` — Impossibility Material Contact Signature** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`, `11_ENDING_AND_MYSTERY.md`): Implement ancient material contact signature: clean surgical cuts, glass-like resonance, and too-neat dust settlement without threat cues.
- [ ] **`044` — Ending Components & Assembly Sockets** (`docs/concept/11_ENDING_AND_MYSTERY.md`): 3–4 required components found off the main descent shaft; owned parts automatically insert into structure sockets without inventory management.
- [ ] **`045` — Final Object Discovery & Presentation** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Modern machine constructed from impossibly ancient materials revealed at the core. Presentation sequence without genre switch or loss of tools.
- [ ] **`046` — Ending Excavation Timelapse** (`docs/concept/11_ENDING_AND_MYSTERY.md`, `13_OPEN_QUESTIONS.md`): Retrospective visual replay at the finale showing the progressive evolution of the player's carved hole from untouched start to bottom.
- [ ] **`047` — Post-Ending "Continue Playing" Sandbox & Media Wall** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `11_ENDING_AND_MYSTERY.md`): Seamless transition back to surface hub with unlocked sandbox excavation. Media wall appears with regional newspaper clippings and quiet TV/radio props.
- [ ] **`048` — Steam Achievements Integration** (`docs/concept/12_ACHIEVEMENTS_AND_COMPLETION.md`): Wire 5–10 fair achievements for reaching each zone, maxing the machine, completing trophy stands, and revealing the mystery.

### Phase 9: Release Pipeline & Build Protection (Before Public Release)
- [ ] **`049` — IL2CPP Scripting Backend Migration**: Switch Windows standalone build to native IL2CPP with MSVC compiler. Configure `link.xml` to prevent code-stripping on UI Toolkit and save types.
- [ ] **`050` — Release Code Obfuscation & Binary Hardening**: Integrate symbol stripping, class/method renaming, string encryption, and metadata obfuscation against reverse-engineering tools.

---

## Completed
> Format: `- [ID] Title: 1-2 sentences on what was implemented and how.`

- **[000] Baseline Transition:** Consolidated project documentation, migrated concept chapters into `docs/concept/`, established baseline prototype inventory (`docs/baseline.md`), decentralized minimal asset tracking, and established 50-task JIT roadmap.
