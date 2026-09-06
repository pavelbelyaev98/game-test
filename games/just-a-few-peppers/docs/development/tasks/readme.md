# Numbered implementation tasks

**This is the chronological feature queue for this game.** Each linked file is a concrete implementation brief with context, dependencies, scope, acceptance criteria, and something the developer can inspect. Use the [new-chat prompt](../new-chat-prompt.md) to select the next task or a specific ID.

The leading digit matches the milestone: `0_xx` planning, `1_xx` first physical batch loop, `2_xx` earned equipment choices, `3_xx` saving, `4_xx` open-yard production and powered machinery, `5_xx` representative presentation, `6_xx` full content, `7_xx` menus/comfort, `8_xx` hardening, and `9_xx` release preparation. The suffix orders work within the milestone.

There are **34 concrete tasks**. 1_01–1_05 provide the interim crate-to-food loop; 1_06 adds portable props, new 1_07 tests physical pepper batches, and new 1_08 adds direct machine operation. 2_01 implements Coins and two working purchases; 2_02 compares/tunes them before the 2_03 human gate. Existing IDs and historical filenames stay stable even where the old title mentions discovery, automatic processing or storage.

The [current food/machinery/Coins decision](../../design-pivot.md#food-machinery-and-coins--september-6-2026) supersedes discovery-only progression, mandatory blocked yard pockets and the distant-final-intake hauling contract. Food is the objective; the mostly accessible yard is the setting. Follow [core mechanics](../../core-loop-and-mechanics.md), [scope](../../scope-and-validation.md) and [state/budget ownership](../state-and-saving.md). Coins and two meaningful prototype purchases are explicitly authorized, not a pending future comparison. Lyutenitsa/rakia remain explicit future activity decisions after the pepper loop is proven.

The [subsequent targeted refinement](../../design-pivot.md#snap-installation-and-restrained-comic-variety--september-6-2026) gives existing 2_01 ownership of one complete assisted-loading snap installation, clear full-price offer cards and recoverable paid kit state. 2_02–2_03 separate one-time fitting from recurring gains; M3 saves the lifecycle and later presentation tasks cover restrained comic variety. No task is added or renumbered, and this documentation work changes no delivery/feedback state.

The [finished-batch refinement](../../design-pivot.md#finished-batches-and-visible-accumulation--september-6-2026) strengthens existing 1_04's recognizable receiving/handoff and nearby graybox food accumulation, with automatic packing and predictable careful placement. 5_01–5_03 own polished food and household views; 2_03/5_05 assess transfer preference, food appeal and upgrade motivation separately. Existing purchase count, one snap installation and task statuses remain intact.

**NEXT: 1_04.** Preserve the delivered 1_02 free handling with its quiet placement presentation and the 1_03 automatic backend; 1_08 owns its later direct-operation extension. See the task rows and delivery records for current evidence and supplied human feedback.

## Context for every new chat

Start with [AGENTS.md](../../../../../AGENTS.md), this queue, the selected brief **in full**, and its dependencies' delivery/feedback records. Inspect the relevant source/scenes and pinned editor/package files. Read linked specifications for the behavior being changed, and if anything is unclear, resolve it from the linked design and implementation specs before editing.

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
- Otherwise take the earliest Todo whose predecessors are technically complete. Ordinary predecessor status Ready for human playtest or Done satisfies technical dependency, unless its record identifies a blocker.
- A request for the next task permits continuing after an ordinary Ready for human playtest handoff; it does not mean the human playtester played or accepted it. Keep Not tested feedback honest.
- At review gates **2_03, 5_05, 8_03, and 9_03**, finish the technical preparation, then record the required human evidence before advancing. A pending/failed gate remains the next task; do not invent ratings or treat elapsed time/a generic next-task prompt as approval. The revised 2_03 gate includes earning, both meaningful purchase choices and repeated use of their effects; missing Coins/purchase evidence cannot be deferred until production.
- For an explicit task ID, check its dependencies and earlier gates first. Do not silently implement several prerequisite features or a later milestone; report the concrete prerequisite and resume the earliest unfinished one when the request is NEXT.
- If every remaining task is already technically ready and only feedback is missing, present that handoff and the specific missing feedback. Do not rerun passing checks or rebuild merely because this is a fresh chat.

## What to do with work requests that are not already in the queue

If a request is required and not represented by a task:

- Pause implementation and define one short follow-up task in `games/just-a-few-peppers/docs/development/tasks/`.
- Add it as a concrete ID in this queue with `Todo` / `Not tested` and explicit dependencies (usually the current milestone predecessor and `AGENTS.md`/spec alignment).
- Continue the current task in the same handoff when possible; advance to the new task only when it is explicitly selected.

Do not implement unqueued scope changes directly during a task handoff. A concrete task entry protects the one-task-per-chat rule and preserves reproducible selection order.

Task IDs are stable. If play reveals a needed change, revise the relevant brief or add a clearly ordered follow-up and record why; do not silently renumber completed work. This maps the current complete scope, not every possible future bug or new idea.

## Ordered task queue

This table is the authoritative per-task status. Milestone progress in [status](../status.md) is a summary, not a second feature checklist.

| ID | Deliverable | Kind | Delivery | Human playtester feedback | Evidence |
| --- | --- | --- | --- | --- | --- |
| [0_01](0_01_repository-and-design-baseline.md) | Repository and design baseline | Planning | Done | N/A | [Planning evidence](../status.md#existing-implementation-and-evidence) |
| [1_01](1_01_unity-foundation-and-walkable-scene.md) | Unity foundation and walkable scene | Feature | Ready for human playtest | Not tested | [Movement revision](1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026) · [Earlier feedback](1_01_unity-foundation-and-walkable-scene.md#human-playtester-feedback--september-6-2026) |
| [1_02](1_02_scooping-and-crate-carrying.md) | Scooping and freely placed crate | Feature | Ready for human playtest | Not tested | [Quiet-placement delivery and positive handling feedback](1_02_scooping-and-crate-carrying.md#quiet-placement-delivery-record--september-6-2026); Not tested applies only to this latest presentation revision |
| [1_03](1_03_tipping-and-automatic-processing.md) | Tipping and automatic processing | Feature | Ready for human playtest | Not tested | [Delivery record](1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026) |
| [1_04](1_04_finished-carrier-and-storage-rack.md) | Finished carrier and handoff rack | Feature | Todo | Not tested | — |
| [1_05](1_05_first-playable-comfort-and-handoff.md) | First playable comfort and handoff | Milestone handoff | Todo | Not tested | — |
| [1_06](1_06_loose-yard-objects-and-playful-handling.md) | Loose yard objects and playful handling | Feature | Todo | Not tested | [Planning origin](1_06_loose-yard-objects-and-playful-handling.md#planning-record--september-6-2026) |
| [1_07](1_07_physical-pepper-batch-comparison.md) | Physical pepper batch comparison | Feature | Todo | Not tested | [Planning origin](1_07_physical-pepper-batch-comparison.md#planning-record--september-6-2026) |
| [1_08](1_08_direct-machine-operation.md) | Direct machine operation | Feature | Todo | Not tested | [Planning origin](1_08_direct-machine-operation.md#planning-record--september-6-2026) |
| [2_01](2_01_wheelbarrow-discovery-and-loader.md) | Coins and two equipment improvements | Feature | Todo | Not tested | — |
| [2_02](2_02_upgrade-throughput-and-handling.md) | Upgrade throughput and handling | Feature | Todo | Not tested | — |
| [2_03](2_03_core-feel-playtest-gate.md) | Core feel playtest gate | Review gate | Todo | Not tested | — |
| [3_01](3_01_snapshots-and-in-scene-restore.md) | Snapshots and in-scene restore | Feature | Todo | Not tested | — |
| [3_02](3_02_local-save-and-continue.md) | Local save and continue | Feature | Todo | Not tested | — |
| [3_03](3_03_save-failure-recovery.md) | Save failure recovery | Milestone handoff | Todo | Not tested | — |
| [4_01](4_01_connected-graybox-yard.md) | Open graybox yard and work area | Feature | Todo | Not tested | — |
| [4_02](4_02_final-processor-and-upgrade-order.md) | Powered processor and purchase combinations | Feature | Todo | Not tested | — |
| [4_03](4_03_harvest-completion-and-ending-state.md) | Harvest completion and completed-yard state | Milestone handoff | Todo | Not tested | — |
| [5_01](5_01_representative-assets-and-yard-section.md) | Representative assets and yard section | Feature | Todo | Not tested | — |
| [5_02](5_02_handling-and-machine-presentation.md) | Handling and machine presentation | Feature | Todo | Not tested | — |
| [5_03](5_03_winter-food-and-family-displays.md) | Winter food and family displays | Feature | Todo | Not tested | — |
| [5_04](5_04_grandpa-and-meal-transition.md) | Grandpa and optional closing presentation | Feature | Todo | Not tested | — |
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

Delivery values: **Todo** (not started), **Partial** (remaining work/blocker recorded), **Ready for human playtest** (technical criteria met and integrated handoff available; human feedback pending), **Done** (task acceptance met, including required review evidence). Feedback values: **Not tested**, **Needs revision**, **Accepted to continue**, or **N/A** for document/tooling evidence that needs no player judgement. Never label unplayed work Accepted.

For ordinary tasks, Ready for human playtest can remain in the table while later tasks proceed at the developer's request. A milestone becomes complete only when its stated exit gate is met; completion of a few child tasks does not complete the whole milestone. In particular, external/smaller-sample playtest decisions follow the existing scope contract, not a fabricated automatic pass.

## Handoff and recording

Follow [AGENTS.md](../../../../../AGENTS.md) for implementation and verification requirements, then:

1. Append a dated **Delivery record** to the task: delivered behavior, actual scene/build and changed paths, verified commands/results, decisions, limitations, and remaining acceptance work. Add human playtester feedback only when supplied.
2. Update this task's row. Update [milestone status](../status.md) only if its aggregate state/blocker changes; keep detailed evidence in the task. Update affected behavior contracts, asset imports, and regressions as needed. Entry pages should link to the authoritative record rather than repeat changing status.
3. Hand off the exact playable scene/build, controls, a 3–5 item check, verification results, limitations, and next task ID. Stop after this task unless a larger range was requested.

If required tooling/access is unavailable, finish independent work and mark the delivery Partial with the exact missing action. Do not substitute an untested scene or routine Inspector chores for a usable handoff. Ordinary playtest builds are the default; development diagnostics use a separate folder under the [build guide](../testing-and-performance.md#verified-foundation-commands).

## Coverage of the current game

| Design area | Tasks that deliver it |
| --- | --- |
| Editor/input foundation and reproducible scene | 1_01 |
| Local pile depletion, raw handling, conserved transfers | 1_02–1_04 |
| Free placement, rotation, dropping, physical loose props, stacking and playful tossing | 1_02 revision, 1_04, 1_06; wheelbarrow in 2_01 |
| Automatic batches, output capacity/reservation, Finished Food Handoff Rack | 1_03–1_04 |
| Basic comfort, reset/recovery, first packaged loop | 1_01–1_05 |
| Physical pepper representation and direct apparatus operation | 1_07–1_08 |
| Coins, two purchases, one complete attachment snap, supported capacity/wheelbarrow, comparison and human decision | 2_01–2_03 |
| Food/Coins/purchase/kit/mechanism snapshots, arranged poses, disk saves and recovery | 3_01–3_03 |
| Mostly open work area, powered operation/whole-job gain, purchase combinations and food completion | 4_01–4_03 |
| Free assets, tactile presentation, food displays, Grandpa and optional closing flourish | 5_01–5_05 |
| Full compact property, selected dialogue and measured pacing | 6_01–6_02 |
| Menus, bindings, camera/audio/display options and remaining-work guidance | 7_01–7_03 |
| Full-game reliability, profiling, clean candidate and player review | 8_01–8_03 |
| Product identity, save-location implications, credits, media and final package | 9_01–9_03 |

There is no hidden household-game backlog. The Coins equipment budget is included; customers, sales management, recurring expenses, sorting, peeling chores, parcel allocation, workers and farming are not. Lyutenitsa/rakia are future scoped activity decisions, not current implementation tasks or permanently banned ideas. Shipping tasks prepare reviewable local artifacts; publishing or contacting others requires authorization for that action.
