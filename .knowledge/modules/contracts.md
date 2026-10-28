---
type: Module
title: src/CleanArchitecture.Contracts
description: Wire DTO records shared between the API and its clients, grouped per feature.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Contracts
tags: [contracts, dto, api]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
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
| `Tokens/GenerateTokenRequest.cs` | `GenerateTokenRequest` |
| `Tokens/TokenResponse.cs` | `TokenResponse` |
| `Subscriptions/CreateSubscriptionRequest.cs` | `CreateSubscriptionRequest` |
| `Subscriptions/SubscriptionResponse.cs` | `SubscriptionResponse` |
| `Reminders/CreateReminderRequest.cs` | `CreateReminderRequest` |
| `Reminders/ReminderResponse.cs` | `ReminderResponse` |
| `Tutorials/CreateTutorialRequest.cs` | `CreateTutorialRequest(Title, Description, Published, CategoryId = null)` |
| `Tutorials/UpdateTutorialRequest.cs` | `UpdateTutorialRequest(Title, Description, Published, CategoryId = null)` |
| `Tutorials/TutorialResponse.cs` | `TutorialResponse(Id, Title, Description, Published, CategoryId, CreatedAt, UpdatedAt)` |
| `Tutorials/MessageResponse.cs` | `MessageResponse` — plain `{ message }` envelope for mutations that return no entity |
| `Categories/CreateCategoryRequest.cs` | `CreateCategoryRequest(Name)` |
| `Categories/CategoryResponse.cs` | `CategoryResponse(Id, Name)` |

# Notes

`CategoryId` is an optional `Guid?` with a `null` default on both tutorial request records, so
existing clients that omit it keep working and an unfiled tutorial round-trips as `CategoryId:
null`.

Mapping between these records and domain/result types happens in private static `ToDto` methods on
the controllers in [Api](/modules/api.md). The contracts `SubscriptionType` is intentionally a
separate type from [the domain smart enum](/modules/domain-users.md) so that wire compatibility and
domain modelling can evolve independently.
