# Tech Leap CRM — Calidad, operación y roadmap

Documento separado desde `Requerimientos.md`. Contiene Definition of Done, pruebas, seguridad, objetivos operativos, despliegue, flujo Git, tablero, roadmap y decisiones pendientes.

---

# 49. Definition of Done

Una historia solamente puede pasar a `DONE` si cumple:

* Criterios funcionales aprobados.
* Reglas de estado aprobadas.
* Permisos definidos.
* Acceso permitido probado.
* Acceso denegado probado.
* Campos sensibles protegidos.
* OpenAPI actualizado.
* Tests unitarios.
* Tests de integración.
* E2E cuando corresponda.
* PostgreSQL validado.
* `StatusHistory` cuando corresponda.
* `AuditLog` cuando corresponda.
* `Activity` cuando corresponda.
* `correlationId` propagado.
* Migración compatible.
* Logs adecuados.
* Métricas.
* Alertas.
* Sin PII en logs.
* Sin secretos en logs.
* Accesibilidad revisada.
* Documentación actualizada.
* ADR actualizado cuando haya cambio arquitectónico.



---

# 50. Estrategia de pruebas

## Unitarias

Deben cubrir:

* Invariantes.
* Cálculo de completitud.
* Transiciones.
* Permisos.
* Deduplicación.

## Integración

Deben cubrir:

* EF Core.
* PostgreSQL.
* Constraints.
* Transacciones.
* Outbox.
* Storage.
* Idempotencia.

## E2E

Debe cubrir:

```text
Company
→ Opportunity
→ Requirement
→ Vacancy
→ Application
→ Selection
→ Engagement
```

Además:

* Seguridad.
* Accesos denegados.
* Concurrencia.
* Exportaciones.
* Recuperación.



---

# 51. Seguridad obligatoria

Debe probarse:

### IDOR

Intentar acceder a:

```text
/companies/{otroId}
/candidates/{otroId}
/invoices/{otroId}
/documents/{otroId}
```

cambiando IDs.

---

### Overposting

Intentar enviar campos que el frontend no muestra.

---

### Repetición

Repetir:

* Webhooks.
* Selección.
* Creación de Engagement.
* Facturación.

No deben producir duplicados.

---

### Archivos

Intentar subir:

* Ejecutables disfrazados.
* PDFs maliciosos.
* Macros maliciosas.

---

### PII

Verificar que no aparezca en:

* Logs.
* Exportaciones no autorizadas.
* Errores.
* Métricas.
* URLs permanentes.

---

### Enlaces temporales

Verificar:

* Expiración.
* Alcance.
* Revocación.
* Permisos.

---

### Estados

No debe ser posible cambiar un estado:

* Sin permiso.
* Desde endpoint genérico.
* Sin cumplir condiciones.
* Ignorando concurrencia.



---

# 52. Objetivos no funcionales

| Métrica          |                Objetivo |
| ---------------- | ----------------------: |
| Disponibilidad   |           99.5% mensual |
| API p95          |                < 500 ms |
| RPO              |                ≤ 15 min |
| RTO              |                   ≤ 4 h |
| Auditoría        | 100% acciones sensibles |
| Alertas críticas |                < 10 min |



---

# 53. Monitoreo

## Técnico

* Error rate.
* p50.
* p95.
* p99.
* CPU.
* Memoria.
* Conexiones DB.
* Locks.
* Queue.
* DLQ.
* Jobs.
* Storage.

## Seguridad

* Fallos de login.
* Cambios de rol.
* Exportaciones masivas.
* Descargas inusuales.
* Webhooks inválidos.

## Negocio

* Opportunities sin movimiento.
* Vacancies sin movimiento.
* Tasks vencidas.
* Interviews próximas.
* Documents pendientes.
* Engagements por iniciar.
* Invoices vencidas.



---

# 54. Ambientes

## Development

Datos:

```text
Sintéticos / anonimizados
```

Acceso:

```text
Equipo técnico
```

---

## Staging

Datos:

```text
Sintéticos
```

o copia productiva:

```text
Anonimizada + aprobada
```

Objetivo:

* Integración.
* Migraciones.
* UAT.

---

## Production

Datos:

```text
Reales
```

Acceso:

```text
Usuarios autorizados
```



---

# 55. Pipeline de despliegue

```text
Pull Request
    ↓
Lint
    ↓
Build
    ↓
Unit Tests
    ↓
SAST
    ↓
Dependency Scan
    ↓
Migration Validation
    ↓
Merge
    ↓
Build Images
    ↓
SBOM
    ↓
ACR
    ↓
Staging
    ↓
Integration Tests
    ↓
Permission Tests
    ↓
Smoke Tests
    ↓
Approval
    ↓
Production
```



---

# 56. Flujo Git recomendado

```text
main
  │
  └── develop
        │
        ├── feature/...
        ├── bugfix/...
        └── hotfix/...
```

Reglas:

* No trabajar directamente sobre `main`.
* No trabajar directamente sobre `develop`.
* Cada funcionalidad debe utilizar una rama.
* Toda integración se realiza mediante Pull Request.
* El PR debe pasar CI.
* Debe existir revisión de código.
* Debe pasar QA cuando corresponda.

---

# 57. Estados del tablero Scrum

```text
BACKLOG
   ↓
READY
   ↓
IN PROGRESS
   ↓
CODE REVIEW
   ↓
QA / TESTING
   ↓
READY FOR RELEASE
   ↓
DONE
```

---

# 58. Organización recomendada en ClickUp

```text
SPACE
└── Tech Leap CRM

    └── FOLDER
        └── CRM MVP

            ├── Sprint 0
            ├── Sprint 1
            ├── Sprint 2
            ├── Sprint 3
            ├── Sprint 4
            ├── Sprint 5
            ├── Sprint 6
            ├── Sprint 7
            ├── Sprint 8
            ├── Sprint 9
            ├── Sprint 10
            ├── Sprint 11
            └── Sprint 12
```

Campos personalizados recomendados:

```text
Epic
Priority
Story Points
Sprint
Responsable
Módulo
Dependencia
Tipo
Backend / Frontend / QA / DevOps
```

---

# 59. Roadmap oficial del proyecto

El documento establece:

## Fase 0 — Alineación

* Aprobar supuestos.
* Aprobar stack.
* Aprobar estados.
* Aprobar campos por modalidad.
* Aprobar seguridad.
* Aprobar SLA.
* Definir sistema contable.

Resultado:

```text
ADR
+
Catálogo funcional aprobado
+
Backlog priorizado
```

---

## Fase 1 — Plataforma

* Repositorio.
* CI/CD.
* Ambientes.
* Auth0.
* Roles.
* Auditoría.
* PostgreSQL.
* Storage.
* Observabilidad.

Resultado:

```text
Login
+
Permisos
+
Staging desplegable
```

---

## Fase 2 — CRM

* Company.
* Contact.
* Opportunity.
* Requirement.
* Task.
* Activity.
* Pipeline.
* Importación.

---

## Fase 3 — ATS

* Vacancy.
* Candidate.
* Skill.
* Application.
* Interview.
* Selección.
* Estados.

---

## Fase 4 — Vinculación

* Engagement.
* Documents.
* Checklist.
* Seguimiento.
* Renovación.
* Invoice operativo.

---

## Fase 5 — Automatización

* Alertas.
* Google Calendar.
* Gmail.
* Drive.
* Dashboards.
* Exportaciones.
* SLA.

---

## Fase 6 — Evolución

* Portales.
* WhatsApp.
* Matching.
* IA.
* Analítica predictiva.
* Integración contable.



---

# 60. Orden oficial del flujo crítico

El equipo debe construir y validar en este orden:

```text
1. Crear empresa y contacto.

2. Crear oportunidad y próxima acción.

3. Levantar y completar requerimiento.

4. Crear/publicar vacante.

5. Crear candidato y asociarlo mediante Application.

6. Gestionar entrevistas/pruebas y estados.

7. Seleccionar candidato.

8. Crear/activar vinculación y documentos.

9. Generar seguimiento y factura operativa.

10. Cerrar, pausar y reabrir con auditoría.
```



---

# 61. Decisiones que NO debemos inventar

Antes de comprometer producción deben resolverse:

1. ¿Azure y Auth0 están aprobados en presupuesto, compras y seguridad?
2. ¿El CRM será solamente para Tech Leap o será SaaS multi-tenant?
3. ¿Qué campos son obligatorios para SENA, Talent Pool y Staffing?
4. ¿Cuántos días sin movimiento generan alertas?
5. ¿Qué documentos, aprobadores, retenciones y firmas requiere cada modalidad?
6. ¿Cuál será el sistema contable autoridad?
7. ¿Qué API ofrece el sistema contable?
8. ¿Cuál es el volumen actual/proyectado de candidatos?
9. ¿Cuál es el volumen de archivos?
10. ¿Cuál es el volumen de actividades?
11. ¿Qué región de Azure está aprobada?
12. ¿Qué transferencias de datos están autorizadas?
13. ¿Qué información podrán consultar futuros portales?



---

# 62. Regla importante para el equipo

Si una de estas decisiones cambia:

```text
NO modificar directamente código
NO modificar directamente modelo de datos
```

Primero:

```text
Crear / actualizar ADR
        ↓
Aprobar decisión
        ↓
Actualizar backlog
        ↓
Actualizar modelo
        ↓
Implementar
```

El documento establece explícitamente que cualquier cambio a un supuesto debe registrarse en un **Architecture Decision Record (ADR)** antes de modificar código o modelo de datos. 

---

# 63. Release Gates

Una versión **no debe aprobarse** si existe:

* Fallo crítico de seguridad.
* Fallo alto de seguridad.
* Problema alto de permisos.
* Problema de integridad de datos.
* Migración sin probar.
* Rollback sin probar.
* E2E crítico fallando.
* Pruebas de acceso denegado fallando.
* Dashboards incompletos.
* Alertas incompletas.
* Runbook desactualizado.
* Aprobación funcional pendiente.
* Aprobación técnica pendiente.



---

# 64. Regla final de arquitectura

No convertir módulos en microservicios simplemente porque técnicamente sea posible.

La extracción de un módulo debe evaluarse solamente cuando existan condiciones sostenidas y medibles, como:

* Escalabilidad independiente.
* Despliegues con frecuencia claramente diferente.
* Equipo autónomo propietario.
* Problemas de rendimiento que no se solucionen con Worker/cola.
* Requisitos de seguridad diferentes.
* Requisitos de disponibilidad diferentes.
* Requisitos de residencia de datos diferentes.
* Beneficio real superior al costo de transacciones distribuidas y operación.



---

# 65. Resultado esperado del MVP

Al finalizar el MVP, el equipo debe poder ejecutar de extremo a extremo:

```text
┌──────────────┐
│   Company    │
└──────┬───────┘
       ↓
┌──────────────┐
│   Contact    │
└──────┬───────┘
       ↓
┌──────────────┐
│ Opportunity  │
└──────┬───────┘
       ↓
┌──────────────┐
│ Requirement  │
└──────┬───────┘
       ↓
┌──────────────┐
│   Vacancy    │
└──────┬───────┘
       ↓
┌──────────────┐
│  Candidate   │
└──────┬───────┘
       ↓
┌──────────────┐
│ Application  │
└──────┬───────┘
       ↓
┌──────────────┐
│ Interview    │
└──────┬───────┘
       ↓
┌──────────────┐
│   SELECTED   │
└──────┬───────┘
       ↓
┌──────────────┐
│ Engagement   │
└──────┬───────┘
       ↓
┌──────────────┐
│  Documents   │
└──────┬───────┘
       ↓
┌──────────────┐
│  Follow-up   │
└──────┬───────┘
       ↓
┌──────────────┐
│   Invoice    │
└──────┬───────┘
       ↓
┌──────────────┐
│ Close/Reopen │
│    + Audit   │
└──────────────┘
```

Todo el recorrido debe quedar **trazable, autorizado, auditable y probado mediante E2E**.

---

## Fuente y criterio de este documento

Este Markdown está construido tomando como fuente el archivo **`Diseno_tecnico_CRM_Tech_Leap_v1.pdf`** proporcionado para el proyecto. He mantenido especialmente las decisiones críticas del documento: arquitectura modular, fronteras de dominio, estados, Application como relación Candidate/Vacancy, reglas de vinculación, permisos, auditoría, Outbox, seguridad documental, roadmap y decisiones pendientes.  

**Importante:** donde el documento marca una decisión como pendiente, la he dejado como pendiente en lugar de inventar una definición. Esto es particularmente importante para modalidad/tenant, campos obligatorios, SLA, documentos, sistema contable, volúmenes y región de despliegue. 
