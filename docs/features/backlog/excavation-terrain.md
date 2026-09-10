# Excavation and terrain

Status: Task `34` is [complete](../../development/completed/34-tiny-terrain-remnants.md): tiny attached remnants clear with their collision. Tasks `26`/`29` remain complete; `06` retains its art gate and `22` verifies release access.

Make digging itself satisfying and allow players to create pits, tunnels, trenches, and strange routes instead of following a vertical corridor. The shaped hole is part of the reward: preserve meaningful supported steps, ledges and return routes through cleanup/save changes. Detector suggestions and completion goals must not demand a prescribed tunnel shape or deletion of every voxel; freely clearing the site remains a player choice.

## Task 06 - finite terrain shell

See [numbered Task `06`](../../development/tasks/06-excavation-presentation.md) for scope, research, questions and acceptance.

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
- Acceptance: device-input chords reject unmodified/held keys, menus/focus remain barriers, refill/selection preserve thrust, every owned upgrade extends real raycast reach, seeded depths vary and replay, live inspection and updated Windows executable.

## Task 26 - detached soil cleanup

- After an accepted scoop severs the last solid-sample connection to the floor/perimeter, remove the entire unsupported soil component immediately. Preserve connected bridges/overhangs; the top surface alone is not support. No falling fragments, physics debris or extra energy charge. Count detached soil in that stroke's removed volume and expand dirty mesh/collider updates beyond the brush and across chunks.
- Check components adjacent to newly severed samples, reuse search buffers and stop at proven anchors; untouched layers below all cuts are valid support. Acceptance: isolated crowns disappear, boundary-attached ledges remain, repeated irregular cuts agree with a whole-field support check, distant/cross-chunk collision clears synchronously, stale hits fail, reset restores soil, one revision/charge per stroke, measured cost and inspected Windows build.

## Task 29 - irregular shovel bites

- Replace spherical scoops with surface-oriented bites: a broad tilted floor, tapered asymmetric footprint, softened chipped shoulders, and bounded seed/penetration variation. Adjacent strokes should form broken soil faces instead of repeated hemispherical cups; preserve smooth meshes without voxel stairs.
- Remove the per-stroke excavated-volume popup. Keep collection feedback and the existing unobtrusive reticle response; volume remains available to admin diagnostics.
- Acceptance: broad-floor geometry for downward/diagonal/lateral cuts, deterministic reset, increasing controlled volume across six shovels, matching seams/collision, supported overhangs and detached-soil cleanup, collection/occlusion, measured edit cost and visual inspection of single/repeated cuts at small/large levels. Deliver the same Windows executable; existing art stays in place.

## Task 34 - tiny terrain remnants

- Extend `ExcavationGrid` cleanup after accepted edits with a local thickness and component-size check for attached protrusions/slivers; retain Task `26` disconnected-soil removal. Reused local search buffers avoid idle work or a whole-field scan.
- At the current 0.125 m grid, eligible remnants are at most 0.25 m thick along a sampled axis, 0.5 m across each axis and 0.03 m3 in sampled volume. Require one connected attachment to thick soil; retain thin necks between separate supports. Recheck detached soil and remove density/render/collision together, with no debris, extra charge or new art/audio.
- Preserve substantial supported ledges, bridges, tunnel roofs, permanent boundaries and untouched ground. Avoid whole-field smoothing or repeated erosion of useful terrain; cleanup must settle deterministically without another paid hit.
- Acceptance: small attached floor spikes/wall slivers no longer snag actual player traversal after downward, diagonal and lateral cuts; useful supported structures survive. Verify cross-chunk collision, discovery exposure, removed-volume accounting, stale-hit rejection and one charge/revision per stroke. Measure local/large-cut costs against `26` and inspect the Windows build.
- Limits deliberately preserve substantial sheets/bridges and the boundary attachment band; use measured troublesome cuts before broadening them. All later excavation sources, including C4 if included, must use this cleanup contract.

## Task 22 - production release gate

See [numbered Task `22`](../../development/tasks/22-release-admin-exclusion.md) for scope, research, questions and acceptance.

## Future material rules

[Task `39`](terrain-materials.md) owns material selection, resistance, approved presentation and focused acceptance; texture is never a treasure marker and upgrades overpower old obstacles without proportional resistance scaling. [Task `35`](core-loop.md#task-35---persistent-excavation-and-progression) owns saving. The [review research](../../research/player-review-findings.md#physical-comfort) reinforces completed `26`/`34` cleanup; it does not create another cleanup task.
