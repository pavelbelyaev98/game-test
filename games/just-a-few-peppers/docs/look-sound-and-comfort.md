# Look, sound, and comfort

[Design index](readme.md) · Just a few peppers · current presentation targets

The opening pairs an unreasonable quantity of peppers with a small appliance. The ending shows the same outdoor Bulgarian property open, winter food stored, family parcels waiting, and a meal under the vine.

## Give the yard an identity

Source standard props, textures, sounds, and UI from free commercially usable assets under the [asset policy](development/unity-and-assets.md). Prefer a coherent set of reusable assets; reserve custom or kitbashed work for distinctive equipment and interaction needs. Early graybox scenes use simple placeholders, with enough feedback to judge the repeated actions.

Use vine shade, enamel basins, wooden/plastic crates, patched outdoor tables, old garden chairs, handwritten labels, and a street gate. Returned jars, older preserves, an open refrigerator used as a tool cupboard, a decorative grinder, and family photos add history.

Keep a few unimportant props physically interactive but non-essential: balls, loose buckets, canisters, old crates, and weathered paper cards can be nudged or picked up visually by the player but do not become inventory or quests.

If used, paper cards are read-only flavor objects with no counters, no quest links, and no economy integration. They should reinforce neighborhood scale and time passage only.

These are selected household details supported by the [research](research-and-authenticity.md), not a claim that every Bulgarian home looks alike. Roasting and preparation stay outdoors; the shed and cellar are compact views, with no indoor cooking level.

Background props have no interaction highlights, collectible prompts, or task counters. Existing lyutenitsa and other preserves belong to the old pantry display. Newly produced food consistently reads as roasted-pepper jars; varied lids and jar shapes do not imply multiple recipes.

## Prioritize the repeated actions

| Action | Visual response | Sound and feel |
| --- | --- | --- |
| Scoop | A local clump enters the carrier and the pile silhouette changes immediately. | Soft pepper contacts and a container-edge scrape. |
| Fill | A readable group grows inside the carrier without blocking forward vision. | Denser contacts and a restrained full cue. |
| Dump | A deliberate tilt releases a short cascade into a broad target. | Distinct impacts followed by an empty-container finish. |
| Process | Feeder motion, short roast/rest/preparation stages, and jars accumulating. | Sizzle, clunks, and a completion cue rather than an urgent alarm. |
| Deposit | The carried jar group settles through the rack handoff and the carrier empties. | One satisfying group clink; nearby winter-food progress changes. |
| Reveal | A useful wheel, path, or machine becomes readable as peppers disappear. | Brief acknowledgement; no repeated camera takeover. |

Pile forms can vary between shallow spill, mound, filled crates, and a slumped sack. They use the same gathering rules. Do not make a tiny invisible leftover block completion.

Try authored pile stages, grouped contents, and a small pool of moving peppers. The transition from pile to carrier to cascade is the expensive visual uncertainty. Avoid unrestricted physics for every pepper and simulated glass breakage.

When the player repeatedly tries invalid actions (full load, empty ground, blocked target), use one short, subtle cue and suppress repeats while input is held. For one invalid state, show one feedback cue, then stay quiet until state changes.

## Grandpa's equipment

The [three station stages](yard-and-progression.md#grandpas-three-equipment-stages) reuse recognizable materials and shapes: modest feeder, oversized folded rack and tipping guide, then the substantial homemade processor. The final machine earns one memorable tarp reveal and a larger dump.

A lever, bracket, or enclosed feed can be an authored animation. There are no component controls or repair prompts. The final reveal exposes/extends a fixed intake close to the remaining supply while feeding the same logical station. Make that shorter route immediately readable using the established dump cue; the output dock and handoff rack stay fixed. The final output visibly supports a larger batch. Spectacle and faster hauling must reinforce the same scoop/carry/dump controls.

Smoke rises into open air and clears the targets quickly. The final machine can become briefly louder during its first impressive load; sustained smoke, shake, and noise must not make ordinary handling uncomfortable.

## Food and the ending

Use the [four progress-derived food displays](household-readiness-and-parcels.md#progress-drives-presentation) and the one meal transition. Cellar and parcel props are views of stored work. They do not become extra carried objects or require animated NPC transport.

Name the sole deposit point **Finished Food Handoff Rack** in English draft signage and guidance. Its feedback should make clear that the player's responsibility ends at the deposit and household distribution is automatic. Cellar/family-box displays receive no interaction highlight or delivery prompt.

Render today's jar groups separately from pre-existing food. Show the current display immediately on load, and handle skipped milestones with one current-state update. The deposit has its own immediate feedback even when it does not cross a display threshold.

The vine table keeps its work dressing until all harvest is cleared and stored. The last accepted deposit automatically gives a brief completion readout (for example, **DAY COMPLETE**) while the machine settles, then starts the short transition placing the cloth and meal, with Grandpa's bottle/gift.

Keep this as a payoff beat only: one short pause before ending, then family-meal reveal. Ordinary work uses a separate bench; there is no table action or meal-preparation chore. These display/ending assets are outside the M1–M2 interaction prototype.

## On-foot movement

Walking, sprinting, and jumping belong in the opening foundation. WASD/arrows move at 3.2 m/s; holding either Shift key sprints at 5.4 m/s, including sideways/backwards movement. Normalize diagonals. Space requests one jump per press, approximately 0.8 m high under 20 m/s² gravity. Keep directional control in the air and allow sprint-jumps. There is no stamina resource, automatic repeat while holding Space, or midair second jump.

Accept a jump up to 0.10 seconds after leaving an edge and remember a press up to 0.12 seconds before landing. These small grace periods help ordinary timing; they do not permit repeated midair jumps. Stop upward velocity at ceilings, keep collision steps small during slower frames, and keep the player within the yard even when jumping from props. Graybox pile/appliance colliders should follow their visible meshes.

Pause/focus loss freezes a jump in place; explicit resume continues its arc. Discard pending jump requests on pause and reset, and require Space release after menu activation/resume/reset. Returning to the gate clears vertical motion and jump timing. Retain a steady camera without sprint FOV changes, head bob, or landing shake. Tunable movement values live on the scene's player component; user settings/rebinding remain in task 7_02. Carrier-specific handling will be defined with the carrier tasks; movement must never bypass progression gates or alter food quantities.

## Comfortable controls and atmosphere

- Broad scoop and transfer targets; immediate response to valid input.
- Hold/toggle alternatives; no rapid individual clicking.
- Stable camera, clear forward view, easy wheelbarrow turning and reversing.
- Adjustable sensitivity/FOV, invert-look choice, readable text, and separate sound volumes.
- Head bob and shake off by default; pause in menus and on lost focus.
- Recover a carrier with existing contents to a valid resting point.
- Simple status cues: Collect, Carrier full, Tip load, Working, Output full, Hand off food, Day complete. Completion is feedback, not a new player command.
- Destination symbols and optional hints; no dependence on pepper color for rules.

No voice acting is required. Use readable text for all dialogue moments and occasional non-verbal grandpa noises (for example, a short **AKUAAH** grunt) as an occasional atmosphere cue. Keep these cues sparse.

Use original or licensed radio material, birds, insects, wind, distant yard activity, and gate/yard objects. Leave long quiet intervals and prioritize action feedback over dialogue. No crowd behavior, new music rights assumptions, or radio mini-game is needed.

The strongest candidate clips are a wheelbarrow emerging, a large dump into Grandpa's absurd machine, and the same cluttered yard becoming a place for the family meal. They are presentation goals, not evidence of demand.

