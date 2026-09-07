# Engagements module

## Purpose

Owns the engagements, checklists, follow-ups and renewals business capability.

## Responsibilities

- Define the module's business rules.
- Expose application use cases and contracts.
- Own persistence mappings and integration adapters.
- Protect the module boundary.

## What belongs here

- Engagements-specific domain concepts.
- Application commands, queries and ports.
- Infrastructure implementations.
- Public contracts and events.

## What does not belong here

- Shared platform primitives; use BuildingBlocks.
- Host startup and middleware.
- Direct writes to another module's tables.

## Related documentation

- [Module boundaries](../README.md).
- [Backend architecture](../../../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

The Engagements project is scaffolded for Sprint 0 and will be populated according to the sprint plan.
