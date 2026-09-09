# 67 - Held precision crouch

- Why: give players careful ledge/air corrections and access to lower passages with the selected held Left Ctrl interaction.
- Integrated: MainGame uses smooth foot-preserving capsule/view changes, immediate 35% horizontal speed, safe blocked standing and automatic recovery after clearing/digging a roof. Space behavior and camera preferences are preserved; Pause lists the control.
- Saves: actual partial/full stance participates in autosave and recovery; version-1 worlds migrate as standing. Impossible saved occupancy stops behind recovery UI and preserves the checkpoint.
- Validation: **98/98 EditMode and 86/86 PlayMode checks passed**, including 30/60/144 Hz terrain traversal, airborne/input recovery, near-plane containment and save integration. Stable crouch: 0 managed bytes over 10,000 updates (1.61 ms total in the live Editor; not a release performance qualification).
- Ctrl binding correction: **9/9 input and 12/12 UI checks passed**, covering inactive Shift, held Ctrl recovery and admin chords; one initial menu-navigation failure passed on rerun. Official CLI verified the updated Pause hint. Evidence: `ctrl-*` files in the same evidence folder.
- Review: official CLI inspected MainGame at FOV 55/75/90, 960×540, 1280×800 and 1920×1080, including held excavation clearing headroom. Brief Windows input checks covered crouch movement, Space, Pause/modifier recovery and crouched save/quit/relaunch. Original native save/preferences restored byte-for-byte. Evidence: `unity/Logs/Task67/`.
- Build: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), `2026-09-09 10:59 UTC`; succeeded with no errors. The sole warning concerns the intentionally absent player-side Pipeline configuration; the official CLI remains an Editor tool.
- Remaining: sustained comfort and future tool/presentation acceptance remain `05`/`54`; next eligible work is [68 — underground lighting design](../tasks/68-underground-lighting-design.md).
