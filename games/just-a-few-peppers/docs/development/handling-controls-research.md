# Physical grabbing and release research

Researched September 6, 2026, following the developer's rejection of the loose-prop handling feel. This note preserves the reference comparison and proposal from that research pass. The developer subsequently requested implementation; the [1_06 revision delivery](tasks/1_06_loose-yard-objects-and-playful-handling.md#physical-handling-revision-delivery--september-6-2026) owns the resulting build, checks and pending human feedback. The [core handling contract](../core-loop-and-mechanics.md#pick-up-place-and-play) owns current behavior.

## What the references establish

These are developer notes and firsthand player/reviewer reports, accessed on the date above. The games were not installed or played during this research. Exact bindings are reported only where a source names them; a community guide is not verification of the current executable.

| Reference | Evidence | Useful lesson for this game |
| --- | --- | --- |
| How to Make an Atomic Bomb in Your Garden | The [official store](https://store.steampowered.com/app/3700980/How_to_Make_an_Atomic_Bomb_in_Your_Garden/) identifies the backyard game. AwkwardTaco's June 10 [firsthand key guide](https://steamcommunity.com/sharedfiles/filedetails/?id=3742335422) reports left click to interact, right click to grab and G to drop. It does not establish a throw gesture or whether grabbing is held/toggled; Steam also exposes an incompatibility/removal notice on the guide, so treat its mapping as provisional evidence. | This is close to the developer's suggested split between using and grabbing. G can coexist with that split; changing G alone cannot establish physical feel. |
| Schedule I | March 30, 2025 [player answers](https://steamcommunity.com/app/3164500/discussions/1/599648576856936768/) describe holding right click to pick up empty furniture. This source concerns relocating equipment, not unrestricted in-hand physics or throwing. | Separate grabbing from operating an object. Do not infer that this furniture command supplies the ball behavior we need. |
| Crime Scene Cleaner | In [Moving Things](https://www.reddit.com/r/CrimeSceneCleanerGame/comments/1skqmiu/moving_things/), players describe tossing bags toward the truck or through windows and retrieving them later to shorten repeated carrying. They also report fatigue from hauling everything individually. Exact PC grab/release keys were not reliably established. | Physical objects can be staged, dropped and thrown as useful shortcuts. Preserve bulk work and the nearby generous food handoff instead of creating compulsory individual hauling. |
| Recycling Center Simulator | Kyle McGrath's [firsthand PC review](https://www.thumbculture.co.uk/recycling-center-simulator-pc-review/amp) describes throwing scrap into the van and onto a conveyor, with a separate click-and-drag sorting interaction. In a June 2024 [developer response](https://steamcommunity.com/app/2928520/discussions/0/6193093297595965519/), Balas says it plans to change bin throwing after players found loading machines frustrating. That is historical feedback, not a claim about a present defect. | Support playful throwing and useful physical transport, with forgiving machine transfers. Do not make food completion depend on accurately throwing a container into a target. |

The recommendation below is an inference from these patterns and the developer's feedback. There is no demonstrated universal control layout across these games.

## Why the pre-revision yard felt rigid

At the research checkpoint, [LoosePropHandling.Release](../../unity/Assets/JustAFewPeppers/Runtime/LoosePropHandling.cs) used a queried supported pose for the usual E release. [PortableBody.SetPose](../../unity/Assets/JustAFewPeppers/Runtime/PortableBody.cs) cleared linear/angular velocity and explicitly called `Sleep()` for careful placement, including the ball. G released at the held pose and woke physics; right mouse tossed a prop. The default interaction emphasized a motionless arranged result, while the physical alternatives used other buttons. The linked source now contains the subsequent revision.

This is not permanent removal of physics: released bodies are dynamic and can wake. Unity's pinned [6000.6 Sleep documentation](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Rigidbody.Sleep.html) describes sleeping until woken. Nevertheless, the default placement path explains the reported mismatch and should not be the ordinary release rule for playful objects.

The [scene tests](../../unity/Assets/JustAFewPeppers/Tests/PlayMode/LoosePropSceneTests.cs) at that checkpoint checked that every carefully placed sample settled near its chosen pose after 0.6 seconds, with separate drop/toss checks. Those passes validated the earlier arrangement rules; they did not establish that the usual controls felt like grabbing physical objects.

## Proposed controls for the revision

This was the proposed prototype mapping. The developer's later “looks better now implement next task” authorizes trying it; it does not establish acceptance of the new build's feel. The developer had suggested both mouse grabbing and retaining E interaction/LMB gathering/G drop, so the mapping remains open to play feedback.

| Input | Candidate behavior |
| --- | --- |
| Right click | Grab the reachable portable object; click again to release it from the hand with physics. Start with a toggle so carrying does not require continuously gripping the mouse. |
| Left mouse | Hold to gather while carrying the raw crate. With a loose prop held, hold/release to throw it with bounded strength. Latch this intent when the press starts: looking away during gathering must never turn the same press into a throw. |
| E | Operate the targeted machine or explicitly tip/collect/hand off food. Secondary careful set-down can remain available where useful for crates, with a clear contextual action; it must not override ordinary physical release. |
| G | Optional duplicate of ordinary release, not the only accessible drop action. |
| F1 | Existing optional full controls/debug reference. Keep normal gameplay quiet and placement indicators hidden. |

A hold-to-grab variation can be compared if toggling still feels wrong; do not add a second control mode or a new settings system during this correction. Existing M7 tasks own persistent options. Rotation remains optional and must not be required to put an ordinary object down.

## Behavior to prove before moving on

- Ordinary release uses the current reachable hand position, allowing a fall without first finding a valid supported placement pose. Keep collision protection and prevent player launch. Regrabbing another target must not silently park the held object somewhere nearby.
- Carry motion should produce bounded, understandable release motion; standing still should give a gentle drop. A ball should fall, bounce or roll when height, momentum, contact or slope warrants it. A ball at rest on a level surface need not move artificially. Let physics settle and sleep naturally.
- Boxes and loaded carriers should be easy to lower onto a surface and leave stable. Any retained placement assist is secondary, adds no throw velocity, and does not pin a playful object in place. A later collision or removed support should still matter.
- Keep exact food ownership and explicit food transfers. Releasing a carrier is not tipping, handoff or a food reset. Preserve lost-object recovery, pause/focus freezing and fresh-input protection.
- Replace the blanket motionless-prop expectation with ordinary-input checks for a falling/released ball, a moving release, a roll on a slope, contact after settling, a stable box/stack and support removal. Verify those in the ordinary packaged player, retain food/pause/recovery regressions, and request human feedback about control clarity and physical feel separately.

This correction belongs to existing 1_06 shared handling. Individual/bulk physical peppers remain 1_07, direct operation/grouping remains 1_08, and no new task or mechanic is selected by this research pass.
