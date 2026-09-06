# ADR-001: Repositorios independientes y Next.js

## Estado

Aceptado — 2026-09-06

## Contexto

El proyecto se encuentra en dos repositorios Git independientes. La documentación inicial describía una solución unificada con `/apps/web` y un frontend basado en Vite, mientras que el frontend existente ya estaba creado con Next.js 16.

## Decisión

- Se mantienen dos repositorios: uno para Backend y otro para frontend.
- El backend conserva sus hosts `apps/api` y `apps/worker`.
- El frontend continúa con Next.js App Router.
- El contrato entre repositorios será HTTP/JSON versionado bajo `/api/v1` y variables de entorno explícitas.

## Consecuencias

- Cada repositorio tendrá su propio CI y ciclo de despliegue.
- El frontend utilizará el cliente de API generado o mantenido contra el contrato OpenAPI del backend.
- La documentación debe referirse a Next.js, no a Vite.
- No se moverá el historial de ninguno de los repositorios a un monorepo.
