# GitHub configuration

## Purpose

Repository-level GitHub configuration for the backend.

## Responsibilities

Contains automation and collaboration settings independent from application runtime.

## What belongs here

- GitHub Actions workflows.
- Repository automation and security configuration.

## What does not belong here

- Application code.
- Secrets or deployment credentials.
- Local runtime settings.

## Related documentation

- [Workflows](workflows/README.md).
- [Local runbook](../docs/runbooks/local-development.md).

## Current status

The backend CI workflow is defined in `.github/workflows/ci.yml`.
