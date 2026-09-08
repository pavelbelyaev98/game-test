# Discovery reveal and collection

Status: Task `28` is complete: left-click visible small finds and simplified HUD text. Task `09` retains final discovery-art acceptance.

Idea coverage: sections 19-22 and relevant tuning in section 53.

## Purpose

Turn excavation into readable discovery moments without tedious final cleaning or collection through covering soil.

## Task 09 - production discovery collection

- Add the reusable production exposure/collection system and a coherent starter set of ordinary discovery assets to [the main game scene](core-loop.md), using terrain occupancy and inventory records. Visuals must come from Blender MCP or free-to-use external sources licensed for commercial use; no primitive stand-ins, randomized generation, or manual exposed flag.
- Acceptance: small finds collect as soon as actual geometry is visible; large finds retain a tunable 80% exposure requirement. Occlusion prevents collection through soil. One left-click adds one ID exactly once; a full inventory leaves the world item intact, and collected records remain absent across surface trips.
- Evidence: controlled excavation/exposure, occlusion, capacity and repeated-interaction integration checks; expose only position/eligibility to the future detector.
- Next: Task `10`. Generated placement and disk persistence remain later work; the implemented collection path and starter assets are production content.

## Later expansion

Task `27` replaces validation-only exposure flags with reusable terrain-driven eligibility. Final authored assets, richer content pools and disk persistence remain later work.

## Task 27 - buried finds and admin X-ray

- Integrate 96 seeded buried finds into the main development game, with a concentration of shallow finds near the entrance. Use the small colored spheres/discs explicitly requested by the user; this is a scoped exception to the default prohibition on primitive presentation. Keep the reusable placement, exposure and collection systems when replacing their art.
- Terrain edits (including detached-soil cleanup and ground reset) update exposure; Task `28` limits the 80% rule to large finds. Collection requires actual line of sight and a separate 3 m reach; X-ray never bypasses soil, capacity or eligibility.
- Each find has one session identity. Successful collection removes its visual/collider and displays its name and carried count. Full inventory leaves it in place. Surface trips and ground resets preserve collected identities; reset reburies uncollected finds.
- Developer admin adds an X-ray button and Ctrl+Shift+X: Task `28` keeps only the buried-position markers. Default off, retained across menu/ground reset, disabled by Restore normal rules and unavailable in release builds. Plain/held keys, focus and other menus remain barriers.
- Raise shovel reaches to 3.0 / 3.2 / 3.4 / 3.6 / 3.8 / 4.0 m without changing strength/cadence. Acceptance: seeded placement without overlaps/boundary breaches, reveal/occlusion/collection/capacity/reset checks, real X-ray input/button gating, six actual shovel reaches, visual inspection and the same Windows executable.
- Production TODO (`09`/`22`): replace the three simple find prefabs with approved final art and turn off the field's development-content restriction only after production acceptance. No disposable practice scene or launcher.

## Required behavior

- Covered finds cannot be named or collected through terrain.
- Cheap finds collect quickly with clear identity feedback; the player should notice what an upgraded tool uncovered.
- Small finds need only a visible part, with no percentage threshold; collider visibility is authoritative even between exposure samples.
- Distinctive finds remain visible while their shape becomes recognizable, then require deliberate collection.
- Large finds start with a forgiving threshold around 70-85%; never require the final hidden speck, precision brushing, washing, or a cleaning minigame.
- Normal objects identify immediately without experts, analysis timers, mailing, or per-item bureaucracy.
- Full inventory leaves the find intact and available.

## Done when

- Exposure transitions are stable as nearby terrain changes.
- One left-click collects one eligible find exactly once.
- Occlusion, capacity, and persistence integration checks pass.

## Task 28 - visible small finds and quiet HUD

- `FindSize` is authored per prefab: current marble/token/bead are Small; only Large uses the exposure threshold and "Uncover more". Left-click targets a visible find before digging; E remains for stations. A held dig does not auto-collect, and a pickup press cannot dig the ground behind the removed object until released.
- X-ray shows markers without a readout, tutorial sentence or toggle popup. Remove first-use HUD hints, dig-button prompts and out-of-range coaching; remove the pause-menu free-jump/shared-battery paragraph. Keep item identity, collection confirmation, full-inventory feedback, large-object exposure and the pause control reference.
- Acceptance: partial and unsampled-sliver pickup, large-find threshold, E exclusion, real LMB press/hold/release, no pickup battery charge/double action, preserved range/occlusion/capacity/reset, inspected clean HUD/X-ray/pause and updated Windows build.
