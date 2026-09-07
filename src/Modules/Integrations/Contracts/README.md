# Integrations — Contracts

## Purpose

Contains the public DTOs, integration messages and stable module contracts for the Integrations module.

## Responsibilities

Implements only concerns owned by Integrations and keeps dependencies flowing inward toward the domain.

## What belongs here

- public DTOs, integration messages and stable module contracts.
- Supporting types and tests specific to this layer.
- Explicit dependencies required by the module.

## What does not belong here

- Concerns owned by another module.
- Host startup, HTTP pipeline or frontend code.
- Secrets or environment-specific values.

## Related documentation

- [Integrations module](../README.md).
- [Module boundaries](../../README.md).

## Current status

This is an intentional Sprint 0 scaffold and will be populated with Integrations functionality in its corresponding sprint.
