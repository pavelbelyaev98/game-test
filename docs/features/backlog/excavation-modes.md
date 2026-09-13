# Experimental excavation modes

State: **excavation guns are intended; implementation is deferred at the user's request.** The current four-mode package remains **experimental, not accepted production content**. The user prefers the pre-147 digging for now, really likes Shave, and finds the other trial modes unconvincing. `154` restores normal play and isolates the comparison; `56`/`120` design future gun progression and Shave's role. Do not interpret the rollback as abandoning guns or re-ask whether the user wants them.

## Normal game

Normal New Game/Continue uses the existing irregular Scoop, paid shovel tiers, previous HUD, ordinary hold/toggle/remapping, 60% aimed collection and automatic remnant cleanup. The 32 m mineral field and current economy remain. No trial tool or mode selector is present, and the experimental cycle binding cannot change normal digging.

## Admin-only comparison

Developer admin offers **Experimental excavation: OFF/ON**. Enabling starts in **Shave** and loads the trial Blender rig. The HUD explicitly says EXPERIMENTAL. The existing experimental binding (Q by default, remappable while enabled) cycles Scoop → Bore → Fan → Shave. The setting and chosen cut are session-only; off, Restore normal rules and reload return to ordinary Scoop and remove the trial camera/rig. Release builds cannot enable it.

| Experimental cut | Comparison purpose | User verdict |
| --- | --- | --- |
| Scoop | Existing general-purpose bite, retained as a reference | Previous normal digging preferred |
| Bore | Narrow/deep along aim; slightly slower and more fuel | Unconvincing; not selected |
| Fan | Wide/shallow sweep across the surface | Unconvincing; not selected |
| Shave | Fast/thin layers, proportionately less fuel per pulse | Liked; promising, not production approval |

Switching or leaving the experiment suppresses held/toggled actions and retains the current cooldown. Interact and lift/drop keep priority; switching cannot throw a held find, collect or spend fuel. Wider cuts never collect off-aim objects. Experimental cuts and legitimate earned state still save into the current excavation; the admin notice states this. Existing v6 files remain readable but cannot reactivate a saved experimental selection. New captures write normal Scoop in the compatibility field.

## Presentation and unresolved progression

The tool mesh, local animation and tier-2/4/6 attachments from `147` are trial presentation only. They are loaded on admin opt-in, hidden in menus/while holding finds and detached on exit. All four imported sounds, source copies and audio code/references are removed by `154`. **Do not add sounds.** [Ownership/removal](../../asset-ledger.md).

Current paid tiers can scale the experiment for comparison, but this does not fulfill the distinctive-progression promise. `56` owns concrete capability milestones including future excavation guns; `120` compares an evolving Shave action, an optional capability and leaving Shave experimental. Guns remain intended whichever Shave option is chosen. Exact gun forms, capabilities and acquisition are unresolved; the current trial does not require a four-mode ladder, mandatory switching, suction/cleaning chore or condition penalty. `25`/`11` retain reviewed progression/presentation delivery; `148` must distinguish normal and experimental evidence.

`147` retains future gun implementation as deferred work, awaiting the detailed `56`/`120` brief and the user's decision to resume implementation. Its previous stress run missed `63`'s edit target (Fan probe p95 14.01 ms versus 8 ms); preserve that evidence without resuming optimization now. [Keep Digging synthesis](../../research/keep-digging-lessons.md) records the intended gun direction and the narrower trial verdict.
