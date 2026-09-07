# Shared web concerns

## Purpose

Reusable HTTP pipeline components that are not business behavior.

## Responsibilities

Provides correlation propagation shared by API endpoints and future HTTP adapters.

## What belongs here

- Correlation middleware.
- Shared HTTP header constants.
- Transport-level diagnostics helpers.

## What does not belong here

- Endpoint handlers.
- Authentication provider configuration.
- Business authorization policies.

## Related documentation

- [API web adapters](../../../../apps/api/TechLeap.Crm.Api/Web/README.md).
- [Observability architecture](../../../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The middleware accepts or creates `X-Correlation-Id` and returns it in the response.
