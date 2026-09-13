# Task 144 - Git LFS storage and rejected push repair

Type: implementation (repository maintenance). Status: `done`. [Completion](../completed/144-git-lfs-storage.md). Explicit user-selected GitHub repair and large-asset workflow.

Prerequisites: existing authored files and three unpublished commits. Context: [asset ownership](../../asset-ledger.md), [large-file workflow](../large-files.md). No gameplay behavior changes.

## Scope and decisions

- GitHub rejects `art/ground-textures/GroundTextures.blend` at 101.94 MiB. Preserve its editable content and all exported game textures.
- Keep files in their owning `art/` and `unity/Assets/` folders; `.gitattributes` routes source-art formats to Git LFS. No dedicated public LFS folder, asset moves or new art.
- Automatically track `.blend`, `.psd`, `.psb` and `.kra`; any other necessary binary at least 10 MiB must be explicitly tracked before commit. Small textures can remain ordinary Git files. Add a staged-file size check and concise contributor/agent instructions.
- Back up the original local tip. Rewrite only unpublished commits, keeping the published `origin/main` base and author/message order. Commit this repair and push normally; the user's request authorizes these Git operations for this task.

## Acceptance

- Full working-file hashes match before/after for assets and gameplay files; `.meta` identities and original folder layout stay intact.
- No outgoing ordinary Git blob reaches GitHub's 100 MiB limit; current source-art files are valid LFS pointers with local content and valid hashes.
- Check the size guard with a temporary staged large file and an LFS-tracked equivalent; do not import fixtures into Unity.
- LFS upload and normal `main` push succeed; verify remote tip and download a remote LFS object into an isolated cache.
- Update setup instructions, task/status and concise completion evidence. Retain the existing Windows build because gameplay bytes do not change.

Research: [GitHub LFS configuration](https://docs.github.com/en/repositories/working-with-files/managing-large-files/configuring-git-large-file-storage), [official migration reference](https://github.com/git-lfs/git-lfs/blob/main/docs/man/git-lfs-migrate.adoc). Explicit include/exclude refs limit rewriting to unpublished commits.

Evidence: all eight source files have valid LFS pointers/content; 882 original non-documentation files retain identical hashes. Largest outgoing ordinary blob is 6,232,074 bytes. Size guard rejects a regular 10 MiB fixture and accepts its LFS equivalent. Normal GitHub push and fresh-cache download of the original 106,892,679-byte source pass. [Validation](../../../unity/Logs/Task144/validation-summary.json).
