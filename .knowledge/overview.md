---
type: Codebase Overview
title: realworld-dotnet
description: Clean Architecture .NET 8 reference service exposing reminder, subscription, token and tutorial HTTP APIs.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/
tags: [dotnet, csharp, clean-architecture, aspnetcore, overview]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T04:15:17Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 2650bfd28ce9976b5db0a140e2928725519d0af3
    title: Codebase knowledge graph
status: stable
---

# Purpose

A Clean Architecture template service (`dotnet new clean-arch`) that models users who hold a
subscription and set reminders against it, plus a standalone CRUD `Tutorial` resource used by a
companion frontend. Every use case is a MediatR request handled behind an authorization and
validation pipeline and returns an `ErrorOr<T>` result.

# Stack

| Concern | Choice |
| --- | --- |
| Runtime | .NET 8 (`global.json` pins SDK `8.0.100`, `rollForward: latestMinor`) |
| Web | ASP.NET Core controllers, Swagger/SwaggerUI in Development, ProblemDetails |
| Mediation | MediatR with open pipeline behaviors |
| Validation | FluentValidation (assembly scan from the Application project) |
| Results | ErrorOr discriminated results; `Throw` for domain guards |
| Persistence | EF Core over SQLite (`Data Source = CleanArchitecture.sqlite`) |
| Auth | JWT bearer tokens, custom role/permission/policy enforcement |
| Email | FluentEmail + SMTP, behind an opt-in hosted service |
| Tests | xUnit, NSubstitute, FluentAssertions |

# Solution Layout

| Project | Role |
| --- | --- |
| `src/CleanArchitecture.Api` | Presentation: controllers, DI for presentation, host |
| `src/CleanArchitecture.Contracts` | Wire DTOs shared with clients |
| `src/CleanArchitecture.Application` | Use cases, pipeline behaviors, repository interfaces |
| `src/CleanArchitecture.Domain` | Entities, value objects, domain events, errors |
| `src/CleanArchitecture.Infrastructure` | EF Core, security, background services, middleware |
| `tests/TestCommon` | Shared factories and constants used by all test projects |
| `tests/CleanArchitecture.Domain.UnitTests` | Entity behavior tests |
| `tests/CleanArchitecture.Application.UnitTests` | Pipeline behavior tests |
| `tests/CleanArchitecture.Application.SubcutaneousTests` | Mediator-level tests on a SQLite test database |
| `tests/CleanArchitecture.Api.IntegrationTests` | HTTP-level tests via `WebApplicationFactory` |

See [Architecture](/architecture.md) for how these depend on each other.

# Entry Points

| File | Role |
| --- | --- |
| `src/CleanArchitecture.Api/Program.cs` | Host builder: `AddPresentation().AddApplication().AddInfrastructure(...)`, then exception handler, infrastructure middleware, Swagger (Development), HTTPS redirect, CORS, authorization, `MapControllers` |
| `src/CleanArchitecture.Api/DependencyInjection.cs` | `AddPresentation`, and the `AllowFrontend` CORS policy pinned to `http://localhost:4200` |
| `src/CleanArchitecture.Application/DependencyInjection.cs` | Registers MediatR plus `AuthorizationBehavior<,>` and `ValidationBehavior<,>` |
| `src/CleanArchitecture.Infrastructure/DependencyInjection.cs` | Services, background services, authentication, authorization, persistence |
| `src/CleanArchitecture.Infrastructure/RequestPipeline.cs` | `UseInfrastructure` installs `EventualConsistencyMiddleware` |

# HTTP Surface

| Route prefix | Controller | Auth |
| --- | --- | --- |
| `tokens` | `TokensController` | `[AllowAnonymous]` |
| `users/{userId:guid}/subscriptions` | `SubscriptionsController` | inherited `[Authorize]` |
| `users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders` | `RemindersController` | inherited `[Authorize]` |
| `api/tutorials` | `TutorialsController` | `[AllowAnonymous]` |

`ApiController` is the shared base: `[ApiController]`, `[Authorize]`, and `Problem(List<Error>)`
mapping `Conflict→409`, `Validation→400`, `NotFound→404`, `Unauthorized→403`, else `500`.

# Running and Building

| Task | Command |
| --- | --- |
| Run the API | `dotnet run --project src/CleanArchitecture.Api` |
| Run everything in containers | `docker-compose up` (see `docker-compose.yml`, `Dockerfile`) |
| Run all tests | `dotnet test` |
| CI | `.github/workflows/build.yml`; template publishing in `.github/workflows/publish.yml` |
| Automated API correction runs | `.github/workflows/api-correction.yml` (manual/dispatch agent workflow) |

# Configuration

`src/CleanArchitecture.Api/appsettings.json` and `appsettings.Development.json` carry the
`JwtSettings` section (bound in `AddAuthentication`) and the `EmailSettings` section
(`EmailSettings.Section`, with a nested `SmtpSettings`). Email notifications and the
`ReminderEmailBackgroundService` only register when `EnableEmailNotifications` is true.
