# Worker application

## Purpose

Groups the background processing host.

## Responsibilities

Provides a separately executable process for Outbox publication, automation and future asynchronous integrations.

## What belongs here

- Worker project composition.
- Background-service configuration.
- Worker host documentation.

## What does not belong here

- HTTP endpoints.
- Module domain rules.
- Direct writes to another module's tables.

## Related documentation

- [Worker project](TechLeap.Crm.Worker/README.md).
- [Outbox architecture](../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The Worker has an Outbox polling loop; Azure Service Bus is not connected in Sprint 0.
