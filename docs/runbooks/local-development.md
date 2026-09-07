# Local development runbook

## Purpose

This runbook describes the repeatable procedure for running and validating the Tech Leap CRM locally.

## Requirements

- .NET SDK 10.0.111 or the version allowed by `global.json`.
- Docker with Docker Compose.
- Node.js 24 and pnpm 11 for the frontend repository.
- A free host port for PostgreSQL, normally `5432`.

## Start PostgreSQL

From `tech_leap_crm_backend`:

```bash
cp infra/local/.env.example infra/local/.env
docker compose --env-file infra/local/.env -f infra/local/compose.yml up -d
docker compose --env-file infra/local/.env -f infra/local/compose.yml ps
```

The Compose healthcheck must report PostgreSQL as healthy before readiness checks or migrations are run.

If port `5432` is already in use, set `POSTGRES_PORT=55432` in `infra/local/.env` and use the same port in `ConnectionStrings__Default`.

Stop the local service when finished:

```bash
docker compose --env-file infra/local/.env -f infra/local/compose.yml down
```

The named volume remains unless it is explicitly removed.

## Apply migrations

Restore the repository-scoped tool and apply the schema:

```bash
dotnet tool restore
dotnet ef database update \
  --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj \
  --startup-project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj
```

Inspect the migration list:

```bash
dotnet ef migrations list \
  --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj \
  --startup-project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj
```

Migration files belong to the API migration directory and must be reviewed together with the model change.

## Start the API

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run \
  --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj \
  --urls http://localhost:5000
```

Check liveness and readiness:

```bash
curl http://localhost:5000/health/live
curl http://localhost:5000/health/ready
```

The liveness endpoint does not require PostgreSQL. The readiness endpoint does.

## Obtain a local development token

Only when the API runs in Development:

```bash
curl -s http://localhost:5000/api/v1/dev/token \
  -H 'Content-Type: application/json' \
  -d '{"subject":"local-user","email":"local@tech-leap.test","role":"Developer"}'
```

Use the returned value as `Authorization: Bearer <accessToken>`:

```bash
curl http://localhost:5000/api/v1/diagnostics \
  -H 'Authorization: Bearer <accessToken>' \
  -H 'X-Correlation-Id: local-check'
```

Never use real identities or production-like credentials with the local token endpoint.

## Start the Worker

In a second backend terminal:

```bash
dotnet run --project apps/worker/TechLeap.Crm.Worker/TechLeap.Crm.Worker.csproj
```

In Sprint 0 the Worker validates its hosted-service lifecycle and Outbox polling boundary. Azure Service Bus publication is future work.

## Start the frontend

From `tech_leap_crm_frontend`:

```bash
cp .env.example .env.local
pnpm install --frozen-lockfile
pnpm dev
```

Set `NEXT_PUBLIC_API_BASE_URL=http://localhost:5000` in `.env.local`. The `/status` page checks the API liveness endpoint.

## Validation commands

Backend:

```bash
dotnet build tech_leap_crm_backend.slnx --configuration Release
dotnet test tech_leap_crm_backend.slnx --configuration Release
```

Frontend:

```bash
pnpm lint
pnpm typecheck
pnpm test:unit
pnpm build
pnpm test:e2e
```

## Troubleshooting

- If readiness fails, inspect Compose health and verify the connection-string port.
- If migrations cannot be created, run `dotnet tool restore` and verify the startup project.
- If diagnostics returns `401`, request a Development token and send it as a Bearer token.
- If the frontend status page cannot connect, verify `NEXT_PUBLIC_API_BASE_URL` and that the API is listening on port `5000`.
- Do not solve local issues by committing `.env`, `appsettings.Development.json` or generated build output.
