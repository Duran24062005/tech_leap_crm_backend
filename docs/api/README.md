# API contract

## Purpose

This is the canonical entry point for the backend HTTP contract consumed by the independent frontend repository.

## Contract conventions

- Base path for business APIs: `/api/v1`.
- Format: JSON UTF-8.
- Property naming: camelCase.
- Authentication: Bearer JWT.
- Correlation header: `X-Correlation-Id`.
- Errors: `application/problem+json`.
- Error responses may include `type`, `title`, `status`, `detail`, `traceId`, `correlationId` and field-level errors.

## Sprint 0 endpoints

| Method | Route | Authentication | Purpose |
| --- | --- | --- | --- |
| GET | `/health/live` | Anonymous | Process liveness without a database dependency. |
| GET | `/health/ready` | Anonymous | PostgreSQL readiness. |
| GET | `/api/v1/diagnostics` | JWT required | Non-sensitive service diagnostics and trace identifiers. |
| GET | `/openapi/v1.json` | Development | OpenAPI document. |
| POST | `/api/v1/dev/token` | Development only | Local JWT issuance; excluded from OpenAPI. |

## Diagnostics response

The diagnostics endpoint returns:

```json
{
  "service": "tech-leap-crm-api",
  "version": "0.1.0",
  "environment": "Development",
  "utc": "2026-09-06T00:00:00Z",
  "traceId": "request-trace-id",
  "correlationId": "client-correlation-id"
}
```

Values are informational and must not contain secrets, tokens, PII or connection details.

## Authentication

Development uses the local JWT configuration:

- `Jwt:Issuer`.
- `Jwt:Audience`.
- `Jwt:SigningKey`.

When `Auth0:Authority` and `Auth0:Audience` are configured, the API validates tokens through Auth0. The frontend never replaces backend authorization; it only presents the user's access token and renders the result.

## What belongs here

- Stable routes.
- Request and response schemas.
- Authentication and error contract changes.
- Compatibility notes.
- OpenAPI-related decisions.

## What does not belong here

- Internal domain implementation.
- Database-only details.
- Secrets.
- Temporary endpoint experiments.

## Related documentation

- [Architecture and contracts](../design/arquitectura/arquitectura-y-contratos.md).
- [Frontend architecture](../../../tech_leap_crm_frontend/docs/architecture.md).
- [Local runbook](../runbooks/local-development.md).

## Current status

Functional module contracts will be added with their respective sprints.
