---
type: Module
title: tests/TestCommon
description: Shared object-mother factories, test constants and NSubstitute helpers consumed by every test project.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/tests/TestCommon
tags: [tests, fixtures, factories]
generated: { by: sage-maintenance/claude-code, at: 2026-10-27T12:15:02Z }
sources:
  - id: cbm-graph
    resource: codebase-memory-mcp project realworld-dotnet @ 40766801559612ce58c0226d1768da27f4b2e358
    title: Codebase knowledge graph
status: stable
---

# Contents

| Area | Files |
| --- | --- |
| Reminders | `ReminderFactory.cs`, `ReminderCommandFactory.cs`, `ReminderQueyFactory.cs` (name is misspelled in the repo), `MediatorExtensions.cs`, `ReminderValidationsExtensions.cs` |
| Subscriptions | `SubscriptionFactory.cs`, `SubscriptionCommandFactory.cs`, `SubscriptionQueryFactory.cs`, `MediatorExtensions.cs`, `SubscriptionValidationExtensions.cs` |
| Tutorials | `TutorialFactory.cs`, `TutorialCommandFactory.cs`, `TutorialQueryFactory.cs` |
| Users | `UserFactory.cs` |
| Security | `CurrentUserFactory.cs`, `TestCurrentUserProvider.cs` |
| Constants | `TestConstants/Constants.Reminder.cs`, `Constants.Subscription.cs`, `Constants.Tutorial.cs`, `Constants.User.cs` |
| Utilities | `TestUtilities/NSubstitute/Must.cs` |

# Notes

* Factories expose `Create…` methods with optional overrides (`CreateSetReminderCommand`,
  `CreateCurrentUser`, `CreateCreateTutorialCommand`, …) so tests state only the field they care
  about.
* `MediatorExtensions` wrap common multi-step arrangements such as `CreateSubscriptionAsync` and
  `SetReminderAsync`, which is why the graph shows 218 calls from source clusters into this
  package.
* `TestCurrentUserProvider` substitutes `ICurrentUserProvider` so authorization tests can assume an
  identity without a real token.
* `Constants` is a partial static class split per entity, mirroring the `Permission` convention in
  [Conventions](/conventions.md).

Consumed by [domain unit tests](/modules/tests-domain-unittests.md),
[application unit tests](/modules/tests-application-unittests.md),
[subcutaneous tests](/modules/tests-subcutaneous.md) and
[integration tests](/modules/tests-api-integrationtests.md).
