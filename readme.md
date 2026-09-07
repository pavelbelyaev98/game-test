# Something Down There

First-person excavation game built with Unity. The repository is ready for feature development; work is selected from the task queue.

## Start here

1. Read [AGENTS.md](AGENTS.md).
2. Open [docs/development/tasks.md](docs/development/tasks.md); continue an `in_progress` task, otherwise take the highest-priority `ready` task.
3. Open the linked feature file; it contains the gameplay purpose, task, requirements, and acceptance checks together.
4. Implement and validate the task, then update its row and the current status briefly.

Use [docs/idea-at-a-glance.md](docs/idea-at-a-glance.md) for general concept context. The intentionally detailed [docs/idea.md](docs/idea.md) is reference material, not a default read.

## Project map

- `unity/` - Unity project, source, scenes, and tests
- `docs/features/backlog.md` - compact feature index
- `docs/features/backlog/` - one small file per planned feature
- `docs/development/tasks.md` - ordered work queue
- `docs/development/status.md` - current state, evidence, and blockers
- `docs/architecture.md` - system ownership
- `docs/scope-and-validation.md` - stable scope and validation policy
- `docs/asset-ledger.md` - provenance for external or generated game assets/audio
- `docs/development/ai-prompts.md` - reusable new-chat prompt

Unity operation details live in [unity/readme.md](unity/readme.md). The actual Blender MCP client configuration is `.vscode/mcp.json`; MCP installation is workstation setup and is not duplicated in project documentation.
