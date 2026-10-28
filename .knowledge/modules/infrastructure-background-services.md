---
type: Module
title: src/CleanArchitecture.Infrastructure/Reminders/BackgroundServices
description: Opt-in hosted service that emails users about due reminders, plus its SMTP settings.
resource: https://github.com/AgentMCPAgenticTwo/realworld-dotnet/tree/main/src/CleanArchitecture.Infrastructure/Reminders/BackgroundServices
tags: [infrastructure, background-service, email, smtp]
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
| `ReminderEmailBackgroundService.cs` | `ReminderEmailBackgroundService` | Hosted service that polls for due reminders and sends mail through FluentEmail |
| `EmailSettings.cs` | `EmailSettings` | `Section` name, `EnableEmailNotifications`, `DefaultFromEmail`, nested `SmtpSettings` |
| `SmtpSettings.cs` | `SmtpSettings` | `Server`, `Port`, `Username`, `Password` |

# Registration

`Infrastructure/DependencyInjection.cs` → `AddBackgroundServices` → `AddEmailNotifications`:

1. Binds the `EmailSettings.Section` configuration section.
2. **Returns early if `EnableEmailNotifications` is false** — neither the hosted service nor
   FluentEmail is registered.
3. Otherwise `AddHostedService<ReminderEmailBackgroundService>()` and
   `AddFluentEmail(DefaultFromEmail).AddSmtpSender(...)` with a `SmtpClient` built from
   `SmtpSettings` and `NetworkCredential`.

# Notes

The early return is the switch to look for when email "does nothing": with the flag off, no
`IFluentEmail` is in the container at all, so a later injection of it would fail rather than
silently no-op. Credentials belong in user secrets, not `appsettings.json`.

Reads reminders produced by [Reminders use cases](/modules/application-reminders.md) through the
repositories in [Persistence](/modules/infrastructure-persistence.md).
