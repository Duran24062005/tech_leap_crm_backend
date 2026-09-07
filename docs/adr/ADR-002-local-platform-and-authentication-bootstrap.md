# ADR-002: Local platform and authentication bootstrap

## Status

Accepted — 2026-09-06

## Decision

- Run PostgreSQL 18 locally with Docker Compose.
- Use a locally signed JWT in Development.
- Keep the local token endpoint available only in Development and exclude it from OpenAPI.
- In non-local environments, validate JWTs through Auth0 using `Auth0:Authority` and `Auth0:Audience`.
- Keep Azure Service Bus, Blob Storage and Application Insights as prepared adapters, not Sprint 0 dependencies.
- Use Testcontainers PostgreSQL for integration tests.

## Security rules

- The local signing key is development-only and must never contain real credentials.
- The local token endpoint must not be registered outside Development.
- `.env` files and secrets are not versioned.
- Diagnostics require JWT authentication.
- The frontend must not be treated as an authorization boundary.

## Consequences

- Developers can run the platform without an external identity provider.
- Production-like environments can adopt Auth0 through configuration.
- Integration tests are isolated from a developer's shared database.
- Real Azure integrations remain a deliberate follow-up rather than hidden local dependencies.

## Related documentation

- [Local infrastructure](../../infra/local/README.md).
- [Local runbook](../runbooks/local-development.md).
- [API contract](../api/README.md).
