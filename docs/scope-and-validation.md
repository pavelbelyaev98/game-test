# Scope and validation

## Product scope

- Build the full first-person excavation game iteratively; every completed gameplay task must be a production-quality part of the final game.
- Preserve the core rhythm: dig, detect, uncover, collect, return, sell, upgrade, repeat.
- Support downward, diagonal, and sideways excavation; do not force a pre-dug route.
- Favor physical, understandable progression over puzzles, bureaucracy, extra currencies, and long menus.
- Implement only the active task's documented slice.

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

## Version and asset policy

- Choose stable packages compatible with the pinned Unity editor; update only when the active task benefits.
- Record dependency changes and their short rationale in the task/status, without copying changelogs.
- Use Blender MCP or free-to-use, commercially licensed external visual assets. Audio must also be free to use commercially. Do not substitute Unity primitives, code-generated art/materials, or image-generated assets in the main game.
