# Task 68 - Design passive underground lighting

Type: design/research; documentation only. Status: `in_progress`, paused by the user. Prerequisites: `63`, `65` (complete). Retain the unselected proposal; do not repeat its questions or auto-resume. [89](89-starter-minor-find-design.md) is next.

Feature: [underground lighting](../../features/backlog/underground-lighting.md). Implementation: [69](69-underground-lighting.md); consumers: `56`, `57`, `39`. [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Why and current gap

The current scene has a directional sun and no dedicated player light. Deep/lateral excavation needs dependable visibility without placing lamps, switching tools or purchasing basic usability. This is a visibility requirement, not a darkness hazard or another battery chore.

## Research and proposal

- Inspect the current URP setup, occlusion, user-owned terrain and approved content; assess enclosed passages, large chambers, close walls and surface transitions at `65` FOVs.
- Propose a passive headlamp or tool-mounted light, beam width/range/falloff, placement and glare/shadow policy. Baseline visibility must work at zero battery; only accepted digs and active thrust consume the shared battery.
- Research official Unity/URP lighting and performance guidance under `63`. Identify through-wall leakage, flat-looking materials and washed-out nearby finds; record a measurable visibility/performance target before integration.
- Compare a sufficient fixed baseline against optional cheap light improvements or integration with an existing equipment track. The report's 2–3 levels are a proposal, not an approved new shop category; pass any selected structure to `56`.
- Define the visible housing, if needed, and its first-person visibility/terrain handoff with `57`. No lights-as-platforms, placeable-light chores or generated substitute props; any actual model/material/audio batch still needs explicit approval.

## Questions to resolve with the user

- Review the [concrete proposal and alternatives](../../features/backlog/underground-lighting.md#proposed-direction-for-review): offscreen headlamp versus stabilized tool mounting; broad nearby light/darker distance versus brighter chambers; fixed baseline versus a later range improvement in an existing track.
- Recommended: offscreen headlamp, clear nearby terrain with darker distance, fixed sufficient light. These three choices are **awaiting user selection**, not approved by the research. Do not re-open the already selected passive/no-drain or no-placement rules.

## Research evidence and handoff

- Official CLI inspection on `2026-09-09`: clean MainGame, Unity `6000.6.0f1`, URP `17.6.0`, Forward renderer, per-pixel additional lights, 4× MSAA, linear color; one intensity-1.4 directional sun with no shadows, flat ambient `(0.58, 0.64, 0.70)`, both main/additional shadow support disabled. Audit: `unity/Logs/Task68/live-audit.json`.
- Current user-owned `GroundTriplanar.shader` already uses URP PBR lighting, additional-light/shadow variants and a punctual ShadowCaster pass; approved soil roughness/contact occlusion and normal maps must retain their relief. World chunk caster/receiver behavior and shader variants still need actual shadow validation in `69`.
- Captured **15 underground/rim views** through CLI evaluation of the existing camera: 55/75/90° at 1280×720 and 1280×800 for a sideways passage and 7 m chamber; near wall, shaft and rim at 75°. These use temporary scripted excavation in an additive MainGame with its save owner disabled/uninitialized. Current volume is 24×12×24 m; deeper future geology is not represented by this inspection.
- [Enclosed passage](../../../unity/Logs/Task68/current-tunnel-720-fov55.png), [wide FOV](../../../unity/Logs/Task68/current-tunnel-800-fov90.png), [chamber](../../../unity/Logs/Task68/current-chamber-720-fov75.png), [near wall](../../../unity/Logs/Task68/current-near-wall-720-fov75.png): current sun/ambient keep enclosed surfaces bright; distant shape is relatively flat and the blue/amber development finds retain strong highlights. Close soil remains textured without a lamp. These are current-state evidence, **not proposed-light previews or gameplay acceptance**.
- [Unity light controls](https://docs.unity3d.com/6000.6/Documentation/Manual/urp/light-component.html) support a soft cone and warn about extreme cone widths reducing shadow detail; [shadow optimization](https://docs.unity3d.com/6000.6/Documentation/Manual/shadows-optimization.html) motivates one spotlight and profiling shadow resolution/filtering; [shadow troubleshooting](https://docs.unity3d.com/6000.6/Documentation/Manual/urp/shadows-troubleshooting-urp.html) motivates per-light bias/leak tests. All checked against the pinned Editor manual on `2026-09-09`; no package change is required for this proposal.
- The feature defines testable 0.3–4 m work/8 m route visibility, FOV coverage, render allocations and an offscreen-housing brief. Beam settings and cost allocations are engineering hypotheses; ordinary digging, moving occlusion, final art, zero-fuel rescue and sustained GPU timing remain `69` acceptance.
- MainGame restored stopped and clean; inspection did not initialize a save profile or persist scene/renderer changes. Evidence scripts, capture index and restoration audit: `unity/Logs/Task68/`. Local document links and `git diff --check` pass; this documentation task does not replace the current Windows gameplay build.

## Done when

Record the chosen mounting, baseline visibility, optional upgrade decision, rationale/avoidances, performance targets and asset brief in the feature. Update `56`/`57`/`69` with the selected scope, explicitly assigning any later paid improvement to its implementation owner without making baseline lighting depend on late tracks; unanswered style questions or a brief alone do not complete gameplay.
