# Modules

# Presentation

* [src/CleanArchitecture.Api](/modules/api.md) - ASP.NET Core presentation layer hosting the controllers, CORS policy and application host.
* [src/CleanArchitecture.Contracts](/modules/contracts.md) - Wire DTO records shared between the API and its clients, grouped per feature.

# Application

* [src/CleanArchitecture.Application/Common](/modules/application-common.md) - MediatR pipeline behaviors, outbound port interfaces and the declarative security vocabulary.
* [src/CleanArchitecture.Application/Reminders](/modules/application-reminders.md) - Reminder use cases — set, dismiss, delete, get and list — plus the reminder domain event handlers.
* [src/CleanArchitecture.Application/Subscriptions](/modules/application-subscriptions.md) - Subscription use cases for creating, cancelling and reading a user's subscription.
* [src/CleanArchitecture.Application/Tokens](/modules/application-tokens.md) - Single query that mints a JWT carrying the caller's identity, subscription, roles and permissions.
* [src/CleanArchitecture.Application/Tutorials](/modules/application-tutorials.md) - Anonymous CRUD use cases over the Tutorial aggregate, including bulk delete, title search and optional category assignment.
* [src/CleanArchitecture.Application/Categories](/modules/application-categories.md) - Anonymous create and list use cases over the Category aggregate, plus the existence check tutorials rely on.

# Domain

* [src/CleanArchitecture.Domain/Common](/modules/domain-common.md) - Entity base class owning identity and the queued domain events, plus the IDomainEvent marker.
* [src/CleanArchitecture.Domain/Users](/modules/domain-users.md) - The User aggregate root with its subscription, reminder calendar, daily limits and domain events.
* [src/CleanArchitecture.Domain/Reminders](/modules/domain-reminders.md) - The Reminder entity — an immutable scheduled note that can be dismissed exactly once.
* [src/CleanArchitecture.Domain/Tutorials](/modules/domain-tutorials.md) - The Tutorial entity — a published/unpublished article with timestamps and an optional category reference.
* [src/CleanArchitecture.Domain/Categories](/modules/domain-categories.md) - The Category entity — a named lookup value that tutorials may optionally point at.

# Infrastructure

* [Persistence](/modules/infrastructure-persistence.md) - EF Core PostgreSQL DbContext, repositories, entity configurations, value converters and the eventual-consistency middleware.
* [Security](/modules/infrastructure-security.md) - JWT generation and validation, current-user resolution and the role/permission/policy enforcer.
* [Background Services](/modules/infrastructure-background-services.md) - Opt-in hosted service that emails users about due reminders, plus its SMTP settings.
* [Services](/modules/infrastructure-services.md) - Ambient-dependency implementations, currently the system clock behind IDateTimeProvider.

# Tests

* [tests/TestCommon](/modules/tests-testcommon.md) - Shared object-mother factories, test constants and NSubstitute helpers consumed by every test project.
* [tests/CleanArchitecture.Domain.UnitTests](/modules/tests-domain-unittests.md) - Fast in-memory tests asserting entity invariants and raised domain events.
* [tests/CleanArchitecture.Application.UnitTests](/modules/tests-application-unittests.md) - Isolated tests for the MediatR authorization and validation pipeline behaviors.
* [tests/CleanArchitecture.Application.SubcutaneousTests](/modules/tests-subcutaneous.md) - Mediator-level tests running the real pipeline and EF Core against a per-test SQLite database.
* [tests/CleanArchitecture.Api.IntegrationTests](/modules/tests-api-integrationtests.md) - HTTP-level tests driving the real controllers through WebApplicationFactory and a Testcontainers PostgreSQL database.
