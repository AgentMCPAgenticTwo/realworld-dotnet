---
type: Architecture
title: Layering and request flow
description: Four-project onion layering with MediatR request pipeline, ErrorOr results and EF Core SQLite persistence.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src
tags: [architecture, layers, mediatr, clean-architecture]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Layers

Dependencies point inward only. Domain references nothing in the solution.

| Layer | Project | May reference |
| --- | --- | --- |
| Presentation | [Api](/modules/api.md), [Contracts](/modules/contracts.md) | Application, Contracts, Domain |
| Application | [Application Common](/modules/application-common.md) and the feature modules | Domain |
| Domain | [Domain Common](/modules/domain-common.md), [Users](/modules/domain-users.md), [Reminders](/modules/domain-reminders.md), [Tutorials](/modules/domain-tutorials.md) | — |
| Infrastructure | [Persistence](/modules/infrastructure-persistence.md), [Security](/modules/infrastructure-security.md), [Background Services](/modules/infrastructure-background-services.md) | Application, Domain |

Infrastructure is wired in only at composition time from `Program.cs`, so Application depends on
abstractions (`IUsersRepository`, `IRemindersRepository`, `ITutorialsRepository`,
`IJwtTokenGenerator`, `IDateTimeProvider`, `IAuthorizationService`) rather than implementations.

# Request Flow

```
HTTP request
  └─ ASP.NET routing → Controller (api/*Controller.cs)
       └─ builds a Command or Query record
            └─ ISender/IMediator.Send
                 ├─ AuthorizationBehavior<TRequest,TResponse>
                 │    reads [Authorize] attributes on the request
                 │    → IAuthorizationService.AuthorizeCurrentUser
                 │         → ICurrentUserProvider + IPolicyEnforcer
                 ├─ ValidationBehavior<TRequest,TResponse>
                 │    → IValidator<TRequest> (FluentValidation)
                 └─ Handler (…CommandHandler / …QueryHandler)
                      ├─ repository interface → EF Core repository → AppDbContext
                      └─ entity method mutates state and queues domain events
  └─ result.Match(success → Ok/Created/NoContent, Problem)
  └─ EventualConsistencyMiddleware publishes queued domain events after the response
```

Every handler returns `ErrorOr<T>`; controllers translate failures through
`ApiController.Problem(List<Error>)`.

# Cross-Cutting Mechanisms

| Mechanism | Where | Notes |
| --- | --- | --- |
| Authorization | `Application/Common/Behaviors/AuthorizationBehavior.cs`, `Infrastructure/Security/` | Roles, permissions and policies declared per request via `AuthorizeAttribute`; enforced by `PolicyEnforcer` |
| Validation | `Application/Common/Behaviors/ValidationBehavior.cs` | Validators live beside their command |
| Domain events | `Domain/Common/Entity.cs` `PopDomainEvents`, `Infrastructure/Common/Middleware/EventualConsistencyMiddleware.cs` | Events are collected on entities, published after the HTTP response completes |
| Error mapping | `Api/Controllers/ApiController.cs`, `Domain/Users/UserErrors.cs` | Single place where `ErrorType` becomes an HTTP status |
| Value conversion | `Infrastructure/Common/Persistence/ValueJsonConverter.cs`, `ListOfIdsConverter.cs`, `FluentApiExtensions.cs` | JSON and id-list column conversions with matching comparers |

# Boundaries and Hotspots

The graph reports two packages (`CleanArchitecture`, `TestCommon`) with 218 calls from source into
test helpers and 19 back, i.e. `TestCommon` is a widely-shared factory layer rather than a
production dependency.

Highest fan-in symbols:

| Symbol | Fan-in |
| --- | --- |
| `AppDbContext.SaveChangesAsync` | 11 |
| `Tutorial.Update` | 5 |
| `AuthorizationBehavior.Handle` | 5 |
| `IAuthorizationService.AuthorizeCurrentUser` | 5 |
| `IUsersRepository.GetByIdAsync` | 5 |
| `IUsersRepository.UpdateAsync` | 4 |
| `IRemindersRepository.GetByIdAsync` | 4 |

`AppDbContext.SaveChangesAsync` is the single persistence choke point — every repository write
funnels through it, so transaction or interception changes belong there.

# Feature Clusters

| Cluster | Application module | Domain module | Infrastructure module |
| --- | --- | --- | --- |
| Reminders | [Reminders use cases](/modules/application-reminders.md) | [Reminder](/modules/domain-reminders.md) | [Persistence](/modules/infrastructure-persistence.md), [Background Services](/modules/infrastructure-background-services.md) |
| Subscriptions | [Subscriptions use cases](/modules/application-subscriptions.md) | [Users](/modules/domain-users.md) | [Persistence](/modules/infrastructure-persistence.md) |
| Tokens | [Tokens use cases](/modules/application-tokens.md) | [Users](/modules/domain-users.md) | [Security](/modules/infrastructure-security.md) |
| Tutorials | [Tutorials use cases](/modules/application-tutorials.md) | [Tutorial](/modules/domain-tutorials.md) | [Persistence](/modules/infrastructure-persistence.md) |

Tutorials is the one feature with no user, subscription or authorization coupling: its controller
is `[AllowAnonymous]` and its handlers touch only `ITutorialsRepository`.

# Test Topology

[Domain unit tests](/modules/tests-domain-unittests.md) exercise entities directly;
[application unit tests](/modules/tests-application-unittests.md) exercise the pipeline behaviors
with substitutes; [subcutaneous tests](/modules/tests-subcutaneous.md) go through the real mediator
against a SQLite test database; [integration tests](/modules/tests-api-integrationtests.md) drive
real HTTP. All four consume [TestCommon](/modules/tests-testcommon.md).
