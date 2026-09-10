# Your player-feel checklist

Start with [the current Windows build](../../builds/windows/SomethingDownThere.exe), then open a task below and edit its result cells. You can also send “Task 100, flight: OK; lateral return: NOT OK, because…” and the results can be recorded for you. No Unity Editor or review-corpus reading is needed.

| Test task | When to use it | What you are judging |
| --- | --- | --- |
| [100 — Movement and excavation](tasks/100-movement-excavation-playtest.md) | Available mechanics in the current build; about 10–15 minutes | Control, stopping, flight, precision and shaping your hole |
| [101 — Collection and a return trip](tasks/101-collection-return-playtest.md) | Available mechanics in the current build; about 10–15 minutes | Pickup clarity, trip length, horizontal travel, surface payoff and recovery |
| [102 — Production discovery and feedback](tasks/102-production-discovery-playtest.md) | After production finds, shovel/detector feedback and selected lighting are delivered | Recognition, curiosity, material response, sound fatigue and interruption |
| [103 — Equipment and expedition choices](tasks/103-equipment-expedition-playtest.md) | After paid equipment, materials and return consequences are delivered | Noticeable power, useful purchases, worthwhile trips and recovery |
| [104 — Full adventure and Continue](tasks/104-full-adventure-playtest.md) | After the roster, display, ending and optional passive rewards are delivered | Discovery density, coherent payoff, lasting memories and desire to continue |
| [105 — Art-style comparison rounds](tasks/105-art-style-texture-trials.md) | During each approved texture-pack trial in a recorded build | Preferred style, cut-surface quality, common-find readability, consistency and visual fatigue |

## Marking a result

The separate [106 UI/state/text review](tasks/106-ui-ux-audit-design.md) will expose every screen, conditional menu and text for KEEP / CHANGE / REMOVE / MERGE / DISCUSS decisions. [107](tasks/107-ui-ux-cleanup.md) then records OK / NOT OK on the implemented cleanup. That audit complements these feel sheets; neither is already complete from creating its task.

- **UNTESTED** means you have not checked it. **OK** means it felt right on the named build/route. **NOT OK** means it needs a change; describe what happened and what you wanted instead. **NOT READY** means the relevant content/system is absent or a prerequisite prevents a fair test.
- Use your own preferred bindings; tasks describe the defaults. Record build/date and route/save once per sheet. Use normal bought equipment for the main verdict; label any developer-assisted comparison separately.
- Start by playing naturally. Read the intended feeling after the first attempt when possible, so the checklist does not tell you what to like. Record boredom, confusion, loss of control and missing feedback separately; they need different fixes.
- A missing sound/model is not evidence that final presentation is good or bad. Current tasks isolate available mechanics; `102` owns production feedback. New physics, E-for-specials, chests, paid refills and teleports get actual test rows only if selected and delivered.
- Comparator feedback is retained: the user enjoyed vertical flight and disliked horizontal return travel **in Meltopia**. `100`/`101` test those feelings in this game; all current-game verdicts remain untested until you report them. Do not turn a comparator criticism into a defect ticket for this build.

## Turning NOT OK into a fix

For each failed row, record a short reproducible action/route, expected versus actual feeling, and optional screenshot/clip. Link the matching unfinished task if it already owns the work. Otherwise create the next unused numbered implementation task for the concrete change; an already completed feature gets a follow-up, not a rewritten historical pass.

Keep the original observation in that row until a fix is delivered. Add the fixing task/build in its notes, then replay the failed case and adjacent affected behavior. Change the verdict to OK only after the retest feels right. Routine tuning can proceed from evidence; a new mechanic or changed input/loss policy first uses its design owner. **Neither a written checklist nor a successful compile counts as your playtest approval.**

`15` consumes `100`/`101` for the common-only starter trip. `102` follows actual non-minor detector content and feeds later batches/`49`/`37`; `105` owns style-round verdicts. `37` consumes progression/full-run sheets and `54` final results. Each sheet owns its verdicts; do not duplicate them or invent another issue-number sequence. Status stays open while required rows remain untested/not ready or NOT OK lacks a verified fix.
