# Task 119 - Compare richer backpack inspection and dropping stored finds

Type: design/research; documentation only. Status: `planned`. Prerequisites: `106` current inventory review, `101` collection observations and [117](117-unique-find-purpose-design.md) purpose/protection policy. Independent of final world art.

Feature: [inventory](../../features/backlog/inventory.md). Related: `97` special interactions, `07` existing production inspection validation, `118` backpack-opening cue. New implementation work follows only a selected outcome.

## Why and baseline

The user proposed opening/inspecting the backpack and dropping a stored find. Today the inventory shows names and sale values; `115`/`116` lift/drop/throw applies to world objects outside inventory. Neither a 3D viewer nor returning a stored item to the world is implemented. Decide what improves appreciation or useful choice without making inventory management a repeated chore.

## Required comparison

- Inspect `GameMenuView`, `SessionInventory`, collection/handling and save identity. Use `106`'s observed screens, current common models and `117`'s proposed categories; illustrative special items are not asset approvals.
- Compare the existing list, optional selected-item detail/visual, and a freely rotatable 3D inspection view. Show concrete layouts and entry/exit actions, what the player learns/appreciates, and treatment of duplicates and unavailable artwork. Keep inspection optional, free of battery drain, and separate from selling.
- Compare keeping current storage with a deliberate Drop action that returns the selected stored instance to a nearby valid world position. Explain its useful cases and whether it encourages annoying cheap-item sorting. No cargo weight, bag-size puzzle, equip-to-inspect requirement, underground sale or automatic deletion of low-value loot.
- Apply `117`'s sale/protection rules to ordinary, distinctive and protected items. Show empty/full bag, selection changes, unavailable actions and why a protected object cannot be dropped if that is the selected policy. Do not grant blanket protection merely because an item is called unique.
- Specify the proposed world/inventory transfer: one stable identity, no duplicate sale/record, safe nearby placement, obstruction/failure leaving inventory intact, later recovery, save/reload and rescue. Resolve avoiding immediate automatic recollection while preserving the current direct-aim pickup contract; no silent change to global pickup timing.
- Provide a recommended minimal combination, reasons to omit/defer alternatives, asset/performance implications and a short user-feel comparison. 3D inspection must earn its rendering/UI scope; dropping must be optional and recoverable under the chosen rules.

## Questions and acceptance

Review actual comparison sheets with the user: what inspection should show, whether rotation adds value, whether Drop improves play and how protected items behave. None of these additions is selected simply by scheduling this design.

Done when the user reviews a concrete artifact and the inventory feature records the selected behavior, rejected options and integration boundaries. Update `07` only if its acceptance changes, coordinate `97`/`117`, and create a numbered implementation task for approved missing functionality. Preserve the current backpack and handling until that delivery passes Windows review.
