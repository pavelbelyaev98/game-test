# Task 147 - Excavation guns: deferred implementation and experimental modes

Type: implementation. Status: `planned`, explicitly deferred by the user. **Excavation guns are part of the intended game direction; the user wants to implement them later.** Prerequisites for delivery: revised `120`, `56` milestones, saved-progress compatibility `35` and budgets `63`, followed by the user's decision to resume implementation. The current trial is not a completed production feature.

Feature: [experimental excavation](../../features/backlog/excavation-modes.md). [Keep Digging/user feedback](../../research/keep-digging-lessons.md). `154` owns normal-game restoration, audio removal and admin isolation.

## Retained experiment and verdict

- The implementation supplies Scoop/Bore/Fan/Shave, a trial Blender excavator and tier attachments. These are retained solely for explicit admin comparison; the normal game uses the prior Scoop.
- The user really likes Shave, finds the other trial modes unconvincing and wants better/more distinctive progression. They explicitly clarify that guns are still wanted. The current four-free-modes package and tier scaling do not settle the future gun design. All new sounds are removed; do not add sounds.
- `120` must decide whether/how Shave belongs in a paid capability progression after `56`'s concrete proposal. Do not resume optimization, broaden modes or treat the trial art as accepted production presentation before that decision.

## Existing evidence and limits

- Prior four-mode build: 201 relevant functional checks passed; [evidence](../../../unity/Logs/Task147/validation-summary.json). `154` replaces saved selection and normal availability with session-only admin opt-in; its evidence owns current behavior.
- The 198-cut Editor/save diagnostic measured whole-frame p95/p99/max 13.29/21.51/28.50 ms and capture max 3.75 ms. Accepted-edit p95 was 11.04 ms; an overlapping-scar Fan probe reached p95 14.01 ms versus `63`'s 8 ms target. Oriented density bounds improved grid work; synchronous mesh/collider rebuilding remains the bottleneck.
- Keep the gap visible, without restarting `80`'s user-deferred qualification. A future selected capability must meet the same cost/cleanup/collision/save constraints before expanding throughput.

## Future delivery acceptance when resumed

Implement the reviewed excavation-gun capability/acquisition/input contract, with non-destructive paid-state migration, normal 60% aimed collection and free hold/toggle. Validate actual useful progression, full bags, held finds, fuel/cooldown, geometry/collision, remapping and reload. Inspect approved presentation through CLI and deliver Windows. Keep this future implementation task even if individual trial modes are omitted; the current admin experiment remains separate until the user resumes delivery.
