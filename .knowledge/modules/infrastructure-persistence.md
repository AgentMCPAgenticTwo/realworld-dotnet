---
type: Module
title: src/CleanArchitecture.Infrastructure persistence
description: EF Core SQLite DbContext, repositories, entity configurations, value converters and the eventual-consistency middleware.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Infrastructure
tags: [infrastructure, efcore, sqlite, persistence, domain-events]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Core

| File | Type | Role |
| --- | --- | --- |
| `Common/Persistence/AppDbContext.cs` | `AppDbContext` | The single `DbContext`; its `SaveChangesAsync` is the highest fan-in symbol in the graph (11 callers) and is where domain events are collected onto the request |
| `Common/Middleware/EventualConsistencyMiddleware.cs` | `EventualConsistencyMiddleware` | Installed by `RequestPipeline.UseInfrastructure`; publishes the queued domain events after the response has been written |
| `Common/Persistence/FluentApiExtensions.cs` | `FluentApiExtensions` | `HasValueJsonConverter` and `HasListOfIdsConverter` helpers used by the configurations |
| `Common/Persistence/ValueJsonConverter.cs` | `ValueJsonConverter`, `ValueJsonComparer` | Serialises complex value objects (e.g. `Calendar`) to a JSON column with a matching change comparer |
| `Common/Persistence/ListOfIdsConverter.cs` | `ListOfIdsConverter`, `ListOfIdsComparer` | Stores `List<Guid>` columns such as `_reminderIds` |

# Repositories and Configurations

| Aggregate | Repository | Configuration |
| --- | --- | --- |
| `User` | `Users/Persistence/UsersRepository.cs` | `Users/Persistence/UserConfigurations.cs` |
| `Reminder` | `Reminders/Persistence/RemindersRepository.cs` | `Reminders/Persistence/ReminderConfigurations.cs` |
| `Tutorial` | `Tutorials/Persistence/TutorialsRepository.cs` | `Tutorials/Persistence/TutorialConfigurations.cs` |

Each repository implements the matching interface from
[Application Common](/modules/application-common.md) and calls `SaveChangesAsync` itself — there is
no separate unit of work.

# Migrations

| Migration | Adds |
| --- | --- |
| `Migrations/20231226150412_InitialCreate.cs` | Users and reminders |
| `Migrations/20261027000000_AddTutorials.cs` | The tutorials table |

`Migrations/AppDbContextModelSnapshot.cs` is the EF snapshot and must be regenerated with any new
migration.

# Notes

The connection string is hard-coded in `Infrastructure/DependencyInjection.cs` as
`Data Source = CleanArchitecture.sqlite`, not read from configuration — tests swap the provider via
their own host builders instead. Because the comparers are paired with the converters, forgetting
a comparer on a new JSON column silently disables change tracking for it.
