# Presentation and audio

Status: Task `08` is ready and blocks further player-facing feature work until the visible foundation is no longer placeholder quality.

Idea coverage: sections 42-45.

## Purpose

Establish a cohesive, readable production look and sound direction that starts grounded and can support increasingly strange discoveries. The main game must not resemble a primitive mechanics test.

## Task 08 - production foundation

- Replace player-visible primitive scenery, pedestals, flat generated materials, and debug-style presentation in `MainGame.unity` with a coherent asset set.
- Create visual assets through Blender MCP or download free-to-use assets with an explicitly verified commercial-use license. Retain Blender sources/exports or source/license links and record everything in `docs/asset-ledger.md`.
- Source free-to-use, commercially licensed ambience and essential movement/digging feedback. Use no music and no voice acting.
- Runtime-generated excavation geometry may remain because it is a mechanic, but it must receive approved materials and fit the finished visual direction.
- Improve lighting, composition, scale, boundaries, water/scenery, HUD readability, and camera comfort as one coherent pass.
- Apply the official uGUI skill to the existing HUD: replace legacy `Text` with TextMeshPro, retain Input System navigation, and verify scroll/clipping and layout at supported window sizes. This supplies the presentation required to revalidate Tasks `05`-`07` before `09`.

Do not create substitute assets with Unity primitives, generated meshes/materials, code, or image generation. If Blender MCP is unavailable and suitable licensed assets cannot be obtained, mark the task blocked and ask the user.

## Acceptance

- Nothing visibly presented as final content is an unlabeled primitive, debug object, flat placeholder material, or validation adapter.
- The excavation entrance, permanent boundaries, surface stations/anchors, environment, and HUD read clearly in the Windows build.
- Repeated sounds are restrained and varied; important interaction/detector space remains audible without music.
- Asset-ledger entries contain source, exact commercial license or Blender source, attribution, imported files, and approval state.
- Inspect the result through the official Unity CLI when available, rebuild `SomethingDownThere.exe`, and provide screenshots plus the executable for user review.

## Ongoing direction

Avoid endless brown mud, excessive darkness, and generic procedural scenery. General free-to-use commercially licensed assets are acceptable; distinctive discoveries and the evolving shovel deserve custom Blender work. Later feature tasks must maintain this quality bar.
