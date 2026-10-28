---
type: Module
title: tests/CleanArchitecture.Api.IntegrationTests
description: HTTP-level tests driving the real controllers through WebApplicationFactory and a SQLite test database.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Api.IntegrationTests
tags: [tests, integration, http, aspnetcore]
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
| `Common/WebApplicationFactory/WebAppFactory.cs` | Boots the API with the test database |
| `Common/WebApplicationFactory/SqliteTestDatabase.cs` | Creates and resets the SQLite file |
| `Common/WebApplicationFactory/WebAppFactoryCollection.cs` | xUnit collection definition |
| `Common/AppHttpClient.cs` | Typed client wrapping the endpoints (`GenerateTokenAsync`, `CreateSubscriptionAsync`, …) and attaching the bearer token |
| `Common/Tokens/TokenRequestFactory.cs` | `CreateGenerateTokenRequest` |
| `Common/Subscriptions/SubscriptionRequestFactory.cs` | `CreateCreateSubscriptionRequest` |

# Suites

| File | Covers |
| --- | --- |
| `Controllers/SubscriptionControllers.CreateSubscriptionTests.cs` | Token generation then `POST users/{userId}/subscriptions`, including the permission-denied path |
| `Controllers/TutorialsControllerTests.cs` | The `api/tutorials` CRUD surface over HTTP |

# Notes

This is the only layer that exercises routing, model binding, status codes and the
`ProblemDetails` shape produced by `ApiController` — the mapping table in [Api](/modules/api.md).
Authentication is real: tests mint a token via `tokens/generate` and send it on subsequent calls,
so a change to `JwtSettings` or to `CurrentUserProvider` surfaces here first.
