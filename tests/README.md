# Backend tests

## Purpose

Automated tests for the backend solution.

## Responsibilities

Verifies platform behavior, API contracts and persistence without placing tests in production projects.

## What belongs here

- Unit test projects.
- Integration test projects.
- Shared fixtures when needed by multiple test projects.

## What does not belong here

- Production code.
- Real credentials or production data.
- Tests that silently depend on a developer database.

## Related documentation

- [Unit tests](TechLeap.Crm.UnitTests/README.md).
- [Integration tests](TechLeap.Crm.IntegrationTests/README.md).
- [Quality strategy](../docs/design/gestion/calidad-operacion-y-roadmap.md).

## Current status

Coverage includes correlation, health, diagnostics and the initial Outbox migration.
