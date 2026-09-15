# Research — Evidence Layer

The Steam review corpora behind the game's design. **Evidence only: never edit the game design
from here.**

| Game | Analysis | Raw data | Reviews |
|---|---|---|---|
| A Game About Digging a Hole | `A_GAME_ABOUT_DIGGING_A_HOLE_REVIEW_ANALYSIS.md` | `research_data/a_game_about_digging_a_hole_reviews_raw.json` (6.7 MB) | 20,098 |
| Keep Digging | `KEEP_DIGGING_REVIEW_ANALYSIS.md` | `research_data/keep_digging_reviews_raw.json` (1.2 MB) | 3,266 |
| Meltopia | `MELTOPIA_REVIEW_ANALYSIS.md` | `research_data/meltopia_reviews_raw.json` (1 MB) | 1,722 |
| One Man's Trash | `ONE_MANS_TRASH_REVIEW_ANALYSIS.md` | `research_data/one_mans_trash_reviews_raw.json` (0.2 MB) | 330 |

## Notes

- Review records carry only review-relevant fields, one review per line: `language`, `voted_up`, `votes_up`, `timestamp_created`, `playtime_at_review`, `received_for_free`, `primarily_steam_deck`, `review`, plus `developer_response` when the developer replied. All other scrape fields (IDs, profile/avatar, `app_release_date`, `hardware`, `reactions`, vote scores, purchase/refund/early-access flags) were dropped.
- Paths written inside the analyses (e.g. `research_data/...`) are relative to this folder.
- The game itself is described in the [concept chapters](../NEW_GAME_CONCEPT/CONCEPT/00_README.md).
  Remaining choices are listed in [Open Questions](../NEW_GAME_CONCEPT/CONCEPT/13_OPEN_QUESTIONS.md).
