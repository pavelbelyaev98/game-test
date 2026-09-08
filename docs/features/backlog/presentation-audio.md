# Presentation and audio

Status: Task `08` is deferred. The user is making the ground and will request art work separately. Task `19` is [complete](../../development/completed/19-presentation-rollback.md): the interrupted asset/audio pass is removed and existing rendering improved without new art.

Idea coverage: sections 42-45.

Design: [57 - production presentation brief](../../development/tasks/57-presentation-design.md) owns direction, terrain handoff and starter/equipment/feedback briefs before `08`–`11`. The user's deferred scope and specific asset approval gates remain.

## Purpose

Establish a cohesive, readable production look and sound direction that starts grounded and can support increasingly strange discoveries. The main game must not resemble a primitive mechanics test.

## Task 19 - completed cleanup

- Remove all assistant-added models, textures, fonts, UI sprites, audio, notices and their runtime/scene integrations; also remove the user's TextMeshPro reimports following their explicit follow-up.
- Record the complete owned paths and shared-file rollback in the asset ledger. Add no scenery, props or sounds merely to decorate the terrain.
- Disable poor-quality cast shadows, improve antialiasing/color rendering and existing HUD readability, and eliminate overlapping rim/wall faces.
- Verify the official Blender Lab MCP bridge against the running Blender instance without creating or changing art.
- Acceptance: restored scene has no added presentation/audio references; rim boundaries meet without overlap; relevant checks pass; updated Windows build and concise rollback evidence are available.

## Task 08 - future production foundation (requires a new scoped request)

See [numbered Task `08`](../../development/tasks/08-production-presentation.md) for scope, research, questions and acceptance.

## Acceptance

See [numbered Task `08`](../../development/tasks/08-production-presentation.md) for scope, research, questions and acceptance.

## Ongoing direction

Avoid endless brown mud, excessive darkness, and generic procedural scenery. General free-to-use commercially licensed assets are acceptable; distinctive discoveries and the evolving shovel deserve custom Blender work. Later feature tasks must maintain this quality bar.
