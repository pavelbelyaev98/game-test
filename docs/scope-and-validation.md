# Scope and validation

## Product scope

- Build the full first-person excavation game iteratively; every completed gameplay task must be a production-quality part of the final game.
- Preserve the core rhythm: dig, detect, uncover, collect, return, sell, upgrade, repeat.
- Support downward, diagonal, and sideways excavation; do not force a pre-dug route.
- Favor physical, understandable progression over puzzles, bureaucracy, extra currencies, and long menus.
- Implement only the active task's documented slice.
- Prefer durable implementation and existing documents; do not add temporary files/tools that need later deletion that the developer can forget about. Use one development executable with in-game admin access through uncommon shortcuts; retain durable tools and gate them out of release builds. Task `22` in the [queue](development/tasks.md) verifies release exclusion.
- Every new asset/audio addition requires explicit user approval before entering the project. Explain the specific item or listed batch, purpose, source/license, files/integration and removal steps, then ask and wait. A general request or assumed necessity is not approval. Respect the user's terrain ownership and do not add unrelated scenery or sound.

Read [idea-at-a-glance.md](idea-at-a-glance.md) and [current status](development/status.md), then its linked active/next task and shared feature contract. Read the full queue only to review priorities or promote work; read the intentionally long `idea.md` only when task context is insufficient.

## Task documents

- Each unfinished task has `development/tasks/<numeric-id>-<short-name>.md`, containing status, scope, prerequisites, research, acceptance and questions. The queue contains only short links to unfinished tasks, in priority order; IDs are stable references, not priority ranks.
- `idea.md` is the full concept; shared feature rules in `features/backlog/` retain detailed decisions, rationale, exclusions/rejected alternatives and unresolved proposals. Distinguish selected behaviour from current implementation and examples from approved scope. Task-specific research/questions/work live once in the numbered file; do not create a duplicate decision log. Completed tasks retain their numbered completion records.
- Cancelled tasks do not count as remaining work; preserve their retired IDs and remove them from dependencies. Digging remains click-and-hold by default, with optional accessibility modes deferred to dedicated controls tasks.

For each active task, read its linked feature and relevant findings/decision context before coding. Ask product questions in coherent batches as needed; the user welcomes thorough task-specific discussion. Record answers and why in the feature, sync high-level changes to `idea.md`, and update dependent tasks before completing design. Preserve defer/omit reasons and an evidence-based revisit trigger; future sessions must not depend on chat-only answers or require another attachment upload.

Design/research tasks are explicitly documentation-only: they finish with a concrete researched proposal, the required user decisions recorded and the dependent implementation contract updated. Do not mark them complete from a list of unanswered questions, or mark their gameplay feature complete without implementation. Asset approval remains separate; ordinary technical choices and numerical tuning stay inside implementation tasks.

## Completion contract

A task is complete when:

1. Its requested code/content is implemented unless the task is explicitly documentation-only.
2. Its acceptance criteria pass.
3. Relevant behavior changes are reflected in the owning feature/spec.
4. The numbered task status and current status are updated; finished work leaves the queue and links to its retained completion record. Promote the next eligible task by queue order.
5. New external/generated game assets or audio are recorded in the asset ledger.
6. Player-facing results meet the repository quality bar and the Windows review build is updated.

## Validation policy

- Run quick compile/static checks after code changes and the full relevant task checks once near completion.
- Rerun only after related behavior/dependency changes or a failure.
- Test game-owned behavior, integration, and scene wiring. Do not test Unity or third-party library internals.
- Documentation-only tasks need link/consistency checks, not Unity runtime tests.
- Manual feel, visuals, and usability checks are evidence only when actually performed.
- Deliver visual implementation through the Windows build. Temporary inspection/reference images and videos are permitted, but clean them up at completion and remove stale gallery links. Keep written measurements/results, actual runtime artwork and editable source/exports/licenses; retain decision previews only while their approval is pending. Permanent visual galleries require an explicit user request.
- Prefer isolated Input System test devices for automated behavior/input checks. These do not prove native OS input behavior; native Windows reviews still share the user's desktop.
- Keep native Windows checks brief and announce when input control starts and ends. Verify the target process and foreground window before injected actions; stop and repeat an affected check after observed user-input/focus interference. Treat that run as interrupted evidence, not a gameplay failure. Do not disable the user's physical input.
- The user [cancelled Windows Sandbox setup](development/tasks/76-isolated-windows-review.md). Sandbox installation and Windows repair are not prerequisites for game development; do not resume that work through routine validation.
- Use direct `unity status` and `unity command` calls with `com.unity.pipeline` for live editor inspection. Keep the project open for live checks and use the installed official agent skills. Setup details belong in `unity/readme.md`.

## Version and asset policy

- Choose stable packages compatible with the pinned Unity editor; update only when the active task benefits.
- Record dependency changes and their short rationale in the task/status, without copying changelogs.
- Use Blender MCP or free-to-use, commercially licensed external visual assets. Audio must also be free to use commercially. Do not substitute Unity primitives, code-generated art/materials, or image-generated assets in the main game.
- Before importing, record all owned paths, shared integration edits, license/notice entries and exact rollback steps in the asset ledger. Keep imports isolated and preserve user-owned assets when reverting.
- Keep the ledger about actual asset/audio changes. Do not add per-task absence reports such as "no assets added" to the ledger, task records or status.
