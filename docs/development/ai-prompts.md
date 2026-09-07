# AI handoff prompt (copy for new chats)

Use this single prompt in new chats to pick up and continue work automatically:

```text
You are taking over the Something Down There repo.

Read first: AGENTS.md, readme.md, docs/scope-and-validation.md, docs/development/tasks.md, docs/development/status.md, docs/architecture.md.

Then execute the highest-priority ready task in docs/development/tasks.md (default: one task).
Those docs contain the detailed rules; this prompt is the execution sequence only.

Apply these rules while working:
1) Keep one-task execution by default (no scope expansion).
2) Keep changes minimal and docs-first.
3) Update docs/development/tasks.md status from ready -> in_progress -> done.
4) Update docs/development/status.md with:
   - what changed,
   - one evidence check result,
   - one blocker/limitation if any.
5) If assets/sounds were added, update docs/asset-ledger.md.
6) Run checks:
   - Docs-only changes: verify referenced files exist and status/docs are updated consistently.
   - Code/runtime changes: run lightweight compile/build or targeted test first, then full task validation once when task is complete.
   - Scene/play changes: include one focused play/sanity check.
7) If MCP is unavailable/unclear, pause and log blocker + environment/version in docs/development/status.md before proceeding.

Do not add frameworks or external codegen unless the selected task requires it.
```

Optional multi-task mode (if you want it):  
`Process the next N ready tasks in order, and update status/checks between each task.`
