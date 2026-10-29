# Correction a8e86da2e5ea

**Target:** backend

**Instruction:**

Rules recorded in the memory above are this project's own and still apply: where this change touches the same thing (the same field, form, format or limit), follow them even though this request does not repeat them, and say in your summary which ones you applied.

Add a GET /api/health endpoint that returns { "status": "ok" }, and show "API: ok" in the page footer using it.

---

This branch was forked to isolate this one correction. sage-maintenance-agent's CI workflow commits its result on top of this note once it runs; see the PR's checks and commits for progress.
