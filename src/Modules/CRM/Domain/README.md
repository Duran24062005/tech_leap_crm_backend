# CRM — Domain

## Purpose

Contains the business entities, value objects, invariants and domain policies for the CRM module.

## Responsibilities

Implements only concerns owned by CRM and keeps dependencies flowing inward toward the domain.

## What belongs here

- business entities, value objects, invariants and domain policies.
- Supporting types and tests specific to this layer.
- Explicit dependencies required by the module.

## What does not belong here

- Concerns owned by another module.
- Host startup, HTTP pipeline or frontend code.
- Secrets or environment-specific values.

## Related documentation

- [CRM module](../README.md).
- [Module boundaries](../../README.md).

## Current status

This is an intentional Sprint 0 scaffold and will be populated with CRM functionality in its corresponding sprint.
