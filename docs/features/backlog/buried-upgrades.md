# Buried permanent upgrades

Status: Task `36` is planned after persistent state (`35`), the selected receiving systems and approved discovery presentation. No passive reward type exists in the current game.

Idea coverage: section 12; normal money progression, rescue protection and ending independence remain governed by sections 11, 37 and 48.

## Purpose

Make a few exceptionally rare discoveries improve the player's equipment permanently, rewarding exploration without making the ending depend on finding them.

## Task 36 - permanent passive discoveries

- Include 2–4 very rare buried upgrade discoveries in the finite world population. They grant a passive effect once instead of becoming saleable loot; keep identity and seeded placement compatible with the existing discovery registry.
- Candidate effects are detector improvement, jetpack efficiency and dynamite improvement. These are examples, not an approved item list or a requirement to implement dynamite. Choose only effects for systems actually included in the game, with clear benefit and bounded interaction with bought upgrades.
- Use the normal reveal, visibility, reach and held-pickup rules. A successful pickup grants the effect and consumes the world discovery together; show its identity and concrete benefit clearly. No equipment switching, activation chore or normal inventory slot is required, including when the bag is full.
- Keep rewards outside Sell One/Sell All and ordinary rescue losses. Persist reward IDs and applied benefits through `35`; replayed collection, repeated load, rescue and ending/continue cannot duplicate or remove an effect.
- Placement can reward downward or sideways investigation, with no required route or final-object gate. Detector feedback must not disclose their rarity, identity or value through a special signal.
- Each proposed discovery model and any audio require their own explicit approval before import. Reuse approved game presentation where appropriate; record approved additions in the asset ledger when a specific batch is proposed.

## Acceptance

- In the Windows build, uncover a real reward, collect it while digging, and observe its effect on the actual receiving system. Verify full-bag pickup, occlusion, held-input safety and feedback.
- Verify exactly-once grants, sale/rescue exclusion, composition with paid levels and save/relaunch restoration without respawning the reward.
- Placement contains the chosen 2–4 discoveries without duplicates, boundary breaches, invalid overlaps or unreachable placements. Rarity never replaces the ordinary income pool.
- Complete the ending with none collected; review pacing both without bonuses and with an early lucky discovery under `37`.

## Open decisions

Exact objects, effects, magnitudes and placement weighting remain to be chosen through playtesting and asset approval. The 2–4 content target is committed; collecting any of them is optional for the player.
