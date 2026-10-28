---
type: Module
title: tests/CleanArchitecture.Application.SubcutaneousTests
description: Mediator-level tests running the real pipeline and EF Core against a per-test SQLite database.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Application.SubcutaneousTests
tags: [tests, subcutaneous, mediatr, sqlite]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
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

Each use case may carry up to three files: `…Tests.cs` for the happy and error paths,
`….AuthorizationTests.cs`, and `….ValidationTests.cs`. Tutorials have neither authorization nor
validation suites, matching the anonymous design noted in
[Tutorials use cases](/modules/application-tutorials.md).

# Notes

`ResetDatabase` between tests is what keeps the shared SQLite file usable across a collection; a
new suite that forgets the collection attribute will see cross-test bleed. Arrangement goes
through [TestCommon](/modules/tests-testcommon.md) factories and mediator extensions.
