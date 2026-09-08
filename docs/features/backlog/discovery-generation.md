# Discovery generation

Status: Task `27` implements basic seeded development placement; weighted content, materials and related-item clusters remain planned.

Idea coverage: sections 12-15 and relevant tuning in section 53.

## Purpose

Populate each excavation with a mix of ordinary finds, memorable objects, clusters, and increasingly strange discoveries.

## Implementation task

Create reproducible discovery placement using weighted pools, loose depth influence, material/location rules, and optional clusters. Keep generation separate from reveal and collection.

Task `27`: `DiscoveryField.Generate` independently seeds 96 positions and rotations with 0.9 m minimum center separation and boundary clearance. The first 24 are shallow near the entrance, six concentrated near the player's approach; others span the site/depth. Three user-requested simple prefabs make excavation/collection visible now. Reproducibility, distribution and clearance are checked across multiple seeds; [current collection contract](discovery-collection.md#task-27---buried-finds-and-admin-x-ray).

## Required behavior

- Depth influences probability but never makes sideways exploration worthless.
- Plan roughly 20-30 reusable ordinary find types and 30-50 distinctive types; the exact content list is a separate content task.
- Broad tendencies move from recent household finds toward machinery, bones, fossils, and stranger objects, but ranges overlap in both directions.
- Seeds reproduce placement for debugging and tests.
- Each playthrough draws from the intended distinctive-item pool while positions, sensible depth ranges, rotations, and surroundings vary.
- Related bones, vehicle parts, household objects, or machinery fragments may cluster and naturally encourage lateral exploration.
- Distinctive finds are rarer without forcing a fixed sequence.

## Done when

- The same seed reproduces the same valid placement.
- Placement avoids boundaries and invalid overlaps.
- Distribution checks cover pools, depth influence, and clusters without testing random-library internals.
