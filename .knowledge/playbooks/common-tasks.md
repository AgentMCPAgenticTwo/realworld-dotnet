---
type: Playbook
title: Common tasks
description: Where to go to add an endpoint, use case, entity, repository or test.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/
tags: [playbook, how-to]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Add a new use case to an existing feature

1. Create `src/CleanArchitecture.Application/<Feature>/Commands/<Name>/` (or `Queries/`).
2. Add `<Name>Command.cs` — a record implementing `IAuthorizeableRequest<ErrorOr<T>>`, decorated
   with `[Authorize(Permissions = …, Policies = …)]` if it is not anonymous.
3. Add `<Name>CommandHandler.cs` using a primary constructor for its repository ports.
4. Add `<Name>CommandValidator.cs` if the input needs validation — it is picked up automatically by
   `AddValidatorsFromAssemblyContaining` in `Application/DependencyInjection.cs`.
5. Add the action to the matching controller in `src/CleanArchitecture.Api/Controllers/` and map
   the result with `result.Match(success, Problem)`.
6. Add a subcutaneous test folder mirroring step 1 under
   `tests/CleanArchitecture.Application.SubcutaneousTests/`.

No DI registration is needed: MediatR scans the Application assembly.

# Add an HTTP endpoint only

Edit the controller in [Api](/modules/api.md), add the DTO records to the matching folder in
[Contracts](/modules/contracts.md), and add a private static `ToDto` if a new shape is returned.

# Add a new entity + persistence

1. `src/CleanArchitecture.Domain/<Feature>/<Entity>.cs`, deriving from `Entity`, with a private
   parameterless constructor for EF.
2. `src/CleanArchitecture.Application/Common/Interfaces/I<Entity>sRepository.cs`.
3. `src/CleanArchitecture.Infrastructure/<Feature>/Persistence/<Entity>Configurations.cs` and
   `<Entity>sRepository.cs`.
4. Register the repository in `AddPersistence` in `Infrastructure/DependencyInjection.cs`.
5. Add the `DbSet` to `Common/Persistence/AppDbContext.cs`.
6. `dotnet ef migrations add Add<Entity>s --project src/CleanArchitecture.Infrastructure`, which
   also updates `Migrations/AppDbContextModelSnapshot.cs`.

`Migrations/20261027000000_AddTutorials.cs` plus the Tutorial files are the most recent worked
example of this whole path — see [Tutorial](/modules/domain-tutorials.md).

# Add a new permission, role or policy

* Permission: add `src/CleanArchitecture.Application/Common/Security/Permissions/Permission.<Feature>.cs`
  as another `partial class Permission` with a nested static class.
* Role: add a constant to `Common/Security/Roles/Role.cs`.
* Policy: add a constant to `Common/Security/Policies/Policy.cs` **and** a branch in
  `Infrastructure/Security/PolicyEnforcer/PolicyEnforcer.cs`. A policy constant without an
  enforcer branch will not be enforced.

# Change how an error maps to a status code

`src/CleanArchitecture.Api/Controllers/ApiController.cs` is the only place `ErrorType` becomes an
HTTP status code.

# React to something after the response is sent

Raise a domain event from the entity (`_domainEvents.Add(...)`) and add an
`INotificationHandler<TEvent>` under `Application/<Feature>/Events/`.
`EventualConsistencyMiddleware` publishes it once the response has been written — see
[Architecture](/architecture.md).

# Control the clock in a test

Substitute `IDateTimeProvider`; the production implementation is
`Infrastructure/Services/SystemDateTimeProvider.cs`.

# Enable reminder emails

Set `EnableEmailNotifications` plus `DefaultFromEmail` and the `SmtpSettings` block in the
`EmailSettings` configuration section (user secrets for credentials). Without the flag,
`AddEmailNotifications` registers nothing — see
[Background Services](/modules/infrastructure-background-services.md).

# Change the allowed frontend origin

`AllowFrontendCorsPolicy` in `src/CleanArchitecture.Api/DependencyInjection.cs`, currently pinned
to `http://localhost:4200`.
