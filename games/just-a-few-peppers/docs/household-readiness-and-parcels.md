# Household presentation, cellar, and family parcels

[Design index](readme.md) · Just a few peppers · v4 presentation specification

**Keep the winter-preparation story in the world.** The player clears and processes peppers, then deposits finished carriers at one storage rack. The cellar, family parcels, and final table show what that work means.

This replaces the earlier household task system. Returned jars, parcel packing, food reassignment, and table preparation are not player obligations. The previous 2–3 minute target and under-five-minute chore ceiling are superseded: there are zero additional household tasks.

## One interaction, several visible consequences

| Household detail | Current presentation | Player work |
| --- | --- | --- |
| Returned jars | A static group near the gate with worn labels. | None. No return objective, consumable supply, or jar debt. |
| Winter cellar | Older compote, pickles, and lyutenitsa beside space that fills with today's roasted-pepper jars. | Deposit finished carriers at the one outdoor rack. |
| For Aunt / Take to the city | Two labelled boxes at the gate become visibly filled as stored food increases. | The same rack deposit; no recipient choice or packing interaction. |
| Old refrigerator/tool cupboard | An open cupboard in the shed, with a decorative grinder and reused tools. | Uncover the approach through ordinary pepper clearing; no cupboard or grinder system. |
| Table under the vine | Work clutter during play; tablecloth and meal during the ending. | Choose Finish the day once the harvest is stored. |
| Grandpa and the family | Seated Grandpa, a few remarks, family labels/photos, offscreen arrival audio at the meal. | No NPC schedules, labor allocation, or conversations required to progress. |

Background props should not show pickup prompts or task markers. The processing bench and storage rack remain distinct from the social table.

## Progress drives presentation

Use the existing **stored winter food / total harvest** ratio for the food display. Clearing alone must not show newly preserved food: peppers can still be queued or carried.

A small set of authored display states is enough for the first version:

| Stored share | Presentation target |
| --- | --- |
| 0% | Pre-existing cellar food only; today's reserved spaces and family boxes are empty. |
| More than 0% and below 50% | An early group of today's jars appears; the family boxes remain visible and labelled. |
| 50% to below 100% | More reserved shelf space fills and jars appear in the family boxes. |
| 100% | The complete winter-food display includes the full-looking cellar and both waiting family parcels. |

These are cosmetic milestones, not quotas, rewards, or separate completion flags. Choose the current state directly from saved progress on load. A large deposit can skip an intermediate state; it does not queue multiple scenes or replay congratulations. Cellar access may be uncovered later without requiring earlier deposits to be moved again.

The displays represent portions of one stored supply. Shelf and parcel arrangements are authored together as one combined visual budget. A finished carrier empties when deposited; do not leave another collectible copy at the rack. Decorative jar density may be approximate, so avoid exact per-recipient counts or meters. Pre-existing preserves remain visually distinct and never count toward today's harvest.

No walking helper is needed to distribute jars. A short placement sound and restrained appearance/settling animation can suggest the family handoff. Reuse these display states; do not create an event scheduler or an inventory per shelf and parcel.

## Finish conditions

The day is ready when both statements are true:

1. All authored pepper supply is cleared.
2. All of that harvest has reached the storage rack, with no raw, queued, processing, uncollected, or carried amount left.

With conserved contents, this is one harvest-completion condition shown from the yard and food sides. It has no independent returned-jar, recipient, table, equipment, or collectible requirements.

Show **Ready to finish the day** and a clear **Finish the day** action at the vine table. Until the player chooses it, they can inspect the open yard and food displays. No countdown, automatic credits, required photograph, or final delivery appears.

Choosing Finish the day quiets the machines and uses one short authored transition to replace the table's work dressing with a cloth, bread, cheese, tomatoes, a small plate of peppers, and glasses. Family greetings may come from offscreen. Grandpa's small bottle gift is part of the scene, not an inventory task. Meal props represent existing household food and do not subtract from the stored harvest.

Persist the completed-day state so resuming after the ending restores the finished scene without repeating rewards or adding work. The first version has no post-game favors or new supply loop.

## Evaluation after the core works

Check whether players connect the changing cellar and labelled parcels to their rack deposits, understand that they have no extra errands, and find the meal a satisfying conclusion. If the presentation is too expensive or distracting, reduce the number of display states or transitions. Preserve the labels and ending before adding more animation.

Props, animation, sound, and localization still require work. The attachment's engineering-cost percentages are opinions, not estimates adopted by this spec. The scope saving comes from removing interactive inventories, task conditions, and their combinations.
