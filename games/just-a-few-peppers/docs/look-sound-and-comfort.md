# Look, sound, and comfort

[Design index](readme.md) · Just a few peppers · current presentation targets

The opening pairs an unreasonable harvest with a modest apparatus. Food preparation and the growing stockpile supply the goal; physical handling and directly operated inventions supply the action. Most of the outdoor Bulgarian property is accessible immediately. Completion leaves food stored and normal control available; optional table/gift presentation remains cuttable.

## Give the yard an identity

Source standard props, textures, sounds, and UI from free commercially usable assets under the [asset policy](development/unity-and-assets.md). Prefer a coherent set of reusable assets; reserve custom or kitbashed work for distinctive equipment and interaction needs. Early graybox scenes use simple placeholders, with enough feedback to judge the repeated actions.

Use vine shade, enamel basins, wooden/plastic crates, patched outdoor tables, old garden chairs, handwritten labels, and a street gate. Returned jars, older preserves, an open refrigerator used as a tool cupboard, a decorative grinder, and family photos add history.

Portable cultural props use the [shared grab/place/drop interaction](core-loop-and-mechanics.md#pick-up-place-and-play). A basin, stool, empty crate, or ball can be moved, arranged, stacked where stable, and played with. The small prototype sample starts in 1_06; M5–M6 replace it with suitable assets and apply the same behavior to exposed loose props. Attached fixtures are visually distinct. These objects carry placement state, not household objectives or food credit.

Readable paper cards are a deferred flavor candidate, not a remedy for a boring core. If separately authorized, they remain read-only objects with no pickup inventory, counters, achievements, saving requirements, quest links, or economy integration.

These are selected household details supported by the [research](research-and-authenticity.md), not a claim that every Bulgarian home looks alike. Roasting and preparation stay outdoors; the shed and cellar are compact views, with no indoor cooking level.

Movable props receive consistent grab guidance when targeted; attached scenery does not promise a grab action. Avoid collectible markers and task counters. Existing lyutenitsa and other preserves belong to the old pantry display. Newly produced food consistently reads as roasted-pepper jars; varied lids and jar shapes do not imply multiple recipes. Loose empty jars may be props; progress-derived stored-food groups are displays, with staging/signage that distinguishes the two.

## Prioritize the repeated actions

| Action | Visual response | Sound and feel |
| --- | --- | --- |
| Pick one / gather several | One clearly targeted physical pepper moves, or the previewed reachable set enters the container; the source loses the same material. | Deliberate single pickup and efficient bulk gathering have distinct prompts and soft contacts. |
| Fill | A readable group grows inside the carrier without blocking forward vision. | Denser contacts and a restrained full cue. |
| Place / drop | Rotate a held object, see where it fits, set it down carefully or release it to fall and settle. | Appropriate contact sounds and restrained placement help; no forced socket animation. |
| Dump | A deliberate tilt releases a short cascade into a broad target. | Distinct impacts followed by an empty-container finish. |
| Operate / process | Input directly moves a substantial handle/rack, then compressed internal stages produce visible food. | Mechanical contacts, changing load sound and a restrained finished cue; no doneness alarm. |
| Group / receive | The player-controlled guide gathers loose prepared food into a neat batch in the existing carrier; exact gesture remains provisional in 1_08. | Immediate motion/contact response and restrained glass clink; partial batches remain usable without individual filling/capping. |
| Deposit | The carried jar group settles through the rack handoff and the empty carrier returns; nearby stored-food volume/fill grows even between household milestones. | One satisfying group clink and visible accumulation readable from the work area. |
| Purchase / install | One complete purchased loading kit appears beside a large readable mount; forgiving placement snaps it into place. Card/mount distinguish awaiting installation from installed. | Clear snap/sound and a small mechanism response, without taking over the camera; the next appropriate batch shows the gain. |

Pile forms can vary between shallow spill, mound, filled crates, and a slumped sack. They use the same gathering rules. Do not make a tiny invisible leftover block completion.

Prioritize the [complete finished-batch payoff](core-loop-and-mechanics.md#make-the-finished-batch-worth-handling) before broad environment decoration. M1 uses recognizable simple food/jar groups; M5 adds glossy prepared red-pepper shapes visible through jars, coherent lids, appetizing color/materials and a carrier whose fill has readable substance without obstructing sight or adding sluggish controls. Preserve the tested player-controlled grouping gesture within receiving; automatic detail supplies filling/capping without manual jar/lid alignment or extra product rules. Avoid expensive glass rendering unless it materially helps the tested view.

Careful placement preserves the intended orientation and settles confidently where the carrier fits. Tune support/collision and settling so useful handling does not become wobble, repeated rejection or contents scattering; deliberate drops and the ball can remain playful. Prompt priority follows the intended reachable carrier/target, with no random container rotation imposed during work and no decorative jar stealing the interaction. This requires dependable behavior, not a new settings menu or a blanket physics ban.

Require the representative scattered physical peppers and deliberate single/bulk actions in [1_07](development/tasks/1_07_physical-pepper-batch-comparison.md); compare manageable active batches with grouped resting/distant representation without removing those actions. Match visible volume/contact to exact ownership, using sleeping/reuse and bounded active motion where useful. Carriers and loose objects can be important physical gameplay objects. Mechanisms may use constraints or controlled motion. Recover spills as the same registered food. Unlimited active harvest simulation is unnecessary; cosmetic-only nearby handling is insufficient. No glass-breakage cleanup chore is added.

When the player reaches or repeatedly tries an invalid state (full load, empty ground, blocked target), use at most one short, restrained cue per meaningful state transition. Holding an input must not spam denial sounds. Persistent visual status stays readable without sound.

Use birds, wind, modest work sounds, and sparse character remarks. Do not force a radio, constant grunting, or a dense ambience loop. Every scoop visibly changes the touched region, and every committed handoff gives immediate completed-work feedback even when no large display milestone changes.

## Grandpa's equipment

The [equipment stages](yard-and-progression.md#grandpas-three-equipment-stages) develop a modest apparatus through useful chosen attachments into an excessive powered conversion. Reuse familiar materials, patched brackets and recognizable roasting hardware, but make each conversion physically useful. Show both prototype offers, price and exact effect at the same nearby bench; stored food and Coins need clearly separate labels.

A substantial lever or rack is directly operated under the [machine contract](core-loop-and-mechanics.md#operate-the-machine). The mechanism should follow input and convey material/load movement; cosmetic gears may be controlled animation. The powered conversion changes useful operation/output handling. A tarp or local feeder extension may support its installation, but no mandatory hidden reveal, new route or distant-intake hauling requirement applies. The [one complete attachment snap](core-loop-and-mechanics.md#attach-a-purchased-improvement) is the permitted installation interaction; no component shopping, bolt/wiring puzzles, repair prompts, fuel or jams.

Use a complete price/effect/status/location card at the same bench. The loose kit may be placed freely before fitting; the authored machine mount is visibly a mechanical fit, not a rule for carrier placement. Keep the kit and instructions next to that mount, with broad alignment and no precision rotation. If fitted during work, show the pending safe boundary without hiding operation/output guidance. Quiet, persistent state and visible snap motion must also explain success with sound muted.

Smoke rises into open air and clears the targets quickly. The final machine can become briefly louder during its first impressive load; sustained smoke, shake, and noise must not make ordinary handling uncomfortable.

## Food and harvest completion

Use the [four progress-derived food displays](household-readiness-and-parcels.md#progress-drives-presentation). Cellar and parcel props are views of stored work. They do not become extra carried objects or require animated NPC transport.

Name the sole deposit point **Finished Food Handoff Rack** in English draft signage and guidance. Its feedback should make clear that the player's responsibility ends at the deposit and household distribution is automatic. Cellar/family-box displays receive no interaction highlight or delivery prompt.

Render today's jar groups separately from pre-existing food. Show the current display immediately on load, and handle skipped milestones with one current-state update. The deposit has its own immediate feedback even when it does not cross a display threshold.

Keep the nearby stored-food group in a useful sightline from the machine/handoff. Its fill/volume follows every accepted deposit under the [combined display contract](household-readiness-and-parcels.md#progress-drives-presentation), while the four household states frame larger milestones. Integrate those views without another food inventory or a compulsory walk to inspect progress.

The last valid deposit gives a quiet, nonmodal acknowledgement such as **Harvest complete** or **All peppers prepared** while machines settle into idle. Keep normal camera and movement control, leave the complete displays visible, and let the player leave through ordinary pause/menu controls.

The vine table may later change from work dressing to a simple food/gift tableau, but that flourish is cuttable and cannot pause control, start a required cinematic, replay on load, or add table/meal preparation. Ordinary work uses a separate bench. These display/presentation assets are outside the M1–M2 interaction prototype.

## On-foot movement

Walking, sprinting, and jumping belong in the opening foundation. WASD/arrows move at 3.2 m/s; holding either Shift key sprints at 5.4 m/s, including sideways/backwards movement. Normalize diagonals. Space requests one jump per press, approximately 0.8 m high under 20 m/s² gravity. Keep directional control in the air and allow sprint-jumps. There is no stamina resource, automatic repeat while holding Space, or midair second jump.

Accept a jump up to 0.10 seconds after leaving an edge and remember a press up to 0.12 seconds before landing. These small grace periods help ordinary timing; they do not permit repeated midair jumps. Stop upward velocity at ceilings, keep collision steps small during slower frames, and keep the player within the yard even when jumping from props. Graybox pile/appliance colliders should follow their visible meshes.

Pause/focus loss freezes a jump and released-object motion in place; explicit resume continues them without a physics burst. Discard pending jump/release requests on pause and reset, and require fresh input after menu activation/resume/reset. Returning to the gate clears vertical motion and jump timing. Retain a steady camera without sprint FOV changes, head bob, or landing shake. Tunable movement values live on the scene's player component; user settings/rebinding remain in task 7_02. Carrier tasks provide free placement while preserving food quantities. A physically reachable shortcut is valid; yard bounds and actual occlusion replace invisible route-order gates.

## Optional later upgrade and lighting presentation

An authored machine sketch/outline could appear at the known mount and resolve into the already-defined attachment. This is a later presentation alternative, not arbitrary drawing recognition, generated functional machinery, a runtime AI/network service or a CAD system. Start by testing the physical snap. Do not require drawing and physical assembly for the same upgrade; the candidate creates no automatic task. A later hero conversion may use a few chunky modules only if the prototype evidence warrants it.

Future lighting changes may supply atmosphere without affecting food, Coins or available work. Day/night presentation is not required in the prototype. Add no clock obligation, stress resource, compulsory rest, bedtime, daily reset or night-only production. Completion remains the final food handoff and leaves access to the finished scene.

## Comfortable controls and atmosphere

- Follow the early [single/bulk/placement/pouring control contract](core-loop-and-mechanics.md#single-bulk-placement-and-pouring-controls): show the intended target and teach one action at a time with brief contextual guidance. Occlusion and contents/container priority must match the visible intention. Broad transfer targets respond immediately.
- Offer deliberate single-pepper pickup and explicit hold-left-mouse bulk gathering; release stops bulk pickup. Preview affected peppers/destination before commit. No hidden pickup mode, unexplained modifier or compulsory individual clicking for the entire harvest.
- Stable camera, clear forward view, easy wheelbarrow turning and reversing.
- Adjustable sensitivity/FOV, invert-look choice, readable text, and separate sound volumes.
- Head bob and shake off by default; pause in menus and on lost focus.
- Rotate when useful and place objects where they fit without throw velocity. Keep placement outlines and continuous valid/blocked text hidden; bulk affected-set preview serves selection only. Clearly distinguish careful placement, pouring and deliberate drop/toss across portable props.
- Recover a carrier with existing contents to its last safe pose or a clear fallback location.
- Simple cues: Pick one pepper, Gather peppers, Place, Tip load, Operate rack, Working, Group finished food, Hand off food, Winter food stored, Coins, Upgrade available, Winter preparation complete. Completion is feedback, not an extra command. Show ready/blocked operation without repeated alarms.
- Destination symbols and optional hints; no dependence on pepper color for rules. Purchases stay at one bench; any separately approved machine settings stay on the machine. No phone interface.
- Player-facing pile guidance says Pepper pile / Peppers left; “mound” is not a separate mechanic.

No voice acting is required. Use readable text for all dialogue moments and occasional non-verbal grandpa noises (for example, a short **AKUAAH** grunt) as an occasional atmosphere cue. Keep these cues sparse.

Use original or licensed radio material, birds, insects, wind, distant yard activity, and gate/yard objects. Leave long quiet intervals and prioritize action feedback over dialogue. No crowd behavior, new music rights assumptions, or radio mini-game is needed.

Use the [comic-variety contract](jobs-events-and-comedy.md#optional-messages-and-comic-variety) for optional original messages, labels and authored reactions. A prototype gag needs no computer or mail system; later selected messages remain readable without stopping work. Keep operating instructions clear, allow muted play and suppress stacked joke replay after loading or skipped milestones.

The strongest candidate clips show physical material tumbling into the feeder, a directly moved mechanism, a chosen attachment improving the next batch, and substantial winter food accumulating. An optional family-table tableau is secondary. These are presentation targets, not evidence of demand.
