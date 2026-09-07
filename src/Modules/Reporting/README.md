# Reporting module

## Purpose

Owns the operational reports, filtered exports and read models business capability.

## Responsibilities

- Define the module's business rules.
- Expose application use cases and contracts.
- Own persistence mappings and integration adapters.
- Protect the module boundary.

## What belongs here

- Reporting-specific domain concepts.
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

The Reporting project is scaffolded for Sprint 0 and will be populated according to the sprint plan.
