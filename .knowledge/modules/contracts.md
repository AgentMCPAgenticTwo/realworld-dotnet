---
type: Module
title: src/CleanArchitecture.Contracts
description: Wire DTO records shared between the API and its clients, grouped per feature.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Contracts
tags: [contracts, dto, api]
generated: { by: sage-maintenance/claude-code, at: 2026-10-29T09:46:50Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 77aee7bc72e63696dcba05b7d841c54d863481b2
    title: Codebase knowledge graph
status: stable
---

# Responsibility

Hold the public request/response shapes. This project has no logic and no dependency on Domain or
Application, so it can be shipped to clients independently.

# Types

| File | Type |
| --- | --- |
| `Common/SubscriptionType.cs` | `SubscriptionType` enum (`Basic`, `Pro`) — the wire form of the domain smart enum |
| `Common/HealthResponse.cs` | `HealthResponse` — `{ status }` body of the liveness endpoint |
| `Tokens/GenerateTokenRequest.cs` | `GenerateTokenRequest` |
| `Tokens/TokenResponse.cs` | `TokenResponse` |
| `Subscriptions/CreateSubscriptionRequest.cs` | `CreateSubscriptionRequest` |
| `Subscriptions/SubscriptionResponse.cs` | `SubscriptionResponse` |
| `Reminders/CreateReminderRequest.cs` | `CreateReminderRequest` |
| `Reminders/ReminderResponse.cs` | `ReminderResponse` |
| `Tutorials/CreateTutorialRequest.cs` | `CreateTutorialRequest` |
| `Tutorials/UpdateTutorialRequest.cs` | `UpdateTutorialRequest` |
| `Tutorials/TutorialResponse.cs` | `TutorialResponse` |
| `Tutorials/MessageResponse.cs` | `MessageResponse` — plain `{ message }` envelope for mutations that return no entity |

# Notes

Mapping between these records and domain/result types happens in private static `ToDto` methods on
the controllers in [Api](/modules/api.md). The contracts `SubscriptionType` is intentionally a
separate type from [the domain smart enum](/modules/domain-users.md) so that wire compatibility and
domain modelling can evolve independently.
