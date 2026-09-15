# Repository Guidance: Something Down There

Docs-driven first-person excavation game built in Unity. Runtime code lives in `unity/`; design, architecture, and task state live in `docs/`.

## 1. Quick Start & Navigation

1. **Active Task & Status:** Check `docs/tasks.md` first. It is the single source of truth for the priority queue, current build status, and completed history.
2. **Game Design Authority:** Read `docs/concept/00_README.md` and relevant concept chapters (`docs/concept/01`–`15`).
3. **Current Codebase:** Check `docs/architecture.md` for system ownership and `docs/baseline.md` for what is currently built.
   - *Note:* Existing baseline mechanics and art are working prototype features, **not** signed-off or final. They are expected to be refactored or replaced to match `docs/concept/`.

## 2. Task Workflow

- **Sequential IDs:** Tasks use sequential 3-digit IDs (`001`, `002`, `003`...).
- **Active Task Spec:** When planning or working on a task, keep its temporary specification at `docs/tasks/<id>-<name>.md`.
  - **30-Line Limit:** Specs must be concise (<30 lines) covering only: Goal, Technical Approach/Files, and Acceptance Criteria.
- **Completion Protocol:** A task is complete only when:
  1. Code compiles warning-free and passes relevant tests.
  2. Playable gameplay changes are verified and built to `builds/windows/SomethingDownThere.exe`.
  3. The temporary spec `docs/tasks/<id>-<name>.md` is deleted (Git preserves history).
  4. A 1–2 sentence technical summary is appended to `docs/tasks.md` under `## Completed`.

## 3. Minimal Documentation & Anti-Bloat Rules

- **Strict Scannability:** Keep documentation minimal, concise, and updated in place. Target under 60 lines for working files. No session narratives, chat transcripts, or command logs.
- **Decentralized Asset Tracking:** Do **not** create or maintain a centralized asset ledger. Document assets minimally in their local folder: `art/<name>/README.md` (5–8 line card stating: Item, Purpose, Source/License, Unity Path, Status).
- **Asset Approvals:** New external assets or audio require user approval before entering the project. Visuals/audio must be commercially licensed (e.g. CC0, MIT) or created via Blender MCP.

## 4. Unity & Developer Tooling

- **Unity Environment:** Unity `6000.6.0f1` with URP `17.6.0` and Unity Input System.
- **CLI & Pipeline:** Use the official Unity CLI directly (`unity status`, `unity command`) with `com.unity.pipeline` for live editor inspection. See `unity/readme.md`.
- **3D Modeling:** Use Blender MCP (`127.0.0.1:9876`) for generating and modifying 3D assets, storing recipes and `.blend` files under `art/`.
- **Windows Builds:**
  - In Editor: `unity command menu --path 'Tools/Something Down There/Build Windows Player' --timeout 300 --project-path "$projectPath" --format json`
  - With Editor closed: `./tools/build-windows.ps1`
