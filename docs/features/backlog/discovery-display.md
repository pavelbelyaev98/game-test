# Discovery display

Status: planned after core collection and persistence.

Idea coverage: section 28.

## Purpose

Give notable discoveries a lasting visual record without turning collection into an identification or museum-management chore.

## Implementation task

Create a compact surface display (for example a refrigerator, corkboard, wall, or workbench) that automatically captures how each distinctive item looked when first discovered.

## Required behavior

- Duplicate finds do not create unintended duplicate milestones.
- Each entry shows only item name and discovery depth: no value, condition, rarity, or collection counter.
- The sold object can disappear normally while its personal excavation snapshot remains permanently at the surface.
- Missing or changed content fails safely after updates.
- Display limits and ordering remain simple and predictable.

## Done when

- Discover, deduplicate, save/load, and version-migration checks pass.
- The display does not block selling or the core trip loop.
- Explicit completion goals belong in platform achievements rather than cluttering the display.
