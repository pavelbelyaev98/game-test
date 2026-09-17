# Roadmap & Tasks

## Status
- **Active Task:** None (Ready for `001`)
- **Build:** `builds/windows/SomethingDownThere.exe` (Working prototype baseline)
- **Direction:** Implementing the vertical slice per `docs/concept/`. Starting with the transformative machine, the janky Sell-All machine & salvage winch, the visual workbench, and 100m+ reservoir depth.

---

## Priority Queue
> **Workflow:** When starting a task, create a thorough spec at `docs/tasks/<ID>-<slug>.md` (Objective, live code analysis, architecture changes, edge cases, Acceptance Criteria). On completion: move spec to `docs/tasks/completed/`, update `docs/baseline.md` if baseline systems changed, and mark `[x]` here.
> **Note:** Tasks labelled *Upgrade/Extend/Refactor* modify existing working systems — inspect `docs/baseline.md` and `docs/architecture.md` first, never rebuild from scratch.

### Phase 1: Machine Upgrades & Surface Shop
- [ ] **`001` — Build the Evolving Motorized Tool Rig & 4-Tier Progression** (NEW) (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): The normal game currently has no visible tool (only the admin-only experiment). Build the improvised motor-assisted first-person machine with bolt-on attachments, and refactor `ShovelState`/`EquipmentProgression` from 6 levels to 4 transformative tiers (bite volume, speed, cutting power).
- [ ] **`002` — Upgrade the Sell Station into a Janky Sell-All Machine & Add the Salvage Winch** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Upgrade the existing `SellStation` (which already supports Sell All) into a physical hopper with a lever, grinding audio, digital readout, and add the new surface winch that hauls unburied oversized set pieces up the shaft (dynamically carving dirt bottlenecks).
- [x] **`003` — One-Click Workbench & Sell Machine Table** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): fixed-size parts-board table — upgrades and services in separate columns (upgrades wider), rows are decoration and only the price/payout button is clickable and one click buys; no confirm step, no icons, no resizing.
- [ ] **`004` — Upgrade `SurfaceRecharge` into a Dedicated Yard Fuel Dispenser** (`docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`): Refill logic already exists (`UpgradeStation`/`StationTrade`). Build a dedicated surface dispenser with transparent full/partial refill pricing, whole-dollar `$`, and preserving current fuel when tank capacity upgrades.

### Phase 2: Reservoir Depth, Zones & Ground Feel
- [ ] **`005` — Extend `TerrainVolume` to 100m+ Depth & Reservoir Boundaries** (`docs/concept/03_WORLD_AND_SITE.md`): Scale the existing 32m volume to 100m+ depth with a contained lateral footprint. Build industrial boundaries (cracked concrete dam walls, intake tower silhouette, bedrock floor).
- [ ] **`006` — Partition the Voxel Grid into 4 Geological Depth Zones** (`docs/concept/03_WORLD_AND_SITE.md`): Partition vertical depth proportionally into 4 zones: Zone 1 Recent Fill (0–25m), Zone 2 Old Sediment (25–50m), Zone 3 Deep Clay & Stone (50–75m), Zone 4 Ancient Constructed (>75m).
- [ ] **`007` — Extend `ExcavationGrid` with Material IDs & Tool Auto-Adaptation** (`docs/concept/03_WORLD_AND_SITE.md`, `04_TOOL_AND_MOVEMENT.md`): Assign material IDs to voxels (Soil, Clay, Gravel, Rock, Diggable Concrete). Tool automatically adapts bite speed, sound, and resistance without manual mode switching.
- [ ] **`008` — Update Chunk Meshing & Triplanar Shader for Multi-Material Ground** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Chunk meshing passes material weights/indices into vertex data/shaders to render distinct triplanar textures and normal relief for Soil, Clay, Gravel, Rock, and Concrete.
- [ ] **`009` — Implement Seam Cleaving as a Signature Action** (`docs/concept/03_WORLD_AND_SITE.md`, `14_PROTOTYPE_PLAN.md`): Broad cuts along natural density/material seams trigger crack audio → physical slab shift → fracture break to clear bounded sections efficiently, exposing multiple items at once.
- [ ] **`010` — Author Hard Pockets & Obstructions** (`docs/concept/03_WORLD_AND_SITE.md`): Seed 5–8 authored hard obstacles (concrete plugs, boulder clusters) that resist early tools but offer multiple solutions (upgrades, C4, routing).

### Phase 3: Discoveries, Clusters & Detection
- [ ] **`011` — Rebalance `DiscoveryCatalog` & Quotas into 3 Tiers** (`docs/concept/05_DISCOVERIES.md`): The existing catalog holds 928 minerals + 96 rocks. Rebalance into ~80–120 authored finds across the 4 zones: Commons (sellable junk/ore), Distinctives (repeatable high-value), Uniques (1 per save, unsellable, 0 bag slots).
- [ ] **`012` — Extend Deliberate Exposure to All Finds & Add 5 Signature Slice Finds** (`docs/concept/05_DISCOVERIES.md`, `14_PROTOTYPE_PLAN.md`): Exposure already exists at 60%. Extend so every find (including rubbish) must be deliberately uncovered before collection — no vacuum auto-collect. Add the 5 signature slice finds (*washing machine, drill, gearbox, mammoth bone, gramophone*).
- [ ] **`013` — Implement Buried Connections ("Follow the Thing") & Clusters** (`docs/concept/03_WORLD_AND_SITE.md`, `05_DISCOVERIES.md`): Seed physical connectors (heavy cables, rusted chains, matching floor tiles, pipes) leading through the soil to coherent buried scenes, giving lateral exploration an immediate visual reason.
- [ ] **`014` — Implement Crackable Buried Containers** (`docs/concept/05_DISCOVERIES.md`): Add buried containers (suitcases, toolboxes) cracked open with the machine in the world to reveal nested discoveries.
- [ ] **`015` — Implement Tool-Mounted Silent Visual Detector** (`docs/concept/05_DISCOVERIES.md`): Directional visual pulse cue on the machine indicating distance and broad direction to nearest uncollected distinctive/unique. Target locking prevents flickering; fully exposed items yield priority; multi-sensory accessibility audio toggle available.
- [ ] **`016` — Add Subtle Hover Price Tag to Exposed Finds** (`docs/concept/05_DISCOVERIES.md`, `13_OPEN_QUESTIONS.md`): Display subtle fixed sale price tag when reticle hovers over an exposed, collectible sellable find.
- [ ] **`017` — Whole-Object Salvage Surface Winch Extraction & Handling** (`docs/concept/05_DISCOVERIES.md`): Uncovering an oversized object flags it for salvage. At the surface yard, operating the rim winch deploys a heavy cable down the shaft that locks onto the object and hauls it up while carving away dirt bottlenecks, landing on the salvage pad for payout. Refine physical RMB lift/drop and LMB throw.

### Phase 4: Surface Yard & Trophy Display
- [ ] **`018` — Build Surface Trophy Display Stands & Salvage Records** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): Build physical display stands where players manually socket unique oddities and display miniatures/photos of whole-salvage set pieces.
- [ ] **`019` — Add Unique Find Lore Cards & Yard Inspection** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `05_DISCOVERIES.md`): Interacting with placed trophies displays name, depth found, and 1-sentence deadpan lore card with support for rereading anytime.
- [ ] **`020` — Extend the Existing Surface Yard with Authored Props** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`): The yard exists with Sell/Upgrade/Fuel stations. Add authored props: rusty pickup truck, utility trailer, generator, fuel hose, and floodlights within 10s of the shaft.
- [ ] **`021` — Add Surface Yard Cosmetic Milestones & Sinks** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `06_PROGRESSION_AND_ECONOMY.md`): Optional visual worksite evolutions (shelter over workbench, weather tarp over display wall, tool skins, decorative lamps).

### Phase 5: Movement, Battery & Explosives
- [ ] **`022` — Extend the Existing Jetpack with Hover Hold & Speed Progression** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): Jetpack thrust/hover already exists. Extend with upgrade-driven ascent speed, fuel efficiency, and hover-hold assist in narrow shafts without wall-bump collision damage.
- [ ] **`023` — Add Footprint Narrowing to the Existing Precision Crouch** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Crouch at 35% speed already works (`PlayerCrouch`). Add bite-footprint narrowing while crouched for delicate carving around silhouettes.
- [ ] **`024` — Upgrade `ReturnWarning` to a Depth-Aware Estimate** (`docs/concept/02_CORE_LOOP.md`, `06_PROGRESSION_AND_ECONOMY.md`): The warning currently uses charge fractions. Upgrade it to estimate return energy from depth and ascent route, while preserving full player upgrade freedom. Rebalance dig vs. flight battery drain.
- [ ] **`025` — Refactor `RescueController` to Keep Loot & Add Debt Recovery** (`docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`): The current controller deletes carried items on rescue, which contradicts the concept. Refactor so recovery keeps all finds, charges a depth-scaled fee, and applies interest-free debt when broke. Ordinary falls stay harmless.
- [ ] **`026` — Implement Consumable Sticky C4 Charges** (`docs/concept/04_TOOL_AND_MOVEMENT.md`): Placed sticky C4 charges with remote detonation. Removes a large predictable voxel volume while ensuring buried finds survive intact.
- [ ] **`027` — Implement World Marking & Placeable Work Lamps** (`docs/concept/03_WORLD_AND_SITE.md`, `04_TOOL_AND_MOVEMENT.md`): Add free world-space markings via the tool (arrow, home, return-here) for lateral branches. Placeable lamps illuminate deep shafts; the vertical daylight shaft remains the natural landmark (no map, ever).

### Phase 6: Audio, Sensory Feel & Feedback
- [ ] **`028` — Add Material-Specific Digging Audio Loops** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Material cutting sound loops: sand hiss, clay thump, gravel rattle, rock sharp crack, concrete grinding screech. No music.
- [ ] **`029` — Add Machine Motor Whine & Strain Audio Feedback** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Engine pitch responds to tool upgrade level; motor audibly strains when biting dense rock or concrete; puff release sound on cut completion.
- [ ] **`030` — Configure Cavern Acoustic Reverb & Low-Pass Filtering** (`docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`): Depth-based low-pass audio filtering and cavernous acoustic reverb that deepens as the player descends into deep shafts.
- [ ] **`031` — Add Material Debris Particles & Cut Release Juice** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`): Directional soil crumbs and dust puff particle bursts on stroke completion; subtle visual settling feedback on cuts.

### Phase 7: UI, Controls & Persistence
- [ ] **`032` — Refactor `GameHudView` into the Minimal Diegetic HUD** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): The existing HUD has legacy labels. Refactor to the 5 essentials: real-time Depth meter, Bag gauge with full warning color, Battery bar, adaptive Return Warning estimate, and clean reticle.
- [ ] **`033` — Refactor Pause Menu Navigation** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Pause/settings menus already exist. Refactor to Resume, Save & Load, Settings, Exit to Title; ESC/B closes reliably without trapping input.
- [ ] **`034` — Add Gamepad Binding Support to `InputPreferences`** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`, `10_ACCESSIBILITY_AND_COMFORT.md`): The binding system currently supports keyboard + mouse only. Add full controller binding, UI Toolkit navigation, and automatic button glyph swapping.
- [ ] **`035` — Add Directional Sound Captions** (`docs/concept/10_ACCESSIBILITY_AND_COMFORT.md`): FOV slider and crosshair toggle already exist. Add directional sound captions for hearing accessibility.
- [ ] **`036` — Implement Pause Free-Look Photo Mode** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Free-look camera tool accessible from the pause menu to inspect unburied finds and photograph the carved hole.
- [ ] **`037` — Extend `WorldSaveController` to Multi-Slot Profiles** (`docs/concept/08_INTERFACE_AND_CONTROLS.md`): Versioned atomic saves already exist. Extend them to support multiple profile slots while persisting exact voxel deformations, placed trophies, inventory, and player state.

### Phase 8: Mystery Climax, Ending & Sandbox
- [ ] **`038` — Seed Anachronistic Mystery Trail Oddities with Visible Impossibilities** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Seed subtle anachronistic oddities with visible impossibility (modern objects fused into manufactured ancient stone, matching unusual modular connectors) establishing curiosity.
- [ ] **`039` — Embed Zone 4 Ancient Constructed Structure** (`docs/concept/03_WORLD_AND_SITE.md`, `11_ENDING_AND_MYSTERY.md`): Embed anomalous constructed architecture at the reservoir floor (>75m) with smooth unnatural materials that differ clearly from bedrock walls.
- [ ] **`040` — Implement Ancient Material Impossibility Contact Signature** (`docs/concept/09_FEEL_ART_AND_AUDIO.md`, `11_ENDING_AND_MYSTERY.md`): Implement ancient material contact signature: clean surgical cuts, glass-like resonance, and too-neat dust settlement without threat cues.
- [ ] **`041` — Implement Finale Components & Assembly Sockets** (`docs/concept/11_ENDING_AND_MYSTERY.md`): 3–4 required components in constrained regions with recoverable physical leads (cables/pipes); owned parts automatically insert into structure sockets without inventory management.
- [ ] **`042` — Stage Final Object Presentation Cutscene** (`docs/concept/01_FANTASY_AND_TONE.md`, `11_ENDING_AND_MYSTERY.md`): Reveal the ancient junk fabrication machine. Presentation sequence using normal excavation tools without genre switch or loss of tools.
- [ ] **`043` — Implement Ending Excavation History Timelapse** (`docs/concept/11_ENDING_AND_MYSTERY.md`, `13_OPEN_QUESTIONS.md`): Retrospective visual replay at the finale showing the progressive evolution of the player's carved hole from untouched start to bottom.
- [ ] **`044` — Implement Post-Ending Sandbox State & Media Wall** (`docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `11_ENDING_AND_MYSTERY.md`): Seamless transition back to the surface hub with unlocked sandbox excavation. Media wall appears with regional newspaper clippings and quiet TV/radio props.
- [ ] **`045` — Wire Steam Achievements Manager to Milestones** (`docs/concept/12_ACHIEVEMENTS_AND_COMPLETION.md`): Wire 5–10 fair achievements for reaching each zone, maxing the machine, completing trophy stands, and revealing the mystery.

### Phase 9: Release Pipeline & Build Protection (Before Public Release)
- [ ] **`046` — Migrate Standalone Windows Build Pipeline to Native IL2CPP**: Switch the Windows standalone build to native IL2CPP with the MSVC compiler. Configure `link.xml` to prevent code-stripping on UI Toolkit and save types.
- [ ] **`047` — Integrate Release Code Obfuscation & Binary Hardening**: Integrate symbol stripping, class/method renaming, string encryption, and metadata obfuscation against reverse-engineering tools.

---

## Completed
> Format: `- [ID] Title: 1-2 sentences on what was implemented and how.`

- **[003] One-Click Workbench & Sell Machine Table:** rebuilt both station menus as one fixed-size table (`Station.uss` + `ToolkitStationRows`) with a money-only header, category columns, one-click rows carrying price plus the changed value, and tooltips for secondary stats; added `Add $500` to Developer admin and rebuilt the Windows build.
- **[000] Baseline Transition:** Consolidated project documentation, migrated concept chapters into `docs/concept/`, established baseline prototype inventory (`docs/baseline.md`), decentralized minimal asset tracking, and established the roadmap.

### Dropped as Already Implemented (pre-existing prototype)
- **Hold-to-Dig & Toggle:** Fully implemented in `FpsInput.cs`/`InputPreferences.cs` (hold default, toggle persisted).
- **Terrain Crumb Cleanup:** Fully implemented in `ExcavationGrid.cs` (`RemoveDetachedSoil`, `RemoveTinyRemnants`).
- **Subterranean Daylight Falloff:** Fully implemented in `ExcavationDaylight.cs` (smooth sky-light falloff, 0.45 ambient floor).
