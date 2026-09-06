# Prompt for a fresh implementation chat

Open this repository in your AI coding workspace and paste this exact block below when starting a new implementation turn. No earlier conversation is needed.
Keep `TARGET TASK: NEXT` for chronological work, or replace it with an ID such as `1_02`.
The [queue](tasks/readme.md) is the source of record.

```text
Work on Just a few peppers in this repository.
TARGET TASK: NEXT

Read AGENTS.md and games/just-a-few-peppers/docs/development/tasks/readme.md.
Apply my feedback first, then follow the queue's selection and context rules.
Read the selected task brief in full before making any edits.
Read the selected task's required linked specifications and dependency delivery records, then inspect the relevant Unity files.
If something is unclear, resolve it from the linked docs before implementation.
Resume partial work; implement exactly one task.

Own code, scene/prefab setup, assets, input, UI, and required build configuration.
Verify the changed work and record evidence and honest feedback status using
the queue's handoff rules. Give me the exact scene/build, controls, short
play checklist, checks, limitations, and next task ID. Stop after the handoff.
```

Unity project: `games/just-a-few-peppers/unity/`. [AGENTS.md](../../../../AGENTS.md) holds working rules; the queue holds dependency/review gates. Links are navigation, not a requirement to read design/research sources recursively.

If the requested change is needed but no task exists for it:
- Create a concrete follow-up task file in `games/just-a-few-peppers/docs/development/tasks/`.
- Add a new row to [tasks/readme.md](readme.md) with that task marked `Todo` and `Not tested`.
- Record the new task's dependencies and continue with the currently selected queued task.

## After you play

Send feedback before NEXT so revisions happen first:

```text
Feedback for 1_02: the crate blocks my view. Revise 1_02 before advancing.
Record the feedback and changes, then give me another playable handoff.
```

Or give the actual result and request the next task:

```text
I played 1_02. Gathering and carrying feel good enough to continue.
Record that feedback, then complete TARGET TASK: NEXT, one task only.
```

NEXT can follow an ordinary technical handoff without a playtest; feedback stays Not tested. Explicit review gates still need their stated human evidence. See the [workflow guide](start-here.md#give-feedback-then-continue).
