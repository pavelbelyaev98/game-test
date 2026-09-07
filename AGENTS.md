# Repository guidance for: Something Down There

The repository is docs-first and expects:

- Source-first Unity work under `unity/`
- Design/spec/task records under `docs/`
- Game-specific notes under `docs` and `docs/features`.

## Core workflow

- Do not expand scope while implementing.
- Keep feature requirements and behavior in docs first, then implement code.
- Use small task increments; one queue item at a time.
- Record status changes in `docs/development/status.md` and task notes in `docs/development/tasks.md`.
- Treat documentation as required implementation work:
  - every mechanic/API/asset/dependency change must be documented in the same turn.
  - update `docs/scope-and-validation.md` for rule changes, and `docs/architecture.md` for structural changes.
  - include migration notes in `docs/development/status.md`.

## Game profile

- Game: **Something Down There**
- Perspective: **first-person**
- Core fantasy: absurd excavation progression from a simple shovel to absurdity.
- Current phase: repository/bootstrap only. No gameplay code should be authored yet.

## Technical defaults

- Keep `Unity` default input handling on the Input System.
- For package/Unity version changes:
  - check current Unity package docs and changelogs before updating,
  - keep supported combinations in `unity/Packages/manifest.json`,
  - note decision + reason in task notes/status.
- Asset rule: game assets must be either:
  - pre-existing assets with a clearly valid **commercial-use** license, or
  - newly created via Blender MCP (or direct Blender workflow).

  For any non-primitive asset, record source/license or Blender pipeline provenance in docs.

  For AI-generated assets/sounds, record: generation model, prompt/source intent, resulting file set, and local approval status in the asset ledger.
- Prefer deterministic file edits for:
  - C# changes
  - markdown/docs updates
  - scripted renames/refactors
- Use MCP/AI tools when change depends on live editor context:
  - scene hierarchy/state
  - playmode observations
  - bulk scene-safe editor checks
- Keep `.meta` files preserved during moves and renames.
- Avoid unnecessary asset/framework additions until core loop is stable.
- On dependency updates, prefer stable/minor releases and avoid unnecessary upgrades unless required by target platform or critical bugs.
- Keep package and editor setup intentionally minimal until feature work begins.

## MCP and tool setup

- The repo includes local setup docs for Blender and Unity MCP integration.
- For Blender MCP:
  - configure through `docs/blender-mcp-setup.md`
  - keep Blender running while MCP calls are active
- For Unity MCP:
  - follow `docs/unity-mcp-setup.md`
  - prefer manual file edits for deterministic script/doc changes
- If MCP is not working, behaves inconsistently, or the setup is unclear:
  - pause and ask before proceeding with risky assumptions.
  - log the blocker in `docs/development/status.md` with the environment/version details.

## Delivery expectation

For any gameplay task, deliver:

1. updated design/docs entries
2. proof checks (compile/checklist/tests)
3. a short next action

Cost-aware validation (reduce expensive reruns):

1. Run a quick deterministic check immediately after code edits.
2. Run the fuller task validation once when the task is ready for review.
3. Re-run checks immediately only when:
   - you changed behavior-related code,
   - a dependency changed,
   - an earlier check failed.

Delivery also includes project navigation updates when structure changes:

1. note new/renamed folders in `docs/development/handoff.md` and the local docs index,
2. keep one source-of-truth path map in `readme.md` and `docs/development/readme.md`.
