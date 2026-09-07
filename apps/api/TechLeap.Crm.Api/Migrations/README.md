# API migrations

## Purpose

Versioned Entity Framework Core migrations owned by the API persistence boundary.

## Responsibilities

- Describe schema changes in ordered, reviewable files.
- Keep the platform schema reproducible.
- Preserve the EF Core model snapshot.

## What belongs here

- Versioned migration classes.
- The model snapshot.
- Operational notes for non-obvious migrations.

## What does not belong here

- Manual production SQL as the source of truth.
- Business logic.
- Secrets or connection strings.

## Related documentation

- [Persistence architecture](../../../../docs/design/arquitectura/arquitectura-y-contratos.md).
- [Local development](../../../../docs/runbooks/local-development.md).

## Current status

The initial migration creates the Outbox table and indexes.
