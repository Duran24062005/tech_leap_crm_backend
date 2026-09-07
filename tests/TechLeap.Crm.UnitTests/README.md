# TechLeap.Crm.UnitTests

## Purpose

Fast, isolated unit tests for backend components.

## Responsibilities

Verifies deterministic behavior without PostgreSQL, Docker or external services.

## What belongs here

- Middleware tests.
- Domain invariant tests.
- Application use-case tests.
- Pure mapping and validation tests.

## What does not belong here

- Full HTTP host tests.
- Database-dependent scenarios.
- External provider integration tests.

## Related documentation

- [Integration tests](../TechLeap.Crm.IntegrationTests/README.md).
- [Testing strategy](../../docs/design/gestion/calidad-operacion-y-roadmap.md).

## Current status

Correlation middleware behavior is covered. Module tests will be added with functional work.
