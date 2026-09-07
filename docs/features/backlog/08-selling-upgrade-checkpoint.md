# 08 - Selling and upgrade checkpoint

Status: planned; validation adapters exist without production money.

Idea coverage: sections 25-27.

## Purpose

Make returning to the surface a short, rewarding checkpoint before the player descends again.

## Implementation task

Implement discovery values, explicit sell-one/sell-all actions, money, and one-level-at-a-time upgrades at separate surface stations.

## Required behavior

- Opening a station never sells, buys, or spends automatically.
- Underground inventory inspection cannot bank money.
- The selling location and separate upgrade location sit close to the excavation entrance.
- Prefer a fast physical selling machine with animated/noisy feedback and explicit Sell One/Sell All controls; never require dropping items individually.
- Normal finds provide reliable income. Rare finds can fund several trips or a major upgrade but must not collapse the whole economy.
- Purchases show effect, cost, affordability, and a clear result.

## Done when

- Sale and purchase transactions are atomic and persist correctly.
- Invalid or repeated commands cannot duplicate money/items or skip upgrade levels.
