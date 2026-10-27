---
type: Module
title: src/CleanArchitecture.Infrastructure/Services
description: Ambient-dependency implementations, currently the system clock behind IDateTimeProvider.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Infrastructure/Services
tags: [infrastructure, services, clock]
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
| `SystemDateTimeProvider.cs` | `SystemDateTimeProvider` | Implements `IDateTimeProvider` over the real clock |

Registered as a singleton by `AddServices` in `Infrastructure/DependencyInjection.cs`.

# Notes

Handlers and entities never call `DateTime.UtcNow` directly; they take `IDateTimeProvider` so that
reminder-limit and tutorial-timestamp behavior is deterministic under test. Substituting this
interface is how [subcutaneous tests](/modules/tests-subcutaneous.md) control "today".

The port it satisfies is declared in [Application Common](/modules/application-common.md).
