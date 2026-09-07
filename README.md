# Tech Leap CRM Backend

Backend repository for the Tech Leap employability CRM. It is an API-first modular monolith implemented with .NET 10, PostgreSQL and a separate background Worker.

## Repository status

Sprint 0 provides the executable platform foundation:

- ASP.NET Core API.
- Worker Service.
- Shared BuildingBlocks.
- One project per business module.
- PostgreSQL 18 through Docker Compose.
- EF Core migrations with an Outbox table.
- Unit and Testcontainers integration tests.
- Independent GitHub Actions validation.

Business capabilities are introduced incrementally from Sprint 1.

## Architecture at a glance

```text
Independent Next.js frontend
           |
           | HTTP/JSON, /api/v1
           v
ASP.NET Core API ---- PostgreSQL
           |              |
           |              +-- platform schema and Outbox
           v
       Worker ---- future Azure Service Bus
```

The backend is a modular monolith: modules share a database instance but own their tables, business rules and contracts. The API and Worker are separate executable hosts.

## Repository map

```text
apps/
  api/                    HTTP host
  worker/                 asynchronous processing host
src/
  BuildingBlocks/         cross-cutting platform primitives
  Modules/                business modules
tests/                    unit and integration test projects
infra/
  local/                  PostgreSQL Compose setup
docs/
  adr/                    lasting architecture decisions
  api/                    HTTP contract
  design/                 product and technical design
  runbooks/               repeatable operational procedures
```

Every project-owned directory has a local README explaining its purpose, ownership and expected contents.

## Technology

- .NET 10 and ASP.NET Core.
- Entity Framework Core and Npgsql.
- PostgreSQL 18.
- JWT bearer authentication with local Development tokens.
- Auth0-ready configuration for non-local environments.
- OpenAPI and Problem Details.
- xUnit and Testcontainers.
- Docker Compose for local PostgreSQL.

## Platform contract

- `GET /health/live` checks process liveness without PostgreSQL.
- `GET /health/ready` checks PostgreSQL readiness.
- `GET /api/v1/diagnostics` returns authenticated non-sensitive diagnostics.
- `GET /openapi/v1.json` exposes OpenAPI in Development.
- `X-Correlation-Id` is accepted or generated and returned.
- Problem Details responses include `traceId` and `correlationId` when available.

See the [API contract](docs/api/README.md) and [architecture guide](docs/design/arquitectura/arquitectura-y-contratos.md).

## Local development

Requirements:

- .NET SDK 10.0.111 or the version allowed by `global.json`.
- Docker and Docker Compose.
- Node.js 24 and pnpm 11 for the frontend repository.

Start PostgreSQL:

```bash
cp infra/local/.env.example infra/local/.env
docker compose --env-file infra/local/.env -f infra/local/compose.yml up -d
```

Start the API:

```bash
dotnet tool restore
ASPNETCORE_ENVIRONMENT=Development dotnet run \
  --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj \
  --urls http://localhost:5000
```

Start the Worker in a second terminal when asynchronous processing is needed:

```bash
dotnet run --project apps/worker/TechLeap.Crm.Worker/TechLeap.Crm.Worker.csproj
```

The complete procedure, migration commands and troubleshooting notes are in the [local development runbook](docs/runbooks/local-development.md).

## Validation

```bash
dotnet build tech_leap_crm_backend.slnx --configuration Release
dotnet test tech_leap_crm_backend.slnx --configuration Release
dotnet ef migrations list \
  --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj \
  --startup-project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj
```

CI also validates dependency vulnerabilities, migration scripts and basic SAST.

## Documentation map

- [Architecture and contracts](docs/design/arquitectura/arquitectura-y-contratos.md).
- [API contract](docs/api/README.md).
- [Architecture decisions](docs/adr/README.md).
- [Requirements index](docs/design/Requerimientos.md).
- [Sprint planning](docs/design/gestion/plan-de-sprints.md).
- [Quality and roadmap](docs/design/gestion/calidad-operacion-y-roadmap.md).
- [Local development runbook](docs/runbooks/local-development.md).
- [Modules guide](src/Modules/README.md).

## Scope boundaries

Sprint 0 does not implement Company, Opportunity, ATS, Engagements or other functional modules. Azure Service Bus, Blob Storage and Application Insights remain future adapters. Authentication and all business authorization rules remain backend responsibilities.

## Contribution rules

- Keep module ownership explicit.
- Do not access another module's internals or tables directly.
- Put shared technical primitives in BuildingBlocks only when they have multiple legitimate consumers.
- Add migrations for schema changes.
- Update API contracts and documentation with public behavior changes.
- Keep tests close to the behavior they validate.
- Use small, focused commits and pull requests.
