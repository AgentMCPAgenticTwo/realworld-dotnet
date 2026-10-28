---
type: Module
title: tests/CleanArchitecture.Application.SubcutaneousTests
description: Mediator-level tests running the real pipeline and EF Core against a per-test SQLite database.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Application.SubcutaneousTests
tags: [tests, subcutaneous, mediatr, sqlite]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Harness

| File | Role |
| --- | --- |
| `Common/WebAppFactory.cs` | Builds the host, swaps in the test database and `TestCurrentUserProvider`, exposes `CreateMediator()` |
| `Common/SqliteTestDatabase.cs` | `CreateAndInitialize`, `InitializeDatabase`, `ResetDatabase` |
| `Common/WebAppFactoryCollection.cs` | xUnit collection definition sharing the factory across classes |

Tests send real commands and queries through `IMediator`, so behaviors, validators, handlers,
repositories and EF Core all run; only the HTTP layer is bypassed.

# Suites

| Feature | Files |
| --- | --- |
| Reminders | `Reminders/Commands/{SetReminder,DismissReminder,DeleteReminder}/…Tests.cs`, `Reminders/Queries/{GetReminder,ListReminders}/…Tests.cs` |
| Subscriptions | `Subscriptions/Commands/{CreateSubscription,CancelSubscription}/…Tests.cs`, `Subscriptions/Queries/GetSubscription/…Tests.cs` |
| Tutorials | `Tutorials/Commands/{CreateTutorial,UpdateTutorial,DeleteTutorial}/…Tests.cs`, `Tutorials/Queries/{GetTutorial,ListTutorials}/…Tests.cs` |
| Categories | `Categories/Commands/CreateCategory/CreateCategoryTests.cs`, `Categories/Queries/ListCategories/ListCategoriesTests.cs` |

Each use case may carry up to three files: `…Tests.cs` for the happy and error paths,
`….AuthorizationTests.cs`, and `….ValidationTests.cs`. Tutorials and categories have no
authorization suites, matching the anonymous design noted in
[Tutorials use cases](/modules/application-tutorials.md) and
[Categories use cases](/modules/application-categories.md). They do still exercise validation
through the mediator — `CreateCategory_WhenNameIsEmpty_ShouldReturnValidationError` lives in the
main `CreateCategoryTests.cs` rather than a separate validation file, because `ValidationBehavior`
runs for anonymous requests too (see [Application Common](/modules/application-common.md)).
`ListCategories_WhenCategoriesExist_ShouldReturnThemOrderedByName` pins the `OrderBy(Name)` in
`CategoriesRepository.ListAsync`.

# Notes

`ResetDatabase` between tests is what keeps the shared SQLite file usable across a collection; a
new suite that forgets the collection attribute will see cross-test bleed. Arrangement goes
through [TestCommon](/modules/tests-testcommon.md) factories and mediator extensions.

This project deliberately stayed on SQLite when production and the
[integration tests](/modules/tests-api-integrationtests.md) moved to PostgreSQL, trading provider
fidelity for speed and no Docker dependency. `RequestPipeline.ApplyMigrations` is a no-op here
because it guards on `IsNpgsql()`, so the schema comes from the model rather than from
`Migrations/`.
