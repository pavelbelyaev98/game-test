# Excavation and terrain

Status: Task `06` terrain behavior is verified; Task `18` reopened production presentation acceptance pending `08`. Later tasks extend it toward the full requirements below.

Idea coverage: sections 4-8 and relevant tuning in section 53.

## Purpose

Make digging itself satisfying and allow players to create pits, tunnels, trenches, and strange routes instead of following a vertical corridor.

## Task 06 - finite terrain shell

- Scope: implement `TerrainVolume` behind `IDigTarget` with one tunable soil material, permanent bedrock/perimeter, local mesh/collider updates and in-memory removal state. Establish the [main game scene](core-loop.md) with the existing player, surface, scenery, visible inaccessible water and clear station/return anchors.
- Acceptance: untouched start; downward, diagonal and lateral cuts produce traversable space with matching collision; repeated/rejected hits cannot breach boundaries or spend energy without changing terrain. Returning to the surface preserves cuts for this scene session.
- Evidence: focused terrain/state checks, player collision/dig integration and editor inspection of shell wiring; record representation choice, measured update cost and visual limits. Preserve the existing FPS scene and `.meta` references.
- Production acceptance after `08`: replace visible primitive scenery and generated materials with approved assets, then inspect traversal/boundary readability in the Windows build. Disk saving and material variety remain later work.

## Later expansion

Build persistent terrain removal that supports downward, diagonal, and sideways digging. Separate diggable materials, tool resistance, permanent boundaries, and save/session state behind focused contracts.

## Required behavior

- The site starts untouched and offers no pre-dug solution.
- Pits, trenches, tunnels, and overhangs may emerge if technically stable; no tunnel route is prescribed.
- Sideways excavation can lead to discoveries, clusters, useful routes, and remembered obstacles.
- Materials may include sand, soil, clay, gravel, compact sediment, rock, and occasional constructed material, but texture never acts as a treasure marker.
- Hard terrain communicates whether progress is slow or currently impossible.
- Toughness can rise loosely with depth while local hard formations appear at varied depths.
- Upgrades must overpower old obstacles rather than immediately replacing them with proportionally tougher ground.
- Removed terrain stays removed for the intended persistence lifetime.

## Done when

- Repeated digging produces stable non-linear shapes without holes through permanent boundaries.
- Material/tool rules and persistence pass focused game-owned checks.
- Performance and visual limitations are recorded briefly.
