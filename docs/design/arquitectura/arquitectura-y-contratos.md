# Tech Leap CRM — Backend architecture and contracts

This document is the technical source of truth for the backend repository and its HTTP boundary with the independent Next.js frontend.

## 1. System shape

Tech Leap CRM is an API-first modular monolith. It is one backend solution with explicit business modules and two executable hosts:

```text
+-----------------------------+
| Independent Next.js frontend|
+--------------+--------------+
               | HTTP/JSON
               | /api/v1
               v
+-----------------------------+
| ASP.NET Core API            |
| auth, middleware, endpoints |
+--------------+--------------+
               |
               v
+-----------------------------+
| PostgreSQL 18               |
| module data + platform data |
| Outbox                      |
+--------------+--------------+
               |
               | future message publication
               v
+-----------------------------+
| .NET Worker Service         |
| polling, retries, consumers |
+--------------+--------------+
               |
               v
       Azure Service Bus
       (future integration)
```

The frontend and backend remain separate repositories. The backend owns HTTP contracts, authentication enforcement and business authorization. The frontend owns presentation and user interaction.

## 2. Repository topology

```text
apps/
  api/TechLeap.Crm.Api/             executable HTTP host
  worker/TechLeap.Crm.Worker/       executable background host
src/
  BuildingBlocks/
    TechLeap.Crm.BuildingBlocks/    shared platform library
  Modules/
    Identity/ CRM/ ATS/              business libraries
    Engagements/ Billing/ Documents/
    Work/ Integrations/ Reporting/
tests/
  TechLeap.Crm.UnitTests/
  TechLeap.Crm.IntegrationTests/
infra/local/                         PostgreSQL Compose
docs/                                architecture, contracts and operations
```

Dependency direction:

```text
API host --------+
Worker host -----+----> modules ----> BuildingBlocks
Integration tests+----> API and BuildingBlocks
Unit tests ------+----> the smallest unit under test
```

Hosts compose modules; modules do not depend on hosts. BuildingBlocks contains only cross-cutting technical primitives and must not become a business catch-all.

## 3. Module boundaries

Each module is a separate project with four internal layers:

```text
Module
  Domain          entities, invariants and domain policies
  Application     use cases, commands, queries, validation and ports
  Infrastructure persistence and provider adapters
  Contracts       public DTOs, messages and integration contracts
```

Ownership:

| Module | Owns |
| --- | --- |
| Identity | Users, roles, permissions and memberships. |
| CRM | Companies, contacts, opportunities and requirements. |
| ATS | Vacancies, candidates, skills, applications and interviews. |
| Engagements | Engagements, checklists, follow-ups and renewals. |
| Billing | Operational invoices and collection status. |
| Documents | Private documents, versions and access policies. |
| Work | Tasks, activities, notifications and status history. |
| Integrations | External connections, webhooks, cursors and mappings. |
| Reporting | Operational reports and filtered exports. |

A module must not write another module's tables or call another module's internal classes. Cross-module behavior uses explicit contracts, application ports or events.

## 4. Request lifecycle

The intended use-case flow is:

```text
HTTP request
  -> correlation id accepted or generated
  -> authentication
  -> authorization
  -> request validation
  -> application command/query
  -> domain rules
  -> transaction
  -> module persistence
  -> Outbox record in the same transaction
  -> HTTP response
```

The frontend may hide or disable actions for usability, but it never replaces backend authorization or validation.

## 5. API boundary

The API uses JSON UTF-8, camelCase properties and versioned business routes under `/api/v1`.

Sprint 0 exposes:

- `GET /health/live`: liveness without a database check.
- `GET /health/ready`: readiness with the PostgreSQL check.
- `GET /api/v1/diagnostics`: authenticated, non-sensitive platform diagnostics.
- `GET /openapi/v1.json`: OpenAPI in Development.
- `POST /api/v1/dev/token`: Development-only local token issuance.

Diagnostics include service, version, environment, UTC timestamp, `traceId` and `correlationId`. The endpoint must never expose secrets, tokens, connection strings or personal data.

Errors use `application/problem+json`. Centralized handling adds `traceId` and `correlationId` when available. The exact public contract is maintained in [docs/api](../../api/README.md).

## 6. Authentication and configuration

Development uses a symmetric local JWT configured through:

- `ConnectionStrings__Default`.
- `Jwt__Issuer`.
- `Jwt__Audience`.
- `Jwt__SigningKey`.
- `AllowedOrigins__Frontend`.

Auth0-ready environments additionally use:

- `Auth0__Authority`.
- `Auth0__Audience`.

When both Auth0 values are configured, the API uses the Auth0 authority and audience. Otherwise, it uses the local JWT settings. The Development token endpoint is not registered outside Development.

Secrets are supplied by environment-specific secret stores or ignored local files. They are never committed.

## 7. Persistence

PostgreSQL is the source of truth. EF Core with Npgsql maps the platform schema.

Current conventions:

- PostgreSQL `timestamptz` columns.
- UTC timestamps in application code.
- Consistent snake_case database names.
- JSON payloads in `jsonb` where appropriate.
- Indexes for pending Outbox processing and correlation lookup.
- Versioned migrations owned by the API migration assembly.

The first migration creates `outbox_messages`. Module migrations and mappings must preserve module ownership and be reviewed with the corresponding domain change.

## 8. Outbox and Worker

The Outbox record is written in the same transaction as the state change that produced it. This prevents a successful database change from losing its integration message.

The Worker is a separate process responsible for future:

- Outbox polling.
- Idempotent publication.
- Retry handling.
- Dead-letter behavior.
- Azure Service Bus delivery.

Sprint 0 only provides the persistence model and hosted-service polling boundary. It does not connect to Azure Service Bus.

## 9. Observability

Every request receives or propagates `X-Correlation-Id`. ASP.NET Core's trace identifier is exposed as `traceId` in diagnostics and Problem Details. Correlation is returned in the response header.

Future observability work may add OpenTelemetry, structured metrics and Application Insights. No real Application Insights dependency is required for Sprint 0.

## 10. Local infrastructure

`infra/local/compose.yml` starts PostgreSQL 18 with:

- Persistent named volume.
- Configurable host port.
- Healthcheck.
- Local network.
- Environment values supplied by an ignored `.env` created from `.env.example`.

The API and Worker connect through `ConnectionStrings:Default`. Integration tests use isolated Testcontainers PostgreSQL instances instead of the shared local database.

## 11. Testing and CI

Unit tests validate isolated components such as middleware and future domain invariants.

Integration tests start the API with `WebApplicationFactory`, start PostgreSQL through Testcontainers and validate:

- Liveness.
- Readiness.
- Authentication.
- Diagnostics and correlation.
- Migrations and Outbox persistence.

Backend CI validates restore, build, unit tests, integration tests, migration scripts, dependency vulnerabilities and basic SAST.

## 12. Current limits

Sprint 0 intentionally does not implement:

- Company, Opportunity, Requirement or ATS use cases.
- Engagement or Billing workflows.
- Real Auth0 tenant integration.
- Azure Service Bus.
- Azure Blob Storage.
- Application Insights.
- Production infrastructure.

Functional modules must follow the boundaries and rules in this document as they are introduced.
