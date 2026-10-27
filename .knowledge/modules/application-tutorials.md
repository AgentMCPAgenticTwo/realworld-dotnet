---
type: Module
title: src/CleanArchitecture.Application/Tutorials
description: Anonymous CRUD use cases over the Tutorial aggregate, including bulk delete and title search.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Tutorials
tags: [application, tutorials, cqrs, crud]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
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

* This feature is deliberately decoupled from users, subscriptions and authorization — handlers
  depend only on `ITutorialsRepository`, and the controller is `[AllowAnonymous]`.
* `ListTutorialsQuery` carries an optional `title` filter passed through to
  `ITutorialsRepository.ListAsync`.
* `DeleteAllTutorialsCommand` returns the number of rows removed, which the controller renders as
  a `MessageResponse`.
* `UpdateTutorialCommandHandler` loads the entity and calls `Tutorial.Update(...)`, so `CreatedAt`
  is preserved while `UpdatedAt` advances.

Backed by [Tutorial](/modules/domain-tutorials.md) and `TutorialsRepository` in
[Persistence](/modules/infrastructure-persistence.md); exposed by `TutorialsController` in
[Api](/modules/api.md).
