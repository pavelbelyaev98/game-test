# Task 121 - Restore complete defaults from damaged input preferences

Type: implementation (controls maintenance). Status: `done`. Prerequisites: `78` complete. [Completion and evidence](../completed/121-input-preference-recovery.md). Selected under the user's request for work requiring no design decisions; queued implementation work has unmet design/production dependencies.

Feature: [FPS input accessibility](../../features/backlog/fps-controls.md#input-accessibility-contract-78). Research: [78's selected behavior](78-input-accessibility.md) and [Meltopia accessibility findings](../../research/meltopia-lessons.md).

## Scope and evidence

- The regression reproduced `InputPreferences.Decode` retaining Toggle after rejecting damaged bindings. The decoder now validates the complete binding map before applying its mode, honoring the selected complete-default recovery contract.
- Decode the mode and bindings as one validated settings snapshot. Invalid/duplicate/unsupported maps restore all default controls, including Hold; preserve the source file until an explicit edit/reset.
- Keep valid custom maps, additive Sprint/Grab migration, write-failure/retry behavior and gameplay interruption barriers intact. This is recovery of the existing selected behavior, with no new product choice.

## Acceptance

- Reproduce the partial recovery with a malformed map saved in Toggle mode; cover duplicate, unsupported, missing and truncated bindings, invalid versions and duplicate fields.
- Recovered settings expose Hold and every default binding, perform no write on load/clean flush, and require an explicit edit/reset to replace the damaged file. Valid Toggle maps and legacy migrations still work.
- Run focused deterministic input tests and relevant simulated-input/UI regressions; inspect recovered Controls in MainGame through the official Unity CLI using an isolated preference store and preserve the user's saves/preferences.
- Deliver `builds/windows/SomethingDownThere.exe`, record concise evidence, remove this queue row and restore the next eligible task by queue order. Questions: none.

## Result

- 140 EditMode checks pass, including eleven damaged-file variants and explicit-edit/reset recovery with release-to-stop input. All 18 UI input cases pass across the suite and one isolated rerun of a transient Back-focus failure.
- MainGame CLI inspection confirms Hold/LMB/default bindings, correct Pause reference and Back focus, and preservation of the damaged review file. All 22 user save/preference files retain their hashes; MainGame is clean and stopped.
- Windows build: 2026-09-10 18:34 UTC, zero errors and the existing Pipeline runtime-config warning. [Evidence](../../../unity/Logs/Task121/). Next by queue priority: `106` UI audit/design.
