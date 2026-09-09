# Passive underground lighting

Status: **proposal awaiting Task 68 review; not implemented**. [68 - design](../../development/tasks/68-underground-lighting-design.md) owns research and review; [69 - integration](../../development/tasks/69-underground-lighting.md) follows approved production presentation.

Idea coverage: sections 31 and 44; equipment structure in section 11.

## Decision and rationale

Provide dependable passive headlamp/tool lighting in underground passages. Excavation visibility must not require switching tools, placing lamps or purchasing basic usability. Darker geological atmosphere is compatible with readable terrain and finds; darkness is not an added survival pressure.

The current scene has no dedicated player light. Its shadowless sun and flat ambient light also illuminate enclosed soil surfaces. Task `68` reviews the following concrete proposal using [comfort findings](../../research/player-review-findings.md#physical-comfort); numerical settings remain implementation tuning hypotheses.

## Proposed direction for review

| Choice | Recommended | Alternative and tradeoff |
| --- | --- | --- |
| Mounting | Head-mounted, aligned with the view; housing outside the first-person view | Tool-mounted housing makes equipment visible, but a beam following shovel strokes would sweep away from the work. A stabilized beam avoids that at the cost of less literal mounting. |
| Contrast | Broad, softly edged neutral light; clear close terrain, darker distant chambers | Brighter chamber-wide fill exposes the whole excavation more readily but weakens depth and shape. A narrow dramatic beam concentrates attention but is a poor match for peripheral navigation. |
| Progression | One sufficient fixed light for the entire game; no paid light improvements | One later range improvement within an existing equipment track can reward large chambers, but needs a distinct benefit and implementation owner. A separate light shop track adds little useful choice. |

### Proposed baseline and presentation

- Automatically available from the first playable frame, without a switch, HUD resource or battery dependency. At zero fuel it remains lit through the automatic rescue/return; do not introduce a playable empty-fuel mode or change rescue timing.
- Follow camera orientation and actual standing/crouched position with no aim lag, bob, swing, strobe or purchase animation affecting the beam. Start near the eye, slightly above it and inside the collision envelope; prevent the emitter crossing thin walls or ceilings.
- Preserve readable footing, wall shape and exposed-find identity from **0.3–4 m** (including maximum shovel reach); show the shape of a traversable route or chamber opening to **8 m** when unobstructed. Beyond that, gradual darkness may suggest scale; no requirement to flood an arbitrarily large chamber.
- Initial tuning candidate: one realtime per-pixel spotlight, **110° inner / 135° outer cone**, **12 m cutoff**, neutral approximately **5000 K** appearance. Tune intensity/falloff against approved soil and finds; these are not tested final values. Preserve normal-map relief and roughness instead of whitening close surfaces.
- Maintain useful peripheral visibility at vertical FOV **55/75/90°**, aspects **16:9/16:10**, standing/crouched and while flying. A 135° outer cone geometrically contains the approximately 128° diagonal view at 90°/16:9 before mount-offset/near-wall effects; actual edge brightness still needs review.
- Keep the light available at the surface; daylight should naturally dominate it. Avoid a depth threshold, on/off popping or a compulsory exposure-adaptation effect at the rim. Only use a smooth intensity transition if observation proves one necessary, preserving close-work visibility throughout.
- Distant darkness is atmosphere, never a hazard, resource pressure or item-rarity signal. Maintain a modest neutral ambient floor for orientation; do not make a uniformly bright underground world the substitute for local lighting.

### Occlusion and render policy

- Terrain, boundaries and world finds must occlude local light; hidden finds cannot cast identifiable shadows through their covering soil. Avoid first-person shovel shadows crossing the work area. No outline/X-ray, emissive treasure reveal, cookie texture, bloom, fog beam or new post-processing is proposed.
- `69` must address the existing unoccluded sun as part of lighting integration. Start by evaluating realtime sun and spotlight shadows plus restrained ambient contribution; verify newly dug topology, thin walls and deep-camera coverage. Static baked cave lighting cannot follow changing excavation. Retain Task `19`'s readable surface/clean rim while checking any shadow changes.
- Start with **one** shadowed spotlight and a **1024 px** local shadow tile, modest soft filtering and tuned per-light bias. A shadowed point light requires six views; a static cookie cannot provide changing terrain occlusion. [Unity shadow optimization](https://docs.unity3d.com/6000.6/Documentation/Manual/shadows-optimization.html).
- Wide cones spread shadow resolution; excessive bias can cause leaks. Tune at close walls rather than increasing bias until acne disappears. [Light controls](https://docs.unity3d.com/6000.6/Documentation/Manual/urp/light-component.html), [shadow troubleshooting](https://docs.unity3d.com/6000.6/Documentation/Manual/urp/shadows-troubleshooting-urp.html).
- Engineering allocation, **unmeasured**: total added lighting/occlusion cost versus the current scene **p95 ≤1.5 ms GPU, ≤0.25 ms main-thread CPU**, no steady-state managed allocation from the light controller, and at most one local shadowed light. Include sun-shadow/ambient changes in that delta; the complete frame still meets [63's budgets](release-validation.md#release-budgets). Profile matched cameras/saves at native 1080p/1440p; development hardware is diagnostic, candidate hardware qualification remains `54`.

## Asset brief and ownership

- Recommended headlamp needs **no visible first-person housing**. Do not add a helmet, body, lamp mesh, material or sound solely to explain an offscreen light. `57` retains this presentation boundary; the user's terrain textures/material ownership is preserved.
- If a later approved view makes the lamp visible, `57` must separately brief a compact neutral work-light housing and prepare the specific Blender-source/export or commercially licensed import batch, preview, files, license and removal plan before asset approval. This proposal approves no such batch.
- Under the recommended fixed-light choice, `56` excludes light purchases, `57` owns the offscreen presentation handoff, and `69` delivers the whole baseline without a paid-track dependency. If an improvement is selected instead, `68` must explicitly assign its delivery owner before completion.

## Required behaviour and exclusions

- The baseline works automatically underground, including with an empty battery. Preserve the rule that only accepted digging and active thrust consume energy.
- Keep player-made routes, material/boundary distinctions and exposed finds readable; avoid glare, washed-out materials, through-soil treasure reveal and excessive render cost.
- No placeable-light chores, lamp platforms, fuel consumables, darkness hazard or new purchase track is implied. Markers remain a separate conditional navigation question.
- Respect FOV/low-motion and any selected precision stance. Support enclosed lateral tunnels as well as shafts.
- Visible equipment and material/audio additions require explicit approval through the existing art rules; this feature does not authorize them.

`69` owns normal-play/save/reload, glare, occlusion and measured render acceptance; `39` rechecks mixed materials and `54` long-session visibility. Brightness/reticle preferences remain `81`/`82`; baseline light cannot depend on either a paid upgrade or a brightness-setting workaround.
