---
type: Module
title: src/CleanArchitecture.Infrastructure/Security
description: JWT generation and validation, current-user resolution and the role/permission/policy enforcer.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Infrastructure/Security
tags: [infrastructure, security, jwt, authorization]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Types

| File | Type | Role |
| --- | --- | --- |
| `AuthorizationService.cs` | `AuthorizationService` | Implements `IAuthorizationService.AuthorizeCurrentUser`: resolves the caller, checks required roles and permissions, then defers each policy to the enforcer |
| `CurrentUserProvider/ICurrentUserProvider.cs` | `ICurrentUserProvider` | Port for reading the caller |
| `CurrentUserProvider/CurrentUserProvider.cs` | `CurrentUserProvider` | Reads claims off `IHttpContextAccessor` |
| `CurrentUserProvider/CurrentUser.cs` | `CurrentUser` | Id, name, email, permissions, roles |
| `PolicyEnforcer/IPolicyEnforcer.cs` | `IPolicyEnforcer` | Port for named policy checks |
| `PolicyEnforcer/PolicyEnforcer.cs` | `PolicyEnforcer` | Implements `SelfOrAdminPolicy` — the request's `UserId` must equal the caller's id, or the caller must hold `Admin` |
| `TokenGenerator/IJwtTokenGenerator` impl `TokenGenerator/JwtTokenGenerator.cs` | `JwtTokenGenerator` | Signs tokens from `JwtSettings` |
| `TokenGenerator/JwtSettings.cs` | `JwtSettings` | Bound from the `JwtSettings` configuration section |
| `TokenValidation/JwtBearerTokenValidationConfiguration.cs` | `JwtBearerTokenValidationConfiguration` | `IConfigureOptions<JwtBearerOptions>` wiring validation parameters from the same settings |

# Registration

`Infrastructure/DependencyInjection.cs`:

* `AddAuthorization` → `IAuthorizationService`/`ICurrentUserProvider` scoped, `IPolicyEnforcer` singleton.
* `AddAuthentication` → binds `JwtSettings`, registers `IJwtTokenGenerator` as a singleton,
  `ConfigureOptions<JwtBearerTokenValidationConfiguration>()`, then `AddAuthentication(JwtBearer).AddJwtBearer()`.

# Notes

This is the implementation half of the declarative model defined in
[Application Common](/modules/application-common.md): attributes on the request say *what* is
required, these classes decide *whether* it is satisfied. Because `PolicyEnforcer` is a singleton
it must stay stateless.

Tokens are minted via [Tokens use cases](/modules/application-tokens.md). Authorization outcomes
are covered by [application unit tests](/modules/tests-application-unittests.md) and the
authorization tests in [subcutaneous tests](/modules/tests-subcutaneous.md).
