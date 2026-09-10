# Task 112 - Held pickup under the crosshair

Type: implementation. Status: `done`. Prerequisites: 111. Explicit user correction to the previous interpretation.

Feature: [collection](../../features/backlog/discovery-collection.md), [controls](../../features/backlog/fps-controls.md). Context: [recognition risks](../design-risks.md), [feel playtest](101-collection-return-playtest.md).

## Selected behavior

- Holding Dig while hovering directly over an eligible exposed find collects it. No release/repress is required, including when moving aim onto a resting bottle or another eligible find. Toggle digging provides the same aimed collection while active; its stop press performs no action.
- Collection uses only the centre crosshair ray, real line of sight, the authored 60% bottle exposure and 3 m pickup reach. The shovel's area, a terrain-change notification and physical release never collect finds themselves.
- A digging stroke and pickup remain separate actions. After a wide scoop, bottles outside the current aim stay visible/physical until the player targets them. Keep one-time identity, capacity and zero-energy pickup, existing recovery, physics and save behavior.
- Full bags retain finds. Space becoming available permits pickup only if digging is still active and the centre ray directly targets that eligible find. Menus/focus/rebinding continue to clear held/toggled intent.
- Update prompts, current contracts and future feel checks. `111`'s fresh-press rule was an overcorrection and is superseded; do not reintroduce it as a recognition fix. No new delay or binding.

## Acceptance

- Actual Input System tests: weak/strong held and remapped-toggle pickup without another press; the strongest wide scoop exposes all three bottle variants outside aim without collecting them; approach/hover over those bottles with the same active input to collect once.
- Verify physical drops, full bag, occlusion/reach/exposure, one-action pickup with no fuel cost, toggle stop and menu/focus/rebind barriers. Run proportionate input/discovery/UI regressions.
- Inspect MainGame through the official Unity CLI and provide a current Windows build. Record evidence and leave subjective recognition/feel verdicts to `101`.

Questions: none blocking; user explicitly clarified hover-based held pickup.

Result: [integrated behavior, 169 passing checks, live inspection and Windows build](../completed/112-held-aimed-pickup.md). User feel remains with `101`.
