# Task 124 - Monochrome control states and release save UX

Type: implementation. Status: `done`. [Result and evidence](../completed/124-monochrome-control-states.md). Explicitly selected follow-up to `123`.

Prerequisite: `123` delivered. Features: [menu presentation](../../features/backlog/menu-presentation.md), [PC settings](../../features/backlog/pc-settings.md). Context: [shared components](../ui-review/design-system.md), [settings review](../ui-review/common-settings.md). `106` resumes afterward.

## Selected work

- Preserve the current shaded row layout and dimensions. Use only neutral grayscale for settings, fields, dropdowns and menu actions. Native Toolkit controls stay in use.
- Override all native normal/hover/pressed/focused/disabled states consistently; no white-on-white text, blue focus or ticks. Dropdown selection uses a shaded highlight. Secondary buttons remain borderless except a light keyboard focus cue.
- Right-align Automatic. Correct native slider/value vertical alignment. Back and Reset category use identical compact styling and sit next to each other at bottom-left.
- Graphics contains only a plain TBD label; keep maximum graphics defaults behind the page. VSync defaults Off (144 cap), preserving explicit existing preferences.
- Developer admin uses an ordinary Pause button, hidden in release. Remove Open save folder actions. Production write failure quits silently without the progress-save error/confirmation; development retains useful diagnostics and Retry.

## Research and validation

- [W3C text contrast](https://www.w3.org/WAI/WCAG22/Understanding/contrast-minimum.html): check rendered active text at 4.5:1 or better, including hover/focus; disabled controls have no required minimum but must be distinguishable.
- [W3C non-text contrast](https://www.w3.org/WAI/WCAG22/Understanding/non-text-contrast.html): important input boundaries, thumbs/tracks and focus cues target 3:1 against adjacent surfaces.
- [Unity pseudo-classes](https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-USS-Selectors-Pseudo-Classes.html): override combined native states, including selected dropdown content and slider geometry; inspect actual resolved styles and screenshots.
- Run deterministic and relevant UI/save integration checks; exercise release failure policy without losing user data. Inspect hover/pressed/focus/disabled states via CLI and native Windows, small/wide views and slider endpoints. Record contrast calculations from resolved colors.
- Rebuild builds/windows/SomethingDownThere.exe, update concise feature/review records, promote `106`. No commit.
