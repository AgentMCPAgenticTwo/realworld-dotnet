---
type: Playbook
title: Common tasks
description: Where to go to add an endpoint, use case, entity, repository or test.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/
tags: [playbook, how-to]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Add a new use case to an existing feature

1. Create `src/CleanArchitecture.Application/<Feature>/Commands/<Name>/` (or `Queries/`).
2. Add `<Name>Command.cs` — a record implementing `IAuthorizeableRequest<ErrorOr<T>>`, decorated
   with `[Authorize(Permissions = …, Policies = …)]`. For an anonymous feature such as tutorials or
   categories, use a plain `IRequest<ErrorOr<T>>` instead: `AuthorizationBehavior` then skips the
   request while `ValidationBehavior` still runs.
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

The `Category` files are the most recent worked example of this whole path, end to end:
`Domain/Categories/Category.cs`, `Application/Common/Interfaces/ICategoriesRepository.cs`,
`Application/Categories/`, `Infrastructure/Categories/Persistence/`, `Contracts/Categories/`,
`Api/Controllers/CategoriesController.cs` and the matching test folders — see
[Category](/modules/domain-categories.md) and
[Categories use cases](/modules/application-categories.md).

# Relate one entity to another

`Tutorial`→`Category` is the worked example:

1. Add the foreign-key property to the owning entity (`Guid? CategoryId` on `Tutorial`) and thread
   it through the constructor and the `Update` method. Keep it a bare id — this codebase uses no
   navigation properties.
2. Declare the relationship in the owning entity's configuration:
   `builder.HasOne<Category>().WithMany().HasForeignKey(t => t.CategoryId).OnDelete(...)`.
3. Validate existence in the handlers, not the domain: inject the other repository and return
   `Error.NotFound` when the id is unknown (`CreateTutorialCommandHandler`,
   `UpdateTutorialCommandHandler`).
4. Add the id to the request and response records in [Contracts](/modules/contracts.md) with a
   `= null` default so existing clients keep working.
5. Give the TestCommon factories an optional `categoryId` parameter so existing tests are
   unaffected.

# Change the database or its schema

* Provider and connection string: `AddPersistence` in `Infrastructure/DependencyInjection.cs`
  (`UseNpgsql`, `ConnectionStrings:DefaultConnection`), with values in `appsettings*.json` and
  `.env.example`.
* Schema: add an EF migration (see above). `RequestPipeline.ApplyMigrations` applies it at startup,
  but only when `Database.IsNpgsql()`.
* Integration tests migrate a throwaway container in
  `PostgresTestDatabase.ResetDatabase` (`EnsureDeleted` then `Migrate`), so they need Docker;
  subcutaneous tests still build a SQLite schema from the model and never read `Migrations/`.

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
