# Next Chat Prompts

Depending on how you want to work, copy and paste one of the prompts below into a new chat session:

---

## Option A: Plan & Discuss First (Recommended for New Tasks)
*Use this when you want to review the architectural plan, discuss trade-offs, and approve changes before any code is modified.*

```markdown
Read AGENTS.md, docs/baseline.md, and docs/architecture.md to understand the current project state.

We are starting the next pending task in docs/tasks.md. 

PLAN ONLY — DO NOT EDIT CODE YET:
1. Read the referenced concept chapters in docs/concept/.
2. Inspect the relevant live code files in unity/Assets/Runtime/.
3. Create a thorough spec at docs/tasks/<id>-<slug>.md (Objective, live code analysis, exact architecture changes, edge cases, Acceptance Criteria).
4. Present a concise summary of your proposed technical approach, trade-offs, and any questions/decisions for me.
5. Wait for my review and approval before touching any code.
```

*(Once you discuss and approve the plan, simply reply: **"Plan approved, proceed with implementation."**)*

---

## Option B: Autonomous Execution
*Use this when you want the AI to plan, implement, and verify the task end-to-end in one shot.*

```markdown
Read AGENTS.md, docs/baseline.md, and docs/architecture.md to understand the current project state.

Check docs/tasks.md for the next pending task.

Follow the JIT workflow in AGENTS.md:
1. Read the referenced concept chapters in docs/concept/.
2. Inspect the relevant live code files in unity/Assets/Runtime/.
3. If anything is ambiguous or requires design decisions, ask questions before implementing.
4. Create a thorough spec at docs/tasks/<id>-<slug>.md (Objective, live code analysis, exact architecture changes, edge cases, Acceptance Criteria).
5. Implement the changes and run core tests/benchmarks if applicable.
6. Verify that the Windows build compiles warning-free at builds/windows/SomethingDownThere.exe (via Unity CLI if Editor is open, or ./tools/build-windows.ps1 if closed).
7. Move the spec from docs/tasks/<id>-<slug>.md to docs/tasks/completed/<id>-<slug>.md.
8. If a baseline system was refactored or replaced, update docs/baseline.md.
9. Mark [x] in docs/tasks.md and append a 1–2 sentence technical summary under ## Completed.
```

---

## Option C: Post-Playtest Feedback & Iteration
*Use this after you playtest a build and want to adjust numbers, feel, or mechanics without unnecessary chat chatter or document bloat.*

```markdown
I have playtested the latest build. Here is my feedback and required changes:
[Enter your feedback / changes here]

Follow Section 4 in AGENTS.md (keep responses and doc updates strictly minimal, no walls of text):
1. Update the relevant section in docs/concept/ in place to record the new design intent.
2. Apply C# code and balance changes in catalog.json or EquipmentProgression.cs.
3. Keep docs/baseline.md accurate if functionality changed.
4. Verify that the Windows build compiles at builds/windows/SomethingDownThere.exe.
5. Log a 1-sentence iteration note under ## Completed in docs/tasks.md.
6. Reply in under 3 lines confirming the updates.
```
