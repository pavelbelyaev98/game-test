# Testing, performance, and verification

Status: planned v4 checks and historical baseline evidence. [Implementation status](status.md) records delivered results. Exact new-scene test/build commands will be recorded when M1 installs and verifies its tooling.

## Check the changed work

Documentation-only work needs relevant link and consistency checks. Do not launch Unity, regenerate Stage0, or run gameplay tests for ordinary document edits. The old prototype is disposable and has no standing regression gate for v4.

For new gameplay, run focused rules and integration checks for the changed behavior. Check the player build when the milestone changes packaged behavior. Once appropriate checks pass, repeat or broaden only for changed code/content, a new failure, or an unresolved concern. Preserve verified commands/results in this document as new tooling is implemented.

The AI handles technical verification and gives Pavel an integrated scene/build with a short play checklist. Pavel judges responsiveness, clarity, repetition, and enjoyment. Record what was actually exercised and what remains untested; compile success alone is not a playable handoff. See [Start here](start-here.md).

## Historical baseline

The old standalone Stage0 harness passed **10/10** after relocation on September 5, 2026 using .NET SDK 9.0.314. It tests roast/steam/peel rules, not bulk gameplay. This is retained history, not an instruction to rerun it. Its old scene builder/probe can regenerate Stage0 art and must not be used on new authored content.

## V4 automated checks to add

The current manifest has no Unity Test Framework package or test assemblies. M1 must add a version compatible with the recorded editor, verify it in this project, and record exact EditMode, PlayMode, and new-scene build commands here. Do not describe an uninstalled suite or hypothetical command as passing.

| Layer | Meaningful coverage |
| --- | --- |
| EditMode / pure rules | Conservation, carrier limits, partial transfers, output reservation/accumulation, exactly-once deposits, monotonic upgrades, completion, and content IDs. Add snapshot validation/round trips in M3. |
| PlayMode | Actual targets and component wiring, scoop/tip commits, pause/focus, carrier recovery, output pickup/deposit, discovery, and later save reconstruction/ending. |
| Packaged Windows build | New scene starts, input works, a load finishes, pause/resume works, saves persist when introduced, and the complete game reaches its ending. |
| Human observation | Responsiveness, local depletion continuity, sound, comfort, useful upgrade gain, navigation, and desire to continue. |

Use deterministic tests for quantities and state. Avoid brittle pixel comparisons or tests that merely repeat a private method. Add regression coverage for significant observed failures. A source compile does not establish targeting, scene wiring, or fun.

## Gates before content production

M2 must meet the current [scope/feel gates](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery), with results and sample limits recorded. Compare equal quantities before and after the wheelbarrow and count the entire workflow.

Before expanding the representative slice in M5 into full content:

- The ordinary loop and upgrade have acceptable play observations; no unresolved progression blockers.
- Current EditMode and PlayMode checks pass, including save/reload where relevant.
- A player build exercises the intended scene and the short arc through its ending.
- Pausing, partial final batches, output accumulation, safe recovery, and skipped visual milestones work.
- Representative assets and pile density have a measured performance record.
- The tested build, hardware, content configuration, and known limitations are recorded in status.

Run checks appropriate to the change. After they pass, repeat or broaden only for a new failure, changed behavior, or an unresolved concern.

## Performance targets and measurement

Initial target: smooth 60 FPS at 1920×1080 on a recorded development/test PC. This is a planning target, not an established minimum specification. Record CPU, GPU, memory, resolution, build settings, and representative pile/cascade conditions before claiming a pass or setting shipping requirements.

Start with at most 64 simultaneously moving decorative pepper proxies as a tunable guardrail, independent of total harvest. They carry zero authoritative food state. Profile the largest local pile, a full wheelbarrow dump, machine output, and food-display update. If this visual budget cannot sell the action, compare a revised representation before increasing the pool.

Capture frame timing and allocation behavior during repeated gathering and dumping, plus startup/save/load times once those exist. Investigate sustained frame times above the target, repeatable spikes, per-action memory growth, and save stalls. Prefer bounded effects, authored depletion, and reusable assets before engine-scale optimization. Define tighter measured budgets only when a real problem needs them.

## Regression records

No v4 bugs or preventing tests exist yet. Record important failures here as they occur: observed failure; root cause; violated contract; regression test name/path; affected system; status and fixing change. Reproduce with a failing test first when practical, and retain it afterward.

Investigate unexpected errors from the current game/editor run. Do not carry over exception filters from the old Stage0 probe or suppress unrelated errors to report a pass.

## Release-candidate check

Test the exact candidate artifact on a clean user-data path: launch, new game, save, exit, continue, controls/settings, full completion, and relaunch after the ending. Check supported display/input configurations, backup recovery, credits/licenses, and runtime logs. Record the artifact version and unresolved issues. The selected distribution channel's current requirements must be checked when that release task is performed.
