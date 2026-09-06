# Start developing Just a few peppers

**Ask for one task, play the supplied build, give feedback, then repeat.** You can remain a fullstack developer and product/playtest owner; routine Unity assembly belongs to the AI.

## Your first instruction

Open this repository in your AI coding workspace and paste the [fresh-chat prompt](new-chat-prompt.md). The [task queue](tasks/readme.md#ordered-task-queue) identifies current progress and the next eligible task; a new chat recovers context from the files.

To play, follow the [Unity project guide](../../unity/readme.md). The usual handoff is a Windows executable with its adjacent files; the editor is optional for your playtest. If using Unity, open `games/just-a-few-peppers/unity/` in the pinned editor, then the supplied scene and press Play. Close this project's editor before the AI runs batch verification against it.

## Who does what

| AI implementation agent | Pavel |
| --- | --- |
| Implements the selected task and wires code, scenes, assets, input, and UI. | Chooses direction and task scope. |
| Sources suitable free assets, checks licenses, runs relevant checks, and supplies a playable artifact. | Plays and judges comfort, clarity, appearance, and enjoyment. |
| Records delivery, feedback, limitations, and next task in the queue/task record. | Reports observed behavior and decides whether to revise or continue. |

Each handoff includes an exact scene/build path, controls, and a 3–5 item checklist. A technical pass does not establish fun; incomplete work and missing human evidence stay visible.

## The implementation order and its specifications

All **31 task briefs**, their dependencies and acceptance criteria remain in the [numbered queue](tasks/readme.md). Use its [feature-to-task coverage table](tasks/readme.md#coverage-of-the-current-game) to locate implementation work, the [design index](../readme.md) to find feature specifications, and the [roadmap](roadmap.md) for milestone explanations and exit gates. Research is linked through the design index when useful.

The [combined M1 contract](first-playable-task.md) spans tasks 1_01–1_05; early handoffs intentionally expose only the pieces delivered so far. Later milestones cover the wheelbarrow, saving, full yard, presentation, menus, reliability, and release preparation. Simplifying this workflow does not remove any of that scope.

## Give feedback, then continue

Use the handoff checklist and tell the AI **what you did, what happened, and what you wanted instead**. A short recording or screenshot can help with visual issues, but plain text is enough. For example:

```text
Feedback for 1_02: after filling the crate I cannot see the path ahead.
Revise 1_02 before advancing and give me a new playable handoff.
```

When ready:

```text
I played 1_02. Gathering and carrying feel good enough to continue.
Record that feedback, then complete NEXT and stop after one task.
```

You may request NEXT without testing an ordinary task; feedback remains Not tested. Review gates **2_03, 5_05, 8_03, and 9_03** require their stated human evidence before progress continues. The [queue rules](tasks/readme.md#how-to-select-the-next-task) explain the distinction.

## Keep the work small

Use a focused revision for a specific problem. Discuss additions outside the [scope contract](../scope-and-validation.md#scope-contract) before expanding the game. The AI reads task-relevant context and verifies changed behavior; repeated bootstrap work, full research reading, and Stage0 retesting are not routine prerequisites.
