# Scope and validation

## Product scope

- Build one compact, playable first-person excavation loop before optional systems.
- Preserve the core rhythm: dig, detect, uncover, collect, return, sell, upgrade, repeat.
- Support downward, diagonal, and sideways excavation; do not force a pre-dug route.
- Favor physical, understandable progression over puzzles, bureaucracy, extra currencies, and long menus.
- Implement only the active task's documented slice.

Each gameplay task links one feature file containing its purpose, requirements, and acceptance checks. Read that first, [idea-at-a-glance.md](idea-at-a-glance.md) only when more context is needed, and the intentionally long `idea.md` only when the summary is insufficient.

## Completion contract

A task is complete when:

1. Its requested code/content is implemented unless the task is explicitly documentation-only.
2. Its acceptance criteria pass.
3. Relevant behavior changes are reflected in the owning feature/spec.
4. The task row and current status are updated concisely.
5. New external/generated game assets or audio are recorded in the asset ledger.

## Validation policy

- Run quick compile/static checks after code changes and the full relevant task checks once near completion.
- Rerun only after related behavior/dependency changes or a failure.
- Test game-owned behavior, integration, and scene wiring. Do not test Unity or third-party library internals.
- Documentation-only tasks need link/consistency checks, not Unity runtime tests.
- Manual feel, visuals, and usability checks are evidence only when actually performed.

## Version and asset policy

- Choose stable packages compatible with the pinned Unity editor; update only when the active task benefits.
- Record dependency changes and their short rationale in the task/status, without copying changelogs.
- Use commercially permitted external assets or newly created AI/Blender assets, with provenance in `docs/asset-ledger.md`.
