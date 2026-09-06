# Start developing Just a few peppers

**Ask the AI to deliver one numbered task, play it, describe what feels good or wrong, then repeat.** The [task queue](tasks/readme.md) maps the full current game into 31 concrete briefs, from 0_01 through 9_03. The [roadmap](roadmap.md) summarizes milestones; the queue records feature progress.

Current state: 0_01 planning is complete; gameplay tasks are Todo. Start at [1_01 — Unity foundation and walkable scene](tasks/1_01_unity-foundation-and-walkable-scene.md). The old roasting experiment is disposable. Tasks 1_01–1_05 together build the new first playable loop.

## Your first instruction

Open this repository in your AI coding workspace and paste the [fresh-chat prompt](new-chat-prompt.md). Its short form is:

```text
Follow games/just-a-few-peppers/docs/development/new-chat-prompt.md.
TARGET TASK: NEXT
Read AGENTS.md and the numbered task queue, recover the required context,
and implement one eligible task in chronological order.
Deliver an integrated scene/build and a short playtest checklist.
Record the work in the task, queue, and milestone status, then stop.
```

The first chat delivers a walkable outdoor scene with working input/pause and placeholder targets. The next tasks add scooping/carrying (1_02), tipping/processing (1_03), output/storage (1_04), and the complete M1 handoff (1_05). Simple visuals let us judge the actions before producing the full yard. You can replace NEXT with a particular ID; dependencies still apply.

A new chat reads the queue, selected brief, linked specs, and earlier delivery/feedback records. It does not need the old conversation. When you play, record the result through the AI so later chats can see it.

Open **`games/just-a-few-peppers/unity/`** in Unity Hub, using the version recorded in `ProjectSettings/ProjectVersion.txt` (currently `6000.6.0f1`). After implementation, open the exact new scene given in the handoff and press Play, or run the supplied Windows build. The current Stage0 scene is not that new scene.

## Who does what

| AI implementation agent | Pavel |
| --- | --- |
| Reads the current specs and implements one bounded step. | Chooses the next step and gives ideas or changes in direction. |
| Creates and connects scenes, prefabs, components, input actions, UI, materials, and build settings. | Opens the scene/build and plays. |
| Finds suitable free assets, checks their licenses, imports them, and fixes scale/materials/colliders. | Judges appearance and whether the interaction feels enjoyable. |
| Runs relevant technical checks, fixes failures, and records results honestly. | Reports confusing actions, bugs, boring waits, and desired improvements. |
| Updates status and leaves a reproducible playable handoff. | Decides whether to revise the step or move on. |

A handoff must include the exact scene/build path, controls, expected behavior, a 3–5 item play checklist, and known limitations. Loose scripts with a long list of Inspector chores are not a completed feature. If a login, license acceptance, or unavailable editor tool requires a user action, the AI should finish independent work and explain the exact remaining action; it must not claim to have tested an inaccessible editor.

## The implementation order and its specifications

| Step | What you will play or inspect | Specifications the AI uses |
| --- | --- | --- |
| M1 — Tasks 1_01–1_05 | Scoop, carry, dump, collect jars, store them, and reset a small scene. | [Numbered tasks](tasks/readme.md), [combined M1 contract](first-playable-task.md), [core loop](../core-loop-and-mechanics.md). |
| M2 — First useful upgrade | Uncover the wheelbarrow and compare the same work with crate and barrow. | [Roadmap](roadmap.md), [yard and equipment](../yard-and-progression.md), [feel gates](../scope-and-validation.md). |
| M3 — Save and continue | Quit and resume with food in different parts of the loop. | [Roadmap](roadmap.md), [state and saving](state-and-saving.md). |
| M4 — Whole graybox game | Clear the connected yard, discover the final machine, and finish the day. | [Yard](../yard-and-progression.md), [household and ending rules](../household-readiness-and-parcels.md). |
| M5 — Representative finished section | Play a short arc with intended art, sound, food displays, Grandpa, and the meal. | [Presentation](../look-sound-and-comfort.md), [story](../story-and-characters.md), [asset policy](unity-and-assets.md). |
| M6 — Finish the property | Play the compact full yard with scenery, selected dialogue, and tuned pacing. | [Yard](../yard-and-progression.md), [discoveries and comedy](../jobs-events-and-comedy.md), [scope](../scope-and-validation.md). |
| M7 — Menus and comfort | Try new/continue, bindings, camera/audio/display settings, and restart persistence. | [Roadmap](roadmap.md), [comfort](../look-sound-and-comfort.md), [settings persistence](state-and-saving.md). |
| M8 — Reliability and performance | Play the full candidate while the AI fixes blockers and profiles it. | [Verification](testing-and-performance.md), [state and saving](state-and-saving.md). |
| M9 — Release preparation | Inspect the final build, credits, and accurate store materials. | [Roadmap](roadmap.md), [release checks](testing-and-performance.md#release-candidate-check), [asset records](unity-and-assets.md). |

Every milestone is already split into numbered briefs in the [queue](tasks/readme.md), including art, saving, menus, performance, and shipping preparation. Each brief has dependencies, required specifications, work, acceptance criteria, and Pavel's check. The AI follows that plan one task at a time and refines it only when new evidence warrants a change.

## Give feedback, then continue

Play an ordinary load for 5–10 minutes, or less if the scene is shorter. Notice whether scooping responds where you aim, tipping feels deliberate, the next action is clear, and walking or waiting feels excessive. Try pause/reset and one incomplete load. You can give feedback in plain language:

```text
Revise 1_02 before advancing. Scooping feels weak and the crate blocks my view.
Make those actions clearer and more responsive, keep the current scope,
record my feedback in the task, and give me another playable handoff.
```

Once it feels good enough to build upon:

```text
I played 1_02. Gathering and carrying feel good enough to continue.
Record that feedback, then complete TARGET TASK: NEXT using the numbered queue
and fresh-chat prompt. Stop after the next task's playable handoff.
```

For a smaller request, name the behavior: “Make tipping accept a partly full station without losing peppers.” The AI identifies the affected task and contract, implements it, records the change, and gives you a focused check. A new idea outside the agreed scope should be discussed as a tradeoff before expanding the game.

Technical verification and player feedback are separate evidence. “Ready for Pavel” means the feature is wired and technically checked, with remaining gaps stated. It does not mean Pavel has played it or that it is proven fun. The broader M2 playtest is a milestone decision; recruiting six people is not a requirement for every daily iteration.

## Keep the work small

Finish the repeated action and useful upgrade before dressing the whole yard. Prefer free reusable assets, simple state, and one playable feature at a time. Documentation edits need documentation checks only. New gameplay needs focused verification; passing checks are repeated when behavior changes or a failure warrants it. The discarded roasting loop has no standing regression requirement.
