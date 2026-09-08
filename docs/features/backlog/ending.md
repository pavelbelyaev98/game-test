# Final arc and ending

Status: planned after the full loop and progression are proven.

Idea coverage: sections 46-49 and 51.

## Purpose

Provide a late-game discovery payoff while allowing continued excavation afterward.

## Implementation task

Implement a persisted late-game mystery escalation, end-state trigger, reveal sequence boundary, and continue mode without coupling normal progression to one fragile scene event.

## Required behavior

- The final trigger cannot occur accidentally or become permanently unreachable.
- Early discoveries remain believable, middle discoveries become suspicious, and only late finds establish that something is wrong.
- The protagonist starts seeking money and ends famous because of an impossible human-made discovery; the exact object remains deliberately undecided until the rest of the game works.
- A few protected keys/components may lead to the reveal without becoming an inventory puzzle.
- The playable ending uses the normal upgraded shovel/digging tool, detector, jetpack and any included dynamite under their normal rules. Do not strip equipment/upgrades or introduce stealth, combat or puzzle gameplay, and never require a rare passive discovery to finish.
- Interrupted reveal flow recovers safely.
- Keep the planned ending cutscene as a presentation break. Continue mode restores the same equipment, upgrades, excavation and normal loop through the persistent state in Task `35`.

## Done when

- Trigger, interruption, reload, and continue-mode checks pass.
- Complete the playable ending with ordinary purchased equipment and no rare passive rewards; verify each included tool remains usable before the cutscene and after Continue Playing. Validate late purchases through [Task `37`](selling-upgrades.md#task-37---full-run-upgrade-pacing).
- After the ending, players can keep upgrading, finding missed discoveries, completing achievements, and filling the discovery display.
- Exact narrative/content decisions are resolved from `docs/idea.md` only when this task begins.
