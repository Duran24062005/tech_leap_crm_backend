# .NET local tools

## Purpose

Repository-scoped .NET CLI tools.

## Responsibilities

Pins tooling versions so contributors and CI use the same commands.

## What belongs here

- `dotnet-tools.json`.
- Versioned tools required to maintain the solution.

## What does not belong here

- Application dependencies.
- Machine-specific SDKs.
- Secrets or local settings.

## Related documentation

- [Local development](../docs/runbooks/local-development.md).
- [SDK pinning](../global.json).

## Current status

The repository pins `dotnet-ef` for migration commands.
