# Prompt for a fresh implementation chat

Open this repository in your AI coding workspace and paste the prompt below. No earlier conversation is needed. Keep `NEXT` for chronological work or replace it with an ID such as `1_02`. The [queue](tasks/readme.md) holds current progress and the reading route.

```text
Work on Just a few peppers in this repository.
TARGET TASK: NEXT

Read AGENTS.md and games/just-a-few-peppers/docs/development/tasks/readme.md.
Apply my feedback first, then follow the queue's selection and context rules.
Read the selected brief, relevant specifications, dependency delivery records,
and actual Unity files. Resume partial work; implement exactly one task.

Own code, scene/prefab setup, assets, input, UI, and required build configuration.
Verify the changed work and record evidence and honest feedback status using
the queue's handoff rules. Give me the exact scene/build, controls, short
play checklist, checks, limitations, and next task ID. Stop after the handoff.
```

Unity project: `games/just-a-few-peppers/unity/`. [AGENTS.md](../../../../AGENTS.md) holds working rules; the queue holds dependency/review gates. Links are navigation, not a requirement to recursively read all design and research documents.

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
