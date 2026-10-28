---
type: Module
title: src/CleanArchitecture.Domain/Reminders
description: The Reminder entity — an immutable scheduled note that can be dismissed exactly once.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Reminders
tags: [domain, reminders, entity]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# `Reminder` (`Reminder.cs`)

| Member | Notes |
| --- | --- |
| `UserId`, `SubscriptionId`, `DateTime`, `Text` | Get-only; fixed at construction |
| `Date` | Computed `DateOnly` projection of `DateTime.Date`, used as the `Calendar` key |
| `IsDismissed` | The only mutable state |
| `Dismiss()` | Returns `Error.Conflict("Reminder already dismissed")` when already dismissed, otherwise sets the flag and returns `Result.Success` |

The constructor takes an optional `Guid? id` defaulting to a fresh `Guid.NewGuid()`; a private
parameterless constructor exists for EF Core.

# Notes

`Reminder` raises no domain events of its own — the reminder lifecycle events live on
[User](/modules/domain-users.md), because the limit and ownership rules are the aggregate root's
responsibility. A `Reminder` therefore cannot be dismissed meaningfully without the matching
`User.DismissReminder` call; handlers in
[Reminders use cases](/modules/application-reminders.md) always do both.
