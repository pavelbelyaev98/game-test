# UI review

Task owners: [106 audit/design](../tasks/106-ui-ux-audit-design.md), [107 theme](../tasks/107-ui-ux-cleanup.md), [122 common PC settings](../tasks/122-common-pc-settings.md). Latest delivery: [124 monochrome control states](../tasks/124-monochrome-control-states.md); shared components remain in `123`. Direction: **one row/button/dialog system, immediate settings and concise menus**, retaining the white theme, Continue first and mint shops.

- [Shared component rules](design-system.md): dimensions, spacing, focus, buttons and standalone dialogs.
- [Visual direction, research and ordering](theme-and-navigation.md): selected changes and future suggestions.
- [Common settings, values and views](common-settings.md): current categories, display confirmation and concise copy.
- [State and copy catalog](catalog.csv): searchable/editable exact copy, conditions, actions, source and review verdict.
- [Screen/flow review](screens.md): before/after captures, entry/exit paths, error variants and outstanding observations.
- [Navigation map](navigation.md): startup, settings, trading and failure exits.
- [Bottom action-bar brief](action-bar.md): proposed handoff to `118`, separate from this delivery.

The catalog uses semantic keys rather than new task IDs. `CHANGE` / `MERGE` rows in the selected theme batch are implemented by `107`; `KEEP` means behavior preserved in this pass, not a new user verdict on every string. `DISCUSS` rows remain proposals. User verdicts start **UNREVIEWED**; edit them to KEEP / CHANGE / REMOVE / MERGE / DISCUSS with notes.

Screenshot evidence distinguishes live MainGame views, isolated presentation fixtures and source-only branches. A fixture checks layout; the linked integration tests check actual transactions, navigation and recovery. No claim of exhaustive observed coverage follows from string extraction.
