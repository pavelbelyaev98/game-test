# Task 92 - Expand review evidence and preserve player-feel risks

Type: design/research; documentation only. Status: `done`. Explicitly selected follow-up to [90](90-steam-review-gap-audit.md); preserve next `89` and paused `56`/`68`. [Completion evidence](../completed/92-player-feel-risk-research.md).

Context: [review audit](../../research/steam-review-audit/report.md), [feature contracts](../../features/backlog.md), [concept](../../idea.md). Output: a compact [risk register](../design-risks.md), updated owning contracts and concrete future tasks where ownership is missing.

## Scope

- Collect and read another bounded batch from the same three games, excluding all previously collected review IDs. Use positive counterexamples and check historical/version-specific claims.
- Record each retained risk's intended feeling, observed failure signal, what to avoid, owning task and evidence needed to resolve it. Put detailed selected rules in the owning feature; the register is a short index, not another gameplay specification.
- Strengthen future design/delivery work, including focused update tasks for already implemented behavior when code/evidence exposes an actual missing contract. Preserve completed records and distinguish an observed defect from an untested risk.
- Simplify the review reading view to useful takeaways and source links. Keep deduplication/provenance separately, and avoid reloading metadata or repeated full review text during ordinary task work.

## Acceptance

- New IDs do not overlap the original 375; exact counts, sources and screening limits are recorded. Raw review volume is not treated as player-population prevalence.
- Risks and player-feel expectations are discoverable from status/summary and the affected feature/task; every retained actionable gap has an owner without duplicating existing scope.
- New substantial product choices receive concrete numbered design tasks with proposal deliverables, prerequisites, questions and acceptance. Proposals do not silently change accepted controls, penalties, optional systems or asset approvals.
- Validate dataset/citation links, task numbering/queue dependencies and concise current status. This task changes documentation/research, not playable behavior.
