# 107 - White menu theme and clearer navigation

- Why: the user requested professional white menus/settings, Continue first and a complementary shop design using the supplied game references.
- Integrated: shared white/charcoal tokens and mint shops; Continue → New Game → Settings → Quit; sibling Camera/Controls tabs with direct Back to origin; grouped bindings; separated developer actions; fixed shop balance/footer; aligned current/next statistics; accurate immediate-sale copy; matching HUD colors.
- [Research/decisions and review catalog](../ui-review/index.md). This bounded theme batch was explicitly requested; `106` retains the broader unreviewed audit and `118` the new icon/bar.
- Validation: **140 EditMode + 47 relevant PlayMode cases passed** (18 UI input, 7 startup, 4 station, 7 rescue, 11 saving). Related UI/station suites passed again after layout changes.
- Official CLI: inspected MainGame and isolated empty/full/price/max/settings-error/binding-conflict/save/recovery/admin presentation fixtures; checked actual 960×540, 1280×800 and 1920×1080 viewports. Back/Retry/Close remain accessible; long controls and lists scroll.
- Native Windows: mouse Settings/Controls, keyboard arrows/Enter, Escape back with Settings focused, 1920×1080 startup and 960×540 resize, clean close. All user save/preference files unchanged; only logs/test reports changed.
- Build: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), **2026-09-10 19:19 UTC**, zero errors; one existing Pipeline runtime-configuration warning. Final Controls spacing verified in CLI and native 1920×1080 / 960×540 views. MainGame clean/stopped; no commit.
- [Captures/results/build evidence](../../../unity/Logs/Task107/). Limitation: broader catalog verdicts, exhaustive audit gaps and backpack-icon approval remain open in `106`/`118`; existing world art is still a separate acceptance.
