# 74 — UI Toolkit menus

- Researched installed Unity 6.6 and selected Toolkit for editable screen menus; the HUD was retained during this scope and subsequently migrated in [75](75-ui-toolkit-hud.md). [Contract](../../features/backlog/menu-presentation.md) and [authoring guide](../ui-authoring.md) preserve the shared UI direction for `57`/`05`/`54`.
- Migrated Pause, Camera comfort, inventory, sell/upgrade, rescue/reset, developer and save/recovery/error menus to shared UXML/USS. Charcoal/cream/brass styling, persistent camera controls, safe focus, scrolling and fixed action footers retain existing game commands and transaction guards.
- **68/68 PlayMode tests passed** on the final sources. Official CLI inspected MainGame menus; native 960×540, 1920×1080 and 1280×800 checks covered mouse/keyboard, scrolling, exact FOV steps, reset/local Retry, sale/purchase, recovery and failed-save/quit confirmation (`unity/Logs/Task74/`).
- Authored panel includes Unity's ICU data; the final player log has only the deliberately induced save failure, with no UI/font exceptions. Native/Editor profiles were restored and MainGame is clean at 75°.
- [Windows executable](../../../builds/windows/SomethingDownThere.exe) rebuilt **2026-09-09 06:48 UTC**; build succeeded with zero errors and the expected Pipeline development-build warning.
- Remaining: final HUD/art/long-session acceptance belongs to `05`/`57`/`54`; new front-end/settings features reuse the theme when implemented. Restart Editor Play after UXML/USS edits to rebind the replaced document tree. Next eligible task: `66`.
