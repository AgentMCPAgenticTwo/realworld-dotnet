---
type: Module
title: src/CleanArchitecture.Infrastructure persistence
description: EF Core PostgreSQL DbContext, repositories, entity configurations, value converters and the eventual-consistency middleware.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Infrastructure
tags: [infrastructure, efcore, postgres, persistence, domain-events]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Core

| File | Type | Role |
| --- | --- | --- |
| `Common/Persistence/AppDbContext.cs` | `AppDbContext` | The single `DbContext`, exposing `Users`, `Reminders`, `Tutorials` and `Categories`; its `SaveChangesAsync` is the highest fan-in symbol in the graph (12 callers) and is where domain events are collected onto the request |
| `Common/Middleware/EventualConsistencyMiddleware.cs` | `EventualConsistencyMiddleware` | Installed by `RequestPipeline.UseInfrastructure`; publishes the queued domain events after the response has been written |
| `RequestPipeline.cs` | `RequestPipeline` | `UseInfrastructure` plus `ApplyMigrations`, which calls `Database.Migrate()` at startup only when `Database.IsNpgsql()` |
| `DependencyInjection.cs` | `AddPersistence` | Reads `ConnectionStrings:DefaultConnection`, registers `AppDbContext` with `UseNpgsql`, and registers the four repositories |
| `Common/Persistence/FluentApiExtensions.cs` | `FluentApiExtensions` | `HasValueJsonConverter` and `HasListOfIdsConverter` helpers used by the configurations |
| `Common/Persistence/ValueJsonConverter.cs` | `ValueJsonConverter`, `ValueJsonComparer` | Serialises complex value objects (e.g. `Calendar`) to a JSON column with a matching change comparer |
| `Common/Persistence/ListOfIdsConverter.cs` | `ListOfIdsConverter`, `ListOfIdsComparer` | Stores `List<Guid>` columns such as `_reminderIds` |

# Repositories and Configurations

| Aggregate | Repository | Configuration |
| --- | --- | --- |
| `User` | `Users/Persistence/UsersRepository.cs` | `Users/Persistence/UserConfigurations.cs` |
| `Reminder` | `Reminders/Persistence/RemindersRepository.cs` | `Reminders/Persistence/ReminderConfigurations.cs` |
| `Tutorial` | `Tutorials/Persistence/TutorialsRepository.cs` | `Tutorials/Persistence/TutorialConfigurations.cs` |
| `Category` | `Categories/Persistence/CategoriesRepository.cs` | `Categories/Persistence/CategoryConfigurations.cs` |

Each repository implements the matching interface from
[Application Common](/modules/application-common.md) and calls `SaveChangesAsync` itself — there is
no separate unit of work.

`CategoriesRepository` is read-mostly: `AddAsync` is the only write, `ExistsAsync` is an `AnyAsync`
probe used by the tutorial handlers, and `ListAsync` is `AsNoTracking()` ordered by `Name`.

`TutorialConfigurations` owns the one relationship in this feature pair:
`HasOne<Category>().WithMany().HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.SetNull)`,
so removing a category nulls the tutorials' `CategoryId` rather than cascading a delete. There is
no navigation property on either side — only the foreign-key column.

# Migrations

| Migration | Adds |
| --- | --- |
| `Migrations/20261028114456_InitialCreate.cs` | The whole PostgreSQL schema: users, reminders, tutorials and categories |

The earlier SQLite migrations (`20231226150412_InitialCreate`, `20261027000000_AddTutorials`) were
removed when the provider changed; the history was collapsed into a single baseline rather than
migrated across providers. `Migrations/AppDbContextModelSnapshot.cs` is the EF snapshot and must be
regenerated with any new migration.

# Notes

The connection string is read from configuration (`ConnectionStrings:DefaultConnection`) and the
provider is Npgsql — the former hard-coded `Data Source = CleanArchitecture.sqlite` file is gone.
`ApplyMigrations` guards on `IsNpgsql()`, so hosts that swap in another provider (the subcutaneous
SQLite harness) skip migration and build the schema from the model instead. Because the comparers
are paired with the converters, forgetting a comparer on a new JSON column silently disables change
tracking for it.
