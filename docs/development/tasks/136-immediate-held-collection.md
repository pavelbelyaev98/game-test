# Task 136 - Immediate held collection

Type: implementation. Status: `done`. [Completed result](../completed/136-immediate-held-collection.md). Prerequisites: 131/133 delivered. Explicit user-selected batch with 137, ahead of 126.

Feature: [discovery/collection](../../features/backlog/discovery-collection.md). Context: [131](131-responsive-find-pickup.md).

## Scope

- Holding active Dig while directly aiming at an eligible uncovered item collects it immediately, without the old recognition timer or shovel cooldown. Keep the 60% exposure threshold, direct visibility, 3 m collection reach, gameplay-state and inventory checks.
- Preserve digging cadence/fuel, same-stroke reveal collection, the short independent pickup interval, quick pull animation, walking pickup and lift/drop/throw behavior.
- Remove obsolete recognition state/tuning and update existing integration tests and the owning contract.

## Acceptance

- Reproduce holding Dig then aiming at an eligible bottle/rock while a shovel stroke is still cooling down; collection completes on that input frame, consumes no extra fuel/stroke and creates exactly one inventory identity.
- Held/toggle input, ineligible/occluded/out-of-reach finds, full bags, handling, menus/focus and ordinary terrain cooldown remain correct.
- Fast compile and relevant integration checks pass; inspect through official CLI, deliver a Windows build and clean temporary captures. Keep reservoir staging outside this gameplay change.

## Questions

None. The user's explicit immediate held-pickup request supersedes 115's ordinary 0.6-second recognition delay.
