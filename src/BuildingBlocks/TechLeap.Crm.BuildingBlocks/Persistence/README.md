# Shared persistence

## Purpose

Database primitives shared across executable hosts.

## Responsibilities

- Define the platform DbContext.
- Define the Outbox record and mapping.
- Centralize PostgreSQL naming and UTC conventions.

## What belongs here

- `PlatformDbContext`.
- `OutboxMessage`.
- Shared EF Core configurations that are platform-wide.

## What does not belong here

- Module-owned aggregate mappings.
- API-only migration tooling.
- Credentials or connection strings.

## Related documentation

- [API migrations](../../../../apps/api/TechLeap.Crm.Api/Migrations/README.md).
- [Persistence architecture](../../../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The platform schema currently contains the Outbox table and its indexes.
