---
type: Module
title: src/CleanArchitecture.Api
description: ASP.NET Core presentation layer hosting the controllers, CORS policy and application host.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Api
tags: [api, aspnetcore, controllers, presentation]
generated: { by: sage-maintenance/claude-code, at: 2026-10-28T11:49:52Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ d56e8561a0fe55a0a123dab492ae019f009f7375
    title: Codebase knowledge graph
status: stable
---

# Responsibility

Translate HTTP into MediatR requests and `ErrorOr` results back into HTTP. No business logic lives
here; controllers build a command or query, `Send` it, and `Match` the result.

# Files

| File | Contents |
| --- | --- |
| `Program.cs` | Host composition and middleware order, including `app.ApplyMigrations()` before Swagger |
| `DependencyInjection.cs` | `AddPresentation`, `AllowFrontendCorsPolicy` = `"AllowFrontend"` |
| `IAssemblyMarker.cs` | Assembly marker used by test hosts to locate this assembly |
| `Controllers/ApiController.cs` | `[ApiController] [Authorize]` base with `Problem(List<Error>)` |
| `Controllers/TokensController.cs` | `tokens` |
| `Controllers/SubscriptionsController.cs` | `users/{userId:guid}/subscriptions` |
| `Controllers/RemindersController.cs` | `users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders` |
| `Controllers/TutorialsController.cs` | `api/tutorials` |
| `Controllers/CategoriesController.cs` | `api/categories` |

# Endpoints

| Method + path | Action | Dispatches |
| --- | --- | --- |
| `POST tokens/generate` | `TokensController.GenerateToken` | `GenerateTokenQuery` |
| `POST users/{userId}/subscriptions` | `SubscriptionsController.CreateSubscription` | `CreateSubscriptionCommand` |
| `GET users/{userId}/subscriptions` | `SubscriptionsController.GetSubscription` | `GetSubscriptionQuery` |
| `DELETE users/{userId}/subscriptions/{subscriptionId}` | `SubscriptionsController.DeleteSubscription` | `CancelSubscriptionCommand` |
| `POST .../reminders` | `RemindersController.CreateReminder` | `SetReminderCommand` |
| `GET .../reminders` | `RemindersController.ListReminders` | `ListRemindersQuery` |
| `GET .../reminders/{reminderId}` | `RemindersController.GetReminder` | `GetReminderQuery` |
| `POST .../reminders/{reminderId}/dismiss` | `RemindersController.DismissReminder` | `DismissReminderCommand` |
| `DELETE .../reminders/{reminderId}` | `RemindersController.DeleteReminder` | `DeleteReminderCommand` |
| `GET api/tutorials?title=` | `TutorialsController.ListTutorials` | `ListTutorialsQuery` |
| `GET api/tutorials/{tutorialId}` | `TutorialsController.GetTutorial` | `GetTutorialQuery` |
| `POST api/tutorials` | `TutorialsController.CreateTutorial` | `CreateTutorialCommand` |
| `PUT api/tutorials/{tutorialId}` | `TutorialsController.UpdateTutorial` | `UpdateTutorialCommand` |
| `DELETE api/tutorials/{tutorialId}` | `TutorialsController.DeleteTutorial` | `DeleteTutorialCommand` |
| `DELETE api/tutorials` | `TutorialsController.DeleteAllTutorials` | `DeleteAllTutorialsCommand` |
| `GET api/categories` | `CategoriesController.ListCategories` | `ListCategoriesQuery` |
| `POST api/categories` | `CategoriesController.CreateCategory` | `CreateCategoryCommand` |

# Notes

* `TokensController`, `TutorialsController` and `CategoriesController` are `[AllowAnonymous]`; the
  other two inherit `[Authorize]` from `ApiController`.
* `CategoriesController.CreateCategory` answers `201 Created` via
  `CreatedAtAction(nameof(ListCategories), routeValues: null, …)` — there is no get-by-id action to
  point at, so the `Location` header resolves to the collection.
* `CreateTutorial` and `UpdateTutorial` accept an optional `CategoryId`; an unknown id surfaces as
  a 404 `Problem` because the handler returns `Error.NotFound`.
* Both `TokensController` and `SubscriptionsController` short-circuit with a 400 `Problem` when
  `DomainSubscriptionType.TryFromName` fails, before any mediator call.
* `UpdateTutorial` and `DeleteAllTutorials` answer with `MessageResponse`; `DeleteTutorial`
  answers `204 No Content`.
* CORS is restricted to `http://localhost:4200`, the companion frontend origin.

Depends on [Contracts](/modules/contracts.md) for DTOs and on the Application feature modules for
requests. Error mapping behavior is described in [Conventions](/conventions.md).
