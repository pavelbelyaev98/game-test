# Task 63 — Windows support and performance targets

- Why: terrain, equipment and content need practical performance limits before their cost grows.
- Decision: the user delegated technical choices and preferred final performance testing after the game is complete. Selected Windows 11/1080p60 development targets, candidate hardware, frame/edit/save/load/memory budgets and a final native matrix; public hardware claims remain with `54`.
- Integrated result: the [release contract](../../features/backlog/release-validation.md) owns targets; performance-sensitive tasks/site planning consume them, `55` inherits Steam access needs and the presentation feature retains the user's modest graphics ambition.
- Evidence: official CLI confirmed current MainGame/build configuration; retained Tasks 16/34/35 profiles distinguish measured results from estimates. Documentation links, queue/status state and `git diff --check` pass.
- Limitation: current top-tier edit spikes exceed the selected budget; lower-end hardware, long saves and final content still need measurement. No optimization or certification is claimed.
- Next: `64` is ready. Runtime and the Windows executable are unchanged.
