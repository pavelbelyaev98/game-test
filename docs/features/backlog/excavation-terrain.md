# Excavation and terrain

Status: Task `29` is complete: irregular shovel bites and no excavation-volume popup. Detached-soil cleanup remains; `06` retains its art gate and `22` verifies release access.

Make digging itself satisfying and allow players to create pits, tunnels, trenches, and strange routes instead of following a vertical corridor.

## Task 06 - finite terrain shell

- Scope: implement `TerrainVolume` behind `IDigTarget` with one tunable soil material, permanent bedrock/perimeter, local mesh/collider updates and in-memory removal state. Establish the [main game scene](core-loop.md) with the existing player, surface, scenery, visible inaccessible water and clear station/return anchors.
- Acceptance: untouched start; downward, diagonal and lateral cuts produce traversable space with matching collision; repeated/rejected hits cannot breach boundaries or spend energy without changing terrain. Returning to the surface preserves cuts for this scene session.
- Evidence: focused terrain/state checks, player collision/dig integration and editor inspection of shell wiring; record representation choice, measured update cost and visual limits. Preserve the existing FPS scene and `.meta` references.
- Production acceptance after `08`: replace visible primitive scenery and generated materials with approved assets, then inspect traversal/boundary readability in the Windows build. Disk saving and material variety remain later work.

## Task 20 - smooth digging overhaul

- Replace binary cubes with a sampled signed density field and interpolated surface nets. Small continuous cuts progressively deepen/widen the hole without axis-aligned stairs; Task `29` replaces their spherical base. The user's four screenshots define the shape direction, not permission to import art.
- Preserve the 24 x 24 x 12 m site, untouched start, arbitrary sideways/diagonal cuts, roofs, session state and permanent boundaries. Render and collision use the same mesh, with shared sampling across chunk seams and local rebuilds.
- Six tunable shovel levels remove strictly more equal fresh soil per accepted hit at equal energy. Expose level and successful-stroke feedback in the existing HUD. See [shovel progression](shovel-progression.md).
- Acceptance: smooth silhouettes/normals inspected after 1/4/repeated strokes; matching collision across seams; downward/lateral traversal and return; increasing measured removed volume at all levels; stale/invalid/boundary/pause/focus rejection without energy loss; reset and held-input safety; measured local update cost; Windows build and accessible test instructions.
- Art/audio stays as currently owned by the user. New textures, shovel models, effects and sound require separately scoped approval; task `08`/`11` art requirements stay open.

## Research and implementation decision

- [Reference game's official page](https://store.steampowered.com/app/3244220/A_Game_About_Digging_A_Hole/) establishes digging and equipment upgrades; the supplied images show overlapping rounded excavations. Its internal terrain implementation is unverified.
- [NVIDIA: volumetric terrain](https://developer.nvidia.com/gpugems/gpugems3/part-i-geometry/chapter-1-generating-complex-procedural-terrains-using-gpu) explains density fields, extracted surfaces and gradient normals. A heightmap cannot represent the required tunnels/overhangs.
- [Lysenko: surface nets](https://0fps.net/2012/07/12/smooth-voxel-terrain-part-2/) motivates averaging edge crossings for a compact smooth mesh. Implement independently, use a shared sample halo for seam agreement, and retain synchronous collision for accepted strokes. No copied package/code or dependency upgrade.
- [Unity: mesh preparation](https://docs.unity3d.com/6000.0/Documentation/Manual/prepare-mesh-for-mesh-collider.html) informs collider cooking: indexed, nondegenerate geometry and bounded chunk updates; validate actual edit/cook costs here.

## Tasks 21/23 - organic scoops and developer access

- Each accepted cut gets bounded contour variation (12% default) from a local seed and stroke index, plus penetration variation of +/-5% of radius along the hit normal. Preserve the aimed center, continuous surfaces, strength progression, seams/collision, stale-hit rejection and boundaries. Reset replays the sequence; discovery randomness is independent.
- One `SomethingDownThere.exe` includes durable admin tools during development. Ctrl+Shift+F10 opens the panel; Ctrl+Shift with 1-6/numpad, R or Home selects strength, refills or returns. No practice launcher or launch flag. Full behavior and range table: [shovel/admin contract](shovel-progression.md).
- Acceptance: device-input chords reject unmodified/held keys, menus/focus remain barriers, refill/selection preserve thrust, every owned upgrade extends real raycast reach, seeded depths vary and replay, live inspection and updated Windows executable. No new art/audio or disposable helper files.

## Task 26 - detached soil cleanup

- After an accepted scoop severs the last solid-sample connection to the floor/perimeter, remove the entire unsupported soil component immediately. Preserve connected bridges/overhangs; the top surface alone is not support. No falling fragments, physics debris or extra energy charge. Count detached soil in that stroke's removed volume and expand dirty mesh/collider updates beyond the brush and across chunks.
- Check components adjacent to newly severed samples, reuse search buffers and stop at proven anchors; untouched layers below all cuts are valid support. Acceptance: isolated crowns disappear, boundary-attached ledges remain, repeated irregular cuts agree with a whole-field support check, distant/cross-chunk collision clears synchronously, stale hits fail, reset restores soil, one revision/charge per stroke, measured cost and inspected Windows build.

## Task 29 - irregular shovel bites

- Replace spherical scoops with surface-oriented bites: a broad tilted floor, tapered asymmetric footprint, softened chipped shoulders, and bounded seed/penetration variation. Adjacent strokes should form broken soil faces instead of repeated hemispherical cups; preserve smooth meshes without voxel stairs.
- Remove the per-stroke excavated-volume popup. Keep collection feedback and the existing unobtrusive reticle response; volume remains available to admin diagnostics.
- Acceptance: broad-floor geometry for downward/diagonal/lateral cuts, deterministic reset, increasing controlled volume across six shovels, matching seams/collision, supported overhangs and detached-soil cleanup, collection/occlusion, measured edit cost and visual inspection of single/repeated cuts at small/large levels. Deliver the same Windows executable; existing art stays in place.

## Task 22 - production release gate

- The user's Task `23` preference supersedes deleting all developer tooling. Keep durable admin code; Unity's `Debug.isDebugBuild` flag gates access, actions and UI. No Windows elevation or separate game executable is needed.
- Before production, build without `Development`, verify admin shortcuts/menu/overrides are unavailable, and verify paid upgrades, recharge and rescue work through real game systems. No launch argument or scene setting may enable admin in a release player.
- Practice launchers/flag are removed under `23`. Confirm no old launcher is distributed; historical completion records remain as evidence. This remains a mandatory production release gate.

## Future material rules

Sand, soil, clay, gravel, sediment, rock and construction may differ in toughness; texture is never a treasure marker. Hard terrain communicates slow/impossible progress. Upgrades overpower old obstacles instead of immediately replacing them with proportionally tougher ground. Material rules and disk persistence need focused acceptance when implemented.
