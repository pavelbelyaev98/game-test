# Next Chat Prompt

Copy and paste the prompt below into a new chat session to continue development:

---

```markdown
Read AGENTS.md, docs/baseline.md, and docs/architecture.md to understand the current project state.

Check docs/tasks.md for the next pending task.

Follow the JIT workflow in AGENTS.md:
1. Read the referenced concept chapters in docs/concept/.
2. Inspect the relevant live code files in unity/Assets/Runtime/.
3. If anything is ambiguous or requires design decisions, ask questions before implementing.
4. Create a thorough, well-thought-out spec at docs/tasks/<id>-<slug>.md (Objective, live code analysis, exact architecture changes, edge cases, Acceptance Criteria).
5. Implement the changes and run core tests/benchmarks if applicable.
6. Verify that the Windows build compiles warning-free at builds/windows/SomethingDownThere.exe (via Unity CLI if Editor is open, or ./tools/build-windows.ps1 if closed).
7. Move the spec from docs/tasks/<id>-<slug>.md to docs/tasks/completed/<id>-<slug>.md.
8. If a baseline system was refactored or replaced, update docs/baseline.md.
9. Mark [x] in docs/tasks.md and append a 1–2 sentence technical summary under ## Completed.
```
