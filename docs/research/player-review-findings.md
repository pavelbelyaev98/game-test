# Player-review findings applied to this game

Retained synthesis of the four earlier user-supplied reports, reviewed on 2026-09-08, supplemented by the [Meltopia lessons and task mapping](meltopia-lessons.md) from the 2026-09-09 supplied report. [Source register](player-review-sources.md) preserves links, report provenance and checked/unverified status; the findings below record implications and owners, not a new survey.

## Evidence limits

- Selected anecdotes motivate hypotheses, not population frequencies or proof this game has the same faults. The first report's citations remain unresolved; later reports contain duplicate pages and untraceable individual comments. Confidence labels/example numbers are not test targets.
- Distinguish the implemented baseline, selected direction and still-optional proposals. Preserve user decisions even when a report differs. No recommendation approves an asset/audio batch.
- Prefer observing our actual game for remaining feel/pacing questions; use current official documentation for technical work. More comparator research needs a specific unanswered question.

## Progression and discovery

- Keep the 2–3 hour target dense with recognizable discoveries and useful late purchases. Bigger prices on identical finds or more empty depth do not replace novelty (`37`).
- Generate random candidates, then validate gaps, unrelated major-find clumps and excessive early novelty. Preserve intentional clusters and overlapping encounter bands; bounded repair/rejection cannot create fixed routes or reroll existing saves (`58`/`40`/`45`).
- Compare vertical rushing, signal-led lateral and mixed routes. Depth changes possibilities without dominating value multipliers, and exposed common finds remain worthwhile income (`56`/`45`/`37`).
- The hole itself is a reward: widening, chambers, supported steps/spiral returns and chosen branches are legitimate. Detector signals suggest investigation, not compulsory waypoint chasing. Do not demand a particular hole shape or guaranteed treasure behind every wall (`10`/`15`/`45`/`49`/`37`).
- Foreground one stable physically noteworthy signal, with eligibility separate from price/rarity. Large cheap finds can signal and routine tiny finds usually do not; small noteworthy exceptions need a content rationale. Preserve quiet gaps, no exact markers/metadata and no competing pulse streams (`57`/`10`/`40`/`49`).
- Starter battery/slots already support enjoyable trips. Vary which constraint makes the next purchase useful—capacity, battery, resistance, mobility or searching—without fixed stages or automatically canceling bought power. The sample inventory → battery → strength sequence is not selected (`15`/`56`/`37`).
- Every paid jetpack/shovel level needs a noticeable practical benefit, with major capability milestones; test jetpack gains at matched battery capacity. Do not add startup fees, shovel energy changes or extra categories just because examples mention them (`56`/`11`/`47`).
- Existing `12` supplies atomic sales/purchases and effect comparisons. Major shovel tiers already require visible/audio changes; `57` evaluates whether other equipment deserves visible hardware without committing new models. Retain no cargo weight or multiple-tool switching.

## Physical comfort

- Held digging/collection works from the start and must never be a purchasable ergonomic unlock. [78](../development/completed/78-input-accessibility.md) implements free optional toggle digging and full keyboard/mouse rebinding in Controls. Collision cleanup `26`/`34` stays done; preserve meaningful supported terrain and matching collision.
- Camera comfort `64`/`65` covers FOV, stable reticle and disabling actual camera effects. The current camera has no added shake/bob/jetpack effects; no new motion or fake switches is required.
- Precision movement is a real gap: `66` researches held slow-walk versus true crouch, then `67` implements the chosen interaction. Preserve responsive normal speed; no stealth, stamina or automatic cliff guard.
- Passive underground light is another gap: `68` selects mounting/visibility and any optional upgrades, `69` integrates it. Baseline light works at zero battery without tool switching, lamp placement or a new resource chore. The report's 2–3 upgrades remain a proposal.
- Geology changes gradually within one volume, without biome unlocks. Ordinary excavation material eventually yields; true boundaries need categorically different presentation. Approved material/tool sound variation conveys progress without exhausting repetition (`57`/`58`/`11`/`39`/`54`).
- Stronger jetpacks remain controllable through narrow routes and landings; ordinary wall/ceiling bumps cannot turn a purchase into a penalty (`47`/`50`).

## Clear information and player observation

- `FpsHud` already shows `FINDS count / capacity` and battery charge. Preserve both through UI migration and upgrades (`05`/`07`/`48`); do not create another HUD-resource task.
- Sell/upgrade/save meaning and input should be clear. Use existing station identity, menu labels and compact Pause reference; do not restore removed instructional subtitles or constant saving notices (`35`/`05`/`15`/`57`).
- Observe an unguided roughly 30-minute session when content supports it; inspect actual excavation shape and ask about intent before coaching. Repeated narrow shafts can prompt investigation, not prove failure or mandate branches (`15`/`37`).
- Record limiting factors, purchase reasons, return/revisit mistakes, optional shaping and discovery gaps alongside times. A player report is useful evidence; questionnaires alone cannot establish how the game is played.

## Persistent investment

- `35` preserves one consistent terrain/discovery/economy snapshot with periodic dirty and sale/purchase autosaves, quiet feedback and interrupted-write recovery. Measure costs/loss windows; never restore fresh terrain with old purchases.
- Rescue remains an emergency fallback even at late wealth or with empty bags. Task `86` applies the existing `14` fee/loss automatically on fuel depletion, but its up-to-10-credit cap is not proven full-run balance. `59` researches a transparent deterrent, `50` implements accepted changes and `37` compares physical return with repeated rescue; broke/stranded recovery must stay available.
- The ending pays off normal upgraded excavation and preserves same-save Continue/photos, without stripping tools, requiring rare passives/maxed tracks or changing genre (`51`/`52`). The 2–4 passive finds remain optional rewards (`36`).
- Completion means discovering/recovering interesting things, not deleting every voxel or empty border wedge. Players may clear the site for pleasure. Achievement design/integration (`55`/`53`) avoids terrain-percentage chores and fragile geometry/fall tricks; unusual challenges need an explicit tolerant contract.

## Scope boundaries and conditional investigation

- No new hazards, survival meters, crafting, cargo weight, endless mode, music or voice acting. The reports' conditional hazard examples do not override those rules.
- `70` compares HOME versus simple revisit markers immediately after `15`, using actual orientation/remembering evidence separately from dull commuting; late paid tracks are no longer prerequisites. No minimap, automatic waypoint route, through-wall treasure marker, normal teleport, building menu or lamp platform. Keep/defer/omit are valid recorded outcomes.
- `71` evaluates already-optional placed/remote dynamite: if selected, forgiving valid-surface placement while moving/airborne, clear preview, no throw/bounce/perfect-angle requirement, no charge spent on failure, and substantial predictable saved blasts using shared cleanup. Never require bombs to finish or create a stockpile test.
- `61` evaluates perhaps 3–5 brief non-blocking buyer reactions/headlines for foreshadowing, with exact lines/triggers/repeat rules reviewed before `52`. No forced reading, dialogue system or subtitle spam; ancient chushkopek/moonshine examples do not select the final object.
- Large extraction, condition and protected components retain existing gates. Optional research completion does not authorize implementation: create/sequence a new numbered delivery task only for selected additions.

## How tasks retain decisions

Read the task's linked feature and these relevant findings before work. Features retain selected behaviour, why, exclusions/rejected alternatives and unresolved proposals; the full idea carries concept-level decisions. Task files retain research, questions, work and acceptance. Ask the active task's questions in useful batches with concrete examples, record answers and update affected contracts so no future session depends on chat-only memory.
