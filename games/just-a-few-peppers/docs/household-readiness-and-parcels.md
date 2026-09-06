# Household presentation, cellar, and family parcels

[Design index](readme.md) · Just a few peppers · current presentation specification

**Keep the winter-preparation story in the world.** The player handles and processes finite batches, then deposits finished carriers at the **Finished Food Handoff Rack**. That is their final handling responsibility. The cellar, labelled family boxes, and vine table show what that work means; nobody asks the player to redistribute the food.

This replaces the earlier household task system. Returned jars, parcel packing, food reassignment, and table preparation are not player obligations. The previous 2–3 minute target and under-five-minute chore ceiling are superseded: there are zero additional household tasks.

## One interaction, several visible consequences

| Household detail | Current presentation | Player work |
| --- | --- | --- |
| Returned jars | Worn-labelled empty jars near the gate; exposed loose ones can be moved as props. | Optional arrangement/play only. No return objective, consumable supply, or jar debt. |
| Winter cellar | Older compote, pickles, and lyutenitsa beside space that fills with today's roasted-pepper jars. | Deposit finished carriers at the one handoff rack; never carry them to the cellar. |
| For Aunt / Take to the city | Two labelled boxes at the gate become visibly filled as stored food increases. | The same rack deposit; no recipient choice or packing interaction. |
| Old refrigerator/tool cupboard | An accessible open cupboard with a grinder and reused tools. | Optional inspection/prop handling; no blocked approach, cupboard objective or current grinder-production system. |
| Table under the vine | Work clutter during play; an optional later tablecloth/food tableau after completion. | None. It never gates or triggers completion. |
| Grandpa and the family | Seated Grandpa, understated remarks, family labels/photos, optional offscreen audio, and a cuttable bottle/gift visual. | No NPC schedules, labor allocation, required conversations, meal interaction, or gift inventory. |

Portable background objects use the [shared handling contract](core-loop-and-mechanics.md#pick-up-place-and-play) and contextual grab guidance. Basins, stools, empty jars and crates may be arranged freely without task markers or required cleanup. Fixed fixtures and stored-food displays have distinct presentation and do not promise a grab action. The processing bench and handoff rack remain distinct from the social table. English draft signage names the **Finished Food Handoff Rack** and explains that jars go to the household automatically. This is a fictional deposit convention, not a new household transport simulation.

## Progress drives presentation

Use the existing **stored winter food / total harvest** ratio for the food display. Clearing alone must not show newly preserved food: peppers can still be queued or carried.

A small set of authored display states is enough for the first version:

| Stored share | Presentation target |
| --- | --- |
| 0% | Pre-existing cellar food only; today's reserved spaces and family boxes are empty. |
| More than 0% and below 50% | An early group of today's jars appears; the family boxes remain visible and labelled. |
| 50% to below 100% | More reserved shelf space fills and jars appear in the family boxes. |
| 100% | The complete winter-food display includes the full-looking cellar and both waiting family parcels. |

These are presentation milestones, not equipment unlocks, currency, quotas or separate completion flags. Choose the current state directly on load. Large deposits skip intermediate scenes without replayed congratulations. The cellar/storage view is available initially; returning to it later shows current food without requiring food relocation or clearing a gate. Coins are a separate budget and spending cannot reduce this display.

The displays represent portions of one stored supply. Shelf and parcel arrangements are authored together as one combined visual budget. A finished carrier empties when deposited; do not leave another collectible copy at the rack. Decorative jar density may be approximate, so avoid exact per-recipient counts or meters. Pre-existing preserves remain visually distinct and never count toward today's harvest.

Stored-food display groups are distinct from loose empty jar props and from the collectable finished carrier at the machine. Use packed/shelved group presentation and clear handoff signage for that distinction. Display updates cannot reset a player's nearby prop arrangement or turn their moved empty jar into credited food. Keep portable prop pose saving separate from these progress-derived views.

No walking helper is needed to distribute jars. A short placement sound and restrained appearance/settling animation can suggest the family handoff. Reuse these display states; do not create an event scheduler or an inventory per shelf and parcel.

## Finish conditions

Harvest completion is true when both statements are true:

1. All initial raw food has been used, with no units left in raw sources or registered loose/transit material.
2. All of that harvest has reached the handoff rack, with no raw, queued, processing, uncollected, or carried amount left.

With conserved contents, this is one winter-preparation completion condition. Yard tidiness, arranged props, remaining Coins, purchases, returned jars, recipients and the table add no requirements. The last handoff awards its proportional Coins once alongside stored food and completion.

The final valid deposit commits its normal transfer and harvest-completion state in the same transaction. After its immediate deposit feedback, show a quiet, nonmodal acknowledgement such as **Harvest complete** or **All peppers prepared**. There is no Ready-to-finish stage, Finish Day button, walk to the table, countdown, photograph, cutscene, credit fade, menu ejection, or additional delivery requirement.

Keep normal camera and movement control in the completed yard and let the player leave through ordinary pause/menu controls. Machines become idle naturally, and the complete winter-food display and open property remain visible. Completion creates no new supply, chores, deadlines, or surprise deliveries.

Save/resume restores the completed property without replaying a reward or running a required ending sequence. A simple table tableau, short thank-you, offscreen greeting, or Grandpa's bottle/gift may remain a later, cuttable presentation flourish. Such props represent existing household food, do not subtract from stored harvest, and require no meal interaction, cinematic system, or additional completion flag. The first version has no post-game favors or new supply loop. M4 implements harvest completion; M5 may add optional presentation. The M1–M2 interaction prototype tests complete storage without household display states.

## Evaluation after the core works

Check whether players connect the changing cellar and labelled parcels to their rack deposits, understand that they have no extra errands, and find the completed yard a satisfying conclusion. If optional closing presentation is too expensive or distracting, cut it before changing harvest completion. Preserve the labels, stored-food payoff, and normal post-completion control.

Props, animation, sound, and localization still require work. The attachment's engineering-cost percentages are opinions, not estimates adopted by this spec. The scope saving comes from removing interactive inventories, task conditions, and their combinations.
