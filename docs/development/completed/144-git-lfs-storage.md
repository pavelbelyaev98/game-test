# 144 - Git LFS storage and push repair

- Why: GitHub rejected the 101.94 MiB ground-texture Blender source. The user asks to repair the push and document where future large assets belong.
- Result: eight Blender sources use LFS at their original paths. Root `.gitattributes` also covers PSD/PSB/KRA. Other required binaries at least 10 MiB must be explicitly tracked; small textures stay ordinary Git. [Contributor guide](../large-files.md) and AGENTS include the staged-file check.
- History: migrated three unpublished user commits plus the storage-setup commit, preserving authors/messages and the published base. Normal `main` push succeeded, uploading eight LFS objects / 179 MB; no force-push. Original local tip remains at `refs/backup/task-144-before-lfs`.
- Evidence: 882 original files outside the edited documentation retain identical hashes; all game textures, source art, scripts and `.meta` files remain intact. Largest outgoing ordinary blob is 6,232,074 bytes; LFS fsck passes. The 10 MiB guard rejects regular data and accepts its LFS pointer.
- Remote verification: fetched `GroundTextures.blend` into an empty cache; all 106,892,679 bytes match the original SHA-256. Temporary large validation copies removed. [Validation](../../../unity/Logs/Task144/validation-summary.json).
- Limitation: future clones require Git LFS; LFS storage/bandwidth follows the GitHub account allowance. Existing Windows build from `143` is retained because gameplay content is unchanged.
