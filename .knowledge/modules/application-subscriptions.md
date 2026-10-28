---
type: Module
title: src/CleanArchitecture.Application/Subscriptions
description: Subscription use cases for creating, cancelling and reading a user's subscription.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Subscriptions
tags: [application, subscriptions, cqrs]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Types

| Folder | Types |
| --- | --- |
| `Commands/CreateSubscription` | `CreateSubscriptionCommand`, `CreateSubscriptionCommandHandler`, `CreateSubscriptionCommandValidator` |
| `Commands/CancelSubscription` | `CancelSubscriptionCommand`, `CancelSubscriptionCommandHandler` |
| `Queries/GetSubscription` | `GetSubscriptionQuery`, `GetSubscriptionQueryHandler` |
| `Common` | `SubscriptionResult` — the `Id`/`UserId`/`SubscriptionType` triple returned to the controller |
| `Events` | `SubscriptionCanceledEventHandler` |

# Notes

* Creating a subscription creates the owning `User` aggregate with its `Subscription`; the command
  carries `FirstName`, `LastName`, `Email` and the already-parsed domain `SubscriptionType`.
* Cancelling calls `User.CancelSubscription`, which swaps in the sentinel `Subscription.Canceled`
  and raises `SubscriptionCanceledEvent`. `SubscriptionCanceledEventHandler` is what cascades the
  cleanup, so reminder behavior after cancellation is eventually consistent.
* Handlers never return the domain entity; they project to `SubscriptionResult`, which
  `SubscriptionsController` maps to `SubscriptionResponse`.

Backed by [User and Subscription](/modules/domain-users.md); exposed by `SubscriptionsController`
in [Api](/modules/api.md).
