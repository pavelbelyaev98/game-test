# Task 08 - Production visual and audio foundation

Type: implementation. Status: `planned`. Prerequisites: 57 and the existing user-selected art scope/specific batch approvals.

Feature: [presentation audio](../../features/backlog/presentation-audio.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Before any new asset/audio enters the project, explain the specific item or listed batch, purpose, source/license, files/integration and removal steps; show a preview/sample when available, ask and wait for explicit user approval. Record the approved scope in the ledger. A feature request or assumed necessity is not approval.
- Replace remaining player-visible primitive/flat placeholder presentation in `MainGame.unity` with the specifically approved coherent asset set. Preserve the completed `12` amber SELL/teal UPGRADES station pair and the user's terrain ownership; do not replace already-finished stations as if they were still pedestals.
- Create visual assets through Blender MCP or download free-to-use assets with an explicitly verified commercial-use license. Retain Blender sources/exports or source/license links and record everything in `docs/asset-ledger.md`.
- Add commercially licensed audio only when explicitly requested for a specific interaction. Use no music or voice acting.
- Runtime-generated excavation geometry may remain because it is a mechanic, but it must receive approved materials and fit the finished visual direction.
- Improve lighting, composition, scale, boundaries, water/scenery, HUD readability, and camera comfort as one coherent pass; preserve the working settings from `65` through the production UI migration.
- Apply the official uGUI skill to the existing HUD: replace legacy `Text` with TextMeshPro, retain Input System navigation, and verify scroll/clipping and layout at supported window sizes. This supplies the presentation required to revalidate Tasks `05`-`07` before `09`.
- Reproduce and remove flicker at the surface/excavation boundary after repeated digging; verify from moving player viewpoints while preserving collision boundaries.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect the current approved/user-owned scene and use the accepted [57 presentation brief](57-presentation-design.md). Research implementation-specific contact/reveal readability and sample fatigue. Prepare and obtain explicit approval for the actual asset/audio batches; the brief does not bypass the existing deferred art scope.
- Any approved frequent digging/motor/jetpack/station sounds need restrained levels, appropriate variation and clean start/stop/layering. Compare short repeated sessions before accepting samples; `10` owns detector feedback and `54` reviews full-session fatigue. No extra ambience pack, music or voice acting is authorized by the research.

Do not create substitute assets with Unity primitives, generated meshes/materials, code, or image generation. If Blender MCP is unavailable and suitable licensed assets cannot be obtained, mark the task blocked and ask the user.

## Acceptance

- Nothing visibly presented as final content is an unlabeled primitive, debug object, flat placeholder material, or validation adapter.
- The excavation entrance, permanent boundaries, surface stations/anchors, environment, and HUD read clearly in the Windows build.
- Repeated sounds are restrained and varied; important interaction/detector space remains audible without music.
- Asset-ledger entries contain source, exact commercial license or Blender source, attribution, imported files, and approval state.
- Inspect the result through the official Unity CLI when available, rebuild `SomethingDownThere.exe`, and provide screenshots plus the executable for user review.
