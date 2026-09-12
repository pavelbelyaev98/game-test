# Task 130 - Integrate approved reservoir sections

Type: implementation. Status: `planned`. Prerequisites: [137 first-section review](137-reservoir-first-section-review.md) and explicit approval of the sections to integrate. The former complete environment batch is rejected and its approval request retired.

Features: [art style](../../features/backlog/art-style.md), [presentation](../../features/backlog/presentation-audio.md), [terrain/site](../../features/backlog/terrain-materials.md).

## Selected scope

- The drained-reservoir premise remains selected. The user now requires detailed work from real photographs, one section at a time, reviewed before further construction. Cartoon style means deliberate shapes and palette, not low-quality geometry/materials.
- 137 owns the first concrete dam/outlet section and its review. Do not regenerate or import the old complete banks/dam/pool batch; do not pre-approve later sections.
- After specific approval, integrate only the described section(s) through isolated assets and official Unity CLI. Preserve the 24×24×12 m excavation, station/return positions, boundaries, continuous grass, sun/bright clear sky, saves and shovel/pickup behavior.
- 135's removal of clouds, trees, river and decorative surface rocks remains in force. Reservoir sections are authored surroundings, not new excavation, passages, water gameplay or geological resistance. 58 retains material/resistance and later footprint decisions.

## Acceptance

- User-approved section geometry/materials retain their reviewed quality at player height in MainGame, with clear permanent-boundary affordances.
- Specific approved imports have source/license, exact ownership and reversible scene/setup integration recorded in the ledger; unapproved sections stay outside Assets.
- Relevant scene/integration checks and official CLI inspection pass; provide a Windows build for integrated sections. Preserve saves and do not commit.

## Current staging

- [137](137-reservoir-first-section-review.md) replaces the prior full-environment review with one detailed original Blender section informed by real photographs. Its approval is pending; nothing is imported.
- Original `unity/Logs/Task130/reservoir-preview/` source, FBX, six material maps and license are retained only as a rejected archive. Its old preview images/review gallery are removed, and old integration recipes must not be run. Their earlier reservoir reference links remain historical context, not a selected construction or pending art approval.

## Questions

137 owns the first-section verdict. Later sections are prepared only after that review and the user's next requested step.
