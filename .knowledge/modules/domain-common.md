---
type: Module
title: src/CleanArchitecture.Domain/Common
description: Entity base class owning identity and the queued domain events, plus the IDomainEvent marker.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Domain/Common
tags: [domain, entity, domain-events]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Types

| File | Type | Members |
| --- | --- | --- |
| `Entity.cs` | `abstract class Entity` | `Guid Id { get; private init; }`, `protected readonly List<IDomainEvent> _domainEvents`, `PopDomainEvents()`, protected ctor taking an id, protected parameterless ctor for EF |
| `IDomainEvent.cs` | `interface IDomainEvent : INotification` | Marker; inheriting MediatR's `INotification` is what lets handlers be discovered by assembly scan |

# Notes

`PopDomainEvents()` returns a copy and clears the list, so draining is destructive and
single-shot. `EventualConsistencyMiddleware` in
[Persistence](/modules/infrastructure-persistence.md) is the only production caller.

Base of [User](/modules/domain-users.md), [Reminder](/modules/domain-reminders.md) and
[Tutorial](/modules/domain-tutorials.md).
