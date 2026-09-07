# ADR-001: Independent repositories and Next.js

## Status

Accepted — 2026-09-06

## Context

The project consists of two independent Git repositories. Early documentation described a unified web application, while the existing frontend was already based on Next.js 16.

## Decision

- Keep separate backend and frontend repositories.
- Keep `apps/api` and `apps/worker` as backend hosts.
- Continue with Next.js App Router for the frontend.
- Use versioned HTTP/JSON under `/api/v1` as the repository boundary.
- Provide the frontend API URL through explicit environment variables.

## Consequences

- Each repository has its own CI and deployment lifecycle.
- Frontend API access is centralized in its HTTP client.
- Documentation must refer to the implemented Next.js application and the actual repository topology.
- Neither repository is moved into a monorepo.
- Cross-repository changes require coordinated contract and documentation updates.

## Related documentation

- [Backend architecture](../design/arquitectura/arquitectura-y-contratos.md).
- [API contract](../api/README.md).
- [Frontend architecture](../../../tech_leap_crm_frontend/docs/architecture.md).
