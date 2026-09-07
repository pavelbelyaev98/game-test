# 14 - Final arc and ending

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
- Interrupted reveal flow recovers safely.
- Continue mode preserves the excavation and normal loop.

## Done when

- Trigger, interruption, reload, and continue-mode checks pass.
- After the ending, players can keep upgrading, finding missed discoveries, completing achievements, and filling the discovery display.
- Exact narrative/content decisions are resolved from `docs/idea.md` only when this task begins.
