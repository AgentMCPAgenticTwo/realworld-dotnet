---
type: Module
title: src/CleanArchitecture.Domain/Users
description: The User aggregate root with its subscription, reminder calendar, daily limits and domain events.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Users
tags: [domain, users, subscriptions, aggregate]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Aggregate Root: `User`

`User.cs` holds `Email`, `FirstName`, `LastName`, a `Subscription`, a private `Calendar` and two
private id lists (`_reminderIds`, `_dismissedReminderIds`). Reminders are referenced by id, not by
object, so `Reminder` is its own aggregate.

| Method | Rule enforced |
| --- | --- |
| `SetReminder(Reminder)` | `NotFound` if the subscription is cancelled; throws if the reminder's `SubscriptionId` mismatches; `UserErrors.CannotCreateMoreRemindersThanSubscriptionAllows` when the day is full; otherwise increments the calendar, records the id, raises `ReminderSetEvent` |
| `DismissReminder(Guid)` | `NotFound` for cancelled subscription or unknown reminder, `Conflict` if already dismissed; raises `ReminderDismissedEvent` |
| `DeleteReminder(Reminder)` | `NotFound` for cancelled subscription or unknown reminder; decrements the calendar; raises `ReminderDeletedEvent` |
| `DeleteAllReminders()` | Raises one `ReminderDeletedEvent` per id and clears the list |
| `CancelSubscription(Guid)` | `NotFound` if the id is not the current subscription; replaces it with `Subscription.Canceled` and raises `SubscriptionCanceledEvent` |
| `HasReachedDailyReminderLimit` (private) | Compares the calendar count for that day against `SubscriptionType.GetMaxDailyReminders()` |

# Supporting Types

| File | Type | Notes |
| --- | --- | --- |
| `Subscription.cs` | `Subscription` (namespace `Domain.Subscriptions`) | Wraps a `SubscriptionType`; the static `Canceled` sentinel uses type `"Canceled"` with value `-1` and `Guid.Empty` |
| `SubscriptionType.cs` | `SubscriptionType : SmartEnum<SubscriptionType>` | `Basic` = 0 (3 reminders/day), `Pro` = 1 (`int.MaxValue`), via `GetMaxDailyReminders()` |
| `Calendar.cs` | `Calendar` | `DateOnly → count` dictionary with `Empty()`, `IncrementEventCount`, `DecrementEventCount`, `SetEventCount`, `GetNumEventsOnDay` |
| `UserErrors.cs` | `UserErrors` | `CannotCreateMoreRemindersThanSubscriptionAllows`, a validation error |
| `Events/ReminderSetEvent.cs` | `ReminderSetEvent` | Carries the whole `Reminder` |
| `Events/ReminderDismissedEvent.cs` | `ReminderDismissedEvent` | Carries the reminder id |
| `Events/ReminderDeletedEvent.cs` | `ReminderDeletedEvent` | Carries the reminder id |
| `Events/SubscriptionCanceledEvent.cs` | `SubscriptionCanceledEvent` | Carries the `User` and subscription id |

# Notes

Cancellation is modelled as substitution rather than a flag: every guard reads
`Subscription == Subscription.Canceled`, so the sentinel's identity matters. The `Calendar`
dictionary is persisted through the JSON value converters described in
[Persistence](/modules/infrastructure-persistence.md).

Consumed by [Reminders](/modules/application-reminders.md),
[Subscriptions](/modules/application-subscriptions.md) and
[Tokens](/modules/application-tokens.md); tested in
[domain unit tests](/modules/tests-domain-unittests.md).
