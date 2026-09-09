# Task 76 — Isolate Windows game review from host input

Type: implementation / validation tooling. Status: `cancelled` by the user. This ID is retired and is not a prerequisite or remaining task. [Closure record](../completed/76-isolated-windows-review.md).

Contract: [validation policy](../../scope-and-validation.md#validation-policy). The proposed Windows Sandbox runner aimed to keep native game automation independent of the user's mouse/keyboard.

## Decision and evidence

- The user dropped this setup because Windows servicing work and delay outweighed its value for current development. Do not resume Sandbox installation or Windows repair as part of the game task queue.
- Microsoft's [Sandbox configuration](https://learn.microsoft.com/en-us/windows/security/application-security/application-isolation/windows-sandbox/windows-sandbox-configure-using-wsb-file) informed a prepared runner, but the initial elevated Windows feature query failed with `The component store has been corrupted.` No isolated native run passed; static checks were not proof of working isolation.
- The unneeded installer, host/guest scripts, generated launch configuration and setup guide were removed. Failure/preflight evidence remains under `unity/Logs/WindowsSandbox/`.
- Continue isolated Input System regression tests and brief, announced native Windows checks. Repeat checks affected by observed user input or focus changes; do not classify those interruptions as gameplay failures. The completed Toolkit HUD migration is unaffected.
