# TechLeap.Crm.IntegrationTests

## Purpose

Verifies the running API composition and PostgreSQL integration.

## Responsibilities

- Start the API through `WebApplicationFactory`.
- Use isolated PostgreSQL containers through Testcontainers.
- Verify migrations, health checks, authentication and HTTP contracts.

## What belongs here

- API integration tests.
- EF Core and PostgreSQL scenarios.
- Cross-component behavior not proven in isolation.

## What does not belong here

- Shared developer database dependencies.
- Pure unit tests.
- Production fixtures or real external credentials.

## Related documentation

- [Local infrastructure](../../infra/local/README.md).
- [API contract](../../docs/api/README.md).

## Current status

The project covers health, protected diagnostics and the initial Outbox migration.
