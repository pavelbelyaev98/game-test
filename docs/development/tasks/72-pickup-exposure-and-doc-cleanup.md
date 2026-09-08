# Task 72 — Lower pickup exposure and remove documentation noise

Type: implementation. Status: `done`. Prerequisites: `35` (complete); user-selected tuning. [Completion](../completed/72-pickup-exposure-and-doc-cleanup.md).

Feature: [discovery collection](../../features/backlog/discovery-collection.md). [Queue](../tasks.md).

- Request: require 40% uncovering instead of 50%, retaining held collection, visibility, reach and capacity rules.
- Apply to the three existing find prefabs, runtime default and repeatable scene setup. Existing saves use the current prefab threshold without resetting terrain or find identities.
- Remove redundant reports that tasks did not add assets. Keep actual asset ownership, approval, license, integration, save compatibility and removal information; record this documentation preference in repository guidance.
- Validation: existing discovery placement/collection checks, live MainGame exposure and held pickup, then rebuild the Windows executable.
