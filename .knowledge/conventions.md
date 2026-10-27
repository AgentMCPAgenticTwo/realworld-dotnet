---
type: Convention
title: Solution conventions
description: Naming, folder, result-handling, authorization and testing conventions used across the solution.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src
tags: [conventions, csharp, cqrs]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Use-Case Folder Shape

Each use case is a folder under `<Feature>/Commands/<Name>/` or `<Feature>/Queries/<Name>/`
holding one file per type:

| File | Example |
| --- | --- |
| `<Name>Command.cs` / `<Name>Query.cs` | `CreateTutorialCommand.cs` |
| `<Name>CommandHandler.cs` / `<Name>QueryHandler.cs` | `CreateTutorialCommandHandler.cs` |
| `<Name>CommandValidator.cs` (optional) | `CreateTutorialCommandValidator.cs` |

Requests are records implementing `IAuthorizeableRequest<ErrorOr<T>>`. Handlers are classes using
primary constructors for injection (`public class XHandler(IRepo _repo)`), with the underscore
prefix kept on the parameter name so the body reads like field access.

# Results and Errors

* Handlers return `ErrorOr<T>`; `Success`/`Deleted`/`Updated` are used where no payload exists.
* Domain errors are declared as static members on an errors class, e.g. `Domain/Users/UserErrors.cs`.
* Controllers never build status codes directly; they call `result.Match(success, Problem)` and let
  `ApiController` map `ErrorType` to HTTP.
* `Throw` guard extensions (`.Throw().IfNotEquals(...)`) express invariants inside entities.

# Domain Entities

* Every entity derives from `Domain/Common/Entity.cs`, which owns `Id` and `_domainEvents`.
* Setters are `private set`; state changes go through intention-revealing methods
  (`User.SetReminder`, `Tutorial.Update`).
* A private parameterless constructor exists purely for EF Core materialization.
* Domain events are appended to `_domainEvents` and drained by `PopDomainEvents()`.

# Contracts

Wire DTOs are records in `src/CleanArchitecture.Contracts`, grouped by feature folder and named
`<Thing>Request` / `<Thing>Response`. Controllers map domain or result types to them with private
static `ToDto` methods. The contracts enum `Contracts/Common/SubscriptionType.cs` is deliberately
distinct from the domain smart enum `Domain/Users/SubscriptionType.cs`; controllers alias them
(`using DomainSubscriptionType = ...`) and convert explicitly.

# Authorization

Three composable mechanisms, all declared on the request type via `AuthorizeAttribute`:

| Type | Constant source |
| --- | --- |
| Role | `Application/Common/Security/Roles/Role.cs` (`Admin`) |
| Permission | `Application/Common/Security/Permissions/Permission.*.cs` (e.g. `set:reminder`) |
| Policy | `Application/Common/Security/Policies/Policy.cs` (`SelfOrAdminPolicy`) |

Permissions are nested static classes per feature in partial `Permission` files, so a new feature
adds `Permission.<Feature>.cs` rather than editing a shared list.

# Persistence

* One `IEntityTypeConfiguration` per aggregate, named `<Entity>Configurations.cs`, living beside the
  repository in `Infrastructure/<Feature>/Persistence/`.
* Repositories implement the Application-layer interface and call `AppDbContext.SaveChangesAsync`
  themselves.
* Migrations are in `Infrastructure/Migrations/` with the EF timestamp prefix.

# Testing

* `TestCommon` holds `…Factory` classes (`TutorialFactory`, `ReminderCommandFactory`,
  `CurrentUserFactory`, …) and `Constants.<Entity>.cs` partial constant classes; tests never
  hand-roll fixtures.
* Test names follow `Method_WhenCondition_ShouldOutcome`.
* `GlobalUsings.cs` in each test project centralizes `Xunit`, `FluentAssertions` and NSubstitute
  imports, so individual test files carry few usings.
