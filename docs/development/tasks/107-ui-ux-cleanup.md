# Task 107 - Implement the reviewed UI/UX cleanup

Type: implementation. Status: `planned`. Prerequisites: `106` and its recorded selection of concrete current-UI changes. Independent of world-art production unless a specifically selected change actually needs an asset.

Feature: [menu presentation](../../features/backlog/menu-presentation.md). Input: [106's UI/state/text review](106-ui-ux-audit-design.md). Sources: [UI authoring](../ui-authoring.md).

## Scope and decisions before implementation

- Consume the accepted screen/state/text change list from `106`, inspect current source and refresh any changed catalog entries. Record the bounded implementation scope and exact before/after copy/navigation before editing. Unselected changes remain proposals; if there is no accepted work, retire this reservation rather than inventing a redesign.
- Apply reviewed wording, hierarchy, layout, visibility/disabled explanations, conditional-menu simplification and navigation fixes through existing UI Toolkit sources and shared styling. Preserve already accepted behavior and the current framework; no migration or replacement of the whole UI is implied.
- Keep one delivery owner per change. `93`/`94` retain return-warning meaning, `57` feedback-priority decisions, `81`/`82` accessibility features and future mechanics their own interfaces. Coordinate overlapping source edits and update the review catalog when their changes land; this task implements only the selected current-UI cleanup.
- Retain transaction atomicity, truthful state/error distinctions, save/new-game loss disclosures, safe confirmation defaults, binding/settings persistence, input barriers and developer/release separation. A text cleanup cannot silently change sale, rescue, saving or upgrade policy. Resolve any newly exposed product question with concrete options through its design owner.
- Reuse approved fonts/assets. Any selected new icon, font, texture or sound requires the repository's concrete asset approval and ledger ownership; ordinary copy/layout changes do not wait for unrelated art trials.

## Acceptance and user review

- Every accepted entry has an implemented result or is explicitly reassigned/deferred through review; do not mark this task done with unexplained omitted rows. Update the same catalog with final exact strings/templates, conditions and evidence so it remains a usable reference.
- Run a fast deterministic check after code changes, then relevant navigation/state/integration checks once at completion. Inspect actual Unity import/runtime behavior through the official CLI; a text snapshot alone cannot establish correct UI behavior.
- Verify changed branches and adjacent Back/Escape, cancel/confirm, hidden/disabled/focused states, mouse/keyboard navigation, focus loss and menu/input capture. Exercise affected empty/full/affordability/error/save states with isolated fixtures. Verify long content at supported window sizes; do not corrupt the real user save for failure testing.
- Provide before/after views and a Windows build at `builds/windows/SomethingDownThere.exe`. The user reviews **OK / NOT OK** for clarity, necessary information, navigation and visual hierarchy on each changed flow; failed cases get a named fix and retest. Do not treat compilation or the catalog alone as visible-feature acceptance.
- Update the menu feature only where behavior changed, keep the catalog current, and record concise completion evidence plus any remaining limitation. Future UI additions must supply their text/state/condition entries through this existing inventory rather than becoming undocumented screens.
