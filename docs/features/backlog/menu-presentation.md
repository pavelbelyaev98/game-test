# Menu presentation and authoring

Status: **menus implemented in [74](../../development/tasks/74-ui-toolkit-menus.md), HUD in [75](../../development/tasks/75-ui-toolkit-hud.md)** at the user's request. [Authoring guide](../../development/ui-authoring.md).

## Complete UI/UX review and cleanup

The user requests visibility of every UI/UX state, all player-visible text and every conditional menu before cleanup. [106](../../development/tasks/106-ui-ux-audit-design.md) inventories actual runtime screens, dynamic text, triggers/conditions, actions/navigation, transient feedback and rare/error branches, with a searchable copy catalog and visual review sheets. This audit is requested but not yet performed; existing authoring docs are not a complete screen/text inventory.

The review distinguishes implemented release UI, development-only UI and proposed future screens. Exact current copy/templates and conditions have one catalog owner; review sheets reference those entries and record keep/change/remove/merge decisions. [107](../../development/tasks/107-ui-ux-cleanup.md) implements the selected cleanup and verifies changed flows in a Windows build. New UI changes keep that catalog current.

Preserve useful state/loss information while reviewing text and navigation. Do not hide rare menus from the review, mistake log strings for visible copy, delete safe recovery paths to reduce button count, or treat more concise wording as proof of better understanding. User-selected copy/flow decisions will be linked here after `106`; none is inferred from the request alone.

## Selected approach

- Use UI Toolkit, UI Builder, UXML layouts and shared USS styles for screen menus. Keep C# responsible for existing commands and session data, with clear ownership of callbacks and focus. No external widget library is required for current controls.
- Current scope: startup, Pause, Camera comfort, inventory, sell/upgrade stations, terrain-reset confirmation, developer controls and save/recovery/error screens. [79](../../development/tasks/79-startup-menu.md) adds the startup flow using this theme.
- All screen presentation uses Toolkit, including the gameplay HUD. One retained EventSystem/InputSystemUIInputModule supplies menu input; there is no Canvas HUD. Appearance remains a design/layout decision. World art is outside this migration.
- Implemented direction: warm charcoal, cream and brass, readable type, clear section spacing, softly rounded panels, quiet borders and immediate hover/focus/pressed states. The user's terrain/art ownership remains unchanged.
- Reuse the existing font. UXML/USS/C# are the authoring sources for this requested UI change; imported fonts, textures, icons and sounds still require specific approval. Do not introduce decorative world assets or fake settings to fill a menu.

## Behavior that survives restyling

- Preserve the [FPS input barriers](fps-controls.md) and [camera comfort contract](camera-comfort.md). Escape has one owner; settings Back returns to its originating startup/Pause screen, preserves values and focuses Settings/Camera comfort. Slider/toggle/reset updates retain controls and focus.
- Inventory inspection never sells. Stations show named rows, values, current/next equipment comparisons and explicit purchases; stale offers cannot mutate state. Confirmations select the safe action first and state exact losses.
- Startup offers New Game, Load Game, Settings and Quit. Keep the existing single slot; disable Load Game with an explanation when absent. Browsing/settings/quit leave the world untouched. Explicit New Game confirms replacement when a checkpoint exists, focuses Cancel first, retains a recovery archive and starts fresh only after its first checkpoint succeeds. Load Game never silently generates a world on failure.
- Windows builds allow one concurrent game instance, independently of Steam and development/release mode. Keep exclusive save ownership as a safeguard; contention explains that the save is already open and allows Retry, Back to menu and Quit. OS exception details and personal filesystem paths belong in diagnostic logs, not menu text.
- Save/loading/recovery failures retain their original distinctions and available actions. Camera preference failures stay local and retryable. Starting/loading enters gameplay only when ready and focused; focus loss leaves Pause visible. Ordinary menu navigation cannot create checkpoints or erase excavation.
- Menus adapt to the supported window sizes and scroll long content while keeping necessary actions accessible. Disabled actions explain their state; mouse/keyboard focus remains visible. Hide the gameplay HUD while a menu is open; inventory/trading screens show the relevant carried counts and credits within their own layout. Restore the existing quiet HUD on Resume.

## Ownership and later work

`74` owns completed migration evidence and the authoring guide. `106`/`107` own the new whole-interface review and selected current-UI cleanup; they do not reopen completed tasks. `57` consumes the resulting decisions for broader feedback/presentation, `05` preserves behavior through final HUD acceptance and `54` repeats final readability/input/comfort review. Return-warning policy remains `93`/`94`; future features add their states/text to the same catalog.

**Implemented in `75` at the user's request:** the HUD shares the menu palette, font, document and scaling. Cached controls preserve the reticle, resource bands/recharge, readings, target/feedback and projected developer markers; it ignores pointer picking and hides behind menus. The old Canvas/text-scaling workaround is removed. `57`/`08` refine presentation through these sources and `05` retains the behavior checks; they do not need another framework migration.

[Unity 6.6's comparison](https://docs.unity3d.com/6000.6/Documentation/Manual/UI-system-compare.html) retains uGUI advantages in Animation Clips/Timeline, in-scene authoring and serialized events; this HUD uses none of those. Shared authoring justified migration. Package removal remains separate because Toolkit menus still use the existing uGUI EventSystem; replacing that input routing needs its own concrete benefit and validation.
