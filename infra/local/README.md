# Local infrastructure

## Purpose

Reproducible local PostgreSQL dependency for backend development and validation.

## Responsibilities

- Start PostgreSQL 18 with Docker Compose.
- Persist local data in a named volume.
- Expose a healthcheck and configurable host port.
- Keep local credentials in an ignored `.env` file.

## What belongs here

- `compose.yml`.
- `.env.example`.
- Local infrastructure notes.

## What does not belong here

- Production passwords.
- Azure resources.
- Committed secret connection strings.

## Related documentation

- [Local development](../../docs/runbooks/local-development.md).
- [Integration tests](../../tests/TechLeap.Crm.IntegrationTests/README.md).

## Current status

The Compose service uses PostgreSQL 18 and the `tech_leap_crm_postgres` volume.
