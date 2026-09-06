# 1_06 — Loose yard objects and playful handling

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Let the player arrange and play with a small set of loose yard objects using the established handling controls, then return naturally to pepper work.

**Current correction:** The [physical-handling revision delivery](#physical-handling-revision-delivery--september-6-2026) implements the [human feedback](#human-feedback-and-research-first-revision--september-6-2026) and [handling reference comparison](../handling-controls-research.md). Ordinary grabbing/releasing must feel physical; the earlier placement-first delivery below remains historical evidence. New human handling feedback is pending.

**Depends on:** [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md), including the completed [1_02 free-placement revision](1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) and [1_04 finished carrier](1_04_finished-carrier-and-storage-rack.md). Follow [AGENTS.md](../../../../../AGENTS.md) and the queue's earlier-gate rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), each dependency's delivery/feedback record, the [free handling contract](../../core-loop-and-mechanics.md#pick-up-place-and-play), [research decisions](../../design-pivot.md#free-handling-and-research-review--september-6-2026), [look/comfort](../../look-sound-and-comfort.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [asset](../unity-and-assets.md) / [verification](../testing-and-performance.md#free-handling-coverage) rules. Inspect the current Unity scene, shared handling/input, and pinned versions before implementation.

## Work

- Extend existing grab/rotate/place/drop behavior to a representative loose basin, stool, empty crate, and ball in the same prototype corner. These are a compact test sample, not a permanent one-toy cap or a full household art pass. Use primitives or suitable free assets with recorded licenses.
- Give them appropriate collision/gravity, stable placement and stacking where shape permits, and deliberate small-prop tossing. Make ordinary release from the hand physical without requiring a valid supported placement pose; do not force the ball to sleep on this action. Keep careful set-down easy for boxes/carriers as a secondary action; throwing must not be the only release method. Revise shared controls/target feedback with carriers and resolve overlapping targets clearly.
- Let objects remain where placed and be moved again, including temporary route clutter. Add recoverable identity/pose state and lost-object recovery without resetting food or the player's valid arrangements. Freeze physical motion/input during pause/focus loss. Disk persistence remains M3.
- Provide a little useful free space and a work surface for arranging things. Let a loose object be uncovered through ordinary pile clearing where convenient. No objectives, score, mandatory trick, prop collection, or waiting timer is needed; play remains optional while useful work can continue.
- Own scene/prefab wiring, input, interaction feedback, build configuration, and a focused ordinary Windows player handoff. Record initial physical-body count and observed frame behavior; reuse/sleep bodies instead of tying simulation cost to harvest size.

## Acceptance

- Pick up, rotate, carefully place, stack, drop and toss the sample objects through ordinary input. Comparable loose props give consistent grab feedback; installed fixtures do not promise a grab action.
- The usual grab/release interaction allows the ball to fall, move with a bounded moving release, roll on a slope, respond to later contact and fall when its support moves. Boxes can settle into useful stable arrangements. Tests must not require every prop to become motionless immediately after every release. Keep gathering/use/throw intentions distinct, with no accidental throw when a gathering press loses its target or when input resumes.
- Objects collide and settle without routine tunnelling, explosive stacks, held-object player launch, or duplicated identities. A moving object freezes across pause/focus and resumes safely. Recover an out-of-bounds/stuck object and move a blocking prop out of a route.
- A loaded raw or finished carrier can be set down among these objects, recovered/regrabbed, and used to finish its food transfer. Prop play neither changes harvest accounting nor becomes a completion requirement.
- Relevant scene/physics integration checks and the packaged player cover these cases. Record unsupported cases and human feel feedback honestly; screenshots or compilation alone cannot establish comfortable handling.

## Human playtest check

1. Grab the ball and use the ordinary release action above the ground and while moving; try a slope or contact with another prop. It should behave physically without hunting for a placement spot or separate drop-only key.
2. Lower a box onto the worktop, try a stable stack, deliberately toss a small prop, then clear clutter from your route. Optional rotation/careful placement must not dominate the basic interaction.
3. Leave a loaded carrier nearby, play briefly while processing runs, and return to collect/deposit food.
4. Pause during a fall, resume, and recover a deliberately inaccessible object without resetting the work.

Ask what felt naturally movable, what refused unexpectedly, and whether arranging objects was enjoyable. No extra reward is needed for the check.

**Outside this task:** Individual pepper inventories, simulated glass breakage, moving installed machinery, furniture construction, household quests, achievements, a prop under every pile, full-yard art, or disk saves.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Record the exact scene/build, bindings, focused checklist, checks, limitations, and real feedback; update the queue and M1 summary without inventing acceptance. Stop after the handoff.

Next in order: [1_07 — Physical pepper batch comparison](1_07_physical-pepper-batch-comparison.md).

## Planning record — September 6, 2026

Added by the developer's request for fewer handling restrictions and ideas from `research/case-studies`. This is the one new follow-up for loose-object play; the existing 1_02 brief owns the crate correction. Technical delivery is **Todo**, feedback **Not tested**. No Unity edits, imported assets, build, or gameplay verification were produced by this planning pass.

## Delivery record — September 6, 2026

Implemented this single queued task after applying the developer's [1_05 quiet-help feedback](1_05_first-playable-comfort-and-handoff.md#human-feedback-and-quiet-help-revision--september-6-2026). No later gameplay task was selected. Options were already planned in 7_02/7_03; their briefs now retain optional help and quiet gameplay.

### Artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity`, relative to `games/just-a-few-peppers/unity/`.
- **Ordinary Windows x64 Mono player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with all adjacent files. Development is off. Final build reports **98,318,111 bytes**; runtime DLL SHA-256 **`155B72C97794C24D6225C92D2087989D012DC94D56AF5B62D16B09C463024A18`**. No Inspector setup is needed.
- Four freely handled graybox props stand beside the existing worktop: blue basin, wooden stool, empty crate and ochre ball. Basin/crate have open compound collision, the stool has separate seat/legs, and the ball uses a sphere collider. Ground, the worktop and suitable object tops share placement rules. A basin on the stool with the ball inside is a tested arrangement; no named placement pad grants permission.
- E grabs or carefully places; Z/X optionally rotates; G drops; right mouse deliberately tosses a loose prop. Supported tops mean placement; side targets can switch holders after validating nearby clear space. Food carrier transfers retain their existing priorities and quantities. Right mouse does not throw food carriers. Held compound parts are triggers and cannot push the player; released bodies collide, fall and sleep.
- Each prop retains its stable authored identity, current/safe pose and held/released state in memory. R recovers held/moving/lost/invalid objects and preserves other valid supported arrangements and all food. Out-of-bounds recovery is automatic. F8 explicitly resets food after recovery, preserving valid prop arrangements. Prop play neither grants food nor becomes an objective; processing continues while playing.
- The persistent instruction footer is removed. F1 opens full controls, recovery/restart tools and optional current-work help with simulation paused. F1/Esc closes it to the prior play/pause mode; focus loss returns to the ordinary paused menu. Esc alone opens a compact menu with the existing session sensitivity. Only relevant target feedback and food/status text remain during play.

### Implementation and authoring

New runtime `LoosePropState`, `LooseProp` and `LoosePropHandling` use the existing `PortableBody`, `YardHandling`, `YardInput` and `YardSession` boundaries. Food-model source is unchanged. Compound clearance checks actual colliders after the bounded broad query, allowing another prop inside a hollow container; sphere sweeps match the ball. Existing carrier sweeps, exact food ownership and muted automation remain.

`QuietHelpAuthoring.Apply` and `LoosePropsAuthoring.Apply` are guarded editor migrations, exposed as `AuthorQuietHelp` and `AuthorLooseProps` in the wrapper. `TuneLooseBall` tunes ball/basin clearance and the recovery label. Normal tests/builds load the saved scene. All **2,806 previous serialized scene IDs** remain; **138** were added. All **57** prior input action/map/binding IDs remain, with **four** added IDs for Help/Toss and their bindings; all **107** pre-existing repository meta files retain their hashes. Only trailing scene whitespace was normalized, with identical serialized tokens. The [asset register](../asset-register.md) records primitive materials and reuse of the existing CC0 contact clip; no pack, purchase or production art was introduced.

Unity **6000.6.0f1**, Input System **1.20.0**, uGUI **2.6.0**, Test Framework **1.8.0**, Input System-only configuration and the single build scene remain pinned. Official references checked: [Rigidbody velocity](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Rigidbody-linearVelocity.html), [sphere collision](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SphereCollider.html), [nonalloc sphere sweeps](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.SphereCastNonAlloc.html), and [Input System action authoring](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputActionSetupExtensions.html). Editor launches used the documented working context outside the sandbox from the first command; no administrator launch, editor upgrade or licensing repair was needed.

### Verification

Commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 20/20 passed:** saved scene/input/help/prop wiring, six bodies/four unique IDs, retained asset/build references and food conservation rules.
- **Final PlayMode 33/33 passed:** actual F1 open/close from play/pause and focus behavior; retained sensitivity/food/movement checks; all four props rotated/placed/regrabbed on the worktop; basin/stool/ball stack; drop versus toss; contact/pause/focus/fresh-input behavior; held compound wall contact without player launch; inaccessible and blocked-safe-pose recovery preserving other arrangements; loaded raw/finished switching, processing during play and exact-once handoff. Complete 107-unit food checks remain. Approaches and some food preparation use deterministic setup/public commands, not human navigation.
- **Final ordinary Build and Smoke passed.** The exact player exercises F1/menu/sensitivity, pickup/rotation/worktop placement/regrab/toss for all four props, the hollow-basin stack, physical drop/focus freeze, lost recovery and raw/finished switches followed by an exact-once five-unit handoff. It then passes the retained scoop/processing/carrier checks and all 107 units through nine handoffs, including the final eleven. The complete-job section prepares loads through public gathering commands and uses ordinary transfer inputs/unaccelerated processing time. The probe waits for fresh input after recovery rather than bypassing the gameplay guard.
- The initial scene run passed 30/32: an older placeholder check assumed every target had a marker, and a new carrier regrab approached through the worktop. Prop targets now have their dedicated reachable-input checks; the regrab uses the validated original approach. A stronger interior-placement check then exposed a real rim obstruction. The sphere sweep and roomier basin solve it; the checked final pose rests on the interior base. Earlier failed XMLs are retained under `Logs/Task1_06-PlayMode-initial.xml` and `Task1_06-PlayMode-refined.xml`.
- The first package passed all new prop cases, then its transition to the retained crate test pressed E before recovery's fresh-input boundary had armed. The probe now waits for that boundary; gameplay input rules are unchanged. `Logs/Task1_06-Smoke-initial.txt` retains this failure. The final menu also labels recovery as objects, including props. Passing rules/PlayMode checks were not repeated for this label/probe-only correction; the rebuilt player exercises it.

Fresh 1440 x 900 ordinary-player captures were inspected for the compact pause menu, optional F1 reference, unobstructed gathering, all four worktop placements and the basin/stool/ball stack. Evidence is local/ignored: `Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, corresponding editor/build/smoke logs, `FoundationSmoke/result.txt`, `01-menu.png`, `01-f1-help.png`, `04-scoop.png`, `props-01-sample.png`, `props-02-stack.png` and `props-worktop-*.png`. Final runs have no C# compiler/deprecation warnings or unexpected gameplay exceptions. Documentation links/anchors, task order, asset/meta pairs and whitespace were checked at handoff.

**Physical/frame observation:** six persistent bodies total (four props and two food carriers); no bodies scale with harvest quantity. In the final five-second hidden batch-player ball/processing sample, at most **one** released body was awake and **five** released bodies were asleep at the end (the finished carrier was docked/kinematic). **124,977 update intervals**, median **0.04 ms**, p95 **0.06 ms**, max **0.94 ms** were observed on an AMD Ryzen 7 9700X, Radeon RX 9060 XT, 63,033 MB reported RAM, 1902 x 963 window, vSync 1 and targetFrameRate -1. Batch mode can skip rendering: these measure simulation/update cadence, **not rendered FPS or the 1080p/60 target**. No minimum-hardware or full allocation/profile pass is claimed. Exact local sample: `Logs/FoundationSmoke/loose-props-observation.txt`.

### Controls and human check

Enter/click Walk starts. WASD/arrows move, mouse looks, Shift sprints and Space jumps. E grabs/places/tips/collects/hands off. Hold left mouse scoops; release stops. Optional Z/X rotates, G drops and right mouse tosses loose props. F1 shows all controls/debug tools, Esc pauses, R returns/recovers with food kept, and F8 explicitly resets the food test.

1. Find the four props beside the left worktop. Rotate/place them, then stack the basin on the stool and put the ball inside.
2. Drop and toss a small prop, move the resulting clutter, and resume ordinary walking/work.
3. Leave a loaded raw or finished carrier nearby, play during processing, then regrab it and finish the handoff with the same food.
4. Pause/Alt-Tab during motion, resume, and use R on a lost/stuck prop. Confirm other supported arrangements and food remain.
5. Confirm normal play has no instruction footer; use F1 for the full reference and close it again.

**Limitations and feedback:** temporary primitive art and reused contact audio; optional rotation remains provisional. Conservative support/approach checks can reject narrow, uneven or moving surfaces; R can return toppled/unsupported props to a clear safe pose. Physics play is bounded, not a guarantee for every contrived stack. Bulk food remains in carriers on toppling; individual peppers/spills belong to 1_07. No buried-prop objective, direct operation/grouping (1_08), Coins/purchases (M2), disk saves (M3), or expanded options menu (M7) was added. Automation is muted; audible quality, physical OS focus/cursor behavior, rendered frame performance and human handling enjoyment remain unverified. The developer accepted the prior 1_05 checkpoint overall; these new props and quiet presentation are **Not tested** by the human.

**Next task: 1_07 — Physical pepper batch comparison.** Stop after this handoff.

## Human feedback and research-first revision — September 6, 2026

After trying the delivered interaction, the developer reported that it feels like rigid picking/placing rather than grabbing, holding and releasing physical objects. The ball remaining still is a concrete example; calmer box placement may still be useful. They requested similarities to How to Make an Atomic Bomb in Your Garden, Schedule I, Crime Scene Cleaner and Recycling Center Simulator, and explicitly asked for research first. They suggested LMB use/RMB grab but also allowed E interaction/LMB gathering/G drop; no exact replacement mapping or hold/toggle gesture was settled. This is negative handling feedback, not acceptance of 1_06 or a request to start 1_07.

The [research note](../handling-controls-research.md) records sources, evidence limits, inspected code, proposed controls and revised behavior checks. Shared release currently zeros velocity and sleeps carefully placed objects, including the ball; existing automation checks this arranged result separately from drop/toss. Prior technical passes remain valid evidence of that implementation and do not override the supplied feel feedback.

**Remaining work:** implement the revised ordinary grab/release path and coherent controls, retain useful careful set-down and optional tossing, update scene/input/help and affected regressions, verify the changed ordinary player and obtain new human handling feedback. Keep exact carrier food/recovery, quiet F1 guidance and optional rotation. Resume existing assets/components rather than recreating the sample. The research pass implements no runtime change and produces no revised build.

**Research verification:** documentation checks passed for 60 Markdown files, 817 local links, 312 heading targets, balanced fences, 80 asset/meta pairs and all 34 queue IDs in unchanged order. Whitespace checks passed. A before/after hash audit found only four existing documentation files changed and the research note added; the other 324 existing repository files were unchanged, including Unity source, scene, assets, input and project configuration. No Unity/editor/player tests were run for this documentation-only pass, and the packaged artifact was not rebuilt.

**Research handoff:** the unchanged scene/build and current bindings remain in the earlier artifact/controls sections. The revised human checklist above is for the future correction, not a passing result in that build. **Next work: 1_06 revision**; 1_07 follows after its technical delivery under the queue rules.

## Physical-handling revision delivery — September 6, 2026

The developer's subsequent “looks better now implement next task” authorizes implementing the researched direction. The queue's Partial/Needs revision rule selects this existing 1_06 correction; no 1_07 work was started. This feedback concerns the proposed direction, not a playtest of the revised artifact. Technical delivery is **Ready for human playtest**; feedback on this build is **Not tested**.

### Revised artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity`, relative to `games/just-a-few-peppers/unity/`.
- **Ordinary Windows x64 Mono player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with its adjacent files. Development is off; no Inspector setup is needed. Final build reports **98,326,911 bytes**. Runtime DLL SHA-256: **`8DEF3D499A947B0A7BEA8B498943F5BA7B62454DAA366F8D32C8488B85911E18`**.
- Right click grabs with empty hands or releases the held prop/raw/finished carrier at its current reachable hand pose. G duplicates this ordinary release. No supported placement target is required, and aiming at another object never silently parks or swaps the current holder.
- Ordinary release enters dynamic physics with bounded carry motion, capped at 4.5 m/s. The ball falls, rolls on the new board, responds to later contact and falls when its support is removed. E remains secondary careful set-down with zero initial carry velocity and natural physical settling; it no longer explicitly sleeps the ball. Initialization/recovery can still sleep reconstructed safe arrangements.
- LMB holds gathering while carrying the raw crate. With a loose prop, hold to charge for up to 0.8 seconds, then release to throw from the hand. Strength ranges from 1.5 m/s to the existing per-prop maximum; combined carry/throw velocity is capped at 7 m/s. Intent is latched on a fresh press, and pause/focus/recovery clears charge and stale carry motion. Food carriers have no charged throw.
- E keeps explicit tipping, collecting and handoff. Release/set down the raw crate before collecting finished food; occupied-hand collection preserves every food owner and explains the needed release. RMB/G release takes priority over simultaneous use/throw release. Existing exact quantities, processing, recovery and automatic empty finished-carrier return remain.
- The four existing graybox props and worktop remain. A static wooden board at **(-6, 0.22, 1.6)**, tilted 15 degrees, provides a reachable slope clear of the pepper pile. It reuses the existing material and adds no physical body or imported asset. Normal play retains hidden placement indicators and no instruction footer; F1 supplies the updated complete reference.

### Changed implementation and preservation

Shared runtime changes are in `PortableBody`, `YardHandling`, `YardInput`, `LoosePropHandling`, `RawCarrierView`, `FinishedCarrierView`, `FinishedFoodHandling` and `YardHud`. `FoundationBuildSmoke`/`LoosePropBuildSmoke` and the relevant EditMode/PlayMode suites use the revised ordinary input. `PhysicalHandlingAuthoring.Apply` updates the existing input/help and adds the board through editor APIs; `MoveBoard` records its corrected location. Wrapper modes are `AuthorPhysicalHandling` and `MovePlayBoard`; normal tests/builds load the saved scene without migrations.

All **2,944** previous serialized scene IDs remain, with **five** added. All **61** input map/action/binding IDs and all **118** prior repository meta-file hashes remain unchanged. Scoop/Toss action names become Use/Grab while their LMB/RMB bindings and IDs are retained. Only trailing scene whitespace was normalized, with identical serialized tokens. The food-model source, editor/package pins, project settings and single build entry are unchanged. Current behavior contracts, player guide, asset register and regression coverage now describe the revision; older delivery records remain historical.

Official references consulted against Unity **6000.6.0f1** and Input System **1.20.0**: [Rigidbody velocity](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Rigidbody-linearVelocity.html), [input press/release semantics](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputAction.html) and [action renaming](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputActionSetupExtensions.html). The installed package's Rename implementation was checked for ID preservation. Editor launches used the documented context outside the restricted sandbox from the first attempt. No administrator launch, licensing repair, editor upgrade or package change was needed; existing token-refresh notices did not prevent licensed runs.

### Revision verification

The same wrapper commands shown in the earlier record were run for EditMode, PlayMode, Build and Smoke:

- **EditMode 20/20 passed:** saved scene/input/help wiring, board clear of every pile region, six bodies/four prop identities and retained food-state invariants.
- **PlayMode 36/36 passed:** ordinary ball fall without supported placement, bounded moving release, board rolling, removed stool support, real prop contact moving a resting ball, gather/throw intent separation, simultaneous release priority and charge cancellation. Existing quiet guidance, movement, all four worktop placements, hollow-basin stack, held wall contact, pause/focus, inaccessible-object recovery and full-food conservation checks pass. Finished-food checks now require explicit set-down/release rather than automatic hand switching.
- **Ordinary Build and Smoke passed on the artifact above:** actual RMB/LMB/E handling for all four props; ball fall, carry momentum, slope, removed support and later contact; charge cancellation and held RMB across pause; stack, focus freeze, recovery and loaded-carrier preservation followed by an exact-once five-unit handoff. Retained gathering/processing checks pass, followed by **all 107 units through nine handoffs**, including the last eleven. Batch preparation uses public gathering commands; transfer inputs and processing time are real and unaccelerated. Deterministic approaches/fixture setup do not establish human navigation or comfort.
- Initial integration failures were resolved, not suppressed: one missing probe parameter caused the first compile failure; obsolete F1 wording assertions were updated; the board's first position overlapped the pile and was moved through editor authoring. Recovery correctly restored the former stool/ball stack, so the separate impact test now first drops the ball on clear ground. Initial PlayMode results were 32/36, then 35/36; the final run is 36/36. Initial EditMode was 19/20 before its old help assertion was corrected.
- The first package probe aimed at the hollow crate's thin base beside the worktop. It now aims at the visible body and checks the reachable target before regrabbing. A second run completed all new physics cases but carried impact clutter into an independent charge/pause case; fixtures are now reset between those cases. No targeting/physics rule was weakened. These probe-only corrections were rebuilt and exercised in the final package; already passing gameplay suites were not repeated.

Fresh local evidence: `Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, the matching editor/build/smoke logs, `FoundationSmoke/result.txt` and `loose-props-observation.txt`. Failed attempts are retained as `Task1_06-physical-compile-initial.log`, `Task1_06-physical-PlayMode-initial.xml`, `Task1_06-physical-PlayMode-board-fixed.xml`, `Task1_06-physical-EditMode-initial.xml` and `Task1_06-physical-Smoke-initial/impact.txt` with their smoke logs. Final runs contain no C# compiler/deprecation warnings or unexpected gameplay exceptions.

The final 1440 × 900 F1, gathering and basin/stool/ball captures were inspected: help fits, the aimed pile remains visible, the ball sits inside the basin and ordinary play has no placement indicator or instruction footer. The board capture has poor downward framing; the recorded position/contact assertions establish its physical test, not that image. Screenshots alone do not prove handling feel.

**Physical/frame observation:** six reusable bodies total, maximum **one** awake during the five-second hidden batch-player ball/processing sample, **four** released bodies asleep at its end. **136,100** update intervals: median **0.03 ms**, p95 **0.04 ms**, max **2.56 ms**. Hardware/configuration: AMD Ryzen 7 9700X, Radeon RX 9060 XT, 63,033 MB reported RAM, 1902 × 963 window, vSync 1, targetFrameRate -1. Batch mode can skip rendering; this measures simulation/update cadence, **not rendered FPS or the 1080p/60 target**. No full allocation/profile or minimum-hardware claim is made.

### Revised controls and human check

Enter/click Walk starts; WASD/arrows move, mouse looks, Shift sprints and Space jumps. **RMB grabs/releases; LMB holds gathering or charges a held prop's throw, released to throw; E tips/collects/hands off or carefully sets down.** G duplicates release; Z/X optionally rotates. F1 opens the complete controls/debug reference, Esc pauses, R returns/recovers while keeping food, and F8 explicitly resets the food test.

1. Grab the ball, release it above the ground and while walking, then drop it onto the wooden slope beside the prop area. Check whether grabbing/releasing feels natural.
2. Charge/release a throw, try E set-down on the worktop, and stack the basin on the stool with the ball inside. Remove a support and clear the resulting clutter.
3. Fill and tip the raw crate, release it, play briefly, then collect finished food with E. Release/regrab the finished carrier and hand it off with the same load.
4. Pause or Alt-Tab during motion/charge, resume and confirm no surprise throw. Use R to recover an inaccessible object without losing food or valid arrangements.
5. Confirm the normal view stays quiet; open/close F1 when you need the full reference.

**Limitations / feedback:** primitive art and existing contact audio; conservative collision/reach checks can require aiming higher or approaching around clutter. E set-down still rejects steep/narrow/obstructed supports, while ordinary release is available clear of collisions. Contrived stacks are not guaranteed stable. R can restore a formerly safe stack or return unsupported/toppled props to a fallback. Individual peppers/spills and measured pepper representation remain 1_07; direct machine operation/output grouping remain 1_08. No disk saves or new settings system was added. Automation is muted; audible quality, physical OS focus/cursor behavior, rendered performance and human control clarity/enjoyment remain unverified. **Human feedback on this revised build: Not tested.**

**Next task: 1_07 — Physical pepper batch comparison.** Stop after this handoff.
