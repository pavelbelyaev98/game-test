# Repository Guidance: Something Down There

Docs-driven first-person excavation game built in Unity. Runtime code lives in `unity/`; design, architecture, and task state live in `docs/`.

## 1. Quick Start & Navigation

1. **Active Task & Status:** Check `docs/tasks.md` first. It is the single source of truth for the priority queue and completed history.
2. **Game Design Authority:** Read `docs/concept/00_README.md` and relevant concept chapters (`docs/concept/01`–`15`).
3. **Current Codebase:** Check `docs/architecture.md` for system ownership and `docs/baseline.md` for what is currently built.
   - *Note:* Existing baseline mechanics and art are working prototype features, **not** signed-off or final. They are expected to be refactored or replaced to match `docs/concept/`.

## 2. Task Workflow (Just-In-Time Planning)

- **Execution:** Work on the next pending task in `docs/tasks.md`.
- **Just-In-Time Spec:** When starting an active task, create a thorough, well-thought-out spec at `docs/tasks/<id>-<slug>.md`. Do not artificially restrict its depth: thoroughly define the Objective, Concept Reference, live codebase analysis, exact architecture/class changes, edge cases, and concrete Acceptance Criteria. If anything is ambiguous or involves open design choices, ask questions before implementing.
- **Pragmatic Tests & Benchmarks:** Write tests or benchmarks **only when useful on core systems** (e.g. voxel meshing algorithms, save serialization, progression math, or performance-critical loops). Do not write tests for trivial UI layout, cosmetic props, or simple visual tweaks.
- **Completion Protocol:** A task is complete only when:
  1. Code compiles warning-free and passes relevant tests (including any new high-value tests).
  2. Playable gameplay changes are verified and built to `builds/windows/SomethingDownThere.exe`.
  3. The completed spec is moved from `docs/tasks/<id>-<slug>.md` to `docs/tasks/completed/<id>-<slug>.md` to preserve architectural decisions.
  4. If a baseline system was refactored or replaced, update `docs/baseline.md` so it remains an accurate snapshot of working code.
  5. The task is marked `[x]` in `docs/tasks.md`, and a 1–2 sentence technical summary is appended under `## Completed`.

## 3. Minimal Documentation & Data Rules

- **Strict Scannability:** Keep documentation minimal and concise. No session narratives, chat transcripts, or command logs. Target under 60 lines for roadmap/status files; task specs may be as thorough as needed.
- **Data-Driven Architecture:** **Never store item prices, coordinates, or tool stats in Markdown files.**
  - Discovery properties (prices, depths, exposure, counts) live in `catalog.json` / `DiscoveryCatalog.asset`.
  - Tool upgrade parameters (speed, bite radii, battery capacity, slots) live in `EquipmentProgression.cs`.
- **Decentralized Asset Tracking:** Do **not** maintain a centralized asset ledger. Document assets minimally in their local folder: `art/<name>/README.md` (5–8 line card stating: Item, Purpose, Source/License, Unity Path, Status).
- **Asset Approvals:** New external assets or audio require user approval before entering the project. Visuals/audio must be commercially licensed (e.g. CC0, MIT) or created via Blender MCP.
- **Preserve User Data:** Never delete, overwrite, or commit the user recovery scene `unity/Assets/_Recovery/0.unity` or existing player saves in `persistentDataPath/Save`.

## 4. Unity & Developer Tooling

- **Unity Environment:** Unity `6000.6.0f1` with URP `17.6.0` and Unity Input System.
- **CLI & Pipeline:** Use the official Unity CLI directly (`unity status`, `unity command`) with `com.unity.pipeline` for live editor inspection. See `unity/readme.md`.
- **3D Modeling:** Use Blender MCP (`127.0.0.1:9876`) for generating and modifying 3D assets, storing recipes and `.blend` files under `art/`.
- **Windows Builds:**
  - **If Unity Editor is open:** `unity command menu --path 'Tools/Something Down There/Build Windows Player' --timeout 300 --project-path "$projectPath" --format json`
  - **If Unity Editor is closed:** `./tools/build-windows.ps1`
  - *Warning:* Never run `./tools/build-windows.ps1` while the Editor is open, as batchmode will fail on process locks.
