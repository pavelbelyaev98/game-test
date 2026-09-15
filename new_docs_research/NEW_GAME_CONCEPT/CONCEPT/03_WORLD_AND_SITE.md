# 03 — World and Site

## 1. The site

A drained river-fed reservoir. The working floor sits inside visible former banks, concrete
retaining walls, and leftover waterworks. It is finite, believable and clearly bounded.

- **Dimensions:** depth at least 100 m; exact depth and footprint are set by feel in playtest.
  Depth is the progression axis; the footprint stays contained, with useful lateral room for branches.
- **Surface:** authored, not procedurally generated into ugliness. A small worksite yard
 (see [Surface Hub and Display](07_SURFACE_HUB_AND_DISPLAY.md)).
- **Underground:** fully diggable voxel ground except permanent boundaries.
- **No pre-existing caves or tunnels:** every opening in the ground is one the player made.

- **Buried structures:** authored walls, machinery and filled interiors are allowed. The
 player digs every opening; no pre-dug rooms or passage network.
- **Buried history:** workshop, household and waterworks finds belong together. Major parts are
  connected: in the current experimental direction they are pieces of one huge buried structure the
  player keeps finding — a curved wall in one place, another piece deeper down, pieces that only
  later read as the same object. Another option being considered is parts collected to open
  something, possibly at ground level. What the parts add up to is still an open question.

## 2. Boundaries (why you cannot dig forever)

Permanent boundaries must look categorically different from any diggable material:

- **Sides:** concrete retaining walls, dam infrastructure, steel pilings — industrial, cracked,
 obviously not soil.
- **Bottom:** solid bedrock shelf.
- **One edge:** natural bedrock shelf. No water, no swimming, no flooding — the drained
  reservoir's edge reads as ground meeting stone, with no fake-water interaction problems.

Rule: never use the same material look for "tough but diggable" and "eternal wall". Players must
know at a glance what will eventually yield. If the buried-structure direction is used, it is never
a boundary: its built surfaces stay clearly different from bedrock and concrete walls, and routes
around it stay open.

## 3. The four zones

Each zone changes ground, palette, typical finds and mood. Transitions are gradual; there are no
loading screens or separate levels.

| # | Zone | Ground | Finds typical | Mood |
|---|---|---|---|---|
| 1 | **Recent fill** | Loose soil, gravel, roots, modern rubbish | Bottles, scrap, household junk, common ore | Bright, familiar, hopeful |
| 2 | **Old sediment** | Compacted river sediment, clay lenses | Old tools, machinery parts, first fossils, better ore | Nostalgic, slightly odd |
| 3 | **Deep clay / stone** | Hard clay, rock, occasional concrete | Larger machines, rare ore, deliberate objects | Heavy, dim, purposeful |
| 4 | **Ancient constructed** | Unknown compacted material, ancient fabrication | Impossibilities, final components, the final object | Cold, quiet, wrong in a good way |

Zone names are placeholders; final naming is content work.

## 4. Materials

Working set (exact list TBD): prototype five response groups — loose earth, clay/sediment,
gravel, rock and diggable concrete. Soil, sand and harder variants can look different within these
groups. Each family differs in **behavior**, not just color:

- sand pours and spills quickly;
- clay sticks and clumps;
- gravel trickles;
- compact sediment resists evenly;
- rock chips and cracks;
- concrete sparks and resists, but the starting tool always makes visible progress.

The tool adapts automatically to the material (see [Tool and Movement](04_TOOL_AND_MOVEMENT.md)); materials reward the
right behavior but never lock it out. These are cutting responses and visual debris, not a global
collapse hazard. Power growth outpaces tougher ground over the campaign.

**Dig along the seam**: some ground has visible cracks or material boundaries. Cutting broadly
along one frees a larger local section with less work than digging through its center. For example, follow
a clay seam around a rock section and break that section away. The ground offers a small choice:
"where would a cut do the most?"

- Use the same tool and normal digging input; broad, readable cuts along a seam reward the player
  without requiring a pixel-perfect or fully traced perimeter.
- **Physical payoff:** cutting along a seam triggers distinct feedback — a sharp stress crack, a subtle
  physical shift of the worked slab, and a heavy fracturing break as the section gives way.
- Digging straight through always works. Seams offer an optional efficiency gain from the start;
  stronger upgrades make the resulting cuts larger and more satisfying.
- **Normal cleanup rules apply:** plain dirt crumbs vanish, embedded valuables remain in place without
  bonus duplicates, and interesting finds survive intact for deliberate partial exposure and recognition.
  Breaking a slab never creates extra loot or bypasses recognition.
- Removal stays local to the worked section. Unrelated ledges, tunnels, and overhangs remain stable;
  this does not add a collapse hazard.

## 5. Tough ground: hard pockets

A small number of memorable, optional obstacles (5–8 target) — never walls across the main descent:

| Example | Feel | Behind it |
|---|---|---|
| Concrete plug | Slow but visible progress with early tools | A waterworks alcove with a rare part |
| River-rock lens | Dense boulder cluster | A complete fossil |
| Compacted gravel shelf | Slows digging for a while | An older, richer pocket of finds |

Every pocket has **multiple solutions**: the current tool, C4, or routing. Upgrades make excavation
much faster. Discovering one early and demolishing it later is a designed moment of power.

Solutions must be **legible before commitment**: the player can see that a pocket has an answer before
sinking time into it — distinct seams, cracks or fittings that read as C4-friendly, a material
clearly unlike the eternal boundaries, and a tool that visibly chips even the tough ground. "Come back
with more power" is an optional shortcut, never the only answer. An undiscoverable solution is the same
as no solution.

## 6. Terrain technology and cleanup

- **Full voxel**: every diggable cube can be removed; tunnels, overhangs and trenches are legal. Chunk size prototype-tuned.
- **No floating specks**: disconnected valuables become visible pickups and collect if the bag has
  space; plain dirt crumbs vanish. Full-bag overflow persists nearby without blocking movement.
- **Interesting finds survive cleanup:** terrain removal and C4 never delete them or bypass deliberate
  collection.
- **Debris is visual only**: particles never collide and never deal damage.
- **Collision always matches the visible mesh.**
- **Substantial structures survive**: ledges, tunnels and overhangs the player built are preserved;
 only unsupported crumbs are cleaned.
- **Progress never resets**: the terrain edit history is saved; loading restores exactly the hole.

## 7. Lighting and darkness

- **Sky light reaches down open shafts** and fades with depth.
- **The shaft reads from below:** its light column and drifting dust are landmarks where the shaft
  is visible. Light does not pass through overhangs; the jetpack and reusable lamps support returns.
  No map, ever.
- **A dim ambient floor** exists everywhere — ground and object shapes always remain readable.
- **The dark arrives gradually.** Zone lighting lowers slowly; ambient light preserves recognition.
  Lamps improve detail, color and atmosphere without becoming a requirement to see the ground.
- **Placeable lamps** improve visibility, mark familiar places and light finds for photos. Owned
 lamps are reusable, repositionable and do not expire or drain charge; lost support leaves them
 recoverable nearby. Digging and C4 cannot destroy them.
- **No personal light.** The tool does not act as a headlamp; darkness in covered areas is a reason
 to place lamps.
- **Zone lighting moods:** warm daylight near the surface → cool dim mid-depths → quiet, dim deep
 ground whose silhouettes remain readable.

## 8. Randomization rules

- Authored: zone layout, depth ranges, boundary placement, general difficulty curve and relationships
  between buried places and the connected major finds.
- Randomized per save: find positions, depths within bands, rotations, cluster layouts, some
  surrounding junk. Variation preserves how related objects and major parts fit together.
- The generator produces a candidate layout and validates [discovery pacing](02_CORE_LOOP.md#4-pacing-rules-generation-enforces-these)
 before accepting it.
- Every seed contains all special exhibits, ending parts and achievement-relevant finds, reachable
 and discoverable with baseline equipment.
- The accepted population is finite and persisted; patches never reroll an existing save.

## 9. No hazards

No lava, gas, oxygen, hunger, earthquakes, temperature damage, or monsters. The only pressure
is the shared battery, the bag's capacity, and the player's own greed — all soft, all fair, all
recoverable (see [Progression and Economy](06_PROGRESSION_AND_ECONOMY.md)).
