# Desarrollo local

## Requisitos

- .NET SDK 10.0.111 o compatible con `global.json`.
- Docker y Docker Compose.
- Node.js 24 y pnpm 11 para el frontend.

## PostgreSQL

Desde `tech_leap_crm_backend`:

```bash
cp infra/local/.env.example infra/local/.env
docker compose --env-file infra/local/.env -f infra/local/compose.yml up -d
```

## API

```bash
dotnet tool restore
ASPNETCORE_ENVIRONMENT=Development dotnet run --project apps/api/TechLeap.Crm.Api/TechLeap.Crm.Api.csproj --urls http://localhost:5000
```

Comprobaciones:

```bash
curl http://localhost:5000/health/live
curl http://localhost:5000/health/ready
```

En Development se puede solicitar un token local:

```bash
curl -s http://localhost:5000/api/v1/dev/token \
  -H 'Content-Type: application/json' \
  -d '{"subject":"local-user","email":"local@tech-leap.test","role":"Developer"}'
```

El token se utiliza como `Authorization: Bearer <accessToken>` para `/api/v1/diagnostics`.

## Worker

```bash
dotnet run --project apps/worker/TechLeap.Crm.Worker/TechLeap.Crm.Worker.csproj
```

En Sprint 0 el Worker valida el ciclo de ejecución y deja preparado el polling de Outbox. La publicación real a Azure Service Bus pertenece a Integrations.

## Frontend

Desde `tech_leap_crm_frontend`:

```bash
cp .env.example .env.local
pnpm install --frozen-lockfile
pnpm dev
```

La API se configura con `NEXT_PUBLIC_API_BASE_URL=http://localhost:5000`.
