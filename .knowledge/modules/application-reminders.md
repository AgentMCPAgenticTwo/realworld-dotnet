---
type: Module
title: src/CleanArchitecture.Application/Reminders
description: Reminder use cases — set, dismiss, delete, get and list — plus the reminder domain event handlers.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Reminders
tags: [application, reminders, cqrs]
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
| `Commands/SetReminder` | `SetReminderCommand`, `SetReminderCommandHandler`, `SetReminderCommandValidator` |
| `Commands/DismissReminder` | `DismissReminderCommand`, `DismissReminderCommandHandler` |
| `Commands/DeleteReminder` | `DeleteReminderCommand`, `DeleteReminderCommandHandler` |

# Queries

| Folder | Types |
| --- | --- |
| `Queries/GetReminder` | `GetReminderQuery`, `GetReminderQueryHandler` |
| `Queries/ListReminders` | `ListRemindersQuery`, `ListRemindersQueryHandler` |

# Event Handlers

| File | Reacts to |
| --- | --- |
| `Events/ReminderSetEventHandler.cs` | `ReminderSetEvent` |
| `Events/ReminderDismissedEventHandler.cs` | `ReminderDismissedEvent` |
| `Events/ReminderDeletedEventHandler.cs` | `ReminderDeletedEvent` |

These run after the HTTP response, published by `EventualConsistencyMiddleware` — see
[Architecture](/architecture.md).

# Flow

Reminder mutations are two-sided: the handler loads the `User` aggregate through `IUsersRepository`
to enforce subscription and daily-limit rules (`User.SetReminder`, `User.DismissReminder`,
`User.DeleteReminder`) and persists the `Reminder` itself through `IRemindersRepository`. Both
aggregates are updated in the same handler, which is why `IUsersRepository.GetByIdAsync` and
`UpdateAsync` are among the highest fan-in symbols in the graph.

Backed by [Reminder](/modules/domain-reminders.md) and [User](/modules/domain-users.md); exposed by
`RemindersController` in [Api](/modules/api.md).
