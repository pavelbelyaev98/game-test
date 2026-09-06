# Numbered implementation tasks

**This is the chronological feature queue for Just a few peppers.** Each linked file is a concrete implementation brief with context, dependencies, scope, acceptance criteria, and something Pavel can inspect. Use the [new-chat prompt](../new-chat-prompt.md) to select the next task or a specific ID.

The leading digit matches the existing milestone: `0_xx` planning, `1_xx` first crate loop, `2_xx` wheelbarrow, `3_xx` saving, `4_xx` whole graybox yard, `5_xx` representative presentation, `6_xx` full content, `7_xx` menus/comfort, `8_xx` hardening, and `9_xx` release preparation. The suffix is the order within that milestone.

There are **31 concrete tasks**, including the planning baseline. Every brief and the [feature coverage table](#coverage-of-the-current-game) remain available below. Tasks 1_01–1_05 together deliver M1; 1_01 alone is a walkable foundation.

## Context for every new chat

Start with [AGENTS.md](../../../../../AGENTS.md), this queue, the selected brief **in full**, and its dependencies' delivery/feedback records. Inspect the relevant source/scenes and pinned editor/package files. Read linked specifications for the behavior being changed; a link does not require recursively reading every linked document.

Use this route to load additional context only when it applies:

| Work being done | Read relevant sections |
| --- | --- |
| Determining intended behavior or changing feature scope | [Design index](../../readme.md) and [scope contract](../../scope-and-validation.md#scope-contract), then the brief's specific gameplay specifications. |
| Component ownership, scene composition, or cross-system changes | [Architecture](../../../ARCHITECTURE.md). |
| Quantities, transfers, upgrades, persistence, or reconstruction | [State/save contract](../state-and-saving.md), including its invariants. |
| Unity APIs/packages, assets, imports, or scene authoring | [Unity and asset policy](../unity-and-assets.md); consult official documentation matching the pinned versions. |
| Verification or packaged delivery | Relevant commands, coverage, and regressions in [testing](../testing-and-performance.md); read performance/release sections when the task affects them. |
| Milestone acceptance, review gates, or uncertain aggregate progress | [Milestone status](../status.md), that [roadmap](../roadmap.md) milestone, and the gate's brief/evidence. |
| Reusing Stage0 or resolving a discrepancy with earlier implementation | [Historical audit](../repository-audit.md), current code, and newer delivery records. |
| Cultural/art evidence | Research explicitly relevant to the selected task, reached through the [design index](../../readme.md). |

Do not routinely read the whole roadmap, research collection, old bootstrap, or every future acceptance case. Do not rerun Stage0 checks to recover context. If source and records disagree, record the discrepancy and use actual evidence; preserve user changes.

## How to select the next task

- Follow the numeric order shown below. First handle the earliest task marked Partial or with feedback Needs revision. Resume its remaining work instead of starting over.
- Otherwise take the earliest Todo whose predecessors are technically complete. Ordinary predecessor status Ready for Pavel or Done satisfies technical dependency, unless its record identifies a blocker.
- A request for the next task permits continuing after an ordinary Ready for Pavel handoff; it does not mean Pavel played or accepted it. Keep Not tested feedback honest.
- At review gates **2_03, 5_05, 8_03, and 9_03**, finish the technical preparation, then record the required human evidence before advancing. A pending/failed gate remains the next task; do not invent ratings or treat elapsed time/a generic next-task prompt as approval.
- For an explicit task ID, check its dependencies and earlier gates first. Do not silently implement several prerequisite features or a later milestone; report the concrete prerequisite and resume the earliest unfinished one when the request is NEXT.
- If every remaining task is already technically ready and only feedback is missing, present that handoff and the specific missing feedback. Do not rerun passing checks or rebuild merely because this is a fresh chat.

Task IDs are stable. If play reveals a needed change, revise the relevant brief or add a clearly ordered follow-up and record why; do not silently renumber completed work. This maps the current complete scope, not every possible future bug or new idea.

## Ordered task queue

This table is the authoritative per-task status. Milestone progress in [status](../status.md) is a summary, not a second feature checklist.

| ID | Deliverable | Kind | Delivery | Pavel feedback | Evidence |
| --- | --- | --- | --- | --- | --- |
| [0_01](0_01_repository-and-design-baseline.md) | Repository and design baseline | Planning | Done | N/A | [Planning evidence](../status.md#existing-implementation-and-evidence) |
| [1_01](1_01_unity-foundation-and-walkable-scene.md) | Unity foundation and walkable scene | Feature | Ready for Pavel | Not tested | [Movement revision](1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026) · [Earlier feedback](1_01_unity-foundation-and-walkable-scene.md#pavel-feedback--september-6-2026) |
| [1_02](1_02_scooping-and-crate-carrying.md) | Scooping and crate carrying | Feature | Todo | Not tested | — |
| [1_03](1_03_tipping-and-automatic-processing.md) | Tipping and automatic processing | Feature | Todo | Not tested | — |
| [1_04](1_04_finished-carrier-and-storage-rack.md) | Finished carrier and storage rack | Feature | Todo | Not tested | — |
| [1_05](1_05_first-playable-comfort-and-handoff.md) | First playable comfort and handoff | Milestone handoff | Todo | Not tested | — |
| [2_01](2_01_wheelbarrow-discovery-and-loader.md) | Wheelbarrow discovery and loader | Feature | Todo | Not tested | — |
| [2_02](2_02_upgrade-throughput-and-handling.md) | Upgrade throughput and handling | Feature | Todo | Not tested | — |
| [2_03](2_03_core-feel-playtest-gate.md) | Core feel playtest gate | Review gate | Todo | Not tested | — |
| [3_01](3_01_snapshots-and-in-scene-restore.md) | Snapshots and in-scene restore | Feature | Todo | Not tested | — |
| [3_02](3_02_local-save-and-continue.md) | Local save and continue | Feature | Todo | Not tested | — |
| [3_03](3_03_save-failure-recovery.md) | Save failure recovery | Milestone handoff | Todo | Not tested | — |
| [4_01](4_01_connected-graybox-yard.md) | Connected graybox yard | Feature | Todo | Not tested | — |
| [4_02](4_02_final-processor-and-upgrade-order.md) | Final processor and upgrade order | Feature | Todo | Not tested | — |
| [4_03](4_03_harvest-completion-and-ending-state.md) | Harvest completion and ending state | Milestone handoff | Todo | Not tested | — |
| [5_01](5_01_representative-assets-and-yard-section.md) | Representative assets and yard section | Feature | Todo | Not tested | — |
| [5_02](5_02_handling-and-machine-presentation.md) | Handling and machine presentation | Feature | Todo | Not tested | — |
| [5_03](5_03_winter-food-and-family-displays.md) | Winter food and family displays | Feature | Todo | Not tested | — |
| [5_04](5_04_grandpa-and-meal-transition.md) | Grandpa and meal transition | Feature | Todo | Not tested | — |
| [5_05](5_05_representative-slice-playtest-gate.md) | Representative slice playtest gate | Review gate | Todo | Not tested | — |
| [6_01](6_01_finish-the-compact-property.md) | Finish the compact property | Feature | Todo | Not tested | — |
| [6_02](6_02_campaign-pacing-and-dialogue-pass.md) | Campaign pacing and dialogue pass | Milestone handoff | Todo | Not tested | — |
| [7_01](7_01_new-continue-and-exit-flow.md) | New continue and exit flow | Feature | Todo | Not tested | — |
| [7_02](7_02_input-and-camera-options.md) | Input and camera options | Feature | Todo | Not tested | — |
| [7_03](7_03_audio-display-and-guidance.md) | Audio display and guidance | Milestone handoff | Todo | Not tested | — |
| [8_01](8_01_full-game-reliability.md) | Full game reliability | Feature | Todo | Not tested | — |
| [8_02](8_02_performance-and-build-cleanup.md) | Performance and build cleanup | Feature | Todo | Not tested | — |
| [8_03](8_03_release-candidate-rehearsal.md) | Release candidate rehearsal | Review gate | Todo | Not tested | — |
| [9_01](9_01_product-identity-and-credits.md) | Product identity and credits | Feature | Todo | Not tested | — |
| [9_02](9_02_release-materials-and-player-guide.md) | Release materials and player guide | Preparation | Todo | Not tested | — |
| [9_03](9_03_final-distributable-and-release-handoff.md) | Final distributable and release handoff | Review gate | Todo | Not tested | — |

Delivery values: **Todo** (not started), **Partial** (remaining work/blocker recorded), **Ready for Pavel** (technical criteria met and integrated handoff available; human feedback pending), **Done** (task acceptance met, including required review evidence). Feedback values: **Not tested**, **Needs revision**, **Accepted to continue**, or **N/A** for document/tooling evidence that needs no player judgement. Never label unplayed work Accepted.

For ordinary tasks, Ready for Pavel can remain in the table while later tasks proceed at the user's request. A milestone becomes complete only when its stated exit gate is met; completion of a few child tasks does not complete the whole milestone. In particular, external/smaller-sample playtest decisions follow the existing scope contract, not a fabricated automatic pass.

## Handoff and recording

Follow [AGENTS.md](../../../../../AGENTS.md) for implementation and verification requirements, then:

1. Append a dated **Delivery record** to the task: delivered behavior, actual scene/build and changed paths, verified commands/results, decisions, limitations, and remaining acceptance work. Add Pavel feedback only when supplied.
2. Update this task's row. Update [milestone status](../status.md) only if its aggregate state/blocker changes; keep detailed evidence in the task. Update affected behavior contracts, asset imports, and regressions as needed. Entry pages should link to the authoritative record rather than repeat changing status.
3. Hand off the exact playable scene/build, controls, a 3–5 item check, verification results, limitations, and next task ID. Stop after this task unless a larger range was requested.

If required tooling/access is unavailable, finish independent work and mark the delivery Partial with the exact missing action. Do not substitute an untested scene or routine Inspector chores for a usable handoff. Ordinary playtest builds are the default; development diagnostics use a separate folder under the [build guide](../testing-and-performance.md#verified-foundation-commands).

## Coverage of the current game

| Design area | Tasks that deliver it |
| --- | --- |
| Editor/input foundation and reproducible scene | 1_01 |
| Local pile depletion, raw handling, conserved transfers | 1_02–1_04 |
| Automatic batches, output capacity/reservation, one permanent handoff | 1_03–1_04 |
| Basic comfort, reset/recovery, first packaged loop | 1_01–1_05 |
| Equipment discovery, wheelbarrow, equal-work comparison and fun decision | 2_01–2_03 |
| Snapshot ownership, disk saves, settings separation and failure recovery | 3_01–3_03 |
| Finite yard, shortcuts, final station, alternate discovery order and finish | 4_01–4_03 |
| Free assets, tactile presentation, food displays, Grandpa and meal | 5_01–5_05 |
| Full compact property, selected dialogue and measured pacing | 6_01–6_02 |
| Menus, bindings, camera/audio/display options and remaining-work guidance | 7_01–7_03 |
| Full-game reliability, profiling, clean candidate and player review | 8_01–8_03 |
| Product identity, save-location implications, credits, media and final package | 9_01–9_03 |

There is no additional hidden household-game backlog. Recipes, sorting, manual peeling, jar-return chores, parcel allocation, economies, NPC workers, farming, and distillation remain excluded. Shipping tasks prepare reviewable local artifacts; publishing or contacting others requires authorization for that action.
