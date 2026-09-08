# Player-review findings applied to this game

Source: the user-supplied **Player-Review Research: A Game About Digging a Hole and the Motherload Series**, reviewed on 2026-09-08. This is a retained synthesis for implementation, not a new review survey.

## Evidence limits

- The report combines helpfulness-ranked Steam samples, retrospective community discussion and long-form criticism. Its small selected samples do not establish population frequencies or prove that our players have the same problems.
- Its citations are another conversation's unresolved `turn...` markers, without source URLs. Storefront percentages, individual quotations and prevalence claims have not been independently verified here; do not repeat them as verified facts.
- Use the qualitative findings as hypotheses to test in this game. If a task needs a particular external claim, recover and read the original source first; record its URL, date and sampling limit in that task's evidence. Technical research should use current official documentation.

## Progression and discovery

- The report describes players enjoying excavation, the changing hole and the sell/upgrade/return loop, while tiring when discoveries or useful purchases run out. Preserve the 2–3 hour target; improve novelty and choices rather than adding empty depth or grind.
- Test early/middle/late first-recognition moments, time between distinctive discoveries, purchase timing and reasons to revisit a lateral branch. Random positions and bigger cash values do not substitute for new recognizable objects.
- Major upgrades should overpower the same old obstacle. Ordinary upgrades remain sequential money purchases, without chapter/depth permission gates or mandatory explosives. Show a concise current-to-next effect and qualitative milestone benefit.
- Owners: `25`, `39`–`49`, `37`; existing `12` already supplies atomic sales/purchases and before/after shovel statistics. Extend that working presentation, rather than creating a duplicate shop task.

## Physical comfort

- Small invisible collision obstructions are a high-severity traversal issue even if uncommon in the supplied sample. Cleanup is already implemented in `26`/`34`; retain regression coverage through materials, new content, saving and long excavations instead of reopening those tasks.
- Weightless removal and tiring repeated input/sound are separate concerns. Preserve the current shovel tuning, held pickup and quiet HUD. The report's toggle-dig suggestion was declined by the user: preserve click-and-hold, with no toggle mode (`38` cancelled). Review tool response (`11`) and restrained approved audio (`08`, `10`, `54`).
- Stronger jetpacks should improve control and return freedom. Test narrow shafts, lateral routes, braking and landings across paid levels; ordinary wall/ceiling bumps must not turn a mobility purchase into a penalty (`47`, `50`).

## Persistent investment

- Treat the excavation as the player's accumulated work: save it safely (`35`), preserve discovery snapshots (`51`), and continue the same save after the ending (`52`). Recovery must not silently create fresh terrain or respawn sold finds.
- The finale should pay off the normal equipment and excavation knowledge before its cutscene. It must work without rare passive rewards or maxing every track. No stealth/combat/puzzle replacement or explosive-stockpile requirement.
- The committed 2–4 passive finds (`36`) are rare optional rewards, not another required tree. Achievements (`53`) should remain achievable on the existing save, without a forced wipe or unrepeatable reveal trigger.

## Scope boundaries and conditional investigation

- No new hazards, survival meters, crafting/smelting, cargo-weight simulation, compulsory combat, endless-world mode, music or voice acting follows from this report.
- Dynamite remains optional. If separately selected, research placement/terrain collision and test predictable substantial blasts, saved charge state and `26`/`34` cleanup. Never require bomb-only ordinary paths or an explosives budget to finish.
- During `15`/`37`, distinguish getting lost from a dull commute. First test route readability, jetpack progression, content spacing and surface-trip length. HOME bearing remains conditional on navigation evidence; no GPS/path solving. Outposts/shortcuts are not approved or scheduled mechanics.
- Large-object extraction, item condition, special keys/components and tutorial alternatives retain their existing optional gates. A report example is not a selected feature or approved asset.

## How tasks use this research

Each linked task must inspect what already exists, read the relevant findings, state the remaining gap, and perform its focused research/playtest before dependent implementation. Substantial product decisions now have separate design/research tasks (`40`, `55`–`63`), with concrete proposals and recorded decisions; implementation consumes those results instead of asking the same questions again. Its **Before implementation** entry distinguishes technical choices from questions for the user. Ask only unresolved product questions when that task reaches them; do not ask the whole roadmap at once.

Resolve numerical feel/pacing questions through integrated playtests. Asset questions still require a specific reviewable batch, source/license, files, preview when available and rollback steps before explicit approval. This documentation update approves no art or audio.
