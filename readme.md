# Something Down There

# Project setup (ready-to-use root)

This is the dedicated project root for **Something Down There**.

## Game profile

1. First-person excavation game.
2. Docs-first workflow for scope, design, and task execution.
3. Unity project scaffold in `unity/`.
4. Blender + Unity MCP setup docs and scripts.
5. Minimal dependency policy and check cadence to avoid unnecessary heavy test loops.

## First setup done

1. `docs/idea.md` contains the game concept and design intent.
2. `docs/features/backlog.md` holds the feature backlog for future implementation.
3. `unity/Packages/manifest.json` is present as dependency baseline.
4. `.vscode/mcp.json`, `tools/`, and `docs/*-mcp-setup.md` are in place.
5. `docs/asset-ledger.md` is active for all non-primitive assets and sounds.

## Current status

1. Setup complete, gameplay code not started.
2. No build/scene assets are authored yet.
3. Next action: initialize the first task in `docs/development/tasks.md`.

## Project navigation

1. `docs/development/tasks.md` - queue and execution order.
2. `docs/development/status.md` - current milestone/blockers.
3. `docs/scope-and-validation.md` - in/out-of-scope rules.
4. `docs/architecture.md` - system ownership.
5. `docs/features` - feature ideas and next planning.
6. `docs/development/handoff.md` - handoff packet for the next AI.
7. `docs/asset-ledger.md` - asset/audio provenance and license records.

Default policy:

1. Unity is the production engine by default.
2. Keep task docs and evidence in `docs/development`.
3. Preserve `.meta` files and scene assets when importing existing assets.
4. Use MCP for context-heavy tooling; use direct edits for deterministic code/doc changes.
