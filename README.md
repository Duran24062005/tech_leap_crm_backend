# Tech Leap CRM

CRM de empleabilidad para centralizar la gestión comercial, la selección de talento, las vinculaciones y el seguimiento operativo de Tech Leap.

## Estado del proyecto

El proyecto cuenta con la base de plataforma del Sprint 0: solución .NET 10, API, Worker, PostgreSQL local, migración inicial, pruebas y CI. Las funcionalidades de negocio comienzan en Sprint 1.

## Alcance del MVP

- Gestión de empresas, contactos y oportunidades comerciales.
- Gestión ATS de requerimientos, vacantes, candidatos, aplicaciones, entrevistas y selección.
- Vinculaciones, checklist documental, seguimiento y renovaciones.
- Documentos privados con control de acceso y versiones.
- Facturación operativa asociada a empresas y vinculaciones.
- Reportes operativos y exportaciones filtradas.

Quedan fuera de la primera versión los portales de autoservicio, matching con IA, automatización de WhatsApp, contabilidad completa, nómina y firma electrónica propia.

## Arquitectura prevista

La solución seguirá una arquitectura de monolito modular API-first con tres procesos desplegables:

```text
Frontend Web (React + TypeScript)
							|
							v
API (ASP.NET Core / .NET 10)
							|
			 PostgreSQL + Outbox
							|
							v
Worker + Azure Service Bus
```

Principios técnicos principales:

- PostgreSQL como fuente única de verdad.
- Módulos con propiedad clara de sus tablas y reglas de negocio.
- Autorización y reglas de negocio aplicadas en backend.
- Trazabilidad mediante auditoría, historial de estados y `correlationId`.
- Procesamiento asíncrono con Outbox, idempotencia, reintentos y Dead Letter Queue.

## Stack tecnológico

### Backend

- .NET 10 LTS.
- ASP.NET Core Web API.
- Entity Framework Core y Npgsql.
- OpenAPI, Problem Details y políticas de autorización.
- PostgreSQL 18.

### Frontend

- React con TypeScript estricto.
- Next.js App Router y MUI Core.
- TanStack Query y TanStack Table.
- React Hook Form y Zod.
- Vitest y Playwright.

### Plataforma

- Auth0, OIDC, OAuth 2.0 y Google Workspace SSO.
- Azure Blob Storage para documentos privados.
- Azure Service Bus para mensajería.
- Azure Container Apps, Key Vault y PostgreSQL Flexible Server.
- GitHub Actions, OpenTelemetry y Application Insights.

## Relación entre repositorios

Backend y frontend son repositorios independientes. El frontend consume la API REST versionada bajo `/api/v1` y recibe la URL mediante `NEXT_PUBLIC_API_BASE_URL`.

## Estructura del backend

```text
apps/api/                         # ASP.NET Core API
apps/worker/                      # Worker de automatizaciones
src/BuildingBlocks/               # Componentes compartidos
src/Modules/                      # Librerías por módulo
tests/                            # Unitarias e integración
infra/local/                      # PostgreSQL local con Compose
docs/adr/                         # Decisiones arquitectónicas
docs/api/                         # Contratos API
docs/runbooks/                    # Operación local
```

## Documentación

La documentación está organizada por tema en [`docs/design/Requerimientos.md`](docs/design/Requerimientos.md):

- [Requerimientos funcionales](docs/design/requerimientos/requerimientos-funcionales.md).
- [Arquitectura y contratos técnicos](docs/design/arquitectura/arquitectura-y-contratos.md).
- [Épicas y responsabilidades](docs/design/gestion/epicas-y-responsabilidades.md).
- [Plan de sprints](docs/design/gestion/plan-de-sprints.md).
- [Calidad, operación y roadmap](docs/design/gestion/calidad-operacion-y-roadmap.md).
- [Resumen de necesidades](docs/design/resumen_necesidades_crm_tech_leap.md).

## Flujo de trabajo

- `main` contiene versiones integradas y estables.
- Cada funcionalidad se desarrolla en una rama `feature/...`.
- Los cambios se integran mediante Pull Request.
- El CI debe validar build, pruebas, seguridad y migraciones antes de publicar.

## Próximos pasos

1. Aprobar decisiones pendientes del MVP y las modalidades de vinculación.
2. Implementar Company y Identity en Sprint 1.
3. Configurar Auth0, auditoría y servicios Azure por ambiente.
4. Implementar el flujo crítico: Company → Opportunity → Requirement → Vacancy → Application → Selection → Engagement.
