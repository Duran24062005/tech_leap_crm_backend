# Source libraries

## Purpose

Reusable backend libraries composed by the executable hosts.

## Responsibilities

Holds shared platform capabilities and module-owned business capabilities.

## What belongs here

- BuildingBlocks.
- Bounded modules.
- Reusable backend abstractions with a clear owner.

## What does not belong here

- Executable host startup.
- Test fixtures.
- Environment secrets.

## Related documentation

- [BuildingBlocks](BuildingBlocks/README.md).
- [Modules](Modules/README.md).
- [Architecture](../docs/design/arquitectura/arquitectura-y-contratos.md).

## Current status

BuildingBlocks contains platform primitives; modules are scaffolds for future business work.
