# Task 101 - Your collection and return-trip feel test

Type: validation; user playtest. Status: `ready`. Prerequisites: implemented collection, inventory, stations, recharge, rescue and saves (`12`, `13`, `35`, `72`, `86`). Use the current build; production content is judged in `102`.

Features: [collection](../../features/backlog/discovery-collection.md), [return](../../features/backlog/return-rescue.md), [core loop](../../features/backlog/core-loop.md). [Result/fix workflow](../playtesting.md). Feeds `97`/`98`/`99`, `93` and `15`.

Build/date: **not recorded**. Save/route, bag/battery and bought equipment: **not recorded**. Approximate session: 10–15 minutes; use a familiar return path and one natural expedition.

## Try these and mark the result

| Check | What to do | How it should feel | Result | Notes / fixing task / retest build |
| --- | --- | --- | --- | --- |
| Uncover and collect | Dig around an ordinary find while holding LMB | I see enough to understand what I found; collection is fluid and does not demand cleaning a final speck | UNTESTED | |
| Recognition | Collect a few finds, then check what you remember before opening inventory | I know what entered the bag; fast digging has not silently swallowed every discovery | UNTESTED | |
| Full bag | Attempt another pickup with full inventory, return/sell, then revisit it | I understand the refusal and trust the item remains available | UNTESTED | |
| One more dig | During a natural trip, decide when to turn back | The choice is tempting and understandable; starting equipment allows useful work before returning | UNTESTED | |
| Warning meaning | When the battery changes band, say what you think the message knows | I understand reserve pressure without assuming a guaranteed route home | UNTESTED | |
| Underground retracing | Return through a known lateral branch; note wrong turns separately | Remembering my route and controlling the return is engaging; obvious travel does not overstay its welcome | UNTESTED | |
| Surface walk | From the rim/recharge area, reach the selling/upgrading stations | Banking the trip is nearby; an empty surface walk does not delay going back to the interesting part | UNTESTED | |
| Overall horizontal return | If any segment feels skippable, identify it and why | I want to make another trip instead of dreading the commute | UNTESTED | The user's criticism concerns Meltopia; no defect is established here |
| Surface payoff | Sell, recharge, inspect/buy a wanted upgrade and head back | I understand what I earned and improved; service is brief and gives me a reason to descend | UNTESTED | |
| Recovery | With expendable ordinary loot, optionally let charge reach zero | Rescue/loss are understandable; my hole and purchases remain worth continuing | UNTESTED | This check loses the ordinary bag and up to 10 credits; skip if that loss is unwanted |
| Stop and resume | Use Save and quit, relaunch and Load Game | My hole, items, money, equipment and location form the same understandable continuation | UNTESTED | |

Record approximate **dig/discover / return ascent / underground horizontal / surface walk + service / outbound retrace** times if convenient, and what forced the trip: bag, charge, wanted purchase, confusion or choice. These distinguish causes; there is no invented target percentage to satisfy.

## Follow-up and acceptance

- `99` owns travel comparisons if these observations establish a need, `93`/`94` warning meaning, `97` possible special interactions and `98` charge policy. Present-day ordinary pickup remains LMB and base refill remains free; do not judge E, portable fuel or teleporters as already delivered.
- Treat this as a user feel check, not `80`'s deferred performance benchmark. A save interruption can be reported without restarting that benchmark. Use existing-scope fixes where applicable and numbered follow-ups for actual new defects; repeat affected rows on the fixing build.
- Done when required current rows have your recorded OK and each NOT OK has a verified fix. A skipped loss check may retain NOT READY with its reason until safely testable; it does not count as a pass. Preserve pending production acceptance in `102`/`15`.
