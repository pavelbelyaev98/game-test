# Task 125 - Subtle menu hover and dropdown spacing

Type: implementation. Status: `done`. [Result and evidence](../completed/125-subtle-menu-hover.md). Explicitly selected follow-up to `124`.

Prerequisite: `124` delivered. Feature: [menu presentation](../../features/backlog/menu-presentation.md). Context: [shared components](../ui-review/design-system.md), [theme and navigation](../ui-review/theme-and-navigation.md). `106` resumes afterward.

## Selected work

- Restore subtle grayscale hover feedback on shared buttons and native settings inputs. Preserve readable text, neutral focus and clearly disabled states.
- Darken the native dropdown outline and equalize its inner padding on both axes. Keep selection shading without ticks.
- Reduce the home title from 54 to 42 reference pixels. Preserve the current menu rows, geometry and behavior elsewhere.

## Acceptance and evidence

- Unity imports the shared styles cleanly; relevant UI/startup integration checks pass.
- Inspect actual normal/hover/pressed/disabled states through the official CLI and Windows player, including popup padding and title at small/wide sizes. Enabled text retains at least 4.5:1 contrast; dropdown boundary targets 3:1 ([contrast context](124-monochrome-control-states.md#research-and-validation)).
- Rebuild `builds/windows/SomethingDownThere.exe`, preserve user saves/preferences, update concise records and resume `106`. No commit.
