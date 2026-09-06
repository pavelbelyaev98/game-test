# Repository guidance

## Purpose and stage

This repository groups small-game research and Unity experiments. **Just a few peppers** is a small offline single-player bulk-handling game. Current v4 design is planned; existing playable code is an older one-pepper spike.

## Documentation map

- [Repository entry](readme.md) and [game entry](games/just-a-few-peppers/readme.md).
- [Design and scope](games/just-a-few-peppers/docs/readme.md).
- [Start here: AI implementation and Pavel's playtests](games/just-a-few-peppers/docs/development/start-here.md).
- [Numbered task queue](games/just-a-few-peppers/docs/development/tasks/readme.md) and [new-chat prompt](games/just-a-few-peppers/docs/development/new-chat-prompt.md).
- [Roadmap](games/just-a-few-peppers/docs/development/roadmap.md) and [implementation status](games/just-a-few-peppers/docs/development/status.md).
- [Unity practices and free asset policy](games/just-a-few-peppers/docs/development/unity-and-assets.md).
- [Architecture](games/just-a-few-peppers/ARCHITECTURE.md).
- [State and saving](games/just-a-few-peppers/docs/development/state-and-saving.md).
- [Testing and commands](games/just-a-few-peppers/docs/development/testing-and-performance.md).
- [Research](research/readme.md): evidence and ideation, not additional feature requirements.

The Unity project root is `games/just-a-few-peppers/unity/`. Game-document paths beginning with `Assets/` are relative to it. Each future game belongs in its own `games/<name>/` group.

## Working rules

- Follow the user's current task and v4 scope. Planning/reorganization does not imply implementation.
- For feature implementation, use the numbered task queue: select the requested ID or next eligible task, read its common context/specs/dependency records, and deliver one task unless a larger range is requested. A fresh chat resumes recorded work rather than rebuilding it.
- Keep per-task delivery/feedback status in the queue, execution evidence in the task's delivery record, and milestone summaries in status.md. Record supplied feedback before selecting NEXT. Ordinary technical handoffs permit subsequent work at the user's request without inventing play acceptance; explicit review gates require their stated evidence.
- Inspect the actual project and status; preserve existing user changes.
- Before Unity API/package decisions, read the pinned editor/package versions and consult matching official Unity documentation. Use supported APIs and stable compatible packages; fix new deprecation warnings instead of suppressing them. Do not automatically upgrade the editor on every task.
- Use the Input System for new v4 gameplay; M1 installs and configures it. The old Input Manager is deprecated in the current editor's manual. Existing Stage0 input is an audit fact, not a pattern to extend.
- Prefer free assets licensed for commercial game use. Source and integrate suitable packs before making ordinary assets from scratch; record actual imports and licenses as described in the asset policy. Use graybox placeholders early and custom work only where the game needs it.
- Use simple C#, explicit ownership, and composition. Presentation physics cannot own required progress.
- Separate authored configuration from mutable state; ScriptableObjects are not save state.
- Prefer explicit references and small components over hidden globals and a framework.
- Do not add multiplayer, ECS, economies, recipes, sorting, NPC schedules, or household tasks outside scope.
- Preserve Unity `.meta` files with assets; prefer editor authoring over fragile scene YAML edits.
- Stage0's builder regenerates its scene/art. Do not silently replace new authored work with it.
- Stage0 is disposable. Reuse or retire its pieces during implementation after checking retained references; preserving its old gameplay and rerunning its checks are not prerequisites for v4.
- Earlier proposals and research do not restore removed features.
- Own scene/prefab setup, asset integration, input, UI, and build configuration for the feature. Deliver a playable result with an exact scene/build path, controls, and a short checklist; do not delegate routine Inspector assembly to Pavel.
- Update affected behavior contracts and implementation status in the same change.
- Add a focused execution note or ADR only when useful; no empty template trees.

## Definition of done

A feature needs documented behavior, relevant automated checks, actual scene integration, visible feedback, and recovery/save verification where applicable. Compilation alone is insufficient. Record evidence and limitations in status; partial work stays unchecked. Check a packaged build when the milestone changes packaged behavior. Track technical readiness separately from Pavel's play feedback; do not claim fun from tests.

Use the testing document for relevant checks. Unity EditMode/PlayMode infrastructure is currently planned, not installed. Do not invent passing tests. Documentation-only changes need documentation checks, not Unity or legacy test runs. For new work, run appropriate checks once and repeat only when changed behavior, a failure, or an unresolved concern warrants it.

The older bootstrap in `instructions/` supplies process ideas. Apply its useful principles through these documents rather than generating every template it lists.
