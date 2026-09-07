# AI handoff prompts (reuse across chats)

Use these prompts when starting a new chat with the repo.

## 1) Short default prompt (for day-to-day sessions)

Use this by default for every normal implementation/development turn.

```text
You are continuing work on the Something Down There repo.

Read and follow these local rules first:
- AGENTS.md
- readme.md
- docs/scope-and-validation.md
- docs/development/tasks.md
- docs/development/status.md
- docs/architecture.md
- docs/asset-ledger.md (when assets/sounds are added)

Work from the highest-priority task in docs/development/tasks.md and process one task at a time.

Do not:
- expand scope
- infer behavior from other projects
- assume MCP state is available without checking

Do:
- update task status (ready -> in_progress -> done),
- update docs/development/status.md with one evidence check + blockers/limitations,
- keep `.meta` handling safe,
- keep changes minimal and docs-first,
- if MCP is unavailable or unclear, pause and log the blocker in docs/development/status.md with environment/version details.
```

### When to use
- Every normal continuation chat.
- "Next thing?" style sessions.
- Quick fixes, design updates, queue progress.

## 2) Full context prompt (for fresh context or major handoffs)

Use this when a new AI/agent starts or when task intent is unclear.

```text
You are continuing the Something Down There repo.

First thing: read and follow local rules in AGENTS.md, readme.md, docs/scope-and-validation.md, docs/development/tasks.md, docs/development/status.md, and docs/architecture.md.
Work only from there (no assumptions from other projects).

Follow one-task execution:
1) Execute the top ready task in docs/development/tasks.md.
2) Keep all scope/behavior changes documented in docs same turn.
3) If assets/sounds are added, update docs/asset-ledger.md.
4) Update task status from ready -> in_progress -> done.
5) Update docs/development/status.md with one evidence check and one blocker/limitation if any.
6) Run checks using the repo's cost-aware policy (quick checks often, full checks mainly at task completion).

Do not use external codegen or framework additions unless the task requires it.
If MCP is inconsistent or unavailable, do not guess—pause and record a blocker with environment/version details before proceeding.
```

### When to use
- First chat in a brand-new run.
- If multiple people/agents are taking over.
- If the previous session ended before explicit task completion.

## 3) Prompt for single-task implementation requests

Use this when you want the AI to implement one specific task in `docs/development/tasks.md`.

```text
From this repo, implement only the active task in docs/development/tasks.md that is marked ready/next (task ID: <TASK_ID>).

Before coding:
- confirm the task dependencies in AGENTS.md and docs/scope-and-validation.md.
- restate assumptions briefly in the final response.

During work:
- apply one-task-at-a-time scope,
- keep edits docs-first and minimal,
- update task status and status.md.
- if behavior, add assets, or dependency changes are included, update all required supporting docs.
```

### When to use
- You have already selected the exact task and want to constrain implementation tightly.
- You want fewer irrelevant edits and strict scope boundaries.

## 4) Prompt for repo/bootstrap transition

Use this once, before gameplay coding begins (or after major repo resets).

```text
Treat this repo as a fresh start for development.

Goals:
1) preserve the existing Unity scaffold in unity/,
2) keep docs as the source of truth,
3) set task 0_02 as the first planning task only (no gameplay implementation yet unless explicitly requested),
4) do not bring in assets, scripts, scenes, or packages unless required by the active task.

Then begin task execution using docs/development/ai-prompts.md (short default prompt for routine turns).
```

### When to use
- After large migrations / project reset / when the workflow should be reinitialized.

## 5) Prompt naming and update rule

- Keep these prompts in this file and link it in `readme.md` and `docs/development/readme.md`.
- If task rules change, update this file's behavior expectations, then update
  - `docs/scope-and-validation.md`
  - `docs/architecture.md` (if ownership changes)
  - `docs/development/status.md`
