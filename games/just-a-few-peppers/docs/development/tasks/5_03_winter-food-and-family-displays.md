# 5_03 — Winter food and family displays

Milestone: M5 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Show what the stored harvest means without adding household work.

**Depends on:** [5_02 — Handling and machine presentation](5_02_handling-and-machine-presentation.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Household and ending](../../household-readiness-and-parcels.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md) · [State and saving](../state-and-saving.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Author the four combined cellar/parcel display states for stored shares 0, above 0 below 50%, 50% below 100%, and 100%. Distinguish older pantry food from today's product.
- Connect display selection directly to the stored/initial harvest ratio. Use one combined visual food budget for shelves and the two labelled family boxes.
- Keep the Finished Food Handoff Rack as the player's final handling destination and explain the automatic household handoff through its existing signage/feedback. The player never transports food from it to the cellar or boxes; no helper, distribution animation system, or second inventory.
- The cellar/storage view is accessible initially; returning after earlier handoffs or snapshot restoration immediately shows current food. Large deposits skip intermediate scenes. Spending Coins never reduces stockpile presentation or gates its milestones.

## Acceptance

- Clearing alone never fills today's shelves; deposits change them at the defined thresholds, including exact 50% and 100%.
- Display changes and reloads neither duplicate food nor add recipient inventories, tasks, pickup prompts, or completion conditions.
- At the last deposit, the 100% display resolves with harvest completion; it adds no final distribution step, ending sequence, or household obligation.

## Human playtest check

Deposit across display thresholds, buy an improvement, revisit the cellar view and reload. Stock continues to reflect all stored food while the separate Coin balance reflects spending.

**Outside this task:** Returned-jar quests, manual parcel allocation, helper transport, exact recipient quotas, and extra preserved-food recipes.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [5_04 — Grandpa and optional closing presentation](5_04_grandpa-and-meal-transition.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
