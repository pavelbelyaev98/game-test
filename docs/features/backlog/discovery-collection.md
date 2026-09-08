# Discovery reveal and collection

Current collection: Task `30` supplies larger finds and held collection; Task `31` lowers required exposure to 50%. Task `09` retains final discovery-art acceptance; see the [queue](../../development/tasks.md).

Idea coverage: sections 19-22 and relevant tuning in section 53.

## Purpose

Turn excavation into readable discovery moments without tedious final cleaning or collection through covering soil.

## Task 09 - production discovery collection

- Add the reusable production exposure/collection system and a coherent starter set of ordinary discovery assets to [the main game scene](core-loop.md), using terrain occupancy and inventory records. Visuals must come from Blender MCP or free-to-use external sources licensed for commercial use; no primitive stand-ins, randomized generation, or manual exposed flag.
- Acceptance: finds require their authored exposure threshold and actual visibility. Holding left-click collects the aimed eligible find exactly once without releasing between digging and pickup; full inventory leaves ordinary loot intact. Occlusion/range prevent collection through soil, and collected records remain absent across surface trips.
- Evidence: controlled excavation/exposure, occlusion, capacity and repeated-interaction integration checks; expose only position/eligibility to the future detector.
- Next: Task `10`. Generated placement and disk persistence remain later work; the implemented collection path and starter assets are production content.

## Later expansion

Task `27` replaces validation-only exposure flags with reusable terrain-driven eligibility. Final authored assets, richer content pools and disk persistence remain later work.

## Task 27 - buried finds and admin X-ray

- Integrate 96 seeded buried finds into the main development game, with a concentration of shallow finds near the entrance. Use the small colored spheres/discs explicitly requested by the user; this is a scoped exception to the default prohibition on primitive presentation. Keep the reusable placement, exposure and collection systems when replacing their art.
- Terrain edits (including detached-soil cleanup and ground reset) update exposure; Task `30` supplies the current threshold rules. Collection requires actual line of sight and a separate 3 m reach; X-ray never bypasses soil, capacity or eligibility.
- Each find has one session identity. Successful collection removes its visual/collider and displays its name and carried count. Full inventory leaves it in place. Surface trips and ground resets preserve collected identities; reset reburies uncollected finds.
- Developer admin adds an X-ray button and Ctrl+Shift+X: Task `28` keeps only the buried-position markers. Default off, retained across menu/ground reset, disabled by Restore normal rules and unavailable in release builds. Plain/held keys, focus and other menus remain barriers.
- Raise shovel reaches to 3.0 / 3.2 / 3.4 / 3.6 / 3.8 / 4.0 m without changing strength/cadence. Acceptance: seeded placement without overlaps/boundary breaches, reveal/occlusion/collection/capacity/reset checks, real X-ray input/button gating, six actual shovel reaches, visual inspection and the same Windows executable.
- Production TODO (`09`/`22`): replace the three simple find prefabs with approved final art and turn off the field's development-content restriction only after production acceptance. No disposable practice scene or launcher.

## Required behavior

- Covered finds cannot be named or collected through terrain.
- Cheap finds collect quickly with clear identity feedback; the player should notice what an upgraded tool uncovered.
- Every find needs a recognizable amount uncovered before collection; use the authored threshold plus actual collider visibility. The current set requires 50% of its sampled surface.
- Finds remain visible while their shape becomes recognizable; aimed held collection keeps digging fluid. Never require the final hidden speck, precision brushing, washing, or a cleaning minigame.
- Normal objects identify immediately without experts, analysis timers, mailing, or per-item bureaucracy.
- Full inventory leaves ordinary loot intact and available. [Permanent passive discoveries (`36`)](buried-upgrades.md) use the same reveal/aim rules but grant an upgrade outside the ordinary bag and sale flow.

## Done when

- Exposure transitions are stable as nearby terrain changes.
- Each eligible aimed find transfers exactly once while left-click is held; pickup costs no energy and cannot also dig in the same frame.
- Occlusion, capacity, and persistence integration checks pass.

## Task 28 - visible small finds and quiet HUD

- Historical input/exposure rules are superseded by Task `30`. Keep E for stations, separate collection reach, identity feedback and the quiet HUD.
- X-ray shows markers without a readout, tutorial sentence or toggle popup. Remove first-use HUD hints, dig-button prompts and out-of-range coaching; remove the pause-menu free-jump/shared-battery paragraph. Keep item identity, collection confirmation, full-inventory feedback, large-object exposure and the pause control reference.
- Evidence of the previous behavior remains in the completion record; current collection acceptance is below.

## Task 30 - larger finds and held collection

- Double the dimensions of the three existing approved prefabs: marble 0.8 m diameter; token 1.0 x 0.18 x 1.0 m; bead 0.64 x 0.90 x 0.64 m. Preserve their meshes/materials/metas, values, identities and count. Increase placement clearance and cover to keep all 96 initially buried and separated.
- Each current prefab requires 50% exposure after Task `31` (originally 60%), tunable per prefab. Show current/required exposure only while aiming at a visible, ineligible find; concealed finds reveal no name. Dig the remaining covering soil around the find.
- A held primary action checks collection before digging, including during the shovel cooldown. One action per frame; a pickup has a short recovery before continued held digging/collection. Full inventory feedback does not repeat every frame. Menus/focus still require releasing held input after resume.
- Acceptance: real excavation crosses the threshold before pickup, no visible-sliver bypass, sustained device-input dig-to-pickup-to-dig, no duplicate identity or pickup charge, no through-soil/range/full-inventory bypass, pause/focus safety, reset/placement clearance, live presentation and Windows build. No new art/audio.
