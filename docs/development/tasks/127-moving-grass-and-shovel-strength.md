# Task 127 - Moving surface grass and gentler shovels

Type: implementation. Status: `done`. Prerequisites: accepted 105 ground/style; explicit approval of the grass asset batch before import.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md), [shovels](../../features/backlog/shovel-progression.md). User-selected bounded batch, 2026-09-12; 126 remains next.

## Scope and selected behavior

- Add short three-dimensional grass above the accepted two-dimensional turf, with gentle anchored wind matching Sunny r8. Preserve soil/turf maps, daylight, finds and existing saves.
- Transfer the moving-grass implementation handoff from 08 into this task; the wider production presentation pass remains deferred.
- Approved asset batch (user: "Approve this grass batch", 2026-09-12): original Blender MCP 12-blade clump and six-blade distance mesh, 10-24 cm high, a small baked color atlas with matte material and instanced wind shader. The preview/source and exact integration/removal ownership are retained in the asset ledger.
- Source ownership: `art/ground-grass/` recursively (recipe, blend, exports, atlas, license). Runtime: `unity/Assets/Content/GroundGrass/` recursively and folder meta; `SurfaceGrassRenderer.cs` and editor setup/metas. Record full integration/removal in the ledger before import.
- Use spatial patches, view/distance culling and distance thinning. Recheck only excavation-touched patches; remove unsupported roots and regenerate deterministically from restored ground. No blade GameObjects/colliders or per-frame terrain raycasts.
- Reduce every shovel's nominal excavated volume per stroke to 60% by scaling cutter radii by cube root of 0.6; measure actual sampled removal. Reach, cadence, energy, prices and level/save identities remain unchanged.

## Acceptance

- Approve the concrete Blender grass preview/batch before game import; retain source/license and independently reversible ownership.
- Inspect short upright silhouettes and subtle rooted movement over Sunny turf in MainGame; preserve find visibility and remove grass over excavated air, including restored/reset terrain.
- Verify local patch updates, culling, distance thinning, lifecycle and deterministic restoration; record instance/triangle/draw costs and measured CPU submission/update time.
- Verify all six shovels remove approximately 40% less equal fresh soil than the prior profiles, remain strictly stronger by level, and retain actual reach/energy behavior.
- Run relevant deterministic and integration checks, inspect through official Unity CLI and deliver `builds/windows/SomethingDownThere.exe` with matched views.

## Delivered result and evidence

- Approved editable Blender source/exports and license: `art/ground-grass/`. MainGame and its builder reference the isolated GroundGrass assets and renderer; [ownership/removal](../../asset-ledger.md).
- About 20,451 clumps in 144 patches on intact ground, 168 triangles near / 84 far. Wind moves tips by at most about 1.3 cm; roots remain fixed. Stable random thinning avoids distant rows. Grass receives sunlight/shadows without adding shadow-caster passes.
- All six default and MainGame serialized shovel profiles are updated. Twelve seed/placement comparisons per level retain 58.7–59.6% of prior fresh-soil removal. Cadence, reach, energy, prices and saved level identities remain unchanged.
- **21 relevant tests pass**: fresh-volume comparison, 18 terrain/reach/energy tests, scene integration and grass local cut/reset/restore/re-enable/culling/thinning. [Evidence](../../../unity/Logs/Task127/).
- [Grass implementation record](../completed/127-moving-grass-and-shovel-strength.md), captured through official Unity CLI. Ten real cuts cleared 207 unsupported clumps across 39 patch updates, taking 7.85 ms total grass update time; two eligible bottles remained visible. Reviewed views: 75–128 submitted grass batches, 0.87–1.24 million triangles, 0.16–0.27 ms CPU submission. These are local Editor observations, not GPU/minimum-hardware qualification.
- Windows build **2026-09-12 14:55 UTC**, zero errors; the existing Pipeline runtime-config warning remains. Native headless startup stayed alive for seven seconds without exceptions/errors. Editor reload cleared a transient URP build-cache reference before the clean rebuild.
- [Completion record](../completed/127-moving-grass-and-shovel-strength.md). Native performance remains 54; the user reviews the delivered build. The wider 08 presentation pass remains planned.

Technical reference: [Unity RenderMeshInstanced](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Graphics.RenderMeshInstanced.html). Each spatial draw stays below 511 instances and uses explicit camera/bounds.
