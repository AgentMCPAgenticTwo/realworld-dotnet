---
type: Module
title: tests/CleanArchitecture.Application.UnitTests
description: Isolated tests for the MediatR authorization and validation pipeline behaviors.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/CleanArchitecture.Application.UnitTests
tags: [tests, unit, behaviors, mediatr]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Files

| File | Covers |
| --- | --- |
| `Common/Behaviors/AuthorizationBehaviorTests.cs` | No attribute passes through; single attribute authorized / unauthorized; many attributes authorized / unauthorized |
| `Common/Behaviors/ValidationBehaviorTests.cs` | No validator registered; valid result; invalid result mapped to validation errors |
| `GlobalUsings.cs` | Shared test imports |

# Notes

Both suites substitute the collaborator (`IAuthorizationService`, `IValidator<T>`) with NSubstitute
and assert that `next()` was or was not invoked — the pipeline contract, not the business rule.
End-to-end authorization outcomes are instead asserted by the `*.AuthorizationTests.cs` files in
[subcutaneous tests](/modules/tests-subcutaneous.md).

Targets the behaviors described in [Application Common](/modules/application-common.md).
