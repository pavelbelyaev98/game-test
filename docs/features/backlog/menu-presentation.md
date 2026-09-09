# Menu presentation and authoring

Status: **menus implemented in [74](../../development/tasks/74-ui-toolkit-menus.md), HUD in [75](../../development/tasks/75-ui-toolkit-hud.md)** at the user's request. [Authoring guide](../../development/ui-authoring.md).

## Selected approach

- Use UI Toolkit, UI Builder, UXML layouts and shared USS styles for screen menus. Keep C# responsible for existing commands and session data, with clear ownership of callbacks and focus. No external widget library is required for current controls.
- Current scope: Pause, Camera comfort, inventory, sell/upgrade stations, rescue/terrain-reset confirmations, developer controls and save/recovery/error screens. Future front-end screens reuse the same theme when their gameplay contracts are implemented.
- All screen presentation uses Toolkit, including the gameplay HUD. One retained EventSystem/InputSystemUIInputModule supplies menu input; there is no Canvas HUD. Appearance remains a design/layout decision. World art is outside this migration.
- Implemented direction: warm charcoal, cream and brass, readable type, clear section spacing, softly rounded panels, quiet borders and immediate hover/focus/pressed states. The user's terrain/art ownership remains unchanged.
- Reuse the existing font. UXML/USS/C# are the authoring sources for this requested UI change; imported fonts, textures, icons and sounds still require specific approval. Do not introduce decorative world assets or fake settings to fill a menu.

## Behavior that survives restyling

- Preserve the [FPS input barriers](fps-controls.md) and [camera comfort contract](camera-comfort.md). Escape has one owner; settings Back returns to Pause, preserves values and focuses Camera comfort. Slider/toggle/reset updates retain controls and focus.
- Inventory inspection never sells. Stations show named rows, values, current/next equipment comparisons and explicit purchases; stale offers cannot mutate state. Confirmations select the safe action first and state exact losses.
- Save/loading/recovery failures retain their original distinctions and available actions. Camera preference failures stay local and retryable. Menu changes cannot create world checkpoints or erase excavation.
- Menus adapt to the supported window sizes and scroll long content while keeping necessary actions accessible. Disabled actions explain their state; mouse/keyboard focus remains visible. Hide the gameplay HUD while a menu is open; inventory/trading screens show the relevant carried counts and credits within their own layout. Restore the existing quiet HUD on Resume.

## Ownership and later work

`74` owns implementation, measured migration evidence and the concise authoring guide. `57` consumes this menu direction while designing broader presentation; `05` preserves it through final HUD acceptance. `54` repeats sustained readability/input/comfort review on final content.

**Implemented in `75` at the user's request:** the HUD shares the menu palette, font, document and scaling. Cached controls preserve the reticle, resource bands/recharge, readings, target/feedback and projected developer markers; it ignores pointer picking and hides behind menus. The old Canvas/text-scaling workaround is removed. `57`/`08` refine presentation through these sources and `05` retains the behavior checks; they do not need another framework migration.

[Unity 6.6's comparison](https://docs.unity3d.com/6000.6/Documentation/Manual/UI-system-compare.html) retains uGUI advantages in Animation Clips/Timeline, in-scene authoring and serialized events; this HUD uses none of those. Shared authoring justified migration. Package removal remains separate because Toolkit menus still use the existing uGUI EventSystem; replacing that input routing needs its own concrete benefit and validation.
