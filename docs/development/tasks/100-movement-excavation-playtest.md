# Task 100 - Your movement and excavation feel test

Type: validation; user playtest. Status: `ready`. Prerequisites: implemented `65`, `67`, `78`, `86`, `87`; use the current Windows build. Final production presentation remains separate in `102`.

Features: [movement](../../features/backlog/fps-controls.md), [precision](../../features/backlog/precision-movement.md), [excavation](../../features/backlog/excavation-terrain.md). [How to mark/fix results](../playtesting.md). Evidence feeds `99` travel design and `05`/`15` acceptance.

Latest user verdict, 2026-09-13 after the `147` build: **Shave liked; other new modes unconvincing; previous ordinary digging preferred; progression should be better/more distinctive; no sounds.** `154` restores the baseline and makes the trial admin-only; `56`/`120` own revised progression/adoption. Exact route/kit were not reported, so other movement rows remain untested.

## Try these and mark the result

| Check | What to do | How it should feel | Result | Notes / fixing task / retest build |
| --- | --- | --- | --- | --- |
| Walk and stop | Walk, turn, reverse and release movement near a broad ledge | I go where I intend and stop when I release; no drift or fighting the controls | UNTESTED | |
| Horizontal travel | Walk then hold Shift on a familiar lateral path; try steering in flight too | Sprint is useful and controllable; the return should not feel like holding a key while waiting | UNTESTED | |
| Jump and lift | Tap Space, then try holding it | I can distinguish a normal jump from a deliberate jetpack ascent | UNTESTED | |
| Fall and catch | In a known clear shaft with charge, release flight and press again before landing | Dropping and catching myself is predictable, responsive and enjoyable | UNTESTED | |
| Air control | Ascend, steer sideways and line up a landing | I feel capable without overshooting or needing tiny corrective taps constantly | UNTESTED | |
| Precision | Hold Ctrl near a ledge/in a low tunnel; release under a low roof | Careful movement helps; the stance never fights the roof or throws me sideways | UNTESTED | |
| Camera comfort | Look, turn, dig and fly with your preferred FOV/steady-reticle settings | I can see and aim comfortably; power does not require unwanted camera movement | UNTESTED | |
| Dig control | Dig naturally and shape the excavation | Digging is enjoyable and controllable | OK | User: digging works well and is enjoyable after `126`; no claim that every movement/step case was separately tested |
| Experimental cuts | Admin opt-in only; compare Shave with the restored baseline | Useful control without replacing the liked digging or pretending to complete progression | NEEDS WORK | Shave liked; other modes unconvincing. `154` isolates the trial; `56`/`120` revise progression. |
| Hold / toggle | Try normal held digging; optionally select Toggle in Controls and try stop/pause/resume | Continuous digging is effortless and stops reliably; menus do not cause surprise actions | UNTESTED | |
| Bought power | If affordable, buy a shovel improvement and revisit the same ground | I notice what improved while keeping control over the shape; buying is not just a number changing | UNTESTED | |

The user enjoyed vertical flight and disliked horizontal travel in Meltopia. This sheet tests whether either feeling applies here; no current-game movement verdict is prefilled. Mark the bought-power row NOT READY if the comparison is unavailable; do not grind just to complete the sheet.

- The [feedback protocol](../../research/excavation-distinctiveness-validation.md) offers a three-minute reward-free normal Scoop/admin Shave comparison for the revised `120` decision. The trial is not accepted production progression. Preserve the liked-scoop verdict and record satisfaction/voluntary continuation separately for `148`.

## Follow-up and acceptance

- For NOT OK, describe the specific input/path and desired correction. Existing movement production work belongs to `05`, excavation presentation to `06`; a new change to completed `67`/`78`/`87` gets a numbered follow-up when the issue is concrete. Route/equipment choices go to `99`/`56`, not an automatic new speed mechanic.
- Keep mechanical control separate from missing production visuals. Sound work is deferred by explicit user instruction. `102` must later judge the finished silent feedback; this test cannot pass unfinished presentation by proxy.
- Done only when required current rows have your OK on a recorded build, with any failed cases fixed and retested. Link concise evidence and unresolved later scope in the completion record. Writing this sheet does not complete it.
