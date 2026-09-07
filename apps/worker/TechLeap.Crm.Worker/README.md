# TechLeap.Crm.Worker

## Purpose

Executable .NET Worker Service for asynchronous CRM processing.

## Responsibilities

- Start the background processing host.
- Read pending platform work through shared persistence.
- Provide the future boundary for publication and retries.

## What belongs here

- `Program.cs`.
- Hosted services.
- Worker-specific configuration and scheduling.

## What does not belong here

- HTTP handling.
- Domain entities.
- Synchronous API response logic.

## Related documentation

- [Outbox persistence](../../../src/BuildingBlocks/TechLeap.Crm.BuildingBlocks/Persistence/README.md).
- [Local runbook](../../../docs/runbooks/local-development.md).

## Current status

The publisher logs its polling cycle as a Sprint 0 adapter. Real Azure Service Bus delivery is future work.
