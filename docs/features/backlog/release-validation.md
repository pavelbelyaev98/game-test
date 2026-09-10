# Full-game release qualification

Status: **development targets selected by Task `63` under the user's delegated technical judgment; qualification remains unimplemented**. Task `54` measures the finished game after content/progression, `37`, `53`, `22` and its other prerequisites.

Design: [63 - Windows support and performance budgets](../../development/tasks/63-windows-targets-and-budgets.md) runs early, before larger site/content work. Final `54` qualification measures the accepted targets rather than choosing them at release time.

## Purpose and research

Verify the finished excavation game over its intended session length and supported Windows environment. Read [physical comfort](../../research/player-review-findings.md#physical-comfort) and [persistent investment](../../research/player-review-findings.md#persistent-investment). The supplied report's compatibility anecdotes are test prompts, not evidence that our game has those defects.

Camera comfort is implemented early by [65](../../development/tasks/65-camera-comfort-settings.md); release qualification rechecks its settings through final equipment, long sessions and Continue. Recheck [precision movement](precision-movement.md), [underground lighting](underground-lighting.md), basic HUD facts and the player-created routes preserved by cleanup. Save validation includes both periodic and transaction checkpoint interruption, with measured loss windows and consistent terrain/economy recovery.

## Windows support targets

The user delegated hardware/budget choices on 2026-09-09 and preferred final testing after the game is complete. Use these internal targets to guide affordable, smooth first-person digging at the modest visual complexity of their digging-a-hole game reference. Final minimum/recommended hardware and store claims await `54`; revise candidate hardware from measurements, including lowering requirements if supported. [Task `63`](../../development/tasks/63-windows-targets-and-budgets.md#evidence-and-tradeoffs) retains evidence and rationale.

| Tier | Representative test hardware | Intended experience |
|---|---|---|
| Baseline test candidates | Ryzen 5 3600 / Core i5-10400; GTX 1650 4 GB / RX 5500 XT 4 GB; 16 GB RAM; SATA SSD or faster | 1920×1080, sustained 60 fps with readable terrain, finds and passive lighting |
| Higher-resolution test candidates | Ryzen 5 5600 / Core i5-12400; RTX 3060 / RX 6600; 16 GB RAM; SSD | 2560×1440, same 60 fps budgets with approved presentation |
| Current development reference | Ryzen 7 9700X; RX 9060 XT 16 GB; nominal 64 GB RAM; NVMe SSD | Regression evidence only; cannot substitute for minimum hardware |

- Select native **Windows 11 x64**, on serviced consumer releases at qualification; record exact builds/drivers. As of review, test 24H2/25H2 while serviced and recheck at release. Windows 10 22H2 compatibility is deferred to an evidence-backed release review, without a support promise or an outstanding user question. [Microsoft lifecycle](https://learn.microsoft.com/en-us/lifecycle/products/windows-11-home-and-pro).
- Preserve keyboard/mouse, click-and-hold digging and release-before-resume. Qualify DX12 default plus DX11 fallback on AMD and NVIDIA; changing API order/backend needs evidence. Unity's broader [engine requirements](https://docs.unity3d.com/6000.6/Documentation/Manual/system-requirements.html#desktop) do not qualify this game's hardware or other platforms.
- Support the existing bordered, resizable window and maximize; startup fits the desktop. Supported aspects: **16:9 and 16:10**. Check client sizes 960×540 (readability floor), 1280×720, 1280×800, 1920×1080, 1920×1200 and 2560×1440; performance promises apply at each tier's stated resolution. Check desktop scaling at 100/150/200%.
- Keyboard/mouse rebinding and optional toggle digging are implemented by `78`; qualify them through final equipment. Borderless/exclusive fullscreen, ultrawide/4:3 guarantees, controller support, integrated-GPU/8 GB tiers, ARM/emulation, Linux/Deck and automatic benchmarking remain uncommitted.

## Release budgets

All numbers below are **selected engineering goals, still unmeasured on candidate hardware**, including late tools and long saves. They guide development now; final certification and optimization of the finished game belong to `54`. Test at native render scale with final approved presentation; optimize before reducing visibility or undoing player excavation.

| Measure | Acceptance target |
|---|---|
| Active gameplay frame time | Mean ≤16.7 ms, p95 ≤18 ms, p99 ≤25 ms; no game-owned frame >50 ms; frames >33.3 ms ≤0.1% |
| Accepted edit, including cleanup, discovery updates and collision rebuild | p95 ≤8 ms, p99 ≤12 ms, maximum ≤16.7 ms; cut frames also count in the whole-frame budget |
| Collision/interaction consistency | Updated visible terrain and traversable collision agree before player movement/targeting uses the committed cut; no stale floors, fall-through or delayed invisible obstructions |
| Save capture on main thread | p95 ≤5 ms, maximum ≤8 ms; its entire frame must meet the frame budget |
| Durable checkpoint / progress-loss window | Transaction request to durable completion ≤1 s; continuous healthy autosaving loses at most 11 s from oldest dirty change to completed checkpoint, including scheduling/writer latency |
| Normal save-and-quit / window close | Latest dirty state durably written and process exits within 3 s on healthy storage; failed storage retains readable retry/recovery, never a timed destructive exit |
| Launch / restore | Cold process launch to usable fresh game ≤10 s, or late-save Resume ready ≤15 s; warm relaunch ≤5 s / ≤10 s respectively, including collision/discoveries; no user dwell time counted |
| New population generation (`45`) | Bounded attempts and ≤5 s inside the fresh-launch allowance; exhaustion enters a readable retry/failure state and never accepts a broken population |
| Memory throughout 3 h + Continue/load stress | Player private committed bytes ≤2.5 GiB during play, peak ≤3.5 GiB including load/save; dedicated GPU allocation ≤3 GiB on the 4 GB tier; no exhaustion or growing retained resources across repeated same-save cycles |

Measure each gameplay case for ≥10 minutes after a declared 60-second warm-up, retaining all dig/save spikes; score first-use/startup separately, never discard them as outliers. Report sample counts, mean/p95/p99/max and slow-frame count per case. Use final non-development native builds for qualification and matched development profiles for attribution; Editor timings are diagnostic. [Task `54`](../../development/tasks/54-release-qualification.md#native-matrix-from-task-63) owns the executable test matrix and evidence protocol.

Budgets apply before `58` changes site scale and before `39`/`45`/`69` add material, generation or lighting cost. A later selected excavation source shares the same budgets and cleanup contract; the old 4 m stress brush is not an approved tool. Record a measured failing case and resolve the bottleneck in its owning task before expansion; `54` is the final gate, not the first performance review.

## Platform access and release gate

`55` maps achievement conditions to durable facts; `53` owns Steam integration. Later testing needs the real AppID, a licensed Steam test account, app/depot access through a Dev Comp or test package, a private test branch/build, and a Steamworks account with appropriate app permissions. Valve documents [test packages](https://partner.steamgames.com/doc/store/testing) and [Edit App Metadata / Publish App Changes permissions](https://partner.steamgames.com/doc/gettingstarted/managing_users). Access is unverified, not refused; no credentials, SDK installation or remote changes are needed for `63`.

Final qualification exercises Steam launch/overlay focus, offline earned-state persistence and online reconciliation under `55`/`53`; local saves work when the platform is unavailable. Publishing configuration/builds still requires specific authorization. `54` passes only with measured budgets, comfort/persistence cases and `22` admin exclusion in the delivered release build; missing hardware/platform evidence is an explicit qualification gap.

## Save portability decision

[91](../../development/tasks/91-save-portability-design.md) owns the proposed cross-device/Steam Cloud scope identified by the [Steam review audit](../../research/steam-review-audit/delivery-and-positioning.md#decide-save-portability-before-platform-delivery). No Cloud feature or multi-campaign flow is selected. Decide complete-save/account ownership, device-local preferences and conflict/recovery behavior before release. If selected, a separately numbered implementation must demonstrate integrated transfer before `54`; a defer decision must state the transfer procedure and accurate launch promise. Existing local persistence remains required independently.
