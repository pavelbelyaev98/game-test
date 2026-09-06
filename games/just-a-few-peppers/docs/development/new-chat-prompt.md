# Prompt for a fresh implementation chat

Open the **game-test repository** in your AI workspace, then paste the prompt below. The agent needs access to these local files; a web chat without repository access cannot recover the context by path alone. No earlier conversation needs to be pasted when the repository is available.

Leave `TARGET TASK: NEXT` to continue chronologically. Replace `NEXT` with an ID such as `1_02` to request a specific task. The [numbered queue](tasks/readme.md) holds the current status and links to every brief. First implementation starts at **1_01**; 0_01 is completed planning.

```text
Work on Just a few peppers in this repository. This is a fresh chat:
recover the necessary context from repository files, not assumed chat history.

TARGET TASK: NEXT

Read AGENTS.md, then:
- games/just-a-few-peppers/docs/development/tasks/readme.md
- games/just-a-few-peppers/docs/development/status.md
- The common context listed in the task index, the selected numbered task,
  its linked specifications, and its dependencies' delivery records.

Apply any feedback I supply before selecting the task. For NEXT, follow the
queue's chronological selection rules: resume partial work or needed revisions,
otherwise take the next eligible Todo. Check actual source/scenes/packages;
do not assume that a document or an old test pass proves implementation.
State the selected ID and intended playable result, then implement ONE task.
Do not stop at a plan or automatically implement the whole milestone.

Unity project: games/just-a-few-peppers/unity/.
Follow the current v4 scope and unity-and-assets.md. Use supported APIs for
the pinned Unity/package versions and free commercially usable assets or
placeholders. Own code, scene/prefab wiring, input, materials, and required
asset integration. Do not leave routine Inspector assembly for me.

Run only verification relevant to this changed work. Do not repeatedly run
old Stage0 checks. Fix relevant failures and report anything untested honestly.
At a play/review gate, prepare the usable artifact first and keep missing human
evidence pending; do not invent feedback or advance past an unresolved gate.

Append the delivery record to the task file, update the task queue and milestone
status, and update affected contracts/asset/regression records where necessary.
Finish with: task ID, what works, exact scene/build path, controls, a short
play checklist, checks performed, remaining issues, and next task ID.
Stop at that handoff. This request does not authorize public publishing.
```

## After you play

Feedback can be sent in the same chat or a new chat with the prompt above:

```text
Feedback for 1_02: scooping feels too slow, and the crate blocks my view.
Revise 1_02 before advancing. Record this feedback and the changes in its task
file, then give me a new playable handoff with focused checks.
```

To accept and continue, provide the actual result rather than making the agent guess:

```text
I played 1_02. Gathering and carrying feel good enough to continue.
Record that feedback, then complete TARGET TASK: NEXT using the repository queue.
```

You can also request NEXT without having playtested an ordinary task. The agent may continue after its technical handoff, leaving player feedback marked Not tested. The explicit review gates retain their required evidence. The queue's implementation and feedback columns prevent a new chat from confusing code completion with proof that the game is fun.
