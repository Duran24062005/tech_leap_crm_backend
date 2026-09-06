# Sprint 0 — Implementación y aceptación

## Objetivo

Dejar una base ejecutable y reproducible para iniciar Sprint 1 sin implementar todavía funcionalidades de negocio.

## Alcance entregado

- Solución .NET 10 con API, Worker, BuildingBlocks, módulos y pruebas.
- API con health checks, OpenAPI, Problem Details, correlación y diagnóstico protegido.
- PostgreSQL local mediante Docker Compose.
- EF Core, Npgsql y migración inicial de Outbox.
- JWT local en Development y configuración compatible con Auth0.
- Frontend Next.js con cliente HTTP, providers y configuración por ambiente.
- Workflows independientes para Backend y frontend.

## Fuera de alcance

Company, Opportunity, ATS, Engagements funcionales, Azure Service Bus real, Blob Storage real, Application Insights y datos productivos.

## Criterios de aceptación

1. `dotnet build` compila proyectos reales de la solución.
2. API y Worker pueden iniciar.
3. PostgreSQL levanta con `docker compose` y la migración inicial se aplica.
4. `/health/live` funciona sin PostgreSQL y `/health/ready` valida PostgreSQL.
5. `/api/v1/diagnostics` rechaza solicitudes sin JWT y devuelve `traceId` y `correlationId` con JWT válido.
6. Las pruebas de integración utilizan un contenedor PostgreSQL aislado.
7. ESLint, TypeScript y `next build` pasan sin descargar fuentes desde Google.
8. Ambos repositorios tienen CI independiente.
