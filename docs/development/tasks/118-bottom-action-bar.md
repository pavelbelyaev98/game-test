# Task 118 - Show the backpack and available actions at the bottom of the HUD

Type: implementation. Status: `planned`. Prerequisites: [106's reviewed action-bar layout](106-ui-ux-audit-design.md), `107` shared theme, and approval of the specific icon asset(s). Existing inventory/input/HUD `74`/`75`/`78` supply the behavior. No C4 dependency for the backpack delivery.

Feature: [menu/HUD presentation](../../features/backlog/menu-presentation.md). Controls: [FPS contract](../../features/backlog/fps-controls.md). Future C4 entry: [71](71-dynamite-design-research.md).

## Requested outcome

The player can see how to open the backpack without searching Pause or asking for its key. Show a compact backpack icon and the current Inventory binding at the bottom of the gameplay screen; Tab is only the default. The user also proposes future equipment/action entries such as C4 with its selection key. This is a small action bar, not a grid of carried loot or a new tool-switching requirement.

## Scope and behavior

- Use the existing UI Toolkit HUD/document and `106`/`107` styles. Keep the backpack entry visible during active gameplay, including an empty/full bag; hide it with the HUD behind menus/startup. Preserve the existing capacity and battery readings without duplicating them in another resource panel.
- Read the binding from the same Input System preferences/display source as Controls and Pause. Refresh after remapping, conflict replacement, reset and reload; handle long key names and mouse bindings. Never hardcode Tab in the live label or create a second inventory input handler.
- Pressing the displayed key opens the existing inventory. Closing, focus loss/resume and action release use the existing input barriers; the bar cannot resume digging or consume a gameplay click. Keep the gameplay HUD non-picking under pointer lock; a clickable variant would require a reviewed control change.
- C4 and other unimplemented features have no live slot, fake key, locked teaser or empty placeholder. `106` may illustrate them as explicitly proposed future states. When a feature is implemented, its owner adds an entry using its actual availability, binding, selection and charge rules; temporary empty/unusable states must be distinguishable from an absent feature.
- Do not invent C4 bindings, grant charges, add detonation or equip a second primary tool in this task. `71` and its eventual delivery own those actions. Distinguish opening inventory from selecting equipment; selecting an explosive must never implicitly place or detonate it.
- Apply a coherent icon/key presentation at the bottom, avoiding target notices, resource readings and visible tool motion. `106` decides exact alignment, size, labels and state emphasis with concrete examples at supported window sizes. Avoid pulsing tutorial reminders and a permanent list of every movement command.

## Preparation and assets

Inspect `GameHudView`, HUD UXML/USS, `GameMenuView`, `FpsInput` and `InputPreferences`, plus `106`'s catalog. Reuse an existing approved icon if suitable; otherwise prepare a specific licensed commercial-use or Blender-created icon proposal with preview, files and removal steps before adding it. The feature request does not approve an icon pack, generated substitute art, font or sound.

## Acceptance

- The Windows build shows a recognizable backpack icon and truthful binding without opening a menu. The user can discover and open inventory from that cue unaided; empty/full inventory and changed bindings work identically.
- Verify default and remapped keyboard/mouse bindings, reset, focus/menu transitions, relaunch, long labels and supported window sizes. Preserve quiet HUD legibility, capacity/battery facts and actual inventory behavior; no C4 slot appears while C4 is unimplemented.
- Cache/update presentation when state changes rather than rebuilding it each frame. Run a fast deterministic check and focused binding/visibility integration checks; inspect MainGame through the official Unity CLI and provide `builds/windows/SomethingDownThere.exe` with before/after evidence for user review.
- Update the existing UI catalog, owning feature and status. Record the delivered backpack entry and any explicitly deferred future entries; compilation or a mockup alone is not completion.

## Questions owned by the existing review

`106` reviews the exact bottom layout, icon treatment and whether any other already-implemented action earns a place. C4 selection/use and its future entry remain with `71`; do not hold the backpack delivery until optional equipment exists.
