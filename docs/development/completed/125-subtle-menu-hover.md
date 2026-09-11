# 125 - Subtle menu hover and dropdown spacing

- Why: restore restrained interaction feedback while retaining the user's selected row design.
- Integrated: shared grayscale hover fills for buttons/tabs/native settings inputs; disabled states stay quiet. Dropdown outline is darker, outer padding is 4 px on every side, item padding 8 px; inherited extra right padding removed. Home title is 42 px instead of 54 px.
- Evidence: 25 UI + 8 startup PlayMode checks passed. CLI small/wide views and 12 native Windows captures cover title, pointer hover, dropdown selection, footer, slider and disabled reset. [Results and captures](../../../unity/Logs/Task125/).
- Measured: dropdown insets 5/5/5/5 px including its border; 69 enabled text samples >=8.86:1, hover slider contrast >=3.11:1 and dropdown border against row 3.12:1. Save/preferences preserved (24 files); MainGame clean/stopped.
- Windows: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), 2026-09-11 05:02 UTC, zero errors and one existing Pipeline runtime-configuration warning. No commit.
- Remaining: [106](../tasks/106-ui-ux-audit-design.md) resumes the broader UI audit; this refinement does not complete its outstanding user catalog verdicts.
