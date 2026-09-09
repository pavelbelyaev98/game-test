# Idea at a glance

First-person excavation game about progressing from a simple shovel to absurd digging capability.

**Loop:** Dig -> detect -> uncover -> collect -> return -> sell -> upgrade -> repeat.

Core constraints:

- Digging and discovery dominate play time; surface visits are short checkpoints. Starting battery/inventory already support a satisfying expedition.
- Allow downward, diagonal, and meaningful sideways excavation without pre-dug routes. Depth changes possibilities without making a straight shaft overwhelmingly more profitable.
- Validate random layouts for novelty gaps/clumping before accepting them; persist that population. The passive detector foregrounds one physically noteworthy find, never monetary rarity.
- Keep controls/progression clear and menus light; inventory fullness and battery are already visible. Add camera comfort and held precision movement early; stronger equipment must respect them.
- Provide dependable passive underground lighting without extra battery drain or lamp-placement chores; mounting and optional upgrade choices need research.
- Avoid puzzle gating, identification chores, multiple currencies, and item-by-item carrying.
- Start grounded and become increasingly strange and absurd.
- Target a dense 2–3 hour first completion with useful purchases and recognizable discoveries into late play; upgrades overpower old obstacles and purchase priorities vary.
- Treat the shaped excavation as part of the reward. Completion concerns discoveries, not deleting every voxel; detector signals suggest exploration rather than dictate paths.
- Preserve the excavation with periodic and transaction autosaves and interrupted-write recovery, across rescue and ending/Continue. The ending uses normal equipment; no extra hazard, combat, stealth or puzzle loop.
- Default digging is click-and-hold (hold LMB to dig, release to stop). Optional toggle digging is implemented in Controls: set Digging mode to Toggle, then press Dig once to start and again to stop. The 2–4 rare permanent rewards are optional to collect; HOME/dynamite remain conditional systems.

[Player-review findings](research/player-review-findings.md) inform the tasks' research and playtests; the earlier reports and [Meltopia synthesis](research/meltopia-lessons.md) are qualitative, with checked primary sources distinguished from unverified claims. [Feature contracts](features/backlog.md) retain detailed decisions, reasons and exclusions; numbered tasks retain research/questions and delivery work.

Use the numbered task and its linked feature/research context for work. Read the intentionally long [full idea](idea.md) when those sources need more concept detail; preserve recorded choices rather than reconstructing them from chat.


[Meltopia follow-through](research/meltopia-lessons.md): early rebind/toggle accessibility (`78`), incremental terrain capture (`80`), brightness/reticle design and delivery (`81`/`82`) and early navigation comparison (`70`). Review physical upgrade installation (`57`), sparse inspection flavor (`40`/`83`) and late completion assistance (`84`/`85`); measure return friction and preserve late earned power (`37`). Proposed mechanics remain review-gated.
