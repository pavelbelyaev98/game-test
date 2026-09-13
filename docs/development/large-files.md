# Large files and Git LFS

Keep assets in their normal owning folders: editable sources in `art/`, game assets in `unity/Assets/`. There is no dedicated LFS folder. Git LFS stores file content separately and downloads it back to the same paths; never move files into `.git/lfs` manually.

## Rules

- Root `.gitattributes` automatically tracks `.blend`, `.psd`, `.psb` and `.kra` files with LFS, regardless of size.
- Any other required binary **10 MiB or larger** must be tracked with LFS before committing. Small PNG textures can stay in ordinary Git. Do not delete needed source art or textures to satisfy GitHub's limit.
- Code, Markdown, Unity scenes/prefabs/materials and `.meta` files remain ordinary Git text. Investigate unexpectedly large text instead of blindly putting it in LFS.
- Builds, Unity caches/logs, temporary captures and disposable exports stay ignored. LFS is for required source/assets, not generated clutter. Asset approval, licensing and ledger ownership still apply.
- GitHub rejects ordinary Git files above 100 MiB. A later deletion or tracking rule does not remove an oversized blob already present in earlier commits; repair only unpublished history unless a shared-history rewrite is explicitly approved.

## First setup or another computer

Install Git LFS alongside Git, then run these commands inside the repository:

```powershell
git lfs install --local
git lfs pull
```

Cloning with Git LFS installed downloads the real assets automatically. `git lfs pull` repairs a checkout containing small pointer files. Keep the LFS pre-push hook installed so normal Git/VS Code pushes upload the asset content. Prefer a Git clone over a GitHub source ZIP.

## Add or change assets

Source-art patterns above need only the usual `git add`, commit and push. For another large binary, track its **existing path** first:

```powershell
git lfs track --filename "unity/Assets/Content/Example/LargeTexture.png"
git add .gitattributes
git add "unity/Assets/Content/Example/LargeTexture.png"
powershell -NoProfile -File tools/check-large-files.ps1
git lfs status
```

If a previously tracked file is unchanged, use `git add --renormalize -- "path/to/file"` to replace its index entry with an LFS pointer. Stage its `.meta` normally. Commit the pointer and `.gitattributes` together. The check rejects any staged regular file at least 10 MiB; it does not inspect historical commits.

Run the size check before commits involving assets. LFS uploads consume the repository owner's GitHub LFS storage/bandwidth allowance; check usage before committing unusually large batches. [GitHub LFS setup](https://docs.github.com/en/repositories/working-with-files/managing-large-files/configuring-git-large-file-storage).
