# BuildingBlocks

## Purpose

Small cross-cutting capabilities shared by backend hosts and modules.

## Responsibilities

Provides platform primitives without owning a CRM business concept.

## What belongs here

- Shared persistence infrastructure.
- Outbox primitives.
- Correlation and transport-neutral helpers.

## What does not belong here

- Business entities.
- Module-specific workflows.
- A catch-all utility library with unclear ownership.

## Related documentation

- [BuildingBlocks project](TechLeap.Crm.BuildingBlocks/README.md).
- [Module boundaries](../Modules/README.md).

## Current status

The project contains the platform DbContext, Outbox model and correlation middleware.
