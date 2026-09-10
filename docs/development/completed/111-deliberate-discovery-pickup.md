# 111 - Deliberate discovery pickup

Superseded by [112 - held pickup under the crosshair](../tasks/112-held-aimed-pickup.md). The fresh-press requirement was an overstrict interpretation; the user explicitly wants held pickup on directly hovered finds. This record retains the earlier implementation/evidence.

- Why: a powerful revealing stroke was followed by automatic held-input collection, hiding the discovery before the player could appreciate it.
- Result: MainGame requires a fresh bound Dig-button press on an eligible visible find. Revealing, falling, held/toggled digging, changing aim and freeing bag space cannot auto-collect. Pickup transfers one identity without digging/fuel and consumes toggle intent.
- Preserved: 60% bottle exposure, actual line of sight/3 m reach, physical release/rest, full-bag refusal and menu/focus/rebind barriers. Updated target prompt, Controls and Pause reference reflect deliberate pickup.
- Evidence: **136/136 EditMode, 16/16 discovery and 18/18 UI/input** checks. Actual device tests exercise weak/strong shovels, remapped toggle, detached/resting loot and separate presses for successive bottles. [Results and live captures](../../../unity/Logs/Task111/).
- Live review: a bottle dropped 42 cm, settled and stayed visible under repeated held-action calls. A deliberate press added it once without fuel/terrain changes. Inspected the pickup HUD and Controls hint through the official CLI; the isolated MainGame review did not open the user's save profile.
- Windows: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), **2026-09-10 12:24 UTC**, zero errors; one existing editor Pipeline runtime-configuration warning.
- Remaining: user recognition/interaction feel in [101](../tasks/101-collection-return-playtest.md); trial art acceptance remains `09`/`105`. No fixed recognition delay is introduced. [Contract](../../features/backlog/discovery-collection.md), [task](../tasks/111-deliberate-discovery-pickup.md).
