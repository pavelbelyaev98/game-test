# Shovel progression

Status: Task `31` is complete: slightly smaller/weaker scoops at all six levels. Task `25` remains a future TODO for independent speed/strength upgrades. Approved tool art and paid purchasing retain `11`/`12`.

Idea coverage: sections 9-11, the tool arc in section 46, and relevant tuning in section 53.

## Purpose

Make each upgrade visibly and physically improve excavation rather than merely increasing a hidden number.

## Task 11 - production shovel progression

- Scope after Task `10`: add session-owned, data-driven `ShovelState` and production-ready visible shovel assets created through Blender MCP or obtained free under a commercial-use license. Each implemented level must look and feel meaningfully stronger and connect to terrain; paid purchasing follows in Task `12`.
- Acceptance: equal accepted hits into equal fresh soil remove observably more at level 2 for the same energy; boundaries stay intact. Invalid/skipped/repeated level changes fail, and the chosen level survives surface trips.
- Next: Task `12`. Do not mark this done with invisible stat changes or placeholder tool art.

## Task 20 - excavation strength and testing

- Six serialized profiles on the player define scoop radius, cadence multiplier and reach bonus. Start at level 1; each radius strictly increases. Energy per accepted stroke stays equal.
- Normal progression accepts only the next valid level and persists for the scene session. Paid purchase remains Task `12`.
- Developer overrides never change owned progression. Restore normal rules removes the selected-level override, unlimited battery and X-ray without restarting or changing the terrain/inventory.
- Feedback shows effective level, nominal cut width, reach, depth and a success pulse. Tasks `28`/`29` remove out-of-range coaching, routine dig-button prompts and per-stroke volume popups. The irregular cutter's exact footprint varies around the profile width. No new tool art, sounds or materials are authorized.

## Task 23 - reach and developer admin

| Shovel level | 1 | 2 | 3 | 4 | 5 | 6 |
| --- | --- | --- | --- | --- | --- | --- |
| Dig reach (m) | 3.0 | 3.2 | 3.4 | 3.6 | 3.8 | 4.0 |
| Scoop width (m), Task 31 | 0.82 | 1.04 | 1.26 | 1.48 | 1.70 | 1.92 |

- Task `31` reduces Task `24` radii by 7–8%, using even 0.11 m steps from 0.41 to 0.96 m. Aim for roughly 20% less fresh soil per stroke while retaining increasing strength, cadence, organic variation and equal energy. Task `27` reach and its 4 m cap stay unchanged. Acceptance: measured reduction against the previous profiles, six real reach/energy checks, live inspection and Windows build.
- Radius, reach bonus and cadence remain serialized per level; reject non-increasing reach/radius. Owned upgrades extend digging without requiring admin access; interaction/collection reach stays independent and blockers stop long-range rays.
- Deliver one normal development executable. Ctrl+Shift+F10 opens/closes Developer admin (also reachable through Pause); Ctrl+Shift+1-6/numpad selects any shovel, Ctrl+Shift+R refills, Ctrl+Shift+Home returns. Plain keys have no admin effect; keys held before modifiers cannot become shortcuts. Input is release-safe across focus/menu changes.
- Admin provides buttons for the same actions, unlimited battery, restore normal rules, confirmed ground reset and Task `27` X-ray (Ctrl+Shift+X). X-ray marks buried finds without bypassing collection rules. State is session-local; closing/reopening/reset preserves overrides. Startup uses the owned shovel, ordinary battery and X-ray off.
- [Unity 6.6 `Debug.isDebugBuild`](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Debug-isDebugBuild.html) gates admin; use the supported engine flag instead of the deprecated preprocessor directive. The existing Windows build command includes development access; the release command writes the same output path without it. No launchers, extra game modes or Windows elevation. Task `22` verifies final release exclusion.
- Acceptance: real Input System modifier/edge coverage, actual distance checks at all six owned levels, occlusion/interaction isolation, unlimited energy and restore behavior, confirmed reset, organic depth/replay checks, UI inspection and Windows build.

## Task 25 - independent speed and strength upgrades (future)

- TODO only; do not implement separate upgrade tracks during tuning Task `24`. The user wants **digging speed** and **digging strength** as two independently purchasable upgrades with separate levels, costs and saved state.
- Speed changes shovel cycle time/cadence without increasing soil removed per stroke. Strength changes soil removal without automatically accelerating cadence. The current six combined presets remain until this task; reach must stay capped at 4 m.
- Acceptance: each purchase affects only its intended attribute, displays its own cost/effect/level, persists, and rejects invalid/unaffordable/duplicate purchases. Validate balanced throughput across combinations and preserve admin testing.
- Concern: instant late-level cuts can feel excessive because volume grows cubically with radius. Task `24` softens the radius curve now. If that still feels abrupt, discuss removing soil progressively during a short shovel stroke, keeping collision synchronized and charging only once per accepted stroke. This timed-removal option is a proposal, not implemented behavior.

## Required behavior

- Levels are purchased in order and communicate their concrete effect.
- Better tools make previous resistance noticeably easier.
- Keep one recognizable evolving shovel: basic, reinforced, powered, motorized, then an unreasonable homemade machine with visible attachments.
- Make milestones improve sustained digging without removing an implausibly large cavity in one instant.
- Use money rather than arbitrary depth gates. The player chooses among tool, battery, jetpack, inventory, and detector priorities.
- Upgrade data can be balanced without rewriting gameplay logic.

## Done when

- Each implemented level changes observable excavation behavior.
- Purchase, save/load, and invalid-level checks pass.
