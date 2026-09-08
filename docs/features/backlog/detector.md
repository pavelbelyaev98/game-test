# Detector feedback

Status: first detector feedback (`10`) is planned after `09`; paid progression (`49`) follows the full trip, content and other equipment tracks.

Idea coverage: sections 16-18 and relevant tuning in section 53.

Design: [57](../../development/tasks/57-presentation-design.md) defines initial feedback and [56](../../development/tasks/56-progression-design.md) defines paid interpretation milestones before `10`/`49`; actual sound assets still require approval.

## Purpose

Pull the player toward possible discoveries without revealing exact positions, value, or rarity.

## Task 10 - detector integration

See [numbered Task `10`](../../development/tasks/10-passive-detector.md) for scope, research, questions and acceptance.

## Later expansion

Implement a passive detector signal driven by nearby eligible discoveries. Keep direction and strength useful but uncertain, with tunable cadence and overlap handling.

## Required behavior

- Detection does not require switching tools or holding a scan button.
- Signals can encourage downward, sideways, diagonal, or backward investigation.
- Early upgrades improve short-range proximity feedback; later ones may add range, stereo information, and broad direction without becoming a treasure GPS.
- Signals never reveal item identity, value, or rarity. A physically large item may create a broader signal only because of its size.
- Multiple nearby discoveries do not create unreadable or exhausting feedback.
- Quiet intervals matter: the signal should remain an event rather than constant background noise.

## Done when

- Signal changes respond predictably to distance/direction in controlled scenes.
- Hidden metadata is not exposed through UI or audio.
- Cadence and ambiguity can be tuned without changing detector code.

## Task 49 - paid detector progression

See [numbered Task `49`](../../development/tasks/49-detector-upgrades.md) for scope, research, questions and acceptance.
