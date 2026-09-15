# Roadmap & Tasks

## Status
- **Active Task:** None (Ready for `001`)
- **Build:** `builds/windows/SomethingDownThere.exe` (Working prototype baseline)
- **Direction:** Implementing the vertical slice per `docs/concept/`. Starting with transformative machine upgrades, the janky Sell-All dumpster machine, visual workbench, and 100m+ reservoir depth.

---

## Priority Queue
> **Workflow:** When starting a task, create a thorough spec at `docs/tasks/<ID>-<slug>.md` (Objective, live code analysis, architecture changes, edge cases, Acceptance Criteria). On completion: move spec to `docs/tasks/completed/`, update `docs/baseline.md` if baseline systems changed, and mark `[x]` here.

### Phase 1: Machine Upgrades & Surface Shop
- [ ] **`001` — Overhaul `EquipmentProgression` & Tool Rig for Transformative Tiers** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Overhaul `EquipmentProgression` into 4 powerful tiers (Tier 1 base scoop, Tier 2 motorized teeth 2x speed/wider bite, Tier 3 wide rotary cutter, Tier 4 blast cannon) with visual bolt-on attachments on the tool viewmodel.
- [ ] **`002` — Adapt `FpsInput` & `FpsPlayer` for Responsive Hold-to-Dig** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `08_INTERFACE_AND_CONTROLS.md`): Make continuous hold-to-dig the responsive default experience with optional toggle-to-dig setting, eliminating click-per-bite fatigue.
- [ ] **`003` — Replace Sell Station with Janky Sell-All Dumpster Machine** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Build physical hopper machine in the yard with an interactive lever, noisy mechanical grinding audio, digital readout, and single-press "Dump All" payout keeping unique exhibit finds.
- [ ] **`004` — Overhaul Upgrade Bench UI in UI Toolkit** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Overhaul the upgrade bench in UI Toolkit: visual tool preview with bolt-on progression, transparent "Current → Next" stat comparisons across all 6 tracks, and punchy purchase feedback.
- [ ] **`005` — Adapt `SurfaceRecharge` for Dedicated Yard Fuel Dispenser** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Dedicated surface fuel dispenser with transparent pricing, full/partial refill options, whole-dollar `$`, and preserving current fuel when tank capacity upgrades.

### Phase 2: Reservoir Depth, Zones & Ground Feel
- [ ] **`006` — Extend `TerrainVolume` to 100m+ Depth & Reservoir Boundaries** (`docs/concept/03_WORLD_AND_SITE.md`): Scale `TerrainVolume` to 100m+ depth with contained lateral footprint. Build industrial boundaries (cracked concrete dam walls, intake tower silhouette, bedrock floor).
- [ ] **`007` — Partition `TerrainVolume` into 4 Geological Depth Zones** (`docs/concept/03_WORLD_AND_SITE.md`): Partition vertical depth proportionally into 4 zones: Zone 1 Recent Fill (0–25m), Zone 2 Old Sediment (25–50m), Zone 3 Deep Clay & Stone (50–75m), Zone 4 Ancient Constructed (>75m).
- [ ] **`008` — Extend `ExcavationGrid` with Material IDs & Tool Auto-Adaptation** (`docs/concept/03_WORLD_AND_SITE.md`, `04_TOOL_AND_MOVEMENT.md`): Assign material IDs to voxels (Soil, Clay, Gravel, Rock, Diggable Concrete). Tool automatically adapts bite speed, sound, and resistance without manual mode switching.
- [ ] **`009` — Update Chunk Meshing & Triplanar Shader for Multi-Material Ground** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Chunk meshing passes material weights/indices into vertex data/shaders to render distinct triplanar textures and normal relief for Soil, Clay, Gravel, Rock, and Concrete.
- [ ] **`010` — Implement Seam Cleaving Detection & Fracture Feedback** (`docs/concept/03_WORLD_AND_SITE.md`, `14_PROTOTYPE_PLAN.md`): Broad cuts along natural density/material seams trigger crack audio → physical slab shift → fracture break to clear sections efficiently.
- [ ] **`011` — Author Hard Pockets & Obstructions** (`docs/concept/03_WORLD_AND_SITE.md`): Seed 5–8 authored hard obstacles (concrete plugs, boulder clusters) that resist early tools but offer multiple solutions (upgrades, C4, routing).
- [ ] **`012` — Refine Terrain Crumble Cleanup for Player-Carved Structures** (`docs/concept/03_WORLD_AND_SITE.md`): Automatically cull thin unsupported terrain slivers and floating specks while strictly preserving player-carved ledges, tunnels, and overhangs.

### Phase 3: Discoveries, Clusters & Detection
- [ ] **`013` — Rebalance `DiscoveryCatalog` & Quotas into 3 Tiers** (`docs/concept/05_DISCOVERIES.md`): Replace uniform 1,024 mineral grind with ~80–120 authored finds across the 4 zones, classified strictly into Commons (sellable junk/ore), Distinctives (repeatable high-value), and Uniques (1 per save, unsellable).
- [ ] **`014` — Integrate 5 Signature Slice Finds & Silhouette Reveal** (`docs/concept/05_DISCOVERIES.md`, `14_PROTOTYPE_PLAN.md`): Integrate 5 key slice finds (*washing machine, hand drill, gearbox, mammoth bone, gramophone*) with 50–70% exposure thresholds for partial silhouette recognition.
- [ ] **`015` — Implement Themed Buried Find Clusters** (`docs/concept/05_DISCOVERIES.md`): Seed clustered finds (buried workshop scene, mammoth bone bed, old campsite) rewarding lateral exploration off the main vertical shaft.
- [ ] **`016` — Implement Crackable Buried Containers** (`docs/concept/05_DISCOVERIES.md`): Add buried containers (suitcases, toolboxes) cracked open with the machine in the world to reveal nested discoveries.
- [ ] **`017` — Implement Tool-Mounted Silent Visual Detector** (`docs/concept/05_DISCOVERIES.md`): Directional visual pulse cue on the machine indicating distance and broad direction to the nearest uncollected distinctive/unique find without audio beeps, radar maps, or value spoilers.
- [ ] **`018` — Add Subtle Hover Price Tag to Exposed Finds** (`docs/concept/05_DISCOVERIES.md`, `13_OPEN_QUESTIONS.md`): Display subtle fixed sale price tag when reticle hovers over an exposed, collectible sellable find.
- [ ] **`019` — Adapt `FindPhysics` & `FindHandling` for Machinery Mass & Settling** (`docs/concept/05_DISCOVERIES.md`): Refine RMB lift/drop and LMB throw with realistic simulation weight; dropped finds settle stably into rough terrain crevices.

### Phase 4: Surface Yard & Trophy Display
- [ ] **`020` — Build Surface Trophy Display Stands & Placement Sockets** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): Build physical display stands and shelves in the surface yard where players manually socket collected unique oddities into pre-placed spaces.
- [ ] **`021` — Add Unique Find Lore Cards & Yard Inspection** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `05_DISCOVERIES.md`): Interacting with placed trophies displays name, depth found, and 1-sentence deadpan lore card with support for rereading anytime.
- [ ] **`022` — Author Surface Worksite Props (Truck, Trailer, Generator)** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): Populate surface yard with authored props: protagonist's rusty pickup truck, utility trailer, generator, fuel hose, and floodlights within 10s of the shaft.
- [ ] **`023` — Add Surface Yard Cosmetic Milestones & Sinks** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `06_PROGRESSION_AND_ECONOMY.md`): Optional visual worksite evolutions (shelter over workbench, weather tarp over display wall, tool skins, decorative lamps).

### Phase 5: Movement, Battery & Explosives
- [ ] **`024` — Extend `FpsPlayer` Jetpack with Hover Hold & Speed Progression** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Smooth Space/bumper thrust handling. Upgrades improve ascent speed, fuel efficiency, and add hover-hold assist in narrow shafts without wall-bump collision damage.
- [ ] **`025` — Adapt `PlayerCrouch` for Low Horizontal Crawlway Shaping** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Held Ctrl lowers viewpoint and slows horizontal speed to 35%, allowing players to carve tight horizontal tunnels without stamina limits.
- [ ] **`026` — Balance Battery Drain & Adapt `ReturnWarning`** (`docs/concept/02_CORE_LOOP.md`, `06_PROGRESSION_AND_ECONOMY.md`): Link battery drain to digging strokes and flight. Adaptive Return Warning (Safe / Risky / Critical) based on depth and ascent energy required.
- [ ] **`027` — Adapt `RescueController` for Harmless Falls & Debt-Based Rescue** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Ordinary falls deal harmless landing feedback (no health bar, no battery loss). At 0 battery underground, auto-rescue to surface: keep all finds, charge depth fee + apply interest-free debt if broke.
- [ ] **`028` — Implement Consumable Sticky C4 Blasting Charges** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Placed sticky C4 charges with remote detonation. Removes large predictable voxel volume while ensuring buried finds survive intact.
- [ ] **`029` — Implement Purchasable Underground Work Lamps** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Purchasable portable work lamps placed on hole walls to illuminate deep shafts and interesting discoveries.

### Phase 6: Audio, Sensory Feel & Feedback
- [ ] **`030` — Add Material-Specific Digging Audio Loops** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Material cutting sound loops: sand hiss, clay thump, gravel rattle, rock sharp crack, concrete grinding screech. No music.
- [ ] **`031` — Add Machine Motor Whine & Strain Audio Feedback** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Engine pitch responds to tool upgrade level; motor audibly strains when biting dense rock or concrete; puff release sound on cut completion.
- [ ] **`032` — Configure Cavern Acoustic Reverb & Low-Pass Filtering** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Depth-based low-pass audio filtering and cavernous acoustic reverb that deepens as the player descends into deep shafts.
- [ ] **`033` — Tune Subterranean Daylight Falloff in Shafts** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Open sky illumination reaches down shafts and fades smoothly into deep underground gloom without pitch-black blindness.
- [ ] **`034` — Add Material Debris Particles & Cut Release Juice** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Directional soil crumbs and dust puff particle bursts on stroke completion; subtle visual settling feedback on cuts.

### Phase 7: UI, Controls & Persistence
- [ ] **`035` — Refactor `GameHudView` into Minimal Diegetic Concept HUD** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Strip HUD to the 5 essentials: real-time Depth meter (`-42.5 m`), Bag gauge with full warning color (`12 / 15`), Battery bar, adaptive Return Warning, and clean reticle.
- [ ] **`036` — Refactor `GameMenuView` for Clean Pause Navigation** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Resume, Save & Load, Settings, Exit to Title. Ensure ESC/B closes menus reliably without trapping input.
- [ ] **`037` — Extend `InputPreferences` & `ToolkitInputSettings` for Full Gamepad Parity** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`, `10_ACCESSIBILITY_AND_COMFORT.md`): Ensure 100% controller support across all gameplay, UI Toolkit menus, and trophy placement with automatic button glyph swapping.
- [ ] **`038` — Add Accessibility Comfort Settings & Directional Sound Captions** (`docs/concept/10_ACCESSIBILITY_AND_COMFORT.md`): FOV slider (55–90°), crosshair toggle, motion sickness mitigation, and directional sound captions for hearing accessibility.
- [ ] **`039` — Implement Pause Free-Look Photo Mode** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Free-look camera tool accessible from pause menu to inspect unburied finds and capture photos of the carved hole.
- [ ] **`040` — Extend `WorldSaveController` & `WorldSaveCodec` for Multi-Slot Profiles** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Versioned atomic saves persisting exact voxel deformations, placed trophy coordinates, inventory, and player state across multiple profile slots.

### Phase 8: Mystery Climax, Ending & Sandbox
- [ ] **`041` — Seed Anachronistic Mystery Trail Oddities** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Seed subtle anachronistic oddities in Zones 2 and 3 (soda cans far too deep, rustless manufactured parts) establishing curiosity.
- [ ] **`042` — Embed Zone 4 Ancient Constructed Structure** (`docs/concept/03_WORLD_AND_SITE.md`, `11_ENDING_AND_MYSTERY.md`): Embed anomalous constructed architecture at the reservoir floor (>75m) with smooth unnatural materials that differ clearly from bedrock walls.
- [ ] **`043` — Implement Ancient Material Impossibility Contact Signature** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`, `11_ENDING_AND_MYSTERY.md`): Implement ancient material contact signature: clean surgical cuts, glass-like resonance, and too-neat dust settlement without threat cues.
- [ ] **`044` — Implement Finale Components & Assembly Sockets** (`docs/concept/11_ENDING_AND_MYSTERY.md`): 3–4 required components found off the main descent shaft; owned parts automatically insert into structure sockets without inventory management.
- [ ] **`045` — Stage Final Object Presentation Cutscene** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Modern machine constructed from impossibly ancient materials revealed at the core. Presentation sequence without genre switch or loss of tools.
- [ ] **`046` — Implement Ending Excavation History Timelapse** (`docs/concept/11_ENDING_AND_MYSTERY.md`, `13_OPEN_QUESTIONS.md`): Retrospective visual replay at the finale showing the progressive evolution of the player's carved hole from untouched start to bottom.
- [ ] **`047` — Implement Post-Ending Sandbox State & Media Wall** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `11_ENDING_AND_MYSTERY.md`): Seamless transition back to surface hub with unlocked sandbox excavation. Media wall appears with regional newspaper clippings and quiet TV/radio props.
- [ ] **`048` — Wire Steam Achievements Manager to Milestones** (`docs/concept/12_ACHIEVEMENTS_AND_COMPLETION.md`): Wire 5–10 fair achievements for reaching each zone, maxing the machine, completing trophy stands, and revealing the mystery.

### Phase 9: Release Pipeline & Build Protection (Before Public Release)
- [ ] **`049` — Migrate Standalone Windows Build Pipeline to Native IL2CPP**: Switch Windows standalone build to native IL2CPP with MSVC compiler. Configure `link.xml` to prevent code-stripping on UI Toolkit and save types.
- [ ] **`050` — Integrate Release Code Obfuscation & Binary Hardening**: Integrate symbol stripping, class/method renaming, string encryption, and metadata obfuscation against reverse-engineering tools.

---

## Completed
> Format: `- [ID] Title: 1-2 sentences on what was implemented and how.`

- **[000] Baseline Transition:** Consolidated project documentation, migrated concept chapters into `docs/concept/`, established baseline prototype inventory (`docs/baseline.md`), decentralized minimal asset tracking, and established 50-task JIT roadmap.
