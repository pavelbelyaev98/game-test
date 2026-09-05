# Chushkopek Season — Prototype Decision Brief

Status: the full 18-pepper experiment remains proposed and untested. A narrower [one-pepper Stage 0 spike](chushkopek-stage0.md) has been implemented; personal hands-on acceptance is pending. No production decision.

Use the [shared scorecard](prototype-comparison-scorecard.md) for build conditions, the six-player protocol, identical evaluation gates, and the limit of two bounded revisions. The existing research favors meaningful processing states, readable transformations, and upgrades that preserve the enjoyable action; this brief tests those claims with peppers.

## Core hypothesis

Roasting, steaming, and peeling peppers feels good across repeated cycles, so the player voluntarily wants to process another batch.

## Player fantasy

Work at a small Bulgarian yard table: hear the chushkopek sizzle, lift out a blistered pepper, uncover steam, and pull away its skin to build a tray of prepared peppers. The pleasure should come from handling the peppers and getting into a rhythm.

## Exact 5–10 minute loop

Task instruction: **“Prepare the 18 peppers and put them on the tray.”**

| Target time | Player activity | Evidence sought |
| --- | --- | --- |
| 0:00–1:00 | Pick up a pepper, roast it, move it to the steaming bowl, peel it, and place it on the tray. | Understand the whole cycle without a spoken walkthrough. |
| 1:00–3:30 | Repeat until six peppers are prepared, using one roasting socket. | Enjoyment of repetition, not just the first reveal. |
| 3:30–4:00 | Activate the two extra sockets on the same chushkopek. | Recognize the sole capacity increase. |
| 4:00–8:00 | Process the remaining twelve, interleaving roasting, steaming, and peeling. | More satisfying rhythm without excessive monitoring or errors. |
| 8:00–9:00 | Place the last pepper and inspect the finished tray. | A clear, earned end to the work. |

These are pacing targets, not locked phases. Start with roughly 12 seconds to reach perfect roasting, a further 6-second grace window before burning, and 4 seconds of steaming. Peeling follows the player's movement; it is not a countdown. Tune only to improve anticipation and handling. Never stretch waits to force five minutes; record actual duration.

After completion, offer the scorecard's optional two-minute continuation using three more copies of the same pepper and the acquired capacity. This is a separate probe, with no reward or further unlock.

## Core interaction

- **Camera and hands:** First-person mouse look and WASD in a tiny work area. Use one inexpensive tong/viewmodel pose for carrying, plus a close work surface for peeling. No animated full-body character or hand IK is required.
- **Pick/place:** Aim at a pepper and click LMB to hold it. Click a highlighted compatible socket, bowl position, or tray position to place it. Placement has a generous snap area and takes one click. One pepper can be held at a time.
- **Roast:** Inserting a pepper starts its timer. Blisters, sizzle, and a short pop announce the perfect window. Clicking lifts it out and immediately stops heating. The player chooses when to remove it; there is no automatic ejection.
- **Steam:** Click a bowl position with a roasted pepper. A small authored cover animation and steam effect start its timer automatically. The pepper becomes peelable when the steam subsides. The bowl has three positions from the beginning.
- **Peel:** With a peelable pepper resting at a bowl position, hold LMB on a visible loose skin edge and drag along the pepper. Two broad strips respond continuously to the drag: slight resistance, curling skin, then release. Freeze mouse look only during the drag. Releasing LMB retains the strip's progress; the next drag continues it.
- **Finish/recover:** Click the peeled pepper to lift it, then click the next tray position. RMB cancels carrying and returns it to its reserved origin. Invalid placements explain the required state briefly and keep the object held. There is no free-drop system or lost-object hunt.

The controls card states these actions. State prompts use text/icons alongside color. Do not aim-test the player's ability to hit tiny skin edges.

## Minimum scene

One enclosed yard corner, one worktable within a few steps, and a fixed view of the accumulating tray. Basic plaster, fence, and outdoor ambience establish the setting. A seated grandfather with an idle pose is optional only if an existing asset makes it almost free; he has no interactions, dialogue, or gameplay effects.

## Minimum content

| Content | Required amount / behavior |
| --- | --- |
| Pepper crate | One; contains 18 individually selectable peppers. |
| Pepper | One reusable mesh with modest visual variation, simple collider, state materials, and two detachable skin strips. |
| Chushkopek | One three-socket model; one socket active initially, two visibly disabled. |
| Steaming bowl | One with three authored positions and a simple cover/steam animation per position. |
| Finished tray | One with 18 authored positions. |
| Work surfaces | Only the table and simple placement/interaction targets. |

Required pepper states: **Raw → Roasting → Perfect → Burnt**, with the last transition occurring only while heating continues. A removed Perfect or Burnt pepper can enter **Steaming → Peelable → Peeled**. Preserve a separate roast-quality flag so a burnt pepper still looks burnt after its state changes.

An under-roasted pepper remains Roasting when lifted, with progress paused; it can return to the roaster but cannot enter steaming. Burnt peppers remain processable and count toward the 18 prepared peppers. Burning gives readable feedback and a worse-looking result, not a restart or an impossible completion requirement.

## Game-feel requirements

| Element | Minimum honest test |
| --- | --- |
| Anticipation | Blistering and increasingly active sizzle lead into the perfect window; steam visibly settles before peeling. |
| Animation | Short placement/retrieval movements, a slight pepper turn during peeling, and two skin strips that curl with input. |
| Resistance | Skin initially stretches a little; valid dragging then advances it smoothly into a crisp release. Reverse dragging cannot silently complete it. |
| Audio | Distinct insertion tap, sustained sizzle, occasional pop, steam hiss, papery peel, and soft tray placement. Each reflects a state or action. |
| Particles/VFX | Small steam bursts and subtle heat/blister changes; limited smoke communicates burning without obscuring targets. |
| Completion feedback | Each peeled pepper changes silhouette/material and fills a tray position; the last placement closes the task. |
| Camera feedback | Stable view throughout. Feedback comes from the held object and surface, not camera shake, forced zoom, or head bob. |
| Before/after | Raw crate empties while a visibly prepared tray fills; imperfect peppers retain their quality differences. |

## Technical implementation proposal

Use one Unity scene and a small local controller: an array of pepper records, a state enum, roast/steam timers, strip progress, quality, and explicit location/socket ownership. Keep a carried object's origin reserved until placement succeeds. State transitions and completion counts are ordinary game logic, independent of physics.

Use kinematic placement, primitive colliders, authored sockets, and short transform animations. Change materials or blend shapes for blistering. Drive skin-strip meshes through authored poses from drag distance along a broad axis on the work surface. A small selection of contact sounds and particles is sufficient. Do not simulate soft peppers, skin tearing, heat transfer, or steam pressure.

Prototype order: prove one complete pepper first; add the remaining instances; enable the two extra sockets; run the whole batch. Check focus/pause behavior, early removal, burning, invalid placement, partially peeled cancellation, and full reset in the packaged build. The continuation adds three instances using the same logic and a cleared tray area; it does not use the full-scene reset that removes the upgrade.

## Technical unknowns

**Single most dangerous uncertainty:** Can two inexpensive, input-driven skin strips feel like peeling something physical instead of scrubbing a progress bar? Test this on one pepper before making the full yard.

## Design unknowns

**Single most dangerous uncertainty:** Does the complete roast–steam–peel rhythm remain enjoyable, or does the player tolerate waiting and monitoring only to reach the brief peel payoff?

## What NOT to build

No cellar, Zimnina, recipes, jars, selling, currency, shop, crafting, quests, story, family simulation, dialogue, NPC AI, large yard, achievements, complex saving, multiplayer, or procedural produce. No second appliance, automation, heat-management skill tree, or additional upgrade. Local restart and observation logs are sufficient.

## One progression moment

After the sixth peeled pepper reaches the tray, illuminate a single switch on the empty chushkopek. Clicking it enables the two existing extra sockets: **one roasting pepper at a time becomes three**. Roast speed, grace window, steaming capacity, peeling effort, and controls remain constant.

The player still handles and peels every pepper, but can interleave the stages. Test whether this feels more capable and satisfying rather than merely busier. The switch is not a shop or a tutorial chain.

## Completion moment

The eighteenth prepared pepper fills the last tray position. The empty crate, full tray, and quiet chushkopek remain visible; a restrained completion sound and “18 peppers prepared” confirm the result. Keep the camera under player control and allow inspection before offering continuation.

## Instrumentation / observations

Apply every [shared measure and gate](prototype-comparison-scorecard.md#shared-measures-and-decision-gates). Add only pepper-specific tags: state entered, heat stopped, burnt, strip begun/released, tray placement, and active roasting capacity.

- Record first full-cycle time, time per stage, and time the player has no useful available action. Distinguish watching an anticipated roast from bored waiting.
- Record confusion about readiness, false placement rejections, lost skin grabs, repeated three-failure sequences, and signs of frustration when juggling three peppers.
- Compare ordinary cycles before and after capacity increases, including quality errors and whether the player chooses to use all sockets.
- Capture spontaneous praise of roasting, steaming, peeling, and the whole rhythm separately from praise of the Bulgarian atmosphere. Ask which stages they would willingly repeat or skip.
- Measure acceptance and actual duration of the no-reward continuation. A smile at the grandfather or finished tray is not evidence of wanting another batch.

## Kill criteria

After the initial build and at most two bounded input/feedback/timing revisions, stop this concept for this project if any of these remains true:

- Ordinary-action appeal or voluntary continuation misses the shared gates: median appeal below 4/5, fewer than 4/6 naming an action to repeat, or fewer than 4/6 actually continuing.
- At least 3/6 want to skip roasting or steaming and describe the full cycle as waiting for peeling, even if peeling alone receives praise.
- The capacity increase still fails the shared progression gate because it creates monitoring stress or leaves the enjoyable action unchanged in a way players cannot perceive.
- The peel cannot feel controlled within the declared effort cap, or the build cannot meet the common agency, reliability, and clarity gates after those refinements.

Do not compensate with family dialogue, jokes, recipes, a cellar, extra rewards, or more upgrades. A broken build is inconclusive on enjoyment, but an unaffordable fix is a valid feasibility stop.
