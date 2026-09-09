# Task 88 - Save setup API warning

Type: implementation (editor maintenance). Status: `done`. User-selected. Prerequisite: `35` (complete). [Completion](../completed/88-save-setup-api-warning.md).

Feature: [core loop persistence](../../features/backlog/core-loop.md).

- Scope: replace deprecated `FindFirstObjectByType` in `SaveGameSetup.Configure` with `FindAnyObjectByType`; the setup command needs the single MainGame player, not instance-ID ordering.
- Research: the reported CS0618 diagnostic and installed Unity 6000.6 `UnityEngine.CoreModule.xml` identify the supported unordered lookup. No dependency change is needed.
- Acceptance: no deprecated first-object calls remain in project C#; official CLI recompilation succeeds; read-only MainGame inspection confirms one player and a matching lookup. Do not run the scene-writing setup command for validation.
- Questions: none. This editor-only fix changes no playable behavior and needs no Windows rebuild.
