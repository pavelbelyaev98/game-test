# Terrain materials and resistance

Status: Task `39` is planned after the complete production trip (`15`) and independent shovel tracks (`25`). The current density terrain has one removal response; completed excavation/cleanup tasks remain done.

Idea coverage: sections 7–8 and remembered obstacles in section 39.

Design: [58 - site and material progression](../../development/tasks/58-site-and-terrain-design.md) sets vocabulary, resistance policy and scale before `39`; [63](../../development/tasks/63-windows-targets-and-budgets.md) supplies performance targets.

## Purpose and research

Make geological variety and old obstacles give upgrades a physical meaning while retaining free-form excavation. Read [progression/discovery](../../research/player-review-findings.md#progression-and-discovery) and [physical comfort](../../research/player-review-findings.md#physical-comfort); inspect density, removal accounting, chunk collision and saved state before choosing resistance representation.

Geological appearance changes gradually through overlapping formations in one continuous excavation, without separate levels or biome unlocks. Ordinary-looking excavation material eventually yields to suitable equipment; true finite-world boundaries require categorically different structure/scale/presentation, so players do not mistake them for future upgrades. Materials and tool milestones use restrained approved digging-sound variation; the user's terrain direction and asset approval gates remain.

**Selected scope clarification:** underground variety currently means different textures/material areas. Caves and pre-existing tunnel/chamber areas are not planned; the earlier suggested cave reveal is deferred. Players may still excavate their own tunnels. `58` selects the exact material/resistance progression without adding a cave phase.

## Selected ground appearance

Implemented by [77 - original ground textures](../../development/tasks/77-ground-textures.md): short garden turf over warm granular earth with sparse, partly buried, dirt-covered grey/brown stones, varied sizes and worn rocky relief. The user rejected uniform round dots and overly dense, clean, conspicuous stones; soil remains visually dominant. Original Blender-authored seamless textures use consistent world-space coverage: two metres for soil, one metre for turf, including floors, slopes and fresh walls.

Turf follows the original surface height and upward-facing ground; deeper cuts expose soil. This appearance does not select geological formations, change resistance or claim the later lighting/presentation acceptance. Existing user-owned terrain geometry and saved excavation are preserved. Creation was explicitly authorized for this batch; online asset imports still require approval.

## Task 39 - integrated material resistance

See [numbered Task `39`](../../development/tasks/39-terrain-materials.md) for scope, research, questions and acceptance.

## Before implementation

See [numbered Task `39`](../../development/tasks/39-terrain-materials.md) for scope, research, questions and acceptance.

## Acceptance

See [numbered Task `39`](../../development/tasks/39-terrain-materials.md) for scope, research, questions and acceptance.
