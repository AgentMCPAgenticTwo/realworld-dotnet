---
type: Module
title: src/CleanArchitecture.Application/Common
description: MediatR pipeline behaviors, outbound port interfaces and the declarative security vocabulary.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Common
tags: [application, mediatr, behaviors, authorization, interfaces]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Behaviors

Registered as open behaviors in `Application/DependencyInjection.cs`, in this order:

| Class | File | Role |
| --- | --- | --- |
| `AuthorizationBehavior<TRequest, TResponse>` | `Common/Behaviors/AuthorizationBehavior.cs` | Reflects `AuthorizeAttribute` off the request, splits the comma-separated `Permissions`, `Roles` and `Policies`, and calls `IAuthorizationService.AuthorizeCurrentUser`; passes straight through when the request carries no attribute |
| `ValidationBehavior<TRequest, TResponse>` | `Common/Behaviors/ValidationBehavior.cs` | Runs the registered `IValidator<TRequest>` if one exists and converts failures to validation errors |

The two constraints differ, and the difference decides which requests each behavior sees:

| Behavior | `TRequest` constraint | Applies to |
| --- | --- | --- |
| `AuthorizationBehavior` | `IAuthorizeableRequest<TResponse>` | Only the user-scoped reminder, subscription and token requests |
| `ValidationBehavior` | `IRequest<TResponse>` | Every request, including the anonymous tutorial and category ones |

That is why `CreateCategoryCommand` can be a plain `IRequest<ErrorOr<Category>>` with no
`IAuthorizeableRequest` marker and still have `CreateCategoryCommandValidator` enforced by the
pipeline. Both behaviors additionally require `TResponse : IErrorOr`.

# Ports

| Interface | File | Implemented by |
| --- | --- | --- |
| `IUsersRepository` | `Common/Interfaces/IUsersRepository.cs` | `UsersRepository` |
| `IRemindersRepository` | `Common/Interfaces/IRemindersRepository.cs` | `RemindersRepository` |
| `ITutorialsRepository` | `Common/Interfaces/ITutorialsRepository.cs` | `TutorialsRepository` |
| `ICategoriesRepository` | `Common/Interfaces/ICategoriesRepository.cs` | `CategoriesRepository` |
| `IAuthorizationService` | `Common/Interfaces/IAuthorizationService.cs` | `AuthorizationService` |
| `IJwtTokenGenerator` | `Common/Interfaces/IJwtTokenGenerator.cs` | `JwtTokenGenerator` |
| `IDateTimeProvider` | `Common/Interfaces/IDateTimeProvider.cs` | `SystemDateTimeProvider` |

`ITutorialsRepository` exposes `AddAsync`, `GetByIdAsync`, `ListAsync(string? title, …)`,
`RemoveAsync`, `RemoveAllAsync` (returns the deleted count) and `UpdateAsync`.

`ICategoriesRepository` exposes `AddAsync`, `GetByIdAsync`, `ExistsAsync` and `ListAsync` — no
update or remove, matching the write-once [Category](/modules/domain-categories.md) entity.
`ExistsAsync` is the cross-feature hook the tutorial handlers use to validate a `CategoryId`.

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
