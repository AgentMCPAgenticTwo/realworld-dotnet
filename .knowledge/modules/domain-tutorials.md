---
type: Module
title: src/CleanArchitecture.Domain/Tutorials
description: The Tutorial entity — a published/unpublished article with timestamps and an optional category reference.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Tutorials
tags: [domain, tutorials, entity, categories]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# `Tutorial` (`Tutorial.cs`)

| Member | Notes |
| --- | --- |
| `Title`, `Description`, `Published` | `private set`, mutated only via `Update` |
| `CategoryId` | `Guid?`, `private set`; a bare foreign key, not a navigation property |
| `CreatedAt`, `UpdatedAt` | Both initialised to the `createdAt` argument |
| `Tutorial(title, description, published, createdAt, categoryId = null, id = null)` | Generates an id when none is supplied; `categoryId` and `id` are both optional and in that order |
| `Update(title, description, published, categoryId, updatedAt)` | Replaces all four fields and advances `UpdatedAt`; `CreatedAt` is never touched. `categoryId` is required here, so passing `null` clears the category |

A private parameterless constructor exists for EF Core.

# Notes

No domain events and no invariants beyond the timestamp discipline, and no relationship to
[User](/modules/domain-users.md). The one association it does carry is to
[Category](/modules/domain-categories.md), and the domain does not enforce it: `CategoryId` is an
unchecked `Guid?` here, with existence validated in the application handlers and referential
integrity enforced by the `SetNull` foreign key in `TutorialConfigurations`. `Update` has the
highest fan-in of any domain method in the graph (5) because every tutorial mutation path routes
through it rather than touching properties.

Persisted by `TutorialConfigurations` / `TutorialsRepository` in
[Persistence](/modules/infrastructure-persistence.md); driven by
[Tutorials use cases](/modules/application-tutorials.md).
