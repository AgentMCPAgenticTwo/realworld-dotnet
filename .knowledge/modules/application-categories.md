---
type: Module
title: src/CleanArchitecture.Application/Categories
description: Anonymous create and list use cases over the Category aggregate, plus the existence check tutorials rely on.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Categories
tags: [application, categories, cqrs]
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
| `Commands/CreateCategory` | `CreateCategoryCommand(string Name)`, `CreateCategoryCommandHandler`, `CreateCategoryCommandValidator` |

`CreateCategoryCommandValidator` requires `Name` to be non-empty and at most 100 characters, which
matches the `HasMaxLength(100)` column in `CategoryConfigurations`.

# Queries

| Folder | Types |
| --- | --- |
| `Queries/ListCategories` | `ListCategoriesQuery()`, `ListCategoriesQueryHandler` |

`ListCategoriesQueryHandler` delegates straight to `ICategoriesRepository.ListAsync`, which returns
the categories ordered by name.

# Notes

* Like [Tutorials](/modules/application-tutorials.md), this feature is anonymous: the records are
  plain `IRequest<ErrorOr<T>>` with no `[Authorize]` attribute, and `CategoriesController` is
  `[AllowAnonymous]`.
* There is no update or delete use case. `ICategoriesRepository.GetByIdAsync` and `ExistsAsync`
  exist for other features — `ExistsAsync` is what `CreateTutorialCommandHandler` and
  `UpdateTutorialCommandHandler` call before accepting a `CategoryId`, returning
  `Error.NotFound("Category not found")` when it is unknown.

Backed by [Category](/modules/domain-categories.md) and `CategoriesRepository` in
[Persistence](/modules/infrastructure-persistence.md); exposed by `CategoriesController` in
[Api](/modules/api.md); the port is declared in
[Application Common](/modules/application-common.md).
