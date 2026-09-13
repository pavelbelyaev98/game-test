# Return, rescue and ending

Owns automatic rescue, return warnings, falls/return economy, the ending and same-save achievements. Task IDs are plain numbers.

## Automatic rescue (`86` implemented)
- Reaching zero battery automatically returns the player to the clear surface anchor and refills, replacing the old manual confirmation. No pause rescue button.
- The accepted final stroke/thrust completes first. Carried ordinary finds and up to 10 credits (clamped to balance) are lost; feedback states the actual loss.
- A blocked landing keeps items/credits and retries with clear feedback. Pause/focus/save barriers defer rescue until gameplay resumes.
- Excavation, owned equipment and collected identities remain; lost finds never respawn. Permanent rewards (`36`) and ending components (`52`) are protected.
- Depletion, falls, rescue and recovery must never deadlock, including with an empty wallet or bag.

## Return warning (`13` implemented; `140` selected; `93`/`94` open)
- Bands at 35% / 15% labelled SAFE / RISKY / CRITICAL; they do not measure return effort.
- Bottom-centre LOW FUEL / FUEL CRITICAL warnings use the same thresholds, hide with the HUD and never flash or play sound.
- `93`/`94`: approximate return-difficulty wording that acknowledges unknown lateral/obstructed routes; no route solving or guaranteed escape.

## Falls and fee (`59` design, `50` implementation)
- Minor falls do nothing important; large falls may stun or drain; only extreme cases rescue.
- The ≤10-credit fee is an unproven baseline. No hidden debt trap, and rescue must never become optimal transport. Rescue stays viable when broke or empty-bagged.

## Return aids (`70` deferred/gated, `99` proposed)
- Baseline: return is physical; the jetpack helps but does not replace route planning; stations sit near the exit.
- `70` may add a coarse HOME bearing only (no route/path/GPS/exact distance) and evaluates a conditional placed marker/flag/light. Minimap, waypoint chains, through-wall treasure markers and lamp platforms are rejected. Inclusion and paid access are uncommitted; `15`/`37` measure whether return time is meaningful risk or commuting.
- `99` compares a reusable placed teleporter and `98` refill options against demonstrated needs; normal physical return and free refill stay the baseline. No shortcuts are authorized.

## Ending (`52`, design `61`)
- Planned after the production trip, content/economy work and display; verify the no-passive ending before `36`. Tuning follows in `37`.
- The final trigger cannot be accidental or permanently unreachable; interrupted reveal recovers. Tone moves from believable early finds through suspicious middle to unmistakably wrong late finds.
- The playable ending uses normal upgraded equipment under normal rules. Rejected: stripping upgrades, stealth/combat/puzzle gameplay, requiring a rare passive discovery.
- The protagonist seeks money and ends famous for an impossible human-made discovery; the exact object stays undecided until the game works. Optional 3–5 non-blocking buyer/headline moments may foreshadow; no voice acting, dialogue tree, mandatory reading or subtitle spam. A few protected keys/components may lead to the reveal without an inventory puzzle.
- No 100% terrain, max-all, stockpiling or specific optional item is required; generated placement, optional-find luck and selling must not make the ending unreachable. Continue restores the same equipment, upgrades and excavation through persistent state.

## Achievements (`53`, design `55`)
- Planned after `52`/`51`/`37`; no platform implementation exists.
- Goals mostly reward naturally desirable discoveries, purchases and exploration; no every-voxel or fragile trick objectives. Audit what remains earnable after selling, rescue, the ending and updates. Check Steamworks access before choosing integration.

## Open
- `61` final object and foreshadowing; `93` wording; `59`/`50` fee; `70` inclusion/access; `55` achievement set and integration.
