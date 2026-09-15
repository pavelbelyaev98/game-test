# Tasks & Status

## Status
- **Active Task:** None (Ready to start task `001`).
- **Build:** `builds/windows/SomethingDownThere.exe` (Working prototype baseline).
- **Direction:** Transitioning prototype into full vertical slice per `docs/concept/`. Focusing on transformative machine upgrades, tactile selling, 100m+ reservoir depth, and automatic material adaptation.

---

## Priority Queue
> Format: `[ID] - [Title] - [Concept Ref] - [Technical Scope]`
> Active specs live temporarily at `docs/tasks/<ID>-<name>.md` (<30 lines) and are deleted upon completion.

### Phase 1: Machine Upgrades & Surface Shop Overhaul
- [ ] **`001` — Transformative Machine Upgrades & Bolt-On Tool Rig**
  - *Concept:* `docs/concept/04_TOOL_AND_MOVEMENT.md`, `06_PROGRESSION_AND_ECONOMY.md`
  - *Files:* `EquipmentProgression.cs`, `ShovelState.cs`, `TerrainVolume.cs`, `ExcavatorView.cs`.
  - *Scope:* Overhaul `EquipmentProgression` to replace tiny +5% increments with 4 powerful tiers: Tier 1 (Base Scoop), Tier 2 (Motorized Teeth: 2× speed, 50% wider bite), Tier 3 (Wide Rotating Cutter: massive chunk clearance), Tier 4 (Blast Cannon Attachment). Add visual bolt-on attachments to the tool view model.

- [ ] **`002` — Janky Sell-All Dumpster Machine**
  - *Concept:* `docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`
  - *Files:* `SellStation.cs`, `StationTrade.cs`, `SessionInventory.cs`, `SessionWallet.cs`.
  - *Scope:* Replace the boring sell menu with a physical surface hopper/dumpster machine. One-press "Dump All" interaction: mechanical crunch audio, digital credit readout, instant cash payout (`$`), automatically keeping unique exhibit finds.

- [ ] **`003` — Visual Upgrade Workbench UI**
  - *Concept:* `docs/concept/06_PROGRESSION_AND_ECONOMY.md`, `07_SURFACE_HUB_AND_DISPLAY.md`
  - *Files:* `EquipmentProgression.cs`, `UpgradeStation.cs`, `GameMenuView.cs`, `UI/Toolkit/`.
  - *Scope:* Overhaul the upgrade bench in UI Toolkit. Display 3D tool preview with bolt-on progression, transparent "Current → Next" stat comparisons across tracks (Tool, Battery, Jetpack, Bag, Detector, C4), and punchy purchase feedback.

### Phase 2: Reservoir Depth, Zones & Ground Feel
- [ ] **`004` — Expand Reservoir Depth (100m+) & 4 Proportional Zones**
  - *Concept:* `docs/concept/03_WORLD_AND_SITE.md`
  - *Files:* `TerrainVolume.cs`, `ExcavationGrid.cs`, `PermanentTerrainBoundary.cs`.
  - *Scope:* Expand `TerrainVolume` to 100m+ depth with contained lateral footprint. Partition into 4 proportional depth zones: Zone 1 Recent Fill (~0–25m), Zone 2 Old Sediment (~25–50m), Zone 3 Deep Clay & Stone (~50–75m), Zone 4 Ancient Constructed (>75m). Add industrial boundary visuals (cracked concrete dam walls, intake tower, bedrock floor).

- [ ] **`005` — Multi-Material Ground & Automatic Tool Adaptation**
  - *Concept:* `docs/concept/03_WORLD_AND_SITE.md`, `04_TOOL_AND_MOVEMENT.md`
  - *Files:* `ExcavationGrid.cs`, `TerrainVolume.cs`, `FpsPlayer.cs`.
  - *Scope:* Store material IDs in voxels (Soil, Clay, Gravel, Rock, Diggable Concrete). Machine queries targeted material and automatically adapts bite speed, motor strain sound, and visual resistance without manual mode switching.

- [ ] **`006` — Seam Cleaving Mechanics**
  - *Concept:* `docs/concept/03_WORLD_AND_SITE.md`, `14_PROTOTYPE_PLAN.md`
  - *Files:* `TerrainVolume.cs`, `ExcavationGrid.cs`.
  - *Scope:* Detect cuts along natural material density seams. Trigger crack audio → physical slab shift → fracture break to efficiently clear large sections.

- [ ] **`007` — Hard Pockets & Geological Obstructions**
  - *Concept:* `docs/concept/03_WORLD_AND_SITE.md`
  - *Files:* `TerrainVolume.cs`, `DiscoveryField.cs`.
  - *Scope:* Scatter 5–8 authored hard obstacles (concrete plugs, boulder clusters) that resist early tools but offer multiple solutions (upgrades, C4, routing).

### Phase 3: Discoveries, Silhouette Reveal & Detection
- [ ] **`008` — Rebalance Discovery Roster & Tier Classification**
  - *Concept:* `docs/concept/05_DISCOVERIES.md`
  - *Files:* `DiscoveryCatalog.cs`, `DiscoveryField.cs`, `catalog.json`.
  - *Scope:* Replace the 1,024 uniform mineral grind with ~80–120 authored finds across the 4 depth zones, classified strictly into Commons (sellable junk/minerals), Distinctives (repeatable high-value), and Uniques (exactly 1 per save, unsellable).

- [ ] **`009` — Vertical Slice 5 Signature Finds & Silhouette Reveal**
  - *Concept:* `docs/concept/05_DISCOVERIES.md`, `14_PROTOTYPE_PLAN.md`
  - *Files:* `BuriedFind.cs`, `DiscoveryField.cs`, `art/starter-finds/`.
  - *Scope:* Integrate the 5 key slice objects: *washing machine, hand drill, gearbox, mammoth bone, gramophone*. Calibrate 50–70% exposure thresholds so players recognize silhouettes before full extraction.

- [ ] **`010` — Buried Find Clusters & Containers**
  - *Concept:* `docs/concept/05_DISCOVERIES.md`
  - *Files:* `DiscoveryField.cs`, `BuriedFind.cs`.
  - *Scope:* Implement clustered spawns (workshop scene, bone bed, camp relics) and buried containers (suitcases/boxes) cracked open with the tool in the world.

- [ ] **`011` — Tool-Mounted Silent Proximity Detector**
  - *Concept:* `docs/concept/05_DISCOVERIES.md`
  - *Files:* `FpsPlayer.cs`, `GameHudView.cs`, `DiscoveryField.cs`.
  - *Scope:* Directional visual pulse cue (tool LED array or subtle edge glow) indicating distance and broad direction to the nearest uncollected distinctive/unique find. No audio beeps, radar maps, or value spoilers.

### Phase 4: Surface Yard, HUD & Core Tension
- [ ] **`012` — Surface Trophy Exhibit Stands & Lore Cards**
  - *Concept:* `docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`, `05_DISCOVERIES.md`
  - *Files:* `BuriedFind.cs`, `FpsPlayer.cs`, surface yard scene.
  - *Scope:* Build physical pedestals and shelves in the yard where players place unique oddities and interact to inspect 1-sentence deadpan lore cards.

- [ ] **`013` — Surface Yard Props & Worksite Atmosphere**
  - *Concept:* `docs/concept/07_SURFACE_HUB_AND_DISPLAY.md`
  - *Files:* Surface yard scene layout.
  - *Scope:* Populate surface yard with authored props: protagonist's rusty pickup, utility trailer, generator, fuel hose, and floodlights.

- [ ] **`014` — Minimal Diegetic HUD Overhaul**
  - *Concept:* `docs/concept/08_INTERFACE_AND_CONTROLS.md`
  - *Files:* `GameHudView.cs`, `GameHud.uxml`, `Hud.uss`.
  - *Scope:* Strip HUD to the 5 essentials: real-time Depth meter (`-42.5 m`), Bag gauge with full warning color (`12 / 15`), Battery bar, adaptive Return Warning, and clean reticle.

- [ ] **`015` — Battery Drain Balance & Forgiving Rescue Debt**
  - *Concept:* `docs/concept/02_CORE_LOOP.md`, `06_PROGRESSION_AND_ECONOMY.md`
  - *Files:* `Battery.cs`, `FpsPlayer.cs`, `ReturnWarning.cs`, `RescueController.cs`.
  - *Scope:* Balance battery consumption between digging strokes and jetpack flight. At 0 battery underground, auto-rescue to surface: keep all finds, deduct depth fee + apply interest-free debt if wallet is empty so next outing fuel is guaranteed.

- [ ] **`016` — Consumable C4 Blasting Charges**
  - *Concept:* `docs/concept/04_TOOL_AND_MOVEMENT.md`
  - *Files:* `FpsPlayer.cs`, `FpsInput.cs`, `TerrainVolume.cs`.
  - *Scope:* Placed sticky C4 charges with remote detonation. Removes a large predictable voxel volume while ensuring buried finds survive intact.

### Phase 5: Sensory Feel & Controls
- [ ] **`017` — Material Digging Audio, Motor Strain & Particle Juice**
  - *Concept:* `docs/concept/09_FEEL_ART_AND_AUDIO.md`
  - *Files:* `TerrainVolume.cs`, `FpsPlayer.cs`.
  - *Scope:* Material-specific cutting audio (soil crunch, clay squelch, rock ping/sparks), engine motor strain audio under resistance, and directional soil puff particles.

- [ ] **`018` — Subterranean Daylight Falloff & Cavern Acoustics**
  - *Concept:* `docs/concept/03_WORLD_AND_SITE.md`, `09_FEEL_ART_AND_AUDIO.md`
  - *Files:* `ExcavationDaylight.cs`, audio mixer snapshots.
  - *Scope:* Smooth ambient sunlight attenuation into underground gloom; low-pass audio filtering and cavernous reverb that deepens with shaft depth.

- [ ] **`019` — Full Controller Parity & Input Remapping**
  - *Concept:* `docs/concept/08_INTERFACE_AND_CONTROLS.md`, `10_ACCESSIBILITY_AND_COMFORT.md`
  - *Files:* `InputPreferences.cs`, `ToolkitInputSettings.cs`, `GameMenuView.cs`.
  - *Scope:* Ensure 100% gamepad support for all gameplay, UI Toolkit menus, and trophy placement with automatic glyph swapping.

### Phase 6: Mystery Climax & Achievements
- [ ] **`020` — Zone 4 Anomalous Structure & Deep Mystery**
  - *Concept:* `docs/concept/11_ENDING_AND_MYSTERY.md`
  - *Files:* `TerrainVolume.cs`, `DiscoveryField.cs`.
  - *Scope:* Embed anomalous ancient constructed geometry at the reservoir floor (>75m). Add mystery trail oddities connecting to it.

- [ ] **`021` — Ending Sequence & Post-Ending Continue Sandbox**
  - *Concept:* `docs/concept/11_ENDING_AND_MYSTERY.md`
  - *Files:* `MainGameRoot.cs`, `WorldSaveController.cs`.
  - *Scope:* Implement socket trigger for final components, climactic mystery reveal, and seamless transition to the "Continue Playing" sandbox loop with a commemorative yard trophy.

- [ ] **`022` — Steam Achievements Integration**
  - *Concept:* `docs/concept/12_ACHIEVEMENTS_AND_COMPLETION.md`
  - *Files:* `Runtime/Persistence/`, Steamworks wrapper / achievement manager.
  - *Scope:* Wire 5–10 fair achievements for reaching each zone, maxing the machine, completing trophy stands, and revealing the mystery.

### Phase 7: Release Pipeline & Build Protection (Before Public Release)
- [ ] **`023` — IL2CPP Scripting Backend Migration**
  - *Concept:* Runtime architecture & binary security.
  - *Files:* `tools/build-windows.ps1`, `ProjectSettings/ProjectSettings.asset`, `Assets/Editor/WindowsBuild.cs`.
  - *Scope:* Switch Windows standalone build to native IL2CPP with MSVC compiler. Configure `link.xml` to prevent code-stripping on UI Toolkit and save types. Verify warning-free compilation.

- [ ] **`024` — Release Code Obfuscation & Binary Hardening**
  - *Concept:* Anti-reverse engineering & copy protection.
  - *Files:* Build pipeline scripts, Editor post-processors.
  - *Scope:* Integrate symbol stripping, class/method renaming, string encryption, and metadata obfuscation against reverse-engineering tools (ILSpy, dnSpy, Il2CppDumper).

---

## Completed
> Format: `- [ID] Title: 1-2 sentences on what was implemented and how.`

- **[000] Baseline Transition:** Consolidated project documentation, migrated concept chapters (00–15) into `docs/concept/`, established baseline prototype inventory (`docs/baseline.md`), and decentralized minimal asset tracking.

---

## Historical Reference (Legacy Prototype Work)
<details>
<summary>Completed prior to new concept reset (click to expand)</summary>

- `01-04` Foundation, URP setup, Input System.
- `05` FPS movement, camera look, rebindable controls.
- `06, 20, 21, 24, 26, 29, 31, 34` Voxel density field, surface-nets meshing (0.125 m), scoop digging, cut variation, detached soil cleanup.
- `12, 13, 46, 48, 98, 138, 139, 141-143` Surface yard, Sell/Upgrade stations, battery (100–400), bag (10–40 slots), paid refills.
- `16, 66, 67, 86, 87` Jetpack thrust/hover, crouch (Left Ctrl), sprint (Left Shift), auto-rescue.
- `27, 28, 30, 72, 110, 112, 114-116, 131, 136, 140, 145` Aim-assisted reveal, 60% exposure pickup, physical lift/drop/throw, 928 minerals, 96 rocks.
- `32, 33, 64, 65, 74, 75, 78, 79, 107, 122-125` UI Toolkit HUD, pause/settings/trade menus, grayscale design, FOV sliders.
- `35, 63` Atomic versioned whole-world saving (`WorldSaveController`), single-instance process reservation.
- `77, 105, 127-129, 135` Triplanar soil/turf shader, procedural wind grass clumps, sun disc.
</details>
