# Discovery display

Status: Task `51` is planned after saving (`35`), distinctive content/generation (`42`–`45`) and the main progression/traversal tasks through `50`.

Idea coverage: section 28.

Design: [60 - personal display and snapshot rules](../../development/tasks/60-discovery-display-design.md) defines interaction/capture behaviour before `51`. Achievement goals stay separate under `55`/`53`.

## Purpose

Give notable discoveries a lasting visual record without turning collection into an identification or museum-management chore.

## Task 51 - permanent discovery snapshots

See [numbered Task `51`](../../development/tasks/51-discovery-display.md) for scope, research, questions and acceptance.

## Before implementation

See [numbered Task `51`](../../development/tasks/51-discovery-display.md) for scope, research, questions and acceptance.

## Required behavior

- Duplicate finds do not create unintended duplicate milestones.
- Each entry shows only item name and discovery depth: no value, condition, rarity, or collection counter.
- The sold object can disappear normally while its personal excavation snapshot remains permanently at the surface.
- Missing or changed content fails safely after updates.
- Display limits and ordering remain simple and predictable.
- Capture happens before a recognized object disappears; repeated loads/sales cannot replace the first personal snapshot or add duplicates. A failed image write must not lose the find, block collection or corrupt excavation progress.

## Done when

See [numbered Task `51`](../../development/tasks/51-discovery-display.md) for scope, research, questions and acceptance.
