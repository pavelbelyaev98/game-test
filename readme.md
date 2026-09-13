# Something Down There

Source checkout requires **Git LFS** for editable art. Keep assets in their normal folders; run `git lfs install --local` and `git lfs pull` after cloning. See [large-file storage and adding assets](docs/development/large-files.md).

## Play

Launch [SomethingDownThere.exe](builds/windows/SomethingDownThere.exe). You do not need Unity to review the game.

If that file is missing, the latest playable build has not been produced yet. Any completed task that changes playable behavior must rebuild it and provide this path in its handoff.

## Continue development with AI

Use the prompt in [docs/development/ai-prompts.md](docs/development/ai-prompts.md). The AI reads [AGENTS.md](AGENTS.md), continues the active numeric task or selects the next ready one from [tasks.md](docs/development/tasks.md), implements it, validates it, and updates the build.

Current progress and limitations are in [status.md](docs/development/status.md). The full concept remains in [idea.md](docs/idea.md), but routine work starts from the linked feature document.
