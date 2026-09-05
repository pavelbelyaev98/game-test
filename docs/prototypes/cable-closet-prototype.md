# Cable Closet Untangler — Prototype Decision Brief

Status: proposed interaction experiment; implementation approaches researched, but no playable build or measured performance. No production decision.

Use the [shared scorecard](prototype-comparison-scorecard.md) for build conditions, the six-player protocol, identical evaluation gates, and the limit of two bounded revisions. This is the highest-risk interaction experiment. The existing research supports visible organization and functional completion as rewards; it does not establish that dragging tangled cables will feel good.

## Core hypothesis

Tracing, freeing, routing, and reconnecting a messy cable feels controlled and satisfying enough that the player voluntarily wants to fix another one.

## Player fantasy

Understand a cable mess, work a trapped lead free, lay it neatly through a guide, and hear the connector click as equipment comes online. Both the physical organization and the working connection should feel earned.

## Exact 5–10 minute loop

Task instruction: **“Free the eight cables, route them through the guide, and connect each to its matching numbered port.”**

| Target time | Player activity | Evidence sought |
| --- | --- | --- |
| 0:00–1:30 | Trace one accessible cable, free its loop, guide its connector, and plug it into its destination. | Complete one whole interaction without facilitator help. |
| 1:30–4:00 | Repeat until four connections work, inspecting overlaps and choosing an accessible cable. | Interest in freeing and routing, not only snapping plugs into sockets. |
| 4:00–4:30 | Activate the toner tool on a selected remaining cable. | Understand the sole power increase. |
| 4:30–8:00 | Finish the remaining four connections using the toner when useful. | Less tracing friction while the physical action remains worthwhile. |
| 8:00–9:00 | Inspect the organized rack and bring the final connection online. | A visible and audible functional payoff. |

These are pacing targets, not timers or enforced delays. Each cable requires actual freeing, passage through a guide, and a correct connection. Use four pairs with comparable crossing complexity, including an easy first accessible cable. Log which layouts a player handles before and after the upgrade; do not force an arbitrary solving order for measurement.

After completion, offer up to two more minutes with one existing pair reset to its messy poses, using the same tool and interaction rules. This maintenance repeat provides no new unlock or special reward. Keep it separate from full-scene restart.

## Core interaction

- **Camera and hands:** First-person mouse look and WASD around the front and sides of one rack. Show the held connector or a small grip marker; no hand IK is required. Freeze camera look and walking during a drag, then restore them immediately on release.
- **Select/trace:** Click LMB on a visible cable segment or connector. A small local highlight identifies the selected cable at the contact point; its complete length is not revealed automatically. Follow its visible path to its numbered source label. Depth-tested picking must not select an occluded cable behind it.
- **Grab/free:** Hold LMB on a loose connector or clearly exposed loop. Mouse movement moves the grip in a plane parallel to the rack front; the wheel adjusts depth within a small, bounded reach envelope. Pull an overlying loop outward and around its visible snag before drawing it clear. A trapped lower cable resists at the visible crossing until the obstruction moves.
- **Player control:** Input continuously changes the grip and cable shape within the current authored constraint. Moving against the obstruction creates a small tension response without progress. Stopping the mouse stops the action. Reversing the movement before release reverses its progress. There is no hold-to-play untangling movie.
- **Route:** Once free, drag the connector through its numbered channel in the single management guide. The player must cross the opening; merely aiming at the destination cannot route or untangle the cable. The cable visibly settles along that channel as the connector moves onward.
- **Connect:** Move the guided connector into the matching destination port. A generous final snap zone completes only the last few centimeters, with a click and a per-connection online light. A wrong port gives a brief mismatch cue and leaves the connector in the player's control.
- **Release/cancel:** Releasing LMB parks the grip at its last valid pose, preserving partial work. RMB returns the current gesture to its last committed pose through a short visible return. Neither action resets already completed cables. Connected cables are protected from accidental grabs for this prototype.
- **Use tool:** After the upgrade, press Q with a cable selected to pulse that cable's full path. Selection, freeing, guide passage, and final placement still use the same controls.

Use matching numbers and symbols as well as color on both ends and guide channels. Let the player inspect the visible cause of a snag; do not replace it with floating directional arrows or an unseen “wrong move” rule. Generous targets and restrained magnetic placement remove precision friction without doing the main action.

## Minimum scene

One rack in a small utility-room corner, with sufficient front/side access to see crossings. Provide one source bank, one destination bank, a management guide between them, and a device face with connection indicators. Neutral lighting and a contrasting backplate make cable depth readable.

## Minimum content

| Content | Required amount / states |
| --- | --- |
| Cables | Eight copies of one cable style with distinct number/symbol labels. Each source end is fixed; each destination connector starts loose in the mess. |
| Ports | Eight source ports and eight matching destination ports. No network configuration or electrical simulation. |
| Tangled setup | Four authored cable pairs with visible crossings, draped loops, and shallow snags. Dependencies have no cycles; at least one cable is always freeable. No tight mathematical knots. |
| Cable states | Tangled → Freed → Guided → Connected. Grip position, partial release progress, and selection are separate transient data. |
| Guide | One management guide with eight generously sized numbered channels; one valid route per cable is sufficient. |
| Tool | One toner mode, initially unavailable, with a simple path-pulse effect. |
| Functional payoff | One rack/device with eight connection lights and one final ready state. |

The starting layout must visibly contain real obstacles for the chosen representation: a loop passes around a snag or under another cable and must be moved clear. A decorative messy spline that becomes straight after one click does not test the hypothesis.

## Game-feel requirements

| Element | Minimum honest test |
| --- | --- |
| Anticipation | See where the selected cable is trapped and watch slack grow as the obstruction clears. The destination is visible before the last plug. |
| Animation | Continuous loop deformation under input, a small release recoil, slack settling into the guide, and a short connector insertion. |
| Resistance | A snag visibly restrains movement; moving the correct part in a plausible direction loosens it. Resistance cannot come from unexplained input rejection. |
| Audio | Quiet insulation rub, scrape at a guide, a small release tick, a firm plug click, then device activation. Sound intensity follows movement/tension. |
| Particles/VFX | Minimal: contact highlight, toner pulse after unlocking, and status lights. No spark shower or particles needed to sell the action. |
| Completion feedback | Each cable stays organized and its connection light changes with a clear tone; the final device response is distinct. |
| Camera feedback | Stable view and immediate drag release. Convey tension through the cable and grip, without camera yanks, forced zoom, or shake. |
| Before/after | The same rack changes from crossed loose leads and dark indicators to readable runs through the guide and a working device. |

## Technical implementation proposal

### Research comparison before choosing

The following compares implementation choices, not tested results. Effort and feel risks are engineering judgments for this small scene. The workspace currently has no Unity project or package manifest; confirm package compatibility when creating the playable build. Documentation was checked on 2026-09-05.

| Approach | What it would implement | Useful property | Main risk for this experiment | Decision |
| --- | --- | --- | --- | --- |
| **1. Fully simulated flexible cable** | An existing rope solver drives the full cable, with collision between cables, self-collision, fixed ends, and a grabbed attachment. | Physical responses could make freeing and slack management emerge from the player's movements. | Reliable thin-cable crossings, grabbing, camera-depth control, and solver tuning may consume the effort budget before the fun can be assessed. Simulation capability alone does not establish usable untangling. | Documented alternative; do not write a custom rope solver or buy/integrate a package just for this brief. |
| **2. Segmented constrained cable** | Short capsule Rigidbodies joined by ConfigurableJoints, with a smooth visual cable following their centers. | Uses built-in Unity physics and explicit limits; individual segments are easy to inspect. | Coarse bends, snagging around segment boundaries, joint stretch/correction, and collision tuning may undermine delicate manipulation. Increasing segment count adds tuning and runtime work. | Viable mechanical experiment, but not the first full prototype. |
| **3. Spline visuals with authored knot/constraint states** | Spline control points respond to a constrained grip; authored crossing states determine when a loop can clear. | Repeatable starting conditions, explicit recovery, and direct control over the tested obstacle and feedback. | The player may feel confined to an invisible track, or see a cable pass through another during a shape transition. | **First approach to prototype.** Test whether constrained motion preserves the fantasy. |
| **4. Hybrid** | Authored crossings and guide/connection states, with a short physically simulated free tail or active span driving part of the visual spline. | Could add local weight and slack while keeping the puzzle and completion recoverable. | Handing motion between authored and simulated parts can pop, stretch, or feel inconsistent; two representations increase debugging work. | One possible bounded refinement if the first approach's specific failure is missing local elasticity. Not an automatic second project. |

Source basis: Obi documents collisions between actors and optional self-collisions, making it a concrete example for approach 1. Its attachment system distinguishes transform-driven static attachments from dynamically coupled ones. These document available mechanisms, not this game's robustness. [Obi collision documentation](https://obi.virtualmethodstudio.com/manual/7.0/collisions.html), [Obi attachment documentation](https://obi.virtualmethodstudio.com/manual/7.0/attachments.html).

Unity's ConfigurableJoint exposes movement limits, springs, damping, and projection that corrects constraint violations. Those are the building blocks for approach 2; the concerns about cable feel are unmeasured here. [Unity ConfigurableJoint reference](https://docs.unity3d.com/6000.0/Documentation/Manual/class-ConfigurableJoint.html).

Unity Splines' Spline Extrude can generate cable-shaped geometry and refresh it as the spline changes. Its collider-update option is separate from the proposed authored interaction logic. This supplies a visual mechanism for approaches 3 and 4, not an untangling simulation. [Unity Spline Extrude reference](https://docs.unity3d.com/Packages/com.unity.splines@2.8/manual/extrude-component.html).

### Smallest implementation of the chosen approach

Use one scene controller, eight local cable records, and scene-authored handles, poses, guide volumes, and port IDs. Each record owns its state, source/destination, blockers, release progress, and spline control points. No general knot-solving, puzzle-authoring, or interchangeable solver framework.

For each crossing, author a small sequence of geometrically plausible poses around the obstruction. Project grip movement into the allowed local region and drive pose progress from that movement. Use a broad swept path rather than a single pixel-perfect line. Display residual tension when the grip reaches a limit. Only mark Freed when the loop visibly clears its obstruction; do not silently jump to a neat cable.

Once free, bounded connector movement updates the tail's control points with visible slack. Guide entry is an explicit passage test through a simple volume in the correct direction. Committing Guided anchors the run at the guide while leaving the connector movable toward its port. Connecting requires the correct ID and Guided state. Game-state changes remain deterministic; cosmetic settling can use simple damped interpolation.

Render a low-resolution round spline mesh. Use a small set of primitive selection colliders along the visible path and larger connector/loop handles, updating only affected targets as the shape changes. Do not regenerate a detailed MeshCollider for every deformation. Keep visible and selectable geometry aligned, with the nearest unobstructed target winning selection.

Build one crossing pair, one guide channel, and one successful connection before authoring the other pairs. Inspect it from the allowed front and side viewpoints: naive blending between poses must not cut through cables or rack geometry. If the representation cannot meet this test within the declared cap, stop or spend one of the two allowed revisions on a precisely scoped hybrid test. The hybrid is a developer implementation variant, never a second player upgrade; keep its results separate.

Readiness checks: try the lower cable first, drag against a snag, reverse a partial release, cancel at each state, attempt the wrong guide/port, and try selecting overlapping cables from the side. Complete the rack in different valid orders, reset, and repeat at several frame-rate caps. These are checks to perform on the future packaged build, not completed validation.

## Technical unknowns

**Single most dangerous uncertainty:** Can authored spline constraints preserve visible contact, slack, and continuous player-controlled release across a crossing without clipping or an implausible snap?

## Design unknowns

**Single most dangerous uncertainty:** Does the player feel they actually understood and freed a tangled cable, or merely followed a restricted animation before enjoying the final plug click?

## What NOT to build

No economy, jobs board, customer story, NPC AI, large building, achievements, complex saving, multiplayer, workshop, cable purchasing, procedural tangles, arbitrary knot topology, cutting/splicing, network diagnostics, packet simulation, or real electrical faults. No cable comb plus tester plus labels progression tree. No custom physics engine or support for every future rack layout.

## One progression moment

After the fourth correct connection, unlock **the toner path pulse**. Pressing Q highlights the currently selected cable's complete path for a few seconds, including obscured sections. It does not highlight the correct destination, release snags, move the connector, or route the cable.

Numbers and symbols were readable from the start; the new power reduces tracing uncertainty. Freeing, routing, and plugging must remain satisfying afterward. Measure whether the player understands the benefit and still feels responsible for the physical result.

## Completion moment

The eighth correctly guided and connected cable turns on its indicator. The device performs a short light sequence and starts a quiet fan/operating sound; the rack now has an obvious working state. Leave the organized cables visible and the camera free. “Rack online — 8/8 connected” closes the task before the optional maintenance repeat.

## Instrumentation / observations

Apply every [shared measure and gate](prototype-comparison-scorecard.md#shared-measures-and-decision-gates). Add cable/pair ID, implementation variant, selected stage, obstruction, guide crossing, port attempt, and toner use to the minimum record.

- Measure first complete cable time and time spent tracing, freeing, guiding, and plugging separately. Deliberate tracing is not automatically boredom.
- Record wrong-target selection, lost grabs, repeated attempts against an unexplained constraint, wrong guide/port attempts, and facilitator explanations. Distinguish comprehensible matching mistakes from false rejections of valid motion.
- Note frustration when depth or resistance behaves unexpectedly, and whether the player can explain why a cable was stuck and what freed it.
- Capture praise of freeing and routing separately from praise of glowing lights, visual neatness, or the connector click. A satisfying plug alone does not prove the complete loop.
- Compare matched crossing layouts before and after toner use, and record whether revealing the path leaves a meaningful physical task.
- Measure actual no-reward continuation on the reset pair. Preserve comments describing the action as automatic, predetermined, or physically convincing alongside the ratings.

## Kill criteria

After the initial build and at most two bounded input/feedback/representation revisions, stop this concept for this project if any of these remains true:

- Ordinary-action appeal or voluntary continuation misses the shared gates: median appeal below 4/5, fewer than 4/6 naming an action to repeat, or fewer than 4/6 actually continuing.
- At least 3/6 describe freeing as an automatic or arbitrary animation, or want to skip freeing/routing and only plug cables in. Attractive before/after screenshots do not pass this hypothesis.
- Tracing or depth manipulation still produces the common repeated-failure pattern, misses the shared reliability/clarity gates, or requires facilitator coaching to explain invisible constraints.
- The toner still fails the shared progression gate because it removes the only engaging part or does not leave a satisfying task.
- Believable continuous release requires unbounded physics work, authoring each ordinary crossing exceeds the declared effort cap, or either representation still has completion blockers or unacceptable loss of agency after refinement.

Do not rescue weak untangling with more devices, a career story, customer dialogue, additional tools, loot, or larger tangles. A failed constrained implementation does not prove all physical cable games are impossible; it can still be enough evidence to stop this concept within this solo project's limits.
