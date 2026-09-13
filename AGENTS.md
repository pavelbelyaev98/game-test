# Repository guidance: Something Down There

Docs-guided Unity game. Runtime work lives in `unity/`; design and task state in `docs/`.

## Start of work

1. Read `docs/idea-at-a-glance.md` and `docs/development/status.md`; follow the status links to the active/next task spec and its contract in `docs/features/`.
2. Read the long `docs/idea.md` only when those are insufficient.
3. Do not expand beyond the active task.

## Task workflow

- `docs/development/tasks.md` is the priority queue; every unfinished task has one spec at `docs/development/tasks/<id>-<short-name>.md` stating type, status, prerequisites, scope, acceptance and user questions. Start from `status.md`; keep the spec current as decisions change.
- Substantial unresolved product choices get a design/research task that produces a concrete decision artifact for user review and updates the owning contract before implementation. Questions alone are not a completed design.
- Resolve product questions in useful batches with concrete options; record answers and implications in the owning contract and update affected tasks. No dependency may rely on an answer stored only in chat.
- A task is done only when its content is implemented (unless documentation-only), acceptance evidence exists, and the owning contract and `status.md` are current. Then remove its queue row and its spec; lasting decisions live in the owning contract and status (git keeps history).
- Gameplay delivery requires one Windows build at `builds/windows/SomethingDownThere.exe`; give the user a clickable link. The user reviews builds and should not be told to open Unity unless they ask.

## Quality bar

- A small task limits scope, not quality. Do not optimize for a cheap MVP, demo, proof or fastest technically passing result.
- Player-facing work must be coherent, polished, performant and integrated into `MainGame.unity`, including feedback, error states and presentation.
- Primitives, generated materials, debug labels and validation adapters are allowed only in test/validation scenes, never in the main game or Windows build.
- Do not mark a visible feature done from compilation or automated tests alone. Inspect through the official Unity CLI when available and provide the Windows build.
- Do not create permanent screenshot/video galleries or art review images unless the user asks.

## Documentation

- Keep docs current, not chronological; git is the history. Edit or replace stale text. No session narratives, command logs or duplicated descriptions.
- One source per topic: `docs/features/` owns behavior, reasons, exclusions and open questions; the task spec owns scope/questions/acceptance; `status.md` owns current state; `docs/asset-ledger.md` owns asset/audio ownership and approvals.
- `docs/development/completed.md` is the scannable index of delivered work (one line per task); current state stays in `status.md` and the contracts.
- `docs/idea.md`, `docs/idea-at-a-glance.md` and `docs/pitch.md` are self-contained: no links to task/feature documents and no task IDs. Feature contracts may name owning task numbers; research and task specs may link freely.
- Keep working docs scannable, normally 60 lines or fewer. Never rewrite `docs/idea.md`; it is the intentional full concept. Preserve `.meta` files during Unity moves and renames.

## Unity and dependencies

- Use the Unity Input System. Prefer stable, Unity-compatible package versions and record version-change reasons in the task or status. Add libraries only when they materially simplify the active task.
- Prefer direct file edits for deterministic C#, Markdown and scripted refactors.
- Use the official Unity CLI directly (`unity status`, `unity command`) with `com.unity.pipeline` for live scene/editor state; keep the official Unity agent skills installed and do not add another bridge. Use Blender MCP for generated 3D assets. See `unity/readme.md`. If a tool is unavailable or inconsistent, ask before risky assumptions and record a concise blocker.

## Assets and audio

- The user must approve every new asset or audio addition before it enters the project. Explain the specific item or listed batch, purpose, source/license, files/integration and removal; ask and wait. A general feature request or assumed necessity is not approval. Record approval in the ledger.
- Keep visual work within the requested feature and the user's terrain ownership; do not add unrelated scenery, props, tools or content packs. The audio direction is selected (ambient nature plus digging and action feedback); every specific sound still needs user approval, a free commercial license and a ledger entry before it enters the project.
- Player-facing visuals come from Blender MCP (retain `.blend` sources and exports) or free assets explicitly licensed for commercial game use. Audio must also be free and commercially licensed. No primitives, code-generated art/materials, image generators or placeholders in the main game; if neither Blender MCP nor a licensed asset is available, report a blocker.
- Record every asset and sound in `docs/asset-ledger.md` before use: source/tool, path/URL, exact license, attribution, files, task, approval. List owned files (including `.meta`, exports, materials, prefabs, notices) and precise removal/restoration steps; keep each change independently reversible and preserve user-owned imports.
- Other binaries at least 10 MiB follow `docs/development/large-files.md`; after staging asset changes run `powershell -NoProfile -File tools/check-large-files.ps1`.

## Validation

- After code edits run a fast deterministic check; run the task's full relevant checks once at completion and rerun only after related changes or a failure.
- Test repository-owned behavior and integration, not Unity or third-party internals. Manual feel, visual and usability claims are evidence only when actually performed.
- Keep native Windows checks brief, announce input control, and treat runs interrupted by user input as interrupted evidence rather than failures. Do not disable the user's physical input.
