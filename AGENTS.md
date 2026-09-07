# Repository guidance: Something Down There

This is a docs-guided Unity project. Runtime work belongs in `unity/`; design and task state belong in `docs/`.

## Task workflow

1. Continue the single `in_progress` task; if none exists, take the highest-priority `ready` item and mark it `in_progress`. The user may explicitly select a different task or bounded batch.
2. Always read `docs/idea-at-a-glance.md`, then read the active task's linked feature file. Read the long `docs/idea.md` only if those sources are insufficient.
3. Implement the requested outcome as a production-quality part of the full game. A gameplay task is not complete after writing a spec, proving only backend logic, or shipping placeholder presentation.
4. Run proportionate checks and mark the task `done` only when its acceptance criteria pass.
5. Update `docs/development/status.md` with the current result, evidence, and any blocker.

Do not expand beyond the active task. Preserve `.meta` files during Unity moves and renames. This repository is building the full game, not a disposable prototype: implement small playable increments in production-oriented systems and the main game scene unless a file is explicitly labeled as a validation fixture.

## Quality bar

- A small task limits scope, not quality. Do not optimize for a cheap MVP, demo, proof, or fastest technically passing result.
- Player-facing work must be coherent, polished, performant, and integrated into `MainGame.unity`, including appropriate feedback, error states, and presentation.
- Unity primitives, flat generated materials, debug labels, and validation adapters are allowed only in test/validation scenes. They are not acceptable final content in the main game or Windows build.
- Do not mark a visible feature done based only on compilation or automated tests. Inspect it through the official Unity CLI when available and always provide the Windows build for user review.

Task IDs are one zero-padded numeric sequence (`01`, `02`, `03`, ...). Never create prefixes, letter suffixes, decimal subtasks, or a separate feature-number sequence. Add the next integer for every new task, including setup and maintenance.

## Documentation discipline

- Keep docs current, not chronological. Git is the history.
- Edit or replace stale text; do not append session narratives, exhaustive command logs, or duplicated implementation descriptions.
- `tasks.md` is only the queue. Keep each task to one row and link its owning feature/spec.
- `status.md` is only the current milestone, active/next task, latest useful evidence, and blockers.
- When a task is completed, create one concise record named `docs/development/completed/<numeric-id>-<short-name>.md` and link it from the queue row. Record only why, integrated result, evidence, and remaining limitation; normally keep it under 20 lines.
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
- Use the official Unity CLI directly (`unity status`, `unity command`) with `com.unity.pipeline` for live scene/editor state. Keep the official Unity agent skills installed; do not configure an additional Unity server or bridge. Use Blender MCP for generated 3D assets. See `unity/readme.md` for setup. If either tool is unavailable or inconsistent, ask before making risky assumptions and record a concise blocker.

## Assets and audio

Player-facing visual assets must be created through Blender MCP (retain the `.blend` source and exports) or downloaded free-to-use with a license explicitly permitting commercial game use. Do not hand-create substitute art with Unity primitives, generated meshes/materials, code, or an image generator. Runtime procedural geometry required by a mechanic is allowed, but its visible materials and presentation must use approved assets. Audio must be free to use and explicitly licensed for commercial use. If Blender MCP is unavailable and no suitable free licensed asset can be found, report a blocker instead of shipping a placeholder.

Record every imported or Blender-created asset and every sound in `docs/asset-ledger.md` before use, including source/tool, source URL or `.blend` path, exact license, attribution, files, task, and approval state.

## Validation

- After code edits, run a fast deterministic check.
- Run the task's full relevant validation once at completion; rerun only after related behavior/dependency changes or a failure.
- Test repository-owned behavior and integration, not Unity or third-party library internals.
- Gameplay delivery requires implemented code/content, relevant acceptance evidence, updated task/status state, and one concise limitation or next action.
- When a task changes playable behavior, produce one Windows build at `builds/windows/SomethingDownThere.exe` near completion and give the user a clickable link to that executable. The user reviews builds and should not be told to open Unity unless they explicitly ask.
