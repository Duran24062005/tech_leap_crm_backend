# CRM modules

## Purpose

Business modules of the modular monolith.

## Responsibilities

Each module owns a business capability, its rules, persistence model and public contracts.

## What belongs here

- One library per business module.
- Module-local `Domain`, `Application`, `Infrastructure` and `Contracts` layers.
- Explicit module integration contracts.

## What does not belong here

- Shared platform primitives.
- Host startup and middleware.
- Direct access to another module's tables or internals.

## Module ownership

- **Identity** — users, roles, permissions and memberships.
- **CRM** — companies, contacts, opportunities and requirements.
- **ATS** — vacancies, candidates, skills, applications and interviews.
- **Engagements** — engagements, checklists, follow-ups and renewals.
- **Billing** — operational invoices and collection status.
- **Documents** — private documents, versions and access policies.
- **Work** — tasks, activities, notifications and status history.
- **Integrations** — external connections, webhooks and mappings.
- **Reporting** — reports and filtered exports.

## Layer rules

- `Domain` contains invariants and business concepts.
- `Application` contains use cases and ports.
- `Infrastructure` implements persistence and providers.
- `Contracts` contains messages and public DTOs.

Modules communicate through contracts or events, not another module's internals.

## Related documentation

- [Backend architecture](../../docs/design/arquitectura/arquitectura-y-contratos.md).
- [Functional requirements](../../docs/design/requerimientos/requerimientos-funcionales.md).

## Current status

All Sprint 0 module projects are scaffolds. Functional implementation starts with Identity and CRM.
