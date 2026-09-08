# Handoff

## User review

Give the user a clickable link to [SomethingDownThere.exe](../../builds/windows/SomethingDownThere.exe). Do not ask them to open Unity. If playable behavior changed, rebuild once before handoff using the direct CLI menu command in [the Unity guide](../../unity/readme.md) for the open Editor, or `./tools/build-windows.ps1` with it closed.

## Next AI session

1. Read [AGENTS.md](../../AGENTS.md).
2. Continue the active task or take the first ready numeric task from [tasks.md](tasks.md).
3. Read its numbered task file, linked feature contract and research basis. Inspect existing implementation, perform the task's focused research and resolve only its outstanding product questions with a concrete proposal before dependent work. Keep specific asset approvals separate. For design/research tasks, deliver the concrete proposal and resolve the named product choices before updating dependent contracts; do not treat planning as implemented gameplay. For implementation, validate and update the Windows build when gameplay changes.
4. Before coding, follow the task's linked research/decisions. Record answers, reasons and excluded alternatives in the owning feature; sync concept-level changes to `idea.md` and update dependent task contracts. Ask the active design task's questions in useful batches until required decisions are settled; do not rely on remembered chat or re-ask recorded answers.
5. Keep [status.md](status.md) current and add one short completion record when done.

Feature index: [backlog.md](../features/backlog.md). Full concept context is optional and follows the order defined in `AGENTS.md`.
