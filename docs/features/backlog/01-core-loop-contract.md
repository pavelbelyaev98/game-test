# 01 - Core loop and first-playable scene

Status: task `01` is ready. This is explicitly a small planning task; implementation should be queued as concrete follow-ups.

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose

The game is a short, funny first-person excavation game that starts believable and becomes absurd. It depends on a repeatable trip: dig, detect, uncover, collect, decide whether to push farther, return, sell, recharge, upgrade, and repeat. The surface is a short checkpoint, while excavation and discovery occupy most play time.

## Task

Define the smallest scene that can exercise one complete trip. Name scene-owned objects, system ownership, temporary fixtures, and the first implementation tasks. Do not design production versions of every later system.

## Required behavior

- Start from untouched ground with no forced route.
- Let a player find something, return physically, sell it, purchase one improvement, and dig again.
- Use a finite authored surface with attractive scenery, some visible water, no required swimming, visually distinct permanent boundaries, and a persistent excavation.
- Target roughly 2-3 hours for first completion, with replayability from changed underground placements and optional completion.
- Do not turn the project into survival, crafting, puzzle, geology, museum, inventory-management, infinite-world, or automation gameplay. The player keeps doing the digging.

## Done when

- The scene plan maps each object to an existing or planned runtime owner.
- Follow-up tasks are small, ordered, and link their owning feature document.
- The plan names one playable proof of the full loop and its main limitation.
- Its proof recreates the intended tension: a signal tempts the player to make one more discovery before a risky return and meaningful upgrade.
