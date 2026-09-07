# 03 - Excavation and terrain

Status: planned.

Idea coverage: sections 4-8 and relevant tuning in section 53.

## Purpose

Make digging itself satisfying and allow players to create pits, tunnels, trenches, and strange routes instead of following a vertical corridor.

## Implementation task

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
