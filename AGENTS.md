# Repository guidance: Something Down There

This is a docs-guided Unity project. Runtime work belongs in `unity/`; design and task state belong in `docs/`.

## Task workflow

1. Continue the single `in_progress` task; if none exists, take the highest-priority `ready` item and mark it `in_progress`. The user may explicitly select a different task or bounded batch.
2. For gameplay work, read the linked feature file first; it owns the purpose, task, requirements, and acceptance checks. Read `docs/idea-at-a-glance.md` only if more context is needed, then the long `docs/idea.md` only if the summary is insufficient.
3. Implement the requested outcome. A gameplay or implementation task is not complete after writing a spec.
4. Run proportionate checks and mark the task `done` only when its acceptance criteria pass.
5. Update `docs/development/status.md` with the current result, evidence, and any blocker.

Do not expand beyond the active task. Preserve `.meta` files during Unity moves and renames.

## Documentation discipline

- Keep docs current, not chronological. Git is the history.
- Edit or replace stale text; do not append session narratives, exhaustive command logs, or duplicated implementation descriptions.
- `tasks.md` is only the queue. Keep each task to one row and link its owning feature/spec.
- `status.md` is only the current milestone, active/next task, latest useful evidence, and blockers.
- When a task is completed, create one concise record in `docs/development/completed/<task-id>.md` and link it from the queue row. Record only why, integrated result, evidence, and remaining limitation; normally keep it under 20 lines.
- Each gameplay feature file combines its purpose, current task/status, required behavior, acceptance criteria, and unresolved decisions.
- Update a feature spec only when its behavior or acceptance contract changes.
- Update `scope-and-validation.md` only for repository-wide policy changes and `architecture.md` only for ownership/structure changes.
- Do not create a new document when an existing source of truth can be updated.
- Keep ordinary working docs scannable, normally 60 lines or fewer. Split only when topics have separate ownership; setup guides and the intentional full `docs/idea.md` may be longer.
- If a working document becomes difficult to scan, split it by topic and add a short index. Never split or rewrite `docs/idea.md`; it is the intentional full concept source.

## Unity and dependencies

- Use the Unity Input System.
- Prefer stable, Unity-compatible package versions. Check official package documentation/changelogs before version changes and record the short reason in the task row or status.
- Add libraries when they materially simplify the active task; avoid speculative dependencies.
- Prefer direct file edits for deterministic C#, Markdown, and scripted refactors.
- Use Unity MCP for live scene/editor state and Blender MCP or Blender for generated 3D assets. If MCP is unavailable or inconsistent, ask before making risky assumptions and record a concise blocker.

## Assets and audio

Game assets must be commercially usable external assets or newly created assets. Record imported/generated non-primitive art and audio in `docs/asset-ledger.md` before use.

For external assets, record source, license, and attribution. For AI/Blender output, record the tool/model, brief intent or prompt reference, files, task, and approval state. Unity-generated project settings, primitives, package contents, and temporary test fixtures do not belong in the ledger.

## Validation

- After code edits, run a fast deterministic check.
- Run the task's full relevant validation once at completion; rerun only after related behavior/dependency changes or a failure.
- Test repository-owned behavior and integration, not Unity or third-party library internals.
- Gameplay delivery requires implemented code/content, relevant acceptance evidence, updated task/status state, and one concise limitation or next action.
