---
type: Module
title: src/CleanArchitecture.Application/Tokens
description: Single query that mints a JWT carrying the caller's identity, subscription, roles and permissions.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Application/Tokens
tags: [application, tokens, jwt, auth]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Types

| File | Type |
| --- | --- |
| `Queries/Generate/GenerateTokenQuery.cs` | `GenerateTokenQuery` — `Id`, `FirstName`, `LastName`, `Email`, domain `SubscriptionType`, `Permissions`, `Roles` |
| `Queries/Generate/GenerateTokenQueryHandler.cs` | `GenerateTokenQueryHandler` — delegates to `IJwtTokenGenerator.GenerateToken` |
| `Queries/Generate/GenerateTokenResult.cs` | `GenerateTokenResult` — the identity fields plus the signed `Token` |

# Notes

This is a development/demo affordance, not a login: the caller supplies the roles and permissions
they want embedded, and the endpoint is `[AllowAnonymous]`. It exists so that the authorization
pipeline can be exercised without an identity provider. Treat it as something to replace before any
real deployment.

Signing is implemented by `JwtTokenGenerator` in
[Infrastructure Security](/modules/infrastructure-security.md); the resulting token is validated by
`JwtBearerTokenValidationConfiguration` and read back by `CurrentUserProvider`.
