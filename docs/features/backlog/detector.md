# Detector feedback

Status: Task `09` is planned.

Idea coverage: sections 16-18 and relevant tuning in section 53.

## Purpose

Pull the player toward possible discoveries without revealing exact positions, value, or rarity.

## Task 09 - initial detector integration

- Scope after Task `08`: implement `PassiveDetector` over uncollected registry positions, with tunable range, distance-driven pulse interval and quiet gaps; use anonymous visual HUD pulses as the temporary feedback fixture.
- Acceptance: approaching/receding changes cadence predictably, out-of-range/collected finds are silent, overlap produces one readable pulse stream, and menus pause feedback. Never reveal identity, value, rarity, exact distance or a target marker.
- Next: Task `10`. Audio, stereo/directional feedback and detector upgrades remain later work.

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
