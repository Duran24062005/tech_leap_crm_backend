# API persistence adapters

## Purpose

API-host persistence integration that is not shared domain behavior.

## Responsibilities

Provides design-time EF Core support and API-specific persistence wiring.

## What belongs here

- `IDesignTimeDbContextFactory` implementations.
- API-specific persistence adapters.
- Migration assembly configuration.

## What does not belong here

- Entity invariants.
- Shared database primitives.
- Controllers or unrelated middleware.

## Related documentation

- [Shared persistence](../../../../src/BuildingBlocks/TechLeap.Crm.BuildingBlocks/Persistence/README.md).
- [Migrations](../Migrations/README.md).

## Current status

The folder contains the design-time factory used by `dotnet ef`.
