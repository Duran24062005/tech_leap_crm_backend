# Deployable applications

## Purpose

Executable hosts that can be run and deployed independently.

## Responsibilities

Compose shared components and modules into runnable backend processes.

## What belongs here

- HTTP API hosts.
- Background worker hosts.
- Host-specific startup and configuration.

## What does not belong here

- Module domain rules.
- Shared persistence primitives.
- Test projects.

## Related documentation

- [API](api/README.md).
- [Worker](worker/README.md).
- [Architecture](../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The backend has an ASP.NET Core API and a Worker Service.
