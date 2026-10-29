---
type: Module
title: src/CleanArchitecture.Api
description: ASP.NET Core presentation layer hosting the controllers, CORS policy and application host.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Api
tags: [api, aspnetcore, controllers, presentation]
generated: { by: sage-maintenance/claude-code, at: 2026-10-29T09:46:50Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 77aee7bc72e63696dcba05b7d841c54d863481b2
    title: Codebase knowledge graph
status: stable
---

# Responsibility

Translate HTTP into MediatR requests and `ErrorOr` results back into HTTP. No business logic lives
here; controllers build a command or query, `Send` it, and `Match` the result.

# Files

| File | Contents |
| --- | --- |
| `Program.cs` | Host composition and middleware order |
| `DependencyInjection.cs` | `AddPresentation`, `AllowFrontendCorsPolicy` = `"AllowFrontend"` |
| `IAssemblyMarker.cs` | Assembly marker used by test hosts to locate this assembly |
| `Controllers/ApiController.cs` | `[ApiController] [Authorize]` base with `Problem(List<Error>)` |
| `Controllers/TokensController.cs` | `tokens` |
| `Controllers/SubscriptionsController.cs` | `users/{userId:guid}/subscriptions` |
| `Controllers/RemindersController.cs` | `users/{userId:guid}/subscriptions/{subscriptionId:guid}/reminders` |
| `Controllers/TutorialsController.cs` | `api/tutorials` |
| `Controllers/HealthController.cs` | `api/health` |

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
| `GET api/health` | `HealthController.GetHealth` | nothing — returns `HealthResponse("ok")` directly |

# Notes

* `TokensController`, `TutorialsController` and `HealthController` are `[AllowAnonymous]`; the
  other two inherit `[Authorize]` from `ApiController`.
* `HealthController.GetHealth` is the only action that never touches MediatR — it is a liveness
  probe answering `200 OK` with a constant body.
* Both `TokensController` and `SubscriptionsController` short-circuit with a 400 `Problem` when
  `DomainSubscriptionType.TryFromName` fails, before any mediator call.
* `UpdateTutorial` and `DeleteAllTutorials` answer with `MessageResponse`; `DeleteTutorial`
  answers `204 No Content`.
* CORS is restricted to `http://localhost:4200`, the companion frontend origin.

Depends on [Contracts](/modules/contracts.md) for DTOs and on the Application feature modules for
requests. Error mapping behavior is described in [Conventions](/conventions.md).
