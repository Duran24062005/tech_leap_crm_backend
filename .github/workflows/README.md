# GitHub Actions workflows

## Purpose

Continuous integration workflows for the backend repository.

## Responsibilities

Restore, build, test and inspect the .NET solution in a reproducible environment.

## What belongs here

- Build and test workflows.
- Migration validation.
- Dependency scanning and SAST.

## What does not belong here

- Business logic.
- Credentials.
- Local-only runtime scripts.

## Related documentation

- [Architecture](../../docs/design/arquitectura/arquitectura-y-contratos.md).
- [Quality and operations](../../docs/design/gestion/calidad-operacion-y-roadmap.md).

## Current status

`ci.yml` validates restore, build, unit tests, integration tests, migrations, dependencies and basic SAST.
