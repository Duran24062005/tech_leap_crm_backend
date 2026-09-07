# API application

## Purpose

Groups the ASP.NET Core HTTP application.

## Responsibilities

Provides the HTTP entry point and composes authentication, middleware, health checks, OpenAPI and modules.

## What belongs here

- The API project.
- HTTP-facing host composition.
- API-specific configuration.

## What does not belong here

- Frontend code.
- Module domain models.
- Worker-only processing.

## Related documentation

- [API project](TechLeap.Crm.Api/README.md).
- [API contract](../../docs/api/README.md).

## Current status

The host exposes the Sprint 0 platform endpoints.
