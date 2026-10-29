---
type: Module
title: tests/CleanArchitecture.Api.IntegrationTests
description: HTTP-level tests driving the real controllers through WebApplicationFactory and a SQLite test database.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Api.IntegrationTests
tags: [tests, integration, http, aspnetcore]
generated: { by: sage-maintenance/claude-code, at: 2026-10-29T09:46:50Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 77aee7bc72e63696dcba05b7d841c54d863481b2
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
| `Controllers/HealthControllerTests.cs` | `GET api/health` without a token returns `200 OK` and `status: "ok"` |

# Notes

This is the only layer that exercises routing, model binding, status codes and the
`ProblemDetails` shape produced by `ApiController` — the mapping table in [Api](/modules/api.md).
Authentication is real: tests mint a token via `tokens/generate` and send it on subsequent calls,
so a change to `JwtSettings` or to `CurrentUserProvider` surfaces here first.
