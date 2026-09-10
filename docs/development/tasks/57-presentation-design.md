# Task 57 - Define the production presentation brief

Type: design/research; documentation only. Status: `planned`. Prerequisites: `56`, `105`; respect the user-deferred broader terrain/art scope. `105` owns the newly requested style research and texture trials.

Feature: [presentation](../../features/backlog/presentation-audio.md). Implementation: `08`–`11`. [Queue](../tasks.md).

Retain [74/75's shared Toolkit menu and HUD direction](../../features/backlog/menu-presentation.md#ownership-and-later-work) and [authoring sources](../ui-authoring.md). `08` refines the existing presentation; the HUD framework migration is complete. The retained EventSystem is input infrastructure, not a second presentation system.

Consume [106's complete UI/state/text review](106-ui-ux-audit-design.md) and recorded `107` cleanup decisions when available. Reuse its current copy/conditional-state catalog for feedback examples; do not build a second inventory or assume unseen error menus are absent. This task retains new feedback-priority/presentation policy, while `107` delivers selected current-UI cleanup.

## Research and proposal

- Read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect the approved stations, current HUD, user-owned terrain and existing visual evidence. Identify which presentation is unfinished without redoing accepted assets.
- Consume [105's tested art style](../../features/backlog/art-style.md) for a bounded presentation brief: readability/scale, ground versus boundaries, camera/tool visibility, HUD hierarchy and terrain handoff. Shared palette, texture language and asset matching are owned by `105`, not reselected here. Contrast diggable hard stone with finite-world boundaries through structure/scale/silhouette. Preserve visible inventory/battery facts, clear sell/upgrade/save meaning and `64`/`65` comfort decisions; no compulsory camera motion.
- Consume [89's starter selection/counts/specifications](89-starter-minor-find-design.md) for `09`. All starters and other minor/common finds are detector-silent, irrespective of size, metal or value. Brief readable exposed shapes without detector cues, visible shovel milestones for `11`, and sparse initial non-minor detector feedback for `10`. The remaining roster and actual positive detector targets belong to `40`/`42`.
- Brief gradual geological visual progression with overlapping mixtures, not biome/level unlocks; `58` chooses the material plan. Include restrained material/tool-specific digging sound variation, for integration by `11`/`39` rather than leaving it to release polish.
- Propose the minimal interaction/audio scope consistent with the user's deferred work, quiet HUD and no music/voice acting. Research source/license options and repeated-sound fatigue; do not download into the project, create art or introduce new ambience merely to fill the brief.
- Consume [68's player-light/asset brief](../../features/backlog/underground-lighting.md#asset-brief-and-ownership) and prepare `69` integration ownership. Its proposed offscreen headlamp needs no visible housing; await the recorded selection before expanding the brief. Consider visible major battery/detector/jetpack hardware only where players can meaningfully see it; review whether additions earn their scope before assigning them to `46`/`47`/`49`. Shovel milestone visuals/audio remain required; every minor stat tier need not get a new model.
- Identify concrete future asset batches, integration ownership and removal boundaries. A visual direction or reference board is not permission to add its assets.
- Produce the [player-feel](../../research/steam-review-audit/player-feel.md) state/priority matrix: contact, first visible shape, collectible shape, pickup identity, bag full, low reserve and rescue result. Show simultaneous-event examples at slow/fast held digging, muted sound and reload. Inspect the single replaceable `FpsPlayer.ShowFeedback` message; propose which information persists or yields without assuming a missed-notice defect. Preserve current exposure/pickup timing unless a concrete alternative is separately selected; no modal inspection or extra click.
- Brief a concise surface payoff using the completed `12` stations: the player understands what was banked, that recharge happened and what a purchase improved. No longer walk, confirmation cascade or added sorting step. Include a visual-affordance sheet distinguishing decorative props, working stations, buried finds, diggable resistance and true boundaries; decoration must not accidentally promise a usable reward or route. `08` implements the shared presentation, `09` collection specifics, `15` observes understanding.

## Questions to resolve with the user

Review terrain handoff and exact presentation scope; select boundary language, equipment appearance and initial feedback within `105`'s accepted style, retaining `89`'s accepted starter objects. Discuss which non-shovel equipment milestones deserve visible hardware, from which view, and why; a reference or asset brief is not approval. Ask only for relevant decisions with concrete examples. Each actual new asset/audio batch still needs explicit approval before creation/import.

## Done when

- An accepted brief distinguishes user-owned work, retained assets and named future batches. `08`–`11` reference it and can prepare specific approval requests without inventing the direction while importing.
- The event matrix, surface-payoff examples and affordance sheet resolve presentation choices concretely, with intended feeling and failure cases from the [risk register](../design-risks.md). Retain omitted/rejected alternatives and their reasons; a new feedback queue or recognition delay is not preselected.
- The user-deferred art request is not bypassed; record any still-deferred scope honestly. No asset production or gameplay implementation is claimed complete.

## Meltopia feedback and installation proposal

- Brief the full readable chain: shovel contact, dirt fracture/particles, terrain removal, object reveal and pickup response. Compare sustained early/mid/late digging with loot absent; feedback must carry the action without compulsory shake or obstructing aim.
- Propose brief physical installation of major blade/motor/battery attachments onto the same shovel at purchase. Review staging, duration/skip, visibility and interruption/reload behavior; the purchase commits once even if presentation is interrupted. `11` owns delivery of the selected sequence, not every minor stat tier.
- Specify early scrape/thud, powered mechanical impacts and late motor/torque character using approved sound batches. `10` owns independent saved detector volume/mute, sparse pleasant pulses and usable non-audio feedback from this brief; `49`/`54` repeat fatigue checks at high range. No music or new asset approval follows from the [report](../../research/meltopia-lessons.md).
