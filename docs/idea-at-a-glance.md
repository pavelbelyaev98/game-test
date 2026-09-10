# Idea at a glance

First-person excavation game about progressing from a simple shovel to absurd digging capability.

**Loop:** Dig -> detect -> uncover -> collect -> return -> sell -> upgrade -> repeat.

Core constraints:

- Digging and discovery dominate play time; surface visits are short checkpoints. Starting battery/inventory already support a satisfying expedition.
- Allow downward, diagonal, and meaningful sideways excavation without pre-dug routes. Depth changes possibilities without making a straight shaft overwhelmingly more profitable.
- Validate random layouts for novelty gaps/clumping before accepting them; persist that population. The passive detector foregrounds one eligible non-minor find without revealing monetary rarity. **All minor/common finds are silent**, regardless of size, metal, clusters or upgrades.
- Keep controls/progression clear and menus light; inventory fullness and battery are already visible. Add camera comfort and held precision movement early; stronger equipment must respect them.
- Provide dependable passive underground lighting without extra battery drain or lamp-placement chores; mounting and optional upgrade choices need research.
- Avoid puzzle gating, identification chores, multiple currencies, and item-by-item carrying.
- Start grounded and become increasingly strange and absurd.
- Preserve item levels: common means routine finds such as bottles/plain rocks; a familiar toy car is above common. Common collection should feel satisfying; higher tiers carry stronger surprise. Exact higher-tier labels/frequencies remain with roster design `40`.
- Target a dense 2–3 hour first completion with useful purchases and recognizable discoveries into late play; upgrades overpower old obstacles and purchase priorities vary.
- Treat the shaped excavation as part of the reward. Completion concerns discoveries, not deleting every voxel; detector signals suggest exploration rather than dictate paths.
- Preserve the excavation with periodic and transaction autosaves and interrupted-write recovery, across rescue and ending/Continue. The ending uses normal equipment; no extra hazard, combat, stealth or puzzle loop.
- Default digging is click-and-hold (hold LMB to dig, release to stop). Optional toggle digging is implemented in Controls: set Digging mode to Toggle, then press Dig once to start and again to stop. The 2–4 rare permanent rewards are optional to collect; HOME/dynamite remain conditional systems.

[Player-review findings](research/player-review-findings.md) inform research and playtests. The independent [Steam gap audit](research/steam-review-audit/report.md) screens 499 negative and 135 positive reviews and strengthens future task acceptance; its dataset and dated sources are separate from the earlier supplied reports and [Meltopia synthesis](research/meltopia-lessons.md). [Feature contracts](features/backlog.md) retain decisions and exclusions; numbered tasks retain research/questions and delivery work. The [risk register](development/design-risks.md) records intended player feelings, warning signs, what to avoid and future owners. Recommendations do not select optional mechanics.

Use the numbered task and its linked feature/research context for work. Read the intentionally long [full idea](idea.md) when those sources need more concept detail; preserve recorded choices rather than reconstructing them from chat. [Art style](features/backlog/art-style.md) records the preferred texture reference and `105`'s selected process: compare multiple packs in-game, iterate until the user selects a style, then keep future assets consistent.

[New idea assessment](research/player-idea-assessment.md) routes physics, special interactions/chests, refill pricing and mobility/teleport to proposed design tasks `96`–`99`; item sound and dynamite stay with `40`/`71`. [Your playtest sheets](development/playtesting.md) provide intended feelings and editable verdicts for current and future systems. The user's horizontal-travel criticism concerns Meltopia, not an observed failure in this build.


[Meltopia follow-through](research/meltopia-lessons.md): early rebind/toggle accessibility (`78`), incremental terrain capture (`80`), brightness/reticle design and delivery (`81`/`82`) and early navigation comparison (`70`). Review physical upgrade installation (`57`), sparse inspection flavor (`40`/`83`) and late completion assistance (`84`/`85`); measure return friction and preserve late earned power (`37`). Proposed mechanics remain review-gated.
