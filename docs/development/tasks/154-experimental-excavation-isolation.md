# Task 154 - Restore normal digging and isolate the experiment

Type: implementation with documentation triage. Status: `done`. User-selected batch, 2026-09-13. Prerequisites: existing `147` code/content and the user's verdict below. [Completed result and evidence](../completed/154-experimental-excavation-isolation.md).

Features: [excavation experiment](../../features/backlog/excavation-modes.md), [shovel progression](../../features/backlog/shovel-progression.md), [presentation](../../features/backlog/presentation-audio.md).

## User decision and scope

- The user likes Shave, finds the other new modes unconvincing, prefers the earlier digging, and wants better, more distinctive progression. The four-mode package and its authored tool are experimental, not accepted game content.
- Restore the pre-147 Scoop controls, cadence/fuel and normal HUD. Keep the explicitly requested 32 m/eight-mineral progression and existing economy, terrain, collection, preferences and saves.
- Retain the new cuts/tool only behind an explicit Developer admin experimental toggle. Default off on startup/reload and Restore normal rules; enabling starts with Shave. Mark the active experiment clearly and keep it inaccessible in release builds. Experimental cuts still belong to the current saved excavation; the toggle is session-only.
- Remove all four new sounds, sources, copied clips, import settings and code/prefab references. The user's standing instruction is **do not add sounds**; later feature/research requests do not reopen audio permission.
- Read both supplied Keep Digging reports, preserve their useful rationale/limitations in repository context, update the full idea in place, reopen/reprioritize relevant existing tasks, and give any new unresolved product decision its next numeric task.

## Acceptance

- New Game/Continue use the unchanged ordinary Scoop, with no experimental tool/selector/normal mode cycling. Reading v6 saves preserves excavation, minerals, purchases, balances and identities while discarding experimental selection.
- Admin opt-in, Shave default, voluntary cycling, off/restore/reload, hold/toggle release safety, full bag and held-find behavior work without free fuel or cooldown exploits. No experimental state becomes purchased progression.
- MainGame has only its normal camera until opt-in; trial rig/overlay references cleanly detach on exit. No imported sound, AudioSource or audio integration remains from `147`.
- Focused deterministic/integration checks, official CLI visual review, one normal Windows handoff and profile preservation pass. Keep `147`'s existing performance gap deferred with the unaccepted experiment; no long qualification benchmark.
- Docs distinguish user decisions, experiments and report proposals; retain all 54 full-concept sections. Task/feature dependencies must not require unaccepted gun/mode/audio content.
