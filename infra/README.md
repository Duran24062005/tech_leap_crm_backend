# Infrastructure

## Purpose

Infrastructure definitions and environment-specific operational assets.

## Responsibilities

Separates local infrastructure from future cloud provisioning and deployment configuration.

## What belongs here

- Local Compose definitions.
- Future Bicep or deployment assets grouped by environment.
- Infrastructure operational documentation.

## What does not belong here

- Application business logic.
- Secrets.
- Generated deployment artifacts.

## Related documentation

- [Local infrastructure](local/README.md).
- [Local runbook](../docs/runbooks/local-development.md).

## Current status

Only local PostgreSQL infrastructure is implemented in Sprint 0.
