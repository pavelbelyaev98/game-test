# Detector feedback

Status: first detector feedback (`10`) is planned after production non-minor targets in `42`; `09`'s three common starter types are deliberately silent. The common-only trip `15` can be validated earlier. Paid progression (`49`) follows content and other equipment tracks.

Idea coverage: sections 16-18 and relevant tuning in section 53.

Design: [57](../../development/tasks/57-presentation-design.md) defines initial feedback and [56](../../development/tasks/56-progression-design.md) defines paid interpretation milestones before `10`/`49`; actual sound assets still require approval.

## Purpose

Invite investigation of possible discoveries without revealing exact positions, value, or rarity. The player can enjoy ignoring a signal and shaping the hole; quiet or muted play must still feel deliberate and readable. Preserve the [risk register's attention/agency guardrail](../../development/design-risks.md) during `10`/`49`; their existing stable-focus and non-audio acceptance owns delivery.

## Task 10 - detector integration

See [numbered Task `10`](../../development/tasks/10-passive-detector.md) for scope, research, questions and acceptance.

## Later expansion

Implement a passive detector signal driven by nearby eligible discoveries. Keep direction and strength useful but uncertain, with tunable cadence and overlap handling.

## Required behavior

- Detection does not require switching tools or holding a scan button.
- Signals can encourage downward, sideways, diagonal, or backward investigation. They are suggestions, not waypoints: players may ignore them to widen pits, shape useful routes or make chambers. Quiet gaps protect the excavation's value as player expression.
- Early upgrades improve short-range proximity feedback; later ones may add range, stereo information, and broad direction without becoming a treasure GPS.
- Signals never reveal item identity, value, or rarity. A physically large item may create a broader signal only because of its size.
- **All minor/common finds are ineligible**, independent of size, metal content, sale value, depth or number nearby. Common clusters never create an aggregate signal. This category is separate from physical small/large size and has no paid-upgrade or completion-mode exception.
- Within non-minor content, author physical noteworthiness separately from price/rarity. Low-priced distinctive objects may signal; not every distinctive object must. Explicit small noteworthy exceptions apply only within non-minor content. Do not relabel common finds as distinctive merely to populate detector tests.
- Foreground one nearby eligible signal, with stable switching and one pulse stream. No overlapping beeps, rapid target hopping or visible target lock.
- Quiet intervals matter: the signal should remain an event rather than constant background noise.

## Done when

- Signal changes respond predictably to distance/direction in controlled scenes.
- Hidden metadata is not exposed through UI or audio.
- Cadence and ambiguity can be tuned without changing detector code.

## Task 49 - paid detector progression

See [numbered Task `49`](../../development/tasks/49-detector-upgrades.md) for scope, research, questions and acceptance.

## Audio comfort and conditional completion assistance

`10` includes independently saved detector volume/mute and reviewed non-audio feedback; higher levels retain both and quiet intervals. Common finds produce neither an audio nor a non-audio detector cue. [84](../../development/tasks/84-completion-assistance-design.md) compares late/postgame assistance for undocumented distinctive finds; it cannot include minors. Such completion filtering remains proposed, with [85](../../development/tasks/85-completion-assistance.md) conditional on review; no exact treasure GPS or completion counter is authorized.
