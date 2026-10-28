---
type: Module
title: src/CleanArchitecture.Domain/Tutorials
description: The Tutorial entity — a standalone published/unpublished article with creation and update timestamps.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Tutorials
tags: [domain, tutorials, entity]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# `Tutorial` (`Tutorial.cs`)

| Member | Notes |
| --- | --- |
| `Title`, `Description`, `Published` | `private set`, mutated only via `Update` |
| `CreatedAt`, `UpdatedAt` | Both initialised to the `createdAt` argument |
| `Tutorial(title, description, published, createdAt, id = null)` | Generates an id when none is supplied |
| `Update(title, description, published, updatedAt)` | Replaces all three fields and advances `UpdatedAt`; `CreatedAt` is never touched |

A private parameterless constructor exists for EF Core.

# Notes

The simplest aggregate in the solution: no domain events, no invariants beyond the timestamp
discipline, and no relationship to [User](/modules/domain-users.md). `Update` has the highest
fan-in of any domain method in the graph (5) because every tutorial mutation path routes through
it rather than touching properties.

Persisted by `TutorialConfigurations` / `TutorialsRepository` in
[Persistence](/modules/infrastructure-persistence.md); driven by
[Tutorials use cases](/modules/application-tutorials.md).
