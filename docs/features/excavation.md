# Excavation and terrain

Owns terrain/digging, detached-soil cleanup, developer access, materials/resistance, experimental excavation modes and passive underground lighting. Task IDs are plain numbers.

## Terrain shell and digging (`06`, `20`, `29`, `34`, `127`)
- Continuous sampled signed density field + surface nets at 0.125 m, 24 × 24 × 32 m site at the accepted surface position with approved soil/turf art. Render and collision share one mesh; local per-chunk rebuilds; permanent boundary colliders. Binary cubes/heightmaps rejected (stairs, no tunnels/overhangs).
- Preserve untouched start, diagonal/sideways cuts, roofs, bridges/overhangs and permanent boundaries. Old 12 m saves extend downward with samples, poses, identities and values preserved.
- Six shovel levels remove strictly more fresh soil per stroke at equal energy; per-stroke shape has ~12% contour variation and ±5% penetration of radius; adjacent strokes form broken faces, not repeated cups.
- `34`: thin unsupported remnants are removed after a stroke (≤0.25 m thick, ≤0.5 m across, ≤0.03 m³ sampled, still attached to thick soil); substantial sheets/bridges and the boundary attachment band are preserved. Deterministic; no whole-field smoothing and no extra paid hit.
- Rejected: voxel stairs, spherical cups, falling fragments/physics debris, cleanup that erodes supported terrain. All later digging sources including C4 use the `26`/`34` cleanup contract.

## Detached soil (`26`)
- After an accepted scoop severs the last solid connection to floor/perimeter, remove the whole unsupported component immediately; connected bridges/overhangs stay; one revision and charge per stroke; stale hits fail; reset restores soil.

## Developer access (`21`/`23`; release gate `22`)
- One development build shares durable admin tools: Ctrl+Shift+F10 panel, Ctrl+Shift+1–6/numpad shovel level, R refill, Home return, X-ray. Session-local; startup always uses the owned shovel and X-ray off. `22` verifies release builds cannot access admin.
- Every owned shovel upgrade extends real raycast reach; no practice launcher or launch flag. Keep durable tooling; no disposable review launchers.

## Materials and resistance (`39`, after `15`/`25`/`58`)
- Selected: continuous overlapping geological formations in one excavation, no level/biome unlocks; texture is never a treasure marker; upgrades overpower old obstacles without proportional resistance scaling. Caves/pre-dug chambers are not planned.
- Site premise: drained reservoir (`130`), preserving current footprint, saves and accepted grass/ground. The first full environment draft is rejected; `137` prepares one real-photo-informed section for approval before more construction.
- Ground appearance (`77`): short garden turf over warm granular earth with sparse partly buried grey/brown stones, soil visually dominant. Blender seamless textures, 2 m soil / 1 m turf world coverage, turf follows the original surface and upward faces.
- `58`/`39` bring one recognizable unmarked hard formation into the `148` slice; no hard lock, compulsory explosive or guaranteed treasure.

## Experimental excavation modes (`147`, `154`; future guns)
- Normal New Game/Continue uses the pre-147 irregular Scoop: paid tiers, hold/toggle, 60% aimed collection, remnant cleanup and the current economy. No trial tool, selector or binding affects normal digging.
- Developer-only "Experimental excavation: ON" starts in Shave with the trial rig and an EXPERIMENTAL HUD; Q cycles Scoop/Bore/Fan/Shave; session-only; off/restore/reload returns to Scoop; release builds cannot enable it.
- Experimental cuts save into the current excavation, but a saved selection cannot reactivate; bigger cuts never collect off-aim finds; switching suppresses held actions and cannot throw or spend fuel.
- Verdicts: Scoop preferred as normal; Bore and Fan unconvincing, not selected; Shave liked and promising but not production approval. Guns are intended with implementation deferred (`147`); `56` proposes capability milestones and `120` decides Shave's role, acquisition and migration. Trial art is presentation-only and never inherited as final.
- `153` tests honest visual positioning after the slice. Trial stress missed the `63` edit target (Fan p95 14.01 ms vs 8 ms) and is not optimized further.

## Passive underground lighting (`68`/`69`, paused, unselected)
- Decision: dependable passive player light so visibility never requires tool switching, lamp placement or a purchase; darkness is atmosphere, not a survival pressure.
- Implemented baseline: full sun occlusion, excavation-dependent ambient falloff, open sky 1, ambient minimum raised 0.14 → 0.45 for mineral readability at 32 m. `68` lamp/equipment choices remain paused.
- Proposal: automatic from the first frame; no switch/HUD resource/battery; camera/eye-mounted emitter; only accepted digging and thrust consume energy; works standing/crouched/flying. Readability: footing, wall shape and exposed find identity at 0.3–4 m, route opening up to 8 m, then gradual darkness. Candidate: per-pixel spotlight, 110° inner / 135° outer, 12 m cutoff, ~5000 K, one 1024 px local shadow tile, budget ≤1.5 ms GPU / ≤0.25 ms CPU. Rejected: tool-mounted housing, brighter chamber fill, narrow dramatic beam, light shop track, baked cave lighting, placeable lamps, darkness hazard.

## Shared rules
- User owns terrain art. Audio direction: ambient nature plus digging/action feedback. New textures/models/effects and specific sounds need approval; visuals require Blender MCP with retained sources/exports or free commercially licensed assets; no primitives as final content.
- Open: `06` art gate, `22` release exclusion, `58`/`39` material progression, `54` long-session visibility, `68` mounting and delivery owner, C4 inclusion.
