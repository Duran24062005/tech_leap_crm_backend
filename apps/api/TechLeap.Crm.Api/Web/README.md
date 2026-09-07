# API web adapters

## Purpose

HTTP pipeline adapters that support the API without owning business use cases.

## Responsibilities

- Centralized exception handling.
- PostgreSQL readiness checks.
- Other transport-level concerns.

## What belongs here

- ASP.NET Core exception handlers.
- Health-check implementations.
- HTTP-specific middleware adapters.

## What does not belong here

- Module authorization rules.
- Database entities.
- Frontend components.

## Related documentation

- [API contract](../../../../docs/api/README.md).
- [Observability architecture](../../../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The folder contains the centralized Problem Details handler and PostgreSQL health check.
