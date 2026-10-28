---
type: Module
title: src/CleanArchitecture.Application/Tutorials
description: Anonymous CRUD use cases over the Tutorial aggregate, including bulk delete, title search and optional category assignment.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Tutorials
tags: [application, tutorials, cqrs, crud, categories]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Commands

| Folder | Types |
| --- | --- |
| `Commands/CreateTutorial` | `CreateTutorialCommand`, `CreateTutorialCommandHandler`, `CreateTutorialCommandValidator` |
| `Commands/UpdateTutorial` | `UpdateTutorialCommand`, `UpdateTutorialCommandHandler`, `UpdateTutorialCommandValidator` |
| `Commands/DeleteTutorial` | `DeleteTutorialCommand`, `DeleteTutorialCommandHandler` |
| `Commands/DeleteAllTutorials` | `DeleteAllTutorialsCommand`, `DeleteAllTutorialsCommandHandler` |

# Queries

| Folder | Types |
| --- | --- |
| `Queries/GetTutorial` | `GetTutorialQuery`, `GetTutorialQueryHandler` |
| `Queries/ListTutorials` | `ListTutorialsQuery`, `ListTutorialsQueryHandler` |

# Notes

* This feature is deliberately decoupled from users, subscriptions and authorization — the
  controller is `[AllowAnonymous]` and the handlers depend only on `ITutorialsRepository`,
  `ICategoriesRepository` and `IDateTimeProvider`.
* `CreateTutorialCommand` and `UpdateTutorialCommand` both carry an optional
  `Guid? CategoryId = null`. Their handlers call `ICategoriesRepository.ExistsAsync` first and
  return `Error.NotFound(description: "Category not found")` when the id is unknown; a `null`
  `CategoryId` skips the check entirely and leaves the tutorial unfiled. See
  [Categories use cases](/modules/application-categories.md).
* `ListTutorialsQuery` carries an optional `title` filter passed through to
  `ITutorialsRepository.ListAsync`.
* `DeleteAllTutorialsCommand` returns the number of rows removed, which the controller renders as
  a `MessageResponse`.
* `UpdateTutorialCommandHandler` loads the entity (404 if missing), validates the category, then
  calls `Tutorial.Update(title, description, published, categoryId, updatedAt)`, so `CreatedAt` is
  preserved while `UpdatedAt` advances. Because `Update` always writes `CategoryId`, omitting it
  from an update clears the tutorial's category rather than leaving it alone.

Backed by [Tutorial](/modules/domain-tutorials.md) and `TutorialsRepository` in
[Persistence](/modules/infrastructure-persistence.md); exposed by `TutorialsController` in
[Api](/modules/api.md).
