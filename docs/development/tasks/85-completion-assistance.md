# Task 85 - Conditional discovery completion assistance

Type: implementation. Status: `planned`; conditional on inclusion in `84`. Prerequisites: `84`, `49`, `51`, `52`.

Feature: [detector](../../features/backlog/detector.md). [Queue](../tasks.md).

## Scope and research

- Implement only the assistance policy reviewed in `84`, using durable discovery/photo/completion state and the existing detector/settings path. Retain ordinary detector behavior outside its selected activation condition.
- Extend the common save boundary for any new durable state; preserve existing discoveries, sold records, excavation and ending/Continue flags. Presentation uses approved assets and the independent detector volume from `10`.
- Inspect actual late saves and target-selection ownership before implementation. Preserve the selected physical eligibility, quiet gaps, single-stream behavior and disclosure exclusions at increased range.

## Acceptance and questions

- Exercise few remaining finds, duplicates, photographed/sold/rescue-lost finds, no eligible targets, near-boundary targets and interrupted save/reload. No collected find respawns or false guaranteed-completion signal appears.
- Compare matched late/Continue searches with assistance on/off; record reduced unproductive search without exact GPS, forced cleanup or new spoilers. Test muting and overlap at maximum range.
- Deliver the Windows build and evidence for `37`/`54`. Return policy ambiguities to `84`; if its decision is omit/defer, retire this implementation reservation instead of treating it as selected scope.
