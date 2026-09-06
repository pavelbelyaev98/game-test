# Basement Cleanout — Prototype Decision Brief

Status: proposed interaction experiment; not implemented or playtested. No production decision.

Use the [shared scorecard](prototype-comparison-scorecard.md) for build conditions, the six-player protocol, identical evaluation gates, and the limit of two bounded revisions. The existing research suggests that removal, growing order, and meaningful power increases can carry play; discoveries must not conceal a weak ordinary action.

## Core hypothesis

Pulling ordinary clutter free and revealing usable space feels satisfying enough to repeat even when no rare or valuable reward appears.

## Player fantasy

Turn a cramped, neglected storage room into a usable place by physically freeing stuck clutter, clearing surfaces, and finally reaching a cabinet. Feel the room open up through work the player controls.

## Exact 5–10 minute loop

Task instruction: **“Clear the three clutter groups and open the cabinet.”** Do not advertise hidden finds.

| Target time | Player activity | Evidence sought |
| --- | --- | --- |
| 0:00–1:00 | Learn to select, pull, and release an exposed ordinary item in group A. | The action is legible and responsive. |
| 1:00–3:00 | Finish group A using individual pulls. This group contains no discovery. | Ordinary clearing sustains interest on its own. |
| 3:00–3:30 | Activate the group-pull ability. | Understand the sole power increase. |
| 3:30–5:30 | Clear group B; a small keepsake is exposed around its midpoint. | Group pulling retains tactile satisfaction; interest does not exist only at the reveal. |
| 5:30–8:00 | Clear group C from the cabinet frontage, then open the cabinet. | Visible access is a useful, earned transformation. |
| 8:00–9:00 | Inspect the cleared room and the second keepsake inside the cabinet. | The work has a clear ending. |

The content and suggested sequence are authored; times are targets, not timed gates. Place A nearest the starting view, but allow players to approach B or C early. Tag their actual order in observations; a player who clears A later still supplies the no-discovery comparison. The upgrade unlocks after 24 ordinary removals anywhere, leaving 48 objects on which to try it even if the player changes order.

Start with 24 objects per group, 72 total, duplicated from six prop types. Each pull should take only a comfortable short gesture, roughly 1–2 seconds including its resistance, with no mandatory pause afterward. Do not add objects or slow the gesture simply to force the session into five minutes. Record short completion and use the continuation probe to test appetite for repetition.

At completion, offer up to two more minutes by resetting group A alone with the group-pull ability retained. It contains the same ordinary props and no discovery. This explicit continuation reset is separate from full-scene restart.

## Core interaction

- **Camera and hands:** First-person mouse look and WASD in one room. An inexpensive glove/grip viewmodel or contact marker represents the hand; no full-body animation is needed.
- **Select:** Aim at an exposed prop within reach. A tight outline and grip icon identify the exact item. Hidden colliders cannot steal selection through the visible object.
- **Pull:** Hold LMB and drag the mouse toward the player along a broad screen-space pull direction. Freeze mouse look during the drag. The object moves along a short authored escape path in proportion to valid drag movement; holding the button still does not finish it.
- **Resistance/release:** A small initial displacement meets visible resistance. Continued movement loosens the item, produces a release sound, and completes the pull. Direction tolerance is generous; this is not a precision gesture test.
- **Deposit:** On release, the item follows a short visible arc into a nearby collection bin and becomes part of its simplified contents. This support step is automatic from the first object onward. The next object is immediately selectable.
- **Cancel/recover:** Releasing LMB before the release point returns the prop smoothly to its assigned pose and clears pull progress. RMB also cancels. There are no loose carried objects to lose and no penalty for trying another exposed item.
- **Group pull:** After the sole upgrade, the same gesture selects up to three nearby exposed eligible props, all visibly outlined before commitment. It never grabs through an obstructing layer. Their releases are slightly staggered, so the player sees what their pull freed.
- **Cabinet:** Once its frontage is clear, click the handle to play a short hinge animation. The cabinet has no key requirement; clearing access is the task.

Allow several exposed choices within a group. Authored dependencies should correspond to visible overlap, not an arbitrary single correct removal order. A blocked target gives a brief obstruction cue; it must not look selectable and then silently reject input.

## Minimum scene

One small basement with three visibly distinct clutter areas: a shelf/floor corner, a workbench area, and the cabinet frontage. Keep the collection bin close enough that disposal arcs stay readable. A doorway or marked standing position provides a useful view of the room before and after clearing.

## Minimum content

| Content | Required amount / states |
| --- | --- |
| Ordinary prop types | Six: cardboard box, plastic bottle, tin can, old book, cloth bundle, scrap panel. Reuse them across all three groups. |
| Authored clutter | Three groups of 24 instances, arranged in a few visibly overlapping layers. Group A has no find. |
| Prop states | Blocked, Available, Pulling, Removed. Blocked/Available follow the authored obstruction dependencies; cancellation returns to Available. |
| Hidden finds | Two nonvaluable keepsakes: a photograph revealed in B, and a keyring inside the cabinet. Each changes from Concealed to Revealed; no rarity roll or price. |
| Cabinet | One; Inaccessible → Accessible → Open. Its handle becomes reachable when C is clear. |
| Collection bin | One; a few authored fill stages represent deposited clutter. Capacity never blocks work. |

Finds remain where revealed for inspection. They need no inventory, valuation, collection screen, or written backstory. The photograph is already excluded from the clutter target set, so it cannot accidentally be discarded.

## Game-feel requirements

| Element | Minimum honest test |
| --- | --- |
| Anticipation | A visible overlap or contact point explains resistance; slight separation shows that a pull is working. |
| Animation | The selected prop shifts, strains, and follows a plausible short escape path before a readable disposal arc. Remaining layers settle through small authored motions. |
| Resistance | Early movement is restrained, then loosens into release. Mouse motion continues to matter; a timer does not complete the action by itself. |
| Audio | Material-specific scrape or rustle under tension, a distinct release, then a bin impact. Group releases produce several controlled beats without an overwhelming volume spike. |
| Particles/VFX | Brief dust at contact/release and tiny debris accents; no cloud obscuring the next target. |
| Completion feedback | Every removal exposes a real surface or another accessible object. A group-clear sound acknowledges a larger change. |
| Camera feedback | Keep the view stable. Use object motion and a subtle grip/viewmodel response for force, without forced shake or camera lunges. |
| Before/after | Clear shelves, floor, workbench, and cabinet access replace distinct silhouettes of clutter. Lighting stays comparable so the change comes from removal. |

## Technical implementation proposal

Use static or kinematic clutter with primitive colliders, a local prop-state array, and a few authored blocker references per instance. When an item is removed, update the affected neighbors' availability. A small scene controller owns group completion, the one ability flag, bin fill, and cabinet access.

Each prop type uses a shared pull profile; individual instances provide an escape direction and one or two path control points. Drag progress drives the pose. Check the authored path for obvious wall/neighbor penetration during setup. Do not build a general collision-solving grab system. Remove or disable the prop's interaction collider once the disposal animation starts.

Group selection takes at most three available props in a small radius around the aimed item and freezes that selection for the gesture. Move each along its own authored escape path, with a short release stagger. One removal event per prop prevents duplicate counting. Animated bin fill is cosmetic and has no item physics.

Build one no-find group first, including the eventual group-pull gesture, before dressing the full room. In the packaged build, check cancellation, attempts on occluded targets, mixed-size group pulls, unusual clearing orders, cabinet access, and both reset modes. An exposed valid target must always remain available until the group is empty.

## Technical unknowns

**Single most dangerous uncertainty:** Can authored pull paths across differently shaped clutter feel like believable resistance and release without obvious clipping, teleporting, or a full physics pile?

## Design unknowns

**Single most dangerous uncertainty:** Is ordinary removal enjoyable, or does the player merely endure it to discover an item or finish emptying the room?

## What NOT to build

No economy, selling, price appraisal, rare-loot tables, inventory, crafting, renovation, story, quests, NPC AI, large building, achievements, complex saving, multiplayer, procedural room generation, or additional tools. No manual bin-emptying trips. No hundreds of uncontrolled Rigidbody objects or reusable destruction framework.

## One progression moment

The twenty-fourth ordinary removal unlocks one visible control to enable **up to three objects per pull instead of one**. In the suggested order, this coincides with clearing group A. From that moment, the same input and resistance/release action works on a small outlined cluster. Single-object pulls remain possible wherever only one object is eligible.

Do not combine this with faster pull speed, longer reach, automatic room clearing, or larger rewards. Compare ordinary pulls before and after; the upgrade must increase the feeling of productive clearing while leaving a meaningful gesture.

## Completion moment

Completion requires all three groups cleared and the cabinet opened, in either order. In the suggested sequence, opening the newly accessible cabinet reveals the second keepsake and completes the task. If it was opened earlier, the final group removal triggers completion instead. The visible floor and surfaces remain clear, the bin looks full, and a restrained “Room cleared” cue closes the task. Do not force a cinematic or tease another area. The cabinet and room transformation should matter even if the keepsake is uninteresting.

## Instrumentation / observations

Apply every [shared measure and gate](prototype-comparison-scorecard.md#shared-measures-and-decision-gates). Add group ID, prop type, selected count, blocked target, reveal, and cabinet state to the minimum event record.

- Observe voluntary repetition in group A and the no-find continuation. Record whether the player keeps clearing after the first photograph appears.
- Mark confusion about exposed/blocked items, missed grabs, accidental multi-selection, repeated failed pulls, and whether the disposal arc interrupts the next action.
- Record boredom and frustration separately: repetitive empty clicking, uncertainty about what can move, and resistance that feels arbitrary have different causes.
- Compare ordinary removals before and after group pulling, including whether the player can predict which objects will move.
- Capture praise of scraping, release, group pulling, and newly clear surfaces separately from praise of a hidden object. Ask which action they would repeat without another find.
- Record actual group order. If a player has seen a discovery before A, do not label their first no-find period as free from reward expectations; the explicit no-reward continuation is the stronger check.

## Kill criteria

After the initial build and at most two bounded input/feedback/timing revisions, stop this concept for this project if any of these remains true:

- Ordinary-action appeal or voluntary continuation misses the shared gates: median appeal below 4/5, fewer than 4/6 naming an action to repeat, or fewer than 4/6 actually continuing.
- At least 3/6 say they would only continue for another discovery and cannot identify an ordinary removal or transformation they want to repeat. A positive reaction to the two finds does not pass the hypothesis.
- Group pulling still fails the shared progression gate because it makes the pile disappear without satisfying involvement, or players cannot reliably predict the selected objects.
- Believable pulling cannot fit the declared effort cap without replacing the authored setup with a large physical pile, or the common agency, reliability, and clarity gates remain unmet after refinement.

Do not rescue weak pulling with valuable loot, a mystery, NPC requests, crafting, additional rooms, or more upgrades. Technical interruptions are inconclusive on enjoyment; an unaffordable representation is a feasibility stop.
