# Task 74 — Research and migrate screen menus to UI Toolkit

Type: implementation with technical research. Status: `done`. Prerequisites: `35`, `65` (complete). User-selected ahead of `66`. [Completion](../completed/74-ui-toolkit-menus.md).

Feature: [menu presentation](../../features/backlog/menu-presentation.md). Preserve [camera comfort](../../features/backlog/camera-comfort.md), [FPS input](../../features/backlog/fps-controls.md), station/rescue/save contracts and [review findings](../../research/player-review-findings.md#clear-information-and-player-observation).

## Scope and research decision

- The user explicitly requested fresh UI Toolkit research and implementation for attractive, editable menus. This task migrated Pause, Camera comfort, inventory, station, confirmation, developer and save/recovery screens into one shared Toolkit design. The HUD was retained during this scope and subsequently migrated in [75](75-ui-toolkit-hud.md). Unimplemented settings/front-end features from the supplied example remain outside scope.
- Unity 6.6 [comparison](https://docs.unity3d.com/6000.6/Documentation/Manual/UI-system-compare.html) still recommends uGUI generally for runtime and identifies Toolkit for multi-resolution menus/HUD. uGUI is not obsolete; wholesale removal merely because of age is rejected. Toolkit suits the requested shared stylesheet/UI Builder workflow.
- [Runtime events](https://docs.unity3d.com/6000.6/Documentation/Manual/UIE-Runtime-Event-System.html) support coexistence through the existing EventSystem/InputSystemUIInputModule. Keep its custom arrows/Enter bindings and one gameplay-owned Escape; check actual navigation rather than assuming UI consumes gameplay.
- UXML owns layouts, USS owns reusable theme/control states, and C# binds existing commands/data. UI Builder edits those sources. USS is a subset of CSS; validate actual imports/control structure against installed Unity 6000.6.0f1. No engine/package upgrade or widget library is required.
- Installed-engine checks require an authored panel configuration for built-in ICU text data, `FocusController.IgnoreEvent` for explicit navigation order, and suppression of duplicate raw arrow keys in native players. Existing font selection goes through TextCore's runtime cache. Keep required actions outside scrolling content; style the entire scrollbar input container.
- Use the current font and Unity controls. Source layouts/styles implement the explicit UI request; additional imported fonts, images, icons and audio remain subject to the existing item approval rule. Runtime panel/text configuration does not import a new art/font pack.

## Acceptance

- A coherent, visibly improved shared menu theme is integrated in MainGame and the Windows player. Clear hierarchy, consistent buttons, readable values/errors, visible mouse/keyboard focus and distinct destructive actions work at 960×540, 1080p and 16:10. Long inventory/loss lists scroll without hiding required actions.
- Preserve every existing command, safe default selection, transaction revision guard, empty/disabled/overflow state and save recovery path. No world mutation on inventory inspection, stale offer activation or menu navigation.
- Retain Task 65 persistence/reset/error behavior and steady reticle; no per-frame UI reconstruction or preference writes. Preserve selection across value updates and focus loss; arrow navigation reveals offscreen rows.
- Use official CLI to inspect imports, live menus and native captures. Run relevant input/menu/transaction/save tests once at completion; deliver `builds/windows/SomethingDownThere.exe` and concise evidence.
- Record the framework decision and future migration boundary in the owning feature/architecture. This task does not certify deferred world art, future tools or final HUD acceptance (`57`/`08`/`05`).

## Selected presentation and delivery

The recommended warm charcoal/cream/brass direction is implemented; the optional preference question received no different selection. UXML/USS own the shared design, the authored panel carries Unity's text data, and existing gameplay commands remain in their owning systems. [Authoring guide](../ui-authoring.md).

Acceptance passed: **68/68 PlayMode tests**, official CLI menu inspection and native 960×540/1080p/16:10 review, mouse/keyboard scrolling, one-degree FOV/reset/error Retry, sale/purchase, interrupted-save recovery and failed-save/quit confirmation. The Windows build is current; profiles and MainGame were restored. Evidence and remaining scope are in the completion record.
