# Task 111 - Deliberate discovery pickup

Superseded by [112 - held pickup under the crosshair](../tasks/112-held-aimed-pickup.md). The fresh-press requirement was an overstrict interpretation; the user explicitly wants held pickup on directly hovered finds. This record retains the earlier implementation/evidence.

Type: implementation. Status: `done`. Prerequisites: 110. Explicit user-selected correction.

Feature: [collection](../../features/backlog/discovery-collection.md), [starter batch](../../features/backlog/starter-find-batch.md). Context: [recognition risks](../design-risks.md), [feel playtest](101-collection-return-playtest.md).

## Selected behavior and feel

- User observed powerful digging invisibly collecting the reward. Every ordinary find must remain in the world after uncovering until a **fresh press of the bound Dig button while aiming at it**. Holding or toggling digging cannot collect a newly revealed or newly aimed find.
- Preserve 60% bottle exposure, real line of sight, 3 m pickup reach and physical release. A fully detached bottle may fall and settle before pickup; it never transfers to the bag because soil was removed.
- A deliberate press collects one already eligible find, including during shovel cooldown, without also digging or spending fuel. A press that uncovers a find does not also collect it. No delay, timer, inspection screen or extra binding.
- Toggle mode uses the same fresh press for pickup, including the press that stops ongoing digging. A pickup attempt consumes toggle intent; it does not start unattended digging afterwards. Hold mode may resume digging after existing pickup recovery, but cannot collect another find without a new press.
- Full bags retain the find; making space does not auto-collect. Menu/focus/rebind release barriers remain intact. Show the actual bound pickup button in the existing target prompt.

## Acceptance

- Simulate actual Input System hold and remapped toggle inputs against MainGame with weak and strongest shovels. Excavation must leave revealed bottles visible/uncollected until deliberate pickup; test physical drops, aim changes and one pickup per press.
- Verify full-bag recovery, menu/focus barriers, no fuel/stroke on pickup or toggle-stop, and no duplicate inventory identity. Preserve underlying exposure/physics/save behavior.
- Run proportionate input and discovery regressions, inspect the live result through official Unity CLI, and provide a Windows build.
- Update current contracts, risk guidance and user feel checks to supersede the old continuous-collection/no-extra-click rule. Subjective recognition remains user-reviewed in `101`.

Questions: none blocking. User explicitly selected deliberate pickup; special E interactions/chests remain `97`.

Result: [integrated behavior, 170 passing checks, live inspection and Windows build](../completed/111-deliberate-discovery-pickup.md). User feel remains with `101`.
