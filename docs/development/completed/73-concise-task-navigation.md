# Task 73 — Concise task navigation

- Why: a new chat needs a direct next-task link; the queue repeated task details and completed history.
- Result: `status.md` is the session entry point. `tasks.md` contains 45 short links in the original priority order; numeric IDs stay stable. Completed records and full task/feature/research contracts remain available.
- Startup guidance, handoff and reusable prompt now follow that routing; future completion removes the queue row and promotes the next eligible task.
- Evidence: all original unfinished IDs/order and completed records retained; local documentation links, task statuses and `git diff --check` pass.
- Next: Task `63`, Windows support and performance targets.
