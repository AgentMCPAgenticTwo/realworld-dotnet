---
type: Module
title: tests/CleanArchitecture.Domain.UnitTests
description: Fast in-memory tests asserting entity invariants and raised domain events.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Domain.UnitTests
tags: [tests, unit, domain]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Files

| File | Covers |
| --- | --- |
| `Users/UserTests.cs` | `User.SetReminder` limits, `DismissReminder`, `DeleteReminder`, `CancelSubscription`, event emission |
| `Reminders/ReminderTests.cs` | `Reminder.Dismiss` success and the already-dismissed conflict |
| `Tutorials/TutorialTests.cs` | `Tutorial` construction and `Update` timestamp behavior |
| `GlobalUsings.cs` | xUnit / FluentAssertions imports shared by the project |

# Notes

No substitutes, no host, no database — these construct entities directly through
[TestCommon](/modules/tests-testcommon.md) factories and assert on the returned `ErrorOr` and on
`PopDomainEvents()`. They are the right place to add a case when a rule lives inside
[Domain](/modules/domain-users.md) rather than in a handler.
