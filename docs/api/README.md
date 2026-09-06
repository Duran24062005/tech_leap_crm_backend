# Contrato API

La API se publica bajo /api/v1 y utiliza JSON camelCase.

## Plataforma Sprint 0

- GET /health/live: disponibilidad del proceso, sin dependencia de base de datos.
- GET /health/ready: disponibilidad de PostgreSQL.
- GET /api/v1/diagnostics: diagnóstico protegido con JWT.
- GET /openapi/v1.json: documento OpenAPI en Development.

Los errores usan application/problem+json e incluyen traceId y correlationId cuando corresponde.

Los contratos funcionales de Company, Opportunity, ATS y Engagements se agregarán con sus respectivos sprints.
