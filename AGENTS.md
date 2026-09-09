# Repository guidance: Something Down There

This is a docs-guided Unity project. Runtime work belongs in `unity/`; design and task state belong in `docs/`.

## Task workflow

1. Start from `docs/development/status.md` and follow its direct active/next task link. Continue the single `in_progress` task; otherwise mark the next `ready` task `in_progress`. Read `tasks.md` only when reviewing priorities or promoting the next eligible task: list order is priority, numeric IDs are permanent references. The user may explicitly select a different task or bounded batch.
2. Always read `docs/idea-at-a-glance.md`, then the active task's numbered file and its linked feature contract. Read the long `docs/idea.md` only if those sources are insufficient.
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
- Record actual changes, decisions and useful evidence. Keep the asset ledger for actual asset/audio ownership, approvals, integration and removal; omit per-task "no assets added" statements there and in other docs.
- Edit or replace stale text; do not append session narratives, exhaustive command logs, or duplicated implementation descriptions.
- `docs/development/tasks.md` lists unfinished work in priority order, one short linked title per task. Details and status live in `docs/development/tasks/<numeric-id>-<short-name>.md`; do not repeat them in the queue. Keep IDs stable, retire cancelled IDs and remove them from dependencies. Keep the next unused ID at the queue footer.
- `status.md` is the short session entry point: direct active/next task links, current milestone, latest useful evidence/build and blockers. Do not require a new chat to read the full queue, completed records or unrelated feature/research files.
- When a task is completed, remove its queue row, retain its numbered spec, and link that spec to one concise record named `docs/development/completed/<numeric-id>-<short-name>.md`. Record only why, integrated result, evidence and remaining limitation; normally keep it under 20 lines. Promote the next eligible task in `status.md` using queue order and prerequisites.
- Preserve decisions where future work reads them: `idea.md` owns the overall concept; the linked feature owns detailed selected behaviour, rationale, exclusions and unresolved options; the numbered task owns research/questions, work and acceptance. Mark proposed/deferred/selected/implemented states explicitly. Retain important rejected alternatives and why, not a transcript or a second decision log. A source recommendation is not a selected mechanic or asset approval.
- Before work, follow the task's linked decision/research context as well as inspecting code. Resolve product questions in useful batches with concrete options/examples; the user welcomes thorough discussion for the active design task. Record each answer and its implications in the owning contract, update affected tasks, and do not re-ask decisions already settled. No dependency may rely on an answer stored only in chat or an attachment.
- Each numbered task file states its type (design/research, implementation or validation) and owns its scope, prerequisites, research, acceptance criteria and questions for the user. Feature files retain shared gameplay rules, purpose and links to the numbered tasks; do not duplicate detailed task contracts in both places.
- Substantial unresolved product choices get a numbered design/research task before their implementation dependency. It must produce a concrete decision artifact for user review and update the owning contract; questions alone are not a completed design. Technical choices and numerical tuning stay with implementation unless they require a separate product decision. Design completion never marks the gameplay feature implemented.
- Update a feature spec only when its behavior or acceptance contract changes.
- Update `scope-and-validation.md` only for repository-wide policy changes and `architecture.md` only for ownership/structure changes.
- Give each unfinished task one numbered file, moving its contract from the feature document rather than duplicating it. Otherwise update the existing source of truth instead of creating another document. Keep numbered specs when adding concise completion records.
- Keep ordinary working docs scannable, normally 60 lines or fewer. Split only when topics have separate ownership; setup guides and the intentional full `docs/idea.md` may be longer.
- If a working document becomes difficult to scan, split it by topic and add a short index. Never split or rewrite `docs/idea.md`; it is the intentional full concept source.

## Unity and dependencies

- Use the Unity Input System.
- Prefer stable, Unity-compatible package versions. Check official package documentation/changelogs before version changes and record the short reason in the numbered task or status.
- Add libraries when they materially simplify the active task; avoid speculative dependencies.
- Prefer direct file edits for deterministic C#, Markdown, and scripted refactors.
- Use the official Unity CLI directly (`unity status`, `unity command`) with `com.unity.pipeline` for live scene/editor state. Keep the official Unity agent skills installed; do not configure an additional Unity server or bridge. Use Blender MCP for generated 3D assets. See `unity/readme.md` for setup. If either tool is unavailable or inconsistent, ask before making risky assumptions and record a concise blocker.

## Assets and audio

The user must approve every new asset or audio addition before it enters the project. First explain the specific item or clearly listed batch, its purpose, source/license, intended files and integration, and how to remove it; provide a preview or sample when available. Ask and wait for explicit approval before adding it. A general feature request, visual cleanup request, or assumed necessity is not approval. Approval covers only the described additions; record it in the asset ledger.

Keep visual work within the user's requested feature. Do not add unrelated trees, fences, props, tools, scenery, music, ambience, sound effects or content packs. The user currently owns the terrain art and has deferred new art/audio to a separate request.

Player-facing visual assets must be created through Blender MCP (retain the `.blend` source and exports) or downloaded free-to-use with a license explicitly permitting commercial game use. Do not hand-create substitute art with Unity primitives, generated meshes/materials, code, or an image generator. Runtime procedural geometry required by a mechanic is allowed, but its visible materials and presentation must use approved assets. Audio must be free to use and explicitly licensed for commercial use. If Blender MCP is unavailable and no suitable free licensed asset can be found, report a blocker instead of shipping a placeholder.

Record every imported or Blender-created asset and every sound in `docs/asset-ledger.md` before use, including source/tool, source URL or `.blend` path, exact license, attribution, files, task, and approval state.

Make each asset change independently reversible. The ledger must list every owned folder/file (including `.meta`, source downloads, exports, importer outputs, generated font atlases, materials, prefabs, audio settings, and license/ThirdPartyNotices entries), every shared scene/code/config file edited for integration, and precise removal/restoration steps. Use isolated asset folders; label recursive ownership explicitly. Never leave an import's notices, runtime loaders, or scene references behind when removing it. Record user-owned imports separately and preserve them during rollback.

## Validation

- After code edits, run a fast deterministic check.
- Run the task's full relevant validation once at completion; rerun only after related behavior/dependency changes or a failure.
- Test repository-owned behavior and integration, not Unity or third-party library internals.
- Gameplay delivery requires implemented code/content, relevant acceptance evidence, updated task/status state, and one concise limitation or next action.
- When a task changes playable behavior, produce one Windows build at `builds/windows/SomethingDownThere.exe` near completion and give the user a clickable link to that executable. The user reviews builds and should not be told to open Unity unless they explicitly ask.
