# ADR-002: Plataforma local y autenticación de bootstrap

## Estado

Aceptado — 2026-09-06

## Decisión

- PostgreSQL 18 se ejecuta localmente con Docker Compose.
- La API utiliza JWT firmado localmente en Development.
- El endpoint local de emisión de tokens solo existe en Development y no se documenta en OpenAPI.
- En ambientes no locales, la API valida JWT mediante Auth0 usando `Auth0:Authority` y `Auth0:Audience`.
- Azure Service Bus, Blob Storage y Application Insights quedan como adaptadores preparados, no como dependencias obligatorias de Sprint 0.
- Las pruebas de integración usan Testcontainers PostgreSQL.

## Reglas de seguridad

- La clave local es exclusivamente de desarrollo y no contiene datos reales.
- El endpoint de tokens no se registra fuera de Development.
- No se versionan archivos `.env` ni secretos.
- El diagnóstico requiere autenticación JWT.
