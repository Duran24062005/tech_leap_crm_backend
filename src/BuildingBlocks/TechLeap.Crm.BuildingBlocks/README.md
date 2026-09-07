# TechLeap.Crm.BuildingBlocks

## Purpose

Shared backend platform library for the API, Worker and tests.

## Responsibilities

- Provide the platform EF Core DbContext.
- Define the Outbox persistence model.
- Provide correlation middleware.
- Keep technical behavior independent from business modules.

## What belongs here

- Persistence primitives.
- Transport-neutral cross-cutting components.
- Shared abstractions with more than one legitimate consumer.

## What does not belong here

- Module-specific tables and rules.
- Host startup configuration.
- External providers owned by Integrations.

## Related documentation

- [Persistence](Persistence/README.md).
- [Web concerns](Web/README.md).
- [Architecture](../../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

This is the shared platform library for Sprint 0.
