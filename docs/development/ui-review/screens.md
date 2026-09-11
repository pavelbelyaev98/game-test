# Screens and flow review

Current copy/conditions/actions: [catalog.csv](catalog.csv). Visual direction and selected settings order: [theme-and-navigation](theme-and-navigation.md). User verdict for every group: **UNREVIEWED**. The requested theme is implemented; the whole-interface subjective review remains with `106`.

## Theme comparison (107)

These views document the delivered theme. [Common settings](common-settings.md) contains the current five-category shared rows and standalone dialogs from `123`; older captures below retain pre-122 wording.

| Group | Before | Theme view and what to assess |
|---|---|---|
| Startup | [Cream/brass, New Game first](../../../unity/Logs/Task107/before-startup.png) | [White / Continue first](../../../unity/Logs/Task107/after-startup.png): title hierarchy, focus marker, ordered actions |
| Camera | [Nested settings](../../../unity/Logs/Task107/before-settings.png) | [Camera tab](../../../unity/Logs/Task107/review-camera.png): aligned values, white slider, common settings title |
| Controls | Earlier single scrolling list | [Grouped bindings](../../../unity/Logs/Task107/after-controls.png): movement/actions, current keys, persistent footer |
| Pause | Separate Camera comfort / Controls buttons | [Unified Settings entry](../../../unity/Logs/Task107/review-pause.png): Resume first, development actions separated |
| Shops | Cream/brass rows and inline comparisons | [Sell](../../../unity/Logs/Task107/review-sell-full.png) / [upgrade](../../../unity/Logs/Task107/review-upgrade-affordable.png): fixed balance, current/next columns, safe Close focus |

## Conditional screens

These are **isolated MainGame presentation fixtures**, with deliberate data/state setup under `Logs/Task107/PreviewProfile`. They render production UXML/C#; save-error injection previews layout, not the cause of a real storage failure. Separate tests exercise real state transitions and persistence.

| Case / entry | Capture | Actions / exit |
|---|---|---|
| Startup without checkpoint | [No save](../../../unity/Logs/Task107/review-startup-empty.png) | Continue disabled with explanation; New Game focused |
| New Game over existing slot | [Replacement](../../../unity/Logs/Task107/review-new-confirm.png) | Cancel first / Start New Game; Escape cancels |
| Backpack 0 / 10 finds | [Empty](../../../unity/Logs/Task107/review-inventory-empty.png) / [full](../../../unity/Logs/Task107/review-inventory-full.png) | Close / Inventory key / Escape; no transaction |
| Sell with empty bag | [Empty shop](../../../unity/Logs/Task107/review-sell-empty.png) | Sell all disabled; Close available |
| Upgrade without funds / level cap | [Shortfall](../../../unity/Logs/Task107/review-upgrade-empty.png) / [maxed](../../../unity/Logs/Task107/review-upgrade-max.png) | Price/shortfall or max state; Buy disabled; Close available |
| Long current binding | [Numpad Enter](../../../unity/Logs/Task107/review-controls-long.png) | Exact current label; no hardcoded Tab |
| Preference write failures | [Camera](../../../unity/Logs/Task107/review-camera-error.png) / [Controls](../../../unity/Logs/Task107/review-controls-error.png) | Session changes kept; visible Retry and Back |
| Binding capture / conflict | [Listening](../../../unity/Logs/Task107/review-binding-listening.png) / [conflict](../../../unity/Logs/Task107/review-binding-conflict.png) | Capture blocks gameplay; Escape cancels; conflict defaults to Cancel |
| Starting / loading | [Starting](../../../unity/Logs/Task107/review-save-creating.png) / [loading](../../../unity/Logs/Task107/review-save-loading.png) | Wait; no early Resume or destructive fallback |
| Save already open | [Contention](../../../unity/Logs/Task107/review-save-in-use.png) | Retry / Back to menu / Quit |
| New-game / load failure | [Start error](../../../unity/Logs/Task107/review-save-new-error.png) / [load error](../../../unity/Logs/Task107/review-save-load-error.png) | Kept-file disclosure; Retry and exit/recovery route |
| Previous checkpoint recovery | [Recovered](../../../unity/Logs/Task107/review-save-recovery.png) | Continue recovered excavation / Open save folder / Quit |
| Write error / discard confirmation | [Write error](../../../unity/Logs/Task107/review-save-write-error.png) / [discard](../../../unity/Logs/Task107/review-save-discard.png) | Retry / folder / Quit...; then safe Back or explicit unsaved exit |
| Development-only tools / reset | [Admin](../../../unity/Logs/Task107/review-admin.png) / [reset](../../../unity/Logs/Task107/review-reset.png) | Actual session overrides; destructive reset starts with Keep excavation |

## Coverage and remaining review

The catalog contains 139 source-traced entries/families including authored controls, dynamic copy, HUD priority, developer-only branches and excluded legacy fixture names. Runtime code overrides are authoritative over UXML preview defaults. Built-in keyboard labels come from Input System; the five settings categories are included. There are no current tooltips, credits, detector, ending, photo or C4 screens to catalog.

Relevant integration evidence: [UI input](../../../unity/Logs/Task107/ui-tests.json), [startup](../../../unity/Logs/Task107/startup-tests.json), [stations](../../../unity/Logs/Task107/station-tests.json), [rescue](../../../unity/Logs/Task107/rescue-tests.json), [save/recovery](../../../unity/Logs/Task107/save-tests.json), [EditMode](../../../unity/Logs/Task107/edit-tests.json). MainGame captures and native Windows review complement those tests; this is not a claim that every possible numeric text variant was manually observed.

Size evidence: [960×540 long bindings](../../../unity/Logs/Task107/controls-long-960x540.png), [small error/Retry](../../../unity/Logs/Task107/controls-error-960x540.png), [16:10 upgrade](../../../unity/Logs/Task107/upgrade-affordable-1280x800.png). Native build: [startup](../../../unity/Logs/Task107/native-startup.png), [Controls](../../../unity/Logs/Task107/native-controls.png), [keyboard settings](../../../unity/Logs/Task107/native-keyboard-settings.png), [Escape returns focus](../../../unity/Logs/Task107/native-back.png), [960×540 Controls](../../../unity/Logs/Task107/native-controls-960x540.png). These are 107 theme views. The Controls page in 123 scrolls mouse controls and all bindings together; its current small/wide and error views are linked in common-settings.md.

Remaining `106` work: user KEEP/CHANGE verdicts on the full catalog, exhaustive world-sign/asset text reconciliation, before/after bottom-bar image comparison with a specifically approved icon, and observed evidence for overlapping HUD feedback and every unsupported/error detail variant. These do not block the explicitly selected `107` theme and `122`/`123` settings batches. `93`/`94` own reserve-warning meaning, `81`/`82` visual accessibility, `57` broader feedback policy, `118` the new bar.
