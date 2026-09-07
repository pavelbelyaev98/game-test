# Presentation and audio

Status: Task `08` is deferred. The user is making the ground and will request art work separately. Task `19` is [complete](../../development/completed/19-presentation-rollback.md): the interrupted asset/audio pass is removed and existing rendering improved without new art.

Idea coverage: sections 42-45.

## Purpose

Establish a cohesive, readable production look and sound direction that starts grounded and can support increasingly strange discoveries. The main game must not resemble a primitive mechanics test.

## Task 19 - completed cleanup

- Remove all assistant-added models, textures, fonts, UI sprites, audio, notices and their runtime/scene integrations; also remove the user's TextMeshPro reimports following their explicit follow-up.
- Record the complete owned paths and shared-file rollback in the asset ledger. Add no scenery, props or sounds merely to decorate the terrain.
- Disable poor-quality cast shadows, improve antialiasing/color rendering and existing HUD readability, and eliminate overlapping rim/wall faces.
- Verify the official Blender Lab MCP bridge against the running Blender instance without creating or changing art.
- Acceptance: restored scene has no added presentation/audio references; rim boundaries meet without overlap; relevant checks pass; updated Windows build and concise rollback evidence are available.

## Task 08 - future production foundation (requires a new scoped request)

- Before any new asset/audio enters the project, explain the specific item or listed batch, purpose, source/license, files/integration and removal steps; show a preview/sample when available, ask and wait for explicit user approval. Record the approved scope in the ledger. A feature request or assumed necessity is not approval.
- Replace player-visible primitive scenery, pedestals, flat generated materials, and debug-style presentation in `MainGame.unity` with a coherent asset set.
- Create visual assets through Blender MCP or download free-to-use assets with an explicitly verified commercial-use license. Retain Blender sources/exports or source/license links and record everything in `docs/asset-ledger.md`.
- Add commercially licensed audio only when explicitly requested for a specific interaction. Use no music or voice acting.
- Runtime-generated excavation geometry may remain because it is a mechanic, but it must receive approved materials and fit the finished visual direction.
- Improve lighting, composition, scale, boundaries, water/scenery, HUD readability, and camera comfort as one coherent pass.
- Apply the official uGUI skill to the existing HUD: replace legacy `Text` with TextMeshPro, retain Input System navigation, and verify scroll/clipping and layout at supported window sizes. This supplies the presentation required to revalidate Tasks `05`-`07` before `09`.
- Reproduce and remove flicker at the surface/excavation boundary after repeated digging; verify from moving player viewpoints while preserving collision boundaries.

Do not create substitute assets with Unity primitives, generated meshes/materials, code, or image generation. If Blender MCP is unavailable and suitable licensed assets cannot be obtained, mark the task blocked and ask the user.

## Acceptance

- Nothing visibly presented as final content is an unlabeled primitive, debug object, flat placeholder material, or validation adapter.
- The excavation entrance, permanent boundaries, surface stations/anchors, environment, and HUD read clearly in the Windows build.
- Repeated sounds are restrained and varied; important interaction/detector space remains audible without music.
- Asset-ledger entries contain source, exact commercial license or Blender source, attribution, imported files, and approval state.
- Inspect the result through the official Unity CLI when available, rebuild `SomethingDownThere.exe`, and provide screenshots plus the executable for user review.

## Ongoing direction

Avoid endless brown mud, excessive darkness, and generic procedural scenery. General free-to-use commercially licensed assets are acceptable; distinctive discoveries and the evolving shovel deserve custom Blender work. Later feature tasks must maintain this quality bar.
