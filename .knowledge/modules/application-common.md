---
type: Module
title: src/CleanArchitecture.Application/Common
description: MediatR pipeline behaviors, outbound port interfaces and the declarative security vocabulary.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Common
tags: [application, mediatr, behaviors, authorization, interfaces]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Behaviors

Registered as open behaviors in `Application/DependencyInjection.cs`, in this order:

| Class | File | Role |
| --- | --- | --- |
| `AuthorizationBehavior<TRequest, TResponse>` | `Common/Behaviors/AuthorizationBehavior.cs` | Reflects `AuthorizeAttribute` off the request, splits the comma-separated `Permissions`, `Roles` and `Policies`, and calls `IAuthorizationService.AuthorizeCurrentUser`; passes straight through when the request carries no attribute |
| `ValidationBehavior<TRequest, TResponse>` | `Common/Behaviors/ValidationBehavior.cs` | Runs the registered `IValidator<TRequest>` if one exists and converts failures to validation errors |

Both are constrained to `TRequest : IAuthorizeableRequest<TResponse>` / `TResponse : IErrorOr`,
which is why every request record implements `IAuthorizeableRequest`.

# Ports

| Interface | File | Implemented by |
| --- | --- | --- |
| `IUsersRepository` | `Common/Interfaces/IUsersRepository.cs` | `UsersRepository` |
| `IRemindersRepository` | `Common/Interfaces/IRemindersRepository.cs` | `RemindersRepository` |
| `ITutorialsRepository` | `Common/Interfaces/ITutorialsRepository.cs` | `TutorialsRepository` |
| `IAuthorizationService` | `Common/Interfaces/IAuthorizationService.cs` | `AuthorizationService` |
| `IJwtTokenGenerator` | `Common/Interfaces/IJwtTokenGenerator.cs` | `JwtTokenGenerator` |
| `IDateTimeProvider` | `Common/Interfaces/IDateTimeProvider.cs` | `SystemDateTimeProvider` |

`ITutorialsRepository` exposes `AddAsync`, `GetByIdAsync`, `ListAsync(string? title, …)`,
`RemoveAsync`, `RemoveAllAsync` (returns the deleted count) and `UpdateAsync`.

All implementations live in [Infrastructure](/modules/infrastructure-persistence.md) and
[Security](/modules/infrastructure-security.md).

# Security Vocabulary

| File | Contents |
| --- | --- |
| `Common/Security/Request/IAuthorizeableRequest.cs` | Marker carrying `UserId` for self-vs-other policies |
| `Common/Security/Request/AuthorizeAttribute.cs` | `Permissions`, `Roles`, `Policies` string properties |
| `Common/Security/Roles/Role.cs` | `Admin` |
| `Common/Security/Policies/Policy.cs` | `SelfOrAdmin` = `"SelfOrAdminPolicy"` |
| `Common/Security/Permissions/Permission.Reminder.cs` | `set:reminder`, `get:reminder`, `dismiss:reminder`, `delete:reminder` |
| `Common/Security/Permissions/Permission.Subscription.cs` | Subscription permissions |

`Permission` is a partial static class split across one file per feature, so adding a feature means
adding a file rather than editing a shared one.

Enforcement lives in [Infrastructure Security](/modules/infrastructure-security.md); behaviors are
covered by [application unit tests](/modules/tests-application-unittests.md).
