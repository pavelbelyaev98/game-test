# Task 91 - Design save portability and Steam Cloud

Type: design/research; documentation only. Status: `planned`. Prerequisites: `35` (complete); consume `80`'s current save structure and eventual `51` photo requirements without restarting its deferred performance benchmark.

Feature: [release qualification](../../features/backlog/release-validation.md#save-portability-decision). Research: [Steam review audit](../../research/steam-review-audit/delivery-and-positioning.md#decide-save-portability-before-platform-delivery). Coordinate platform access with `53` and final qualification with `54`.

## Scope and research

- Resolve the absent portability contract separately from existing local autosave/recovery. Inspect the actual save directory, whole-snapshot recovery, machine preferences and any existing Steamworks app configuration before claiming a missing integration.
- Prepare concrete options: Steam Cloud for launch using Auto-Cloud or the Steam API as appropriate; or explicit deferral with a documented complete manual transfer. Recommend the simplest coherent option using official Valve documentation and measured save/file sizes.
- Specify what travels together: terrain, seed/population, consumed IDs, inventory/economy/equipment, ending/achievement facts and personal photos. Identify device-local display/window settings and exclude editor/validation profiles and temporary writes.
- Define per-Steam-account ownership, first import of the current local save, clean-machine restore, offline divergence, older cloud snapshots, interrupted sync, damaged primary/valid backup and complete-generation selection. Do not merge independent newest files into a mismatched excavation/economy.
- Evaluate future photo count and late terrain size against app quotas and practical sync latency; research cannot claim a configured service or qualified build. Preserve local offline play and recovery if synchronization fails.
- Explicitly decide whether multiple campaign slots are needed or deferred. The shared-household review is evidence to consider, not automatic approval to change the one-world startup flow or add a save browser.
- Keep this within Windows support. No custom account/backend service, networking, new supported platform or remote configuration is authorized by the research task.

## Review and acceptance

- Present a concrete file/account/conflict flow and a comparison of launch Cloud versus deferral; explain costs, dependencies and the player-visible outcome. Resolve the meaningful choices with the user and record the selection in the owning feature.
- If Cloud is selected, create the next integer-numbered implementation task with exact local/remote ownership and a two-device/account test plan; make its delivery a prerequisite of `54`. Obtain whatever app access and external-write authorization that later concrete work requires.
- If deferred, record the reason, the complete safe transfer procedure and what launch communication can promise. Do not silently omit portability or advertise Cloud.
- No cloud implementation is complete from this design, and no new gameplay or asset is selected.
