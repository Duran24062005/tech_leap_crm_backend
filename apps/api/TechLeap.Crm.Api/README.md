# TechLeap.Crm.Api

## Purpose

Executable ASP.NET Core API for Tech Leap CRM.

## Responsibilities

- Compose BuildingBlocks and CRM modules.
- Configure JWT authentication and CORS.
- Expose health, diagnostics and OpenAPI endpoints.
- Apply correlation and centralized error handling.
- Own the API migration assembly.

## What belongs here

- `Program.cs`.
- HTTP pipeline configuration.
- Host-specific configuration.
- API-owned EF migrations.
- HTTP exception and health adapters.

## What does not belong here

- Module business rules.
- Frontend presentation logic.
- Azure provider implementations owned by Integrations.

## Related documentation

- [API contract](../../../docs/api/README.md).
- [Migrations](Migrations/README.md).
- [Persistence](Persistence/README.md).
- [Web concerns](Web/README.md).

## Current status

The API exposes the Sprint 0 health, diagnostics and development-token endpoints.
