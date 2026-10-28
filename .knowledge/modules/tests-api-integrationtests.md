---
type: Module
title: tests/CleanArchitecture.Api.IntegrationTests
description: HTTP-level tests driving the real controllers through WebApplicationFactory and a Testcontainers PostgreSQL database.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Api.IntegrationTests
tags: [tests, integration, http, aspnetcore, postgres, testcontainers]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Harness

| File | Role |
| --- | --- |
| `Common/WebApplicationFactory/WebAppFactory.cs` | `IAsyncLifetime`; starts the container in `InitializeAsync`, then `RemoveAll<DbContextOptions<AppDbContext>>()` and re-adds the context with `UseNpgsql(_testDatabase.ConnectionString)`, and forces `EmailSettings:EnableEmailNotifications` off |
| `Common/WebApplicationFactory/PostgresTestDatabase.cs` | Wraps a Testcontainers `PostgreSqlContainer` (`tutorialsdb`/`postgres`/`postgres`); `CreateAndInitializeAsync` starts it, `ResetDatabase` does `EnsureDeleted()` then `Migrate()`, `DisposeAsync` tears the container down |
| `Common/WebApplicationFactory/WebAppFactoryCollection.cs` | xUnit collection definition |
| `Common/AppHttpClient.cs` | Typed client wrapping the endpoints (`GenerateTokenAsync`, `CreateSubscriptionAsync`, …) and attaching the bearer token |
| `Common/Tokens/TokenRequestFactory.cs` | `CreateGenerateTokenRequest` |
| `Common/Subscriptions/SubscriptionRequestFactory.cs` | `CreateCreateSubscriptionRequest` |

# Suites

| File | Covers |
| --- | --- |
| `Controllers/SubscriptionControllers.CreateSubscriptionTests.cs` | Token generation then `POST users/{userId}/subscriptions`, including the permission-denied path |
| `Controllers/TutorialsControllerTests.cs` | The `api/tutorials` CRUD surface over HTTP |
| `Controllers/CategoriesControllerTests.cs` | `POST`/`GET api/categories`, the empty-name 400, and the tutorial↔category cases: `CreateTutorial_WhenCategoryExists_ShouldReturnTheCategoryId`, `CreateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound`, `UpdateTutorial_WhenCategoryDoesNotExist_ShouldReturnNotFound` |

# Notes

This is the only layer that exercises routing, model binding, status codes and the
`ProblemDetails` shape produced by `ApiController` — the mapping table in [Api](/modules/api.md).
Authentication is real: tests mint a token via `tokens/generate` and send it on subsequent calls,
so a change to `JwtSettings` or to `CurrentUserProvider` surfaces here first.

It is also the only suite running on the production provider: the container is migrated with the
real `Migrations/` history, so a migration that does not apply cleanly fails here and nowhere else.
The flip side is that these tests need a working Docker daemon — `Testcontainers.PostgreSql` is a
package reference of this project only. Cross-feature coverage lives in `CategoriesControllerTests`
rather than `TutorialsControllerTests`, which is worth knowing when changing
[Tutorials use cases](/modules/application-tutorials.md).
