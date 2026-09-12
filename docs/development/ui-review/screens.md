# Screens and flow review

Current copy/conditions/actions: [catalog.csv](catalog.csv). Visual direction and selected settings order: [theme-and-navigation](theme-and-navigation.md). User verdict for every group: **UNREVIEWED**. The requested theme is implemented; the whole-interface subjective review remains with `106`.

## Theme comparison (107)

These views document the delivered theme. [Common settings](common-settings.md) contains the current five-category shared rows and standalone dialogs from `123`; older captures below retain pre-122 wording.

| Group | Before | Theme view and what to assess |
|---|---|---|
| Startup | Cream/brass, New Game first (temporary capture cleaned) | White / Continue first (temporary capture cleaned): title hierarchy, focus marker, ordered actions |
| Camera | Nested settings (temporary capture cleaned) | Camera tab (temporary capture cleaned): aligned values, white slider, common settings title |
| Controls | Earlier single scrolling list | Grouped bindings (temporary capture cleaned): movement/actions, current keys, persistent footer |
| Pause | Separate Camera comfort / Controls buttons | Unified Settings entry (temporary capture cleaned): Resume first, development actions separated |
| Shops | Cream/brass rows and inline comparisons | Sell (temporary capture cleaned) / upgrade (temporary capture cleaned): fixed balance, current/next columns, safe Close focus |

## Conditional screens

These are **isolated MainGame presentation fixtures**, with deliberate data/state setup under `Logs/Task107/PreviewProfile`. They render production UXML/C#; save-error injection previews layout, not the cause of a real storage failure. Separate tests exercise real state transitions and persistence.

| Case / entry | Capture | Actions / exit |
|---|---|---|
| Startup without checkpoint | No save (temporary capture cleaned) | Continue disabled with explanation; New Game focused |
| New Game over existing slot | Replacement (temporary capture cleaned) | Cancel first / Start New Game; Escape cancels |
| Backpack 0 / 10 finds | Empty (temporary capture cleaned) / full (temporary capture cleaned) | Close / Inventory key / Escape; no transaction |
| Sell with empty bag | Empty shop (temporary capture cleaned) | Sell all disabled; Close available |
| Upgrade without funds / level cap | Shortfall (temporary capture cleaned) / maxed (temporary capture cleaned) | Price/shortfall or max state; Buy disabled; Close available |
| Long current binding | Numpad Enter (temporary capture cleaned) | Exact current label; no hardcoded Tab |
| Preference write failures | Camera (temporary capture cleaned) / Controls (temporary capture cleaned) | Session changes kept; visible Retry and Back |
| Binding capture / conflict | Listening (temporary capture cleaned) / conflict (temporary capture cleaned) | Capture blocks gameplay; Escape cancels; conflict defaults to Cancel |
| Starting / loading | Starting (temporary capture cleaned) / loading (temporary capture cleaned) | Wait; no early Resume or destructive fallback |
| Save already open | Contention (temporary capture cleaned) | Retry / Back to menu / Quit |
| New-game / load failure | Start error (temporary capture cleaned) / load error (temporary capture cleaned) | Kept-file disclosure; Retry and exit/recovery route |
| Previous checkpoint recovery | Recovered (temporary capture cleaned) | Continue recovered excavation / Open save folder / Quit |
| Write error / discard confirmation | Write error (temporary capture cleaned) / discard (temporary capture cleaned) | Retry / folder / Quit...; then safe Back or explicit unsaved exit |
| Development-only tools / reset | Admin (temporary capture cleaned) / reset (temporary capture cleaned) | Actual session overrides; destructive reset starts with Keep excavation |

## Coverage and remaining review

The catalog contains 139 source-traced entries/families including authored controls, dynamic copy, HUD priority, developer-only branches and excluded legacy fixture names. Runtime code overrides are authoritative over UXML preview defaults. Built-in keyboard labels come from Input System; the five settings categories are included. There are no current tooltips, credits, detector, ending, photo or C4 screens to catalog.

Relevant integration evidence: [UI input](../../../unity/Logs/Task107/ui-tests.json), [startup](../../../unity/Logs/Task107/startup-tests.json), [stations](../../../unity/Logs/Task107/station-tests.json), [rescue](../../../unity/Logs/Task107/rescue-tests.json), [save/recovery](../../../unity/Logs/Task107/save-tests.json), [EditMode](../../../unity/Logs/Task107/edit-tests.json). MainGame captures and native Windows review complement those tests; this is not a claim that every possible numeric text variant was manually observed.

Size evidence: 960×540 long bindings (temporary capture cleaned), small error/Retry (temporary capture cleaned), 16:10 upgrade (temporary capture cleaned). Native build: startup (temporary capture cleaned), Controls (temporary capture cleaned), keyboard settings (temporary capture cleaned), Escape returns focus (temporary capture cleaned), 960×540 Controls (temporary capture cleaned). These are 107 theme views. The Controls page in 123 scrolls mouse controls and all bindings together; its current small/wide and error views are linked in common-settings.md.

Remaining `106` work: user KEEP/CHANGE verdicts on the full catalog, exhaustive world-sign/asset text reconciliation, before/after bottom-bar image comparison with a specifically approved icon, and observed evidence for overlapping HUD feedback and every unsupported/error detail variant. These do not block the explicitly selected `107` theme and `122`/`123` settings batches. `93`/`94` own reserve-warning meaning, `81`/`82` visual accessibility, `57` broader feedback policy, `118` the new bar.
