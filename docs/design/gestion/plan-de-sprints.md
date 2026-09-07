# Tech Leap CRM — Plan de sprints

Documento separado desde `Requerimientos.md`. Contiene el trabajo planificado de Sprint 0 a Sprint 12.

---

# 36. Sprint 0 — Alineación y plataforma

## Objetivo

Crear la base técnica antes de desarrollar módulos funcionales.

### Tareas

#### `TEC-001`

**Definir ADR inicial y fronteras de módulos**

Responsable:

```text
Integrante 1 + Todos
```

Story Points:

```text
1
```

---

#### `TEC-002`

**Crear estructura de solución**

```text
tech_leap_crm_frontend/ (independent repository)
/apps/api
/apps/worker

/src/Modules

/infra

/docs
```

Responsable:

```text
Integrante 1
```

Story Points:

```text
3
```

---

#### `TEC-003`

**Configurar PostgreSQL + EF Core + migraciones**

Responsable:

```text
Integrante 1
```

Story Points:

```text
3
```

---

#### `TEC-004`

**Configurar API**

Debe incluir:

* `/api/v1`
* OpenAPI.
* Problem Details.
* `traceId`.
* `correlationId`.

Responsable:

```text
Integrante 1
```

Story Points:

```text
3
```

---

#### `DEV-001`

**Crear CI/CD inicial**

Debe ejecutar:

* Build.
* Tests.
* SAST.
* Dependency scanning.
* Validación de migraciones.

Responsable:

```text
Integrante 2
```

Story Points:

```text
5
```

---

# 37. Sprint 1 — Identity + Company

## Identity

### `SEC-001`

Integrar:

```text
Auth0
OIDC
OAuth2
JWT
```

SP:

```text
5
```

---

### `SEC-002`

Implementar:

```text
RBAC
Deny-by-default
Authorization Policies
```

SP:

```text
5
```

---

### `SEC-003`

Crear:

```text
AuditLog
StatusHistory
```

SP:

```text
5
```

---

## Company

### `CRM-001`

Crear Company.

Debe incluir:

* Dedupe.
* Soft delete.
* Owner.
* Status.
* Auditoría.

SP:

```text
5
```

---

### `CRM-002`

Crear Contact.

Regla:

```text
1 contacto principal activo por empresa
```

SP:

```text
3
```

---

# 38. Sprint 2 — Opportunity + Requirement

## `CRM-003`

Crear Opportunity.

Debe permitir:

* Empresa.
* Contacto.
* Responsable.
* Modalidad.
* Valor estimado.
* Probabilidad.
* Próxima acción.
* Fecha de seguimiento.
* Fecha estimada de cierre.

SP:

```text
5
```

---

## `CRM-004`

Crear Requirement.

Debe soportar:

* Versionado.
* Modalidad.
* Posiciones.
* Perfil.
* Skills.
* Experiencia.
* Ubicación.
* Modalidad de trabajo.
* Presupuesto.
* Fecha objetivo.
* Etapas de selección.
* Estado de completitud.

SP:

```text
8
```

---

# 39. Sprint 3 — Vacancy

## `ATS-001`

Crear Vacancy desde Requirement completo.

Debe:

* Heredar información.
* Permitir ajustes propios.
* Validar campos.
* Manejar estados.
* Crear SLA.
* Crear tareas.

SP:

```text
5
```

---

# 40. Sprint 4 — Candidate + Application

## `ATS-002`

Crear Candidate.

Debe incluir:

* Datos personales.
* Experiencia.
* Disponibilidad.
* Fuente.
* Consentimiento.
* Retención.

SP:

```text
5
```

---

## `ATS-003`

Crear Application.

Regla obligatoria:

```text
UNIQUE(vacancyId, candidateId)
```

SP:

```text
5
```

---

## `ATS-004`

Implementar workflow de Application.

Estados:

```text
ASSOCIATED
PRESELECTED
VALIDATED
PRESENTED
INTERVIEW
TECH_TEST
SELECTED
ENGAGED
NOT_SELECTED
WITHDRAWN
```

SP:

```text
8
```

---

# 41. Sprint 5 — Interview + Selection

## `ATS-005`

Crear Interview.

Debe incluir:

* Tipo.
* Estado.
* Inicio.
* Fin.
* Meeting URL.
* Entrevistador.
* Resultado.
* Feedback.
* Próximo paso.

SP:

```text
5
```

---

## `ATS-006`

Implementar selección.

Debe controlar:

* Elegibilidad.
* Número de posiciones.
* Condiciones.
* Auditoría.
* Checklist.
* Tareas jurídicas.

SP:

```text
8
```

---

# 42. Sprint 6 — Engagement

## `ENG-001`

Crear Engagement idempotente.

Debe originarse únicamente desde:

```text
Application = SELECTED
```

SP:

```text
8
```

---

## `ENG-002`

Implementar:

* Checklist.
* Condiciones.
* Documentación.
* Estados.
* Activación.
* Seguimiento.

SP:

```text
8
```

---

# 43. Sprint 7 — Documents

## `DOC-001`

Implementar Upload Sessions.

Debe:

* Validar tipo.
* Validar tamaño.
* Validar categoría.
* Validar entidad destino.
* Generar URL temporal.

SP:

```text
5
```

---

## `DOC-002`

Implementar:

* SHA-256.
* Malware scanning.
* DocumentVersion.
* Approval.
* Lock.
* Descargas temporales.

SP:

```text
8
```

---

# 44. Sprint 8 — Work

## `WORK-001`

Crear:

```text
Task
Activity
Timeline
```

SP:

```text
5
```

---

## `WORK-002`

Implementar:

* Pause.
* Close.
* Reopen.
* Reassign.
* Audit.

SP:

```text
5
```

---

# 45. Sprint 9 — Billing

## `BIL-001`

Crear Invoice operativa.

Debe incluir:

* Engagement.
* Company.
* Invoice number.
* Periodo.
* Fechas.
* Totales.
* Moneda.
* Estado.
* PaidAt.
* ID externo contable.

SP:

```text
8
```

---

# 46. Sprint 10 — Reporting

## `REP-001`

Crear reportes operativos.

SP:

```text
5
```

---

## `REP-002`

Crear exportaciones:

```text
CSV
XLSX
```

Las exportaciones grandes deben ejecutarse asíncronamente.

SP:

```text
5
```

---

# 47. Sprint 11 — Worker + Integraciones

## `INT-001`

Implementar:

```text
Outbox
Azure Service Bus
Retries
DLQ
Idempotencia
```

SP:

```text
8
```

---

## `INT-002`

Procesar eventos prioritarios.

Debe soportar:

* Notificaciones.
* Tareas.
* Alertas.
* Seguimientos.
* Integraciones.

SP:

```text
8
```

---

# 48. Sprint 12 — Security + QA + Release

## `QA-001`

Implementar E2E del camino crítico:

```text
Company
 ↓
Opportunity
 ↓
Requirement
 ↓
Vacancy
 ↓
Application
 ↓
Selection
 ↓
Engagement
```

SP:

```text
8
```

---

## `QA-002`

Pruebas de seguridad:

* IDOR.
* Overposting.
* RBAC.
* Enlaces temporales.
* Exportaciones.
* Duplicados.
* Race conditions.
* Archivos maliciosos.

SP:

```text
8
```

---

## `DEV-002`

Implementar observabilidad:

* OpenTelemetry.
* Logs.
* Métricas.
* Trazas.
* Alertas.
* Application Insights.

SP:

```text
5
```

---

## `REL-001`

Implementar:

* Backups.
* PITR.
* Restauración.
* Runbook.
* Rollback.
* Release gates.

SP:

```text
8
```

---

