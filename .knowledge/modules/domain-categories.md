---
type: Module
title: src/CleanArchitecture.Domain/Categories
description: The Category entity — a named lookup value that tutorials may optionally point at.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Categories
tags: [domain, categories, entity]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# `Category` (`Category.cs`)

| Member | Notes |
| --- | --- |
| `Name` | `private set`; assigned once by the constructor, there is no rename method |
| `Category(name, id = null)` | Derives from `Entity`; generates an id when none is supplied |
| `Category()` | Private parameterless constructor for EF Core |

# Notes

* The entity is immutable after construction: no `Update`, no delete semantics and no domain
  events — the smallest aggregate in the solution, smaller even than
  [Tutorial](/modules/domain-tutorials.md).
* Nothing in `Category` knows about tutorials. The association is one-directional and lives on
  `Tutorial.CategoryId`, configured in `TutorialConfigurations` — see
  [Persistence](/modules/infrastructure-persistence.md).

Created and read by [Categories use cases](/modules/application-categories.md); persisted by
`CategoryConfigurations` / `CategoriesRepository` in
[Persistence](/modules/infrastructure-persistence.md).
