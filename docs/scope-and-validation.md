# Scope and validation

## Product scope

- Build the full first-person excavation game iteratively; every completed gameplay task must be a production-quality part of the final game.
- Preserve the core rhythm: dig, detect, uncover, collect, return, sell, upgrade, repeat.
- Support downward, diagonal, and sideways excavation; do not force a pre-dug route.
- Favor physical, understandable progression over puzzles, bureaucracy, extra currencies, and long menus.
- Implement only the active task's documented slice.
- Prefer durable implementation and existing documents; do not add temporary files/tools that need later deletion that the developer can forget about. Use one development executable with in-game admin access through uncommon shortcuts; retain durable tools and gate them out of release builds. Task `22` in the [queue](development/tasks.md) verifies release exclusion.
- Every new asset/audio addition requires explicit user approval before entering the project. Explain the specific item or listed batch, purpose, source/license, files/integration and removal steps, then ask and wait. A general request or assumed necessity is not approval. Respect the user's terrain ownership and do not add unrelated scenery or sound.

Always read [idea-at-a-glance.md](idea-at-a-glance.md), then the active task's feature file. Read the intentionally long `idea.md` only when those sources are insufficient.

## Completion contract

A task is complete when:

1. Its requested code/content is implemented unless the task is explicitly documentation-only.
2. Its acceptance criteria pass.
3. Relevant behavior changes are reflected in the owning feature/spec.
4. The task row and current status are updated concisely.
5. New external/generated game assets or audio are recorded in the asset ledger.
6. Player-facing results meet the repository quality bar and the Windows review build is updated.

## Validation policy

- Run quick compile/static checks after code changes and the full relevant task checks once near completion.
- Rerun only after related behavior/dependency changes or a failure.
- Test game-owned behavior, integration, and scene wiring. Do not test Unity or third-party library internals.
- Documentation-only tasks need link/consistency checks, not Unity runtime tests.
- Manual feel, visuals, and usability checks are evidence only when actually performed.
- Use direct `unity status` and `unity command` calls with `com.unity.pipeline` for live editor inspection. Keep the project open for live checks and use the installed official agent skills. Setup details belong in `unity/readme.md`.

## Version and asset policy

- Choose stable packages compatible with the pinned Unity editor; update only when the active task benefits.
- Record dependency changes and their short rationale in the task/status, without copying changelogs.
- Use Blender MCP or free-to-use, commercially licensed external visual assets. Audio must also be free to use commercially. Do not substitute Unity primitives, code-generated art/materials, or image-generated assets in the main game.
- Before importing, record all owned paths, shared integration edits, license/notice entries and exact rollback steps in the asset ledger. Keep imports isolated and preserve user-owned assets when reverting.
