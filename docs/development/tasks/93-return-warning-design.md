# Task 93 - Define an honest return-power warning

Type: design/research; documentation only. Status: `planned`. Prerequisites: completed `13`, `75`, `86`; use current controller/jetpack values, then hand paid-tier checks to `47`.

Feature: [return and rescue](../../features/backlog/return-rescue.md). Implementation: [94](94-return-warning-update.md). Context: [warning finding](../../research/steam-review-audit/player-feel.md#return-advice-and-the-surface-payoff), [risk register](../design-risks.md), concept section 33. [Queue](../tasks.md).

## Gap and scope

`13` deliberately implemented charge bands, not a return-cost estimate. `ReturnWarning.Evaluate(Battery)` uses 35%/15%; `GameHudView.UpdateBattery` displays SAFE/RISKY/CRITICAL. The concept asks for approximate return difficulty. Keep `13` done: this task resolves that remaining scope and whether the existing wording implies more certainty than the model provides. No observed player failure is claimed.

## Research and concrete proposal

- Inspect recharge, shared dig/flight consumption, effective flight settings, rescue and paused HUD behavior. Map what the game actually knows about the excavation and what it cannot reliably infer about a walkable return.
- Produce a side-by-side warning table: present charge-only baseline; an approximate return-effort proposal using available depth/flight-efficiency information; and a truthful fallback when route effort is unknown. Include exact example wording, inputs, activation/recovery behavior and likely false reassurance/false alarm cases. Prefer a recommended modest solution; no GPS, generated route or claim of guaranteed escape.
- Compare equal battery percentages in a shallow open pit, deep vertical shaft, long shallow lateral tunnel and stepped return; include unequal battery/flight upgrades, obstructed ascent and near-surface recharge. A vertical estimate must explicitly acknowledge that it cannot price lateral detours or know which player-built route will be used.
- Specify how a warning is noticeable without constant alarm, compulsory audio, repeated tutorial text or obscuring a find. Keep reserve facts distinguishable from route advice; use non-color meaning and retain existing comfort settings. Numerical thresholds/hysteresis remain implementation tuning.
- Propose the chosen HUD examples and comprehension questions for `15`: what does this message tell the player, what does it not know, and why did they return? A screenshot/table is sufficient for design review; no new assets are needed to make the proposal concrete.
- Resolve the concept/feature wording together. If charge-only wording is recommended instead of a rough return-effort estimate, explicitly present that concept tradeoff for user selection; do not silently redefine section 33.

## Questions for review

Review the concrete recommended warning and contrasting route examples: whether rough effort adds useful advice, what uncertainty should be visible, and how urgent its presentation should feel. Keep automatic zero-battery rescue, current losses and physical return unchanged; `59` owns later consequence/economy decisions.

## Acceptance

- A concrete reviewed decision artifact is recorded in the return feature, with exact semantics, representative messages, inputs/limitations, exclusions and a scenario matrix. Questions alone do not finish the task.
- `94` receives an unambiguous update contract; `15` and `47` receive comprehension and paid-tier cases. No warning implementation, route guarantee, sound approval or playtest success is claimed by completing this design.
