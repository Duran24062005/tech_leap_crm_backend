# Tech Leap CRM — Arquitectura y contratos técnicos

Documento separado desde `Requerimientos.md`. Contiene la arquitectura, el stack, las reglas técnicas, el modelo de datos, la seguridad y los eventos.

---

# 5. Arquitectura

La solución estará compuesta por tres procesos desplegables:

```text
┌─────────────────────────┐
│      Frontend Web       │
│ React + TypeScript      │
└────────────┬────────────┘
             │ REST / JSON
             ▼
┌─────────────────────────┐
│          API            │
│ ASP.NET Core / .NET 10  │
└────────────┬────────────┘
             │
     ┌───────┴────────┐
     ▼                ▼
PostgreSQL       Outbox
                     │
                     ▼
              Azure Service Bus
                     │
                     ▼
                  Worker
```

Los módulos comparten la base de datos, pero cada módulo es dueño de sus tablas y reglas de negocio. 

---

# 6. Stack tecnológico obligatorio

## Frontend

* React.
* TypeScript estricto.
* Next.js App Router.
* MUI Core.
* TanStack Query.
* TanStack Table.
* React Hook Form.
* Zod.
* Vitest.
* Playwright.

## Backend

* .NET 10 LTS.
* ASP.NET Core Web API.
* Dependency Injection.
* Authorization Policies.
* Problem Details.
* OpenAPI.

## Base de datos

* PostgreSQL 18.
* Entity Framework Core.
* Npgsql.
* Migraciones versionadas.
* Constraints.
* Índices.
* `timestamptz`.
* UTC.

## Identidad

* Auth0.
* OIDC.
* OAuth 2.0.
* Google Workspace SSO.
* MFA para roles sensibles.

## Mensajería

* Azure Service Bus.
* Worker Service.
* Outbox.
* Idempotencia.
* Retries.
* Dead Letter Queue.

## Documentos

* Azure Blob Storage.
* Contenedores privados.
* Versionado.
* Soft delete.
* SHA-256.
* Escaneo.
* URLs temporales.

## Observabilidad

* OpenTelemetry.
* Application Insights.
* Logs estructurados.
* Métricas.
* Trazas.
* Alertas.

## Infraestructura

* Azure Container Apps.
* PostgreSQL Flexible Server.
* Key Vault.
* Azure Container Registry.
* Front Door/WAF.
* Bicep.

## CI/CD

* GitHub Actions.
* Build.
* Tests.
* SAST.
* Dependencias.
* Construcción de imágenes.
* Staging.
* Smoke tests.
* Aprobación de producción.



---

# 7. Estructura de la solución

```text
/apps
    /api
    /worker

/src
    /Modules
        /Identity
        /CRM
        /ATS
        /Engagements
        /Billing
        /Documents
        /Work
        /Integrations
        /Reporting

/infra

/docs
    /adr
    /api
    /runbooks
```

### `/apps/api`

ASP.NET Core:

* Autenticación.
* Autorización.
* Módulos.
* Endpoints.

### `/apps/worker`

Responsable de:

* Automatizaciones.
* Recordatorios.
* Integraciones.
* Exportaciones.
* Procesamiento de documentos.

### `/src/Modules`

Cada módulo contiene:

* Dominio.
* Aplicación.
* Infraestructura.
* Contratos.

### `/infra`

* Bicep.
* Configuración por ambiente.
* Políticas.
* Monitoreo.

### `/docs`

* ADR.
* API.
* Runbooks.

El frontend vive en un repositorio independiente y utiliza Next.js App Router. Su contrato con este repositorio es la API REST versionada bajo `/api/v1`.



---

# 8. Fronteras de módulos

## Identity & Access

### Es dueño de:

* User.
* Role.
* Permission.
* Membresías.
* Políticas.

### No debe:

* Guardar contraseñas.
* Duplicar la identidad de Auth0.

---

## CRM

### Es dueño de:

* Company.
* Contact.
* Opportunity.
* Requirement.

### No debe:

* Modificar directamente Applications.
* Modificar directamente Invoices.

---

## ATS

### Es dueño de:

* Vacancy.
* Candidate.
* Skill.
* Application.
* Interview.

### No debe:

* Crear Engagement sin una selección válida.

---

## Engagements

### Es dueño de:

* Engagement.
* Checklist.
* Seguimientos.
* Renovaciones.

### No debe:

* Emitir facturas contables.
* Editar la selección histórica.

---

## Billing

### Es dueño de:

* Invoice.
* Estados operativos de cobro.

### No debe:

* Reemplazar un ERP.

---

## Documents

### Es dueño de:

* Document.
* DocumentVersion.
* Políticas.
* Enlaces.

### No debe:

* Exponer rutas de almacenamiento al navegador.

---

## Work

### Es dueño de:

* Task.
* Activity.
* Notification.
* StatusHistory.

### No debe:

* Decidir las reglas de transición de otros módulos.

---

## Integrations

### Es dueño de:

* Conexiones.
* Webhooks.
* Cursores.
* Mapeos externos.

### No debe:

* Escribir directamente tablas de otros módulos.



---

# 9. Reglas críticas del backend

Cada caso de uso debe implementarse como:

```text
Command / Query
      ↓
Validación
      ↓
Autorización
      ↓
Reglas de negocio
      ↓
Transacción
      ↓
Persistencia
      ↓
Outbox
```

## Reglas

* No crear vacantes desde requerimientos incompletos.
* No seleccionar Applications no elegibles.
* No activar Engagement sin condiciones aprobadas.
* Utilizar concurrencia optimista.
* Devolver `409 Conflict` cuando exista conflicto de versión.
* Todos los timestamps en UTC.
* Listados paginados.
* Filtros permitidos.
* Ordenamiento controlado.
* No construir SQL desde parámetros libres.
* Operaciones repetibles requieren idempotencia.
* Webhooks requieren idempotencia.
* Importaciones requieren idempotencia.
* El Outbox se escribe dentro de la misma transacción.
* El Worker publica los eventos después del commit.



---

# 10. Reglas del frontend

El frontend debe:

* Organizarse por dominios.
* Utilizar rutas por dominio.
* Ocultar/deshabilitar acciones según permisos.
* No tomar decisiones de seguridad.
* Utilizar formularios por pasos para:

  * Requirement.
  * Vacancy.
  * Engagement.
* Mostrar en cada detalle:

  * Resumen.
  * Estado.
  * Responsable.
  * Próxima acción.
  * Documentos.
  * Tareas.
  * Timeline.
* Utilizar modales para cambios de estado.
* Mostrar consecuencias antes de confirmar.
* Exigir motivo cuando corresponda.
* Mantener filtros de tablas en URL.
* Respetar los filtros al exportar.
* Ejecutar exportaciones grandes de forma asíncrona.
* Cumplir WCAG 2.2 AA.



---

# 11. API

Base:

```text
/api/v1
```

Formato:

```text
JSON UTF-8
```

Propiedades:

```text
camelCase
```

Errores:

```text
application/problem+json
```

Deben incluir:

* `type`
* `title`
* `status`
* `detail`
* `traceId`
* errores por campo

Seguridad:

```text
Bearer JWT
```

Validación:

* issuer.
* audience.
* firma.
* expiración.
* scopes.

Autorización:

* políticas internas.

Concurrencia:

```text
ETag / If-Match
```

o:

```text
version
```

Idempotencia:

```text
Idempotency-Key
```

Auditoría:

```text
X-Correlation-Id
```



---

# 12. Endpoints principales

```http
POST /companies
```

Crear empresa con control de duplicados y responsable.

```http
POST /companies/{id}/contacts
```

Crear contacto y opcionalmente establecerlo como principal.

```http
POST /opportunities
```

Crear oportunidad y programar próxima acción.

```http
POST /opportunities/{id}/requirements
```

Crear o versionar Requirement.

```http
POST /requirements/{id}/vacancies
```

Crear Vacancy en borrador.

```http
POST /vacancies/{id}/applications
```

Asociar Candidate mediante Application.

```http
POST /applications/{id}/interviews
```

Programar Interview.

```http
POST /applications/{id}/select
```

Seleccionar candidato.

```http
POST /engagements
```

Crear Engagement idempotente.

```http
POST /{resource}/{id}/transitions
```

Ejecutar transición.

```http
POST /activities
```

Registrar actividad.

```http
POST /documents/upload-sessions
```

Crear sesión temporal de carga.

```http
GET /reports/{reportKey}/export
```

Generar exportación.



---

# 27. Modelo de datos

## Company

Campos principales:

```text
id
legalName
tradeName
taxId
website
domain
phone
email
city
region
commercialStatus
source
ownerUserId
lastActivityAt
```

Relaciones:

```text
Company
 ├── Contact
 └── Opportunity
```

---

## Contact

```text
companyId
firstName
lastName
jobTitle
email
phone
preferredChannel
isPrimary
consentBasis
```

Regla:

> Solo un contacto principal activo por Company.

---

## Opportunity

```text
companyId
primaryContactId
ownerUserId
name
modality
estimatedValue
currency
probability
status
nextAction
nextFollowUpAt
expectedCloseDate
closeReason
```

---

## Requirement

```text
opportunityId
versionNo
modality
positionsRequested
description
seniority
experience
location
workMode
budgetMin
budgetMax
targetStartDate
selectionSteps
completenessStatus
```

Las versiones aprobadas son inmutables.

---

## Vacancy

```text
requirementId
title
description
positionsCount
filledPositions
seniority
location
workMode
compensationMin
compensationMax
status
ownerUserId
openedAt
targetStartDate
closedAt
```

Regla:

```text
filledPositions <= positionsCount
```

---

## Candidate

```text
firstName
lastName
email
phone
city
currentRole
yearsExperience
availabilityStatus
source
consentStatus
consentAt
retentionUntil
anonymizedAt
```

Estados globales:

```text
AVAILABLE
UNAVAILABLE
ENGAGED
ARCHIVED
```

Los datos personales son restringidos.

---

## Skill

```text
normalizedName
category
```

---

## CandidateSkill

```text
candidateId
skillId
level
years
verifiedAt
```

---

## Application

```text
vacancyId
candidateId
stage
ownerUserId
matchScore
presentedAt
selectedAt
rejectionReason
withdrawnReason
```

Restricción:

```text
UNIQUE(vacancyId, candidateId)
```

Application es la fuente del estado del candidato dentro del proceso.

---

## Interview

```text
applicationId
type
status
startsAt
endsAt
meetingUrl
interviewerUserId
contactId
result
feedback
nextStep
```

Relación:

```text
Application 1:N Interview
```

El feedback interno no debe exponerse a futuros portales.

---

## Task

```text
title
description
status
priority
assigneeUserId
createdBy
dueAt
completedAt
reminderAt
relatedType
relatedId
```

Cada Task debe tener responsable.

---

## Activity

```text
type
channel
direction
occurredAt
actorUserId
summary
outcome
externalId
relatedType
relatedId
```

Los eventos importados deben ser idempotentes mediante:

```text
provider + externalId
```

---

## Document

```text
fileName
category
status
sensitivity
mimeType
sizeBytes
sha256
storageProvider
storageKey
currentVersionNo
approvedBy
approvedAt
lockedAt
```

No guarda una URL permanente.

---

## DocumentVersion

```text
documentId
versionNo
storageVersionId
sha256
sizeBytes
uploadedBy
createdAt
```

Restricción:

```text
UNIQUE(documentId, versionNo)
```

Una versión aprobada no puede ser reemplazada.

---

## Engagement

```text
applicationId
companyId
candidateId
vacancyId
modality
status
startDate
endDate
value
currency
billingCycle
contractualStatus
ownerUserId
renewalReviewAt
endReason
```

En MVP:

```text
Application 0..1 Engagement
```

---

## Invoice

```text
engagementId
companyId
invoiceNumber
billingPeriodStart
billingPeriodEnd
issueDate
dueDate
subtotal
tax
total
currency
status
paidAt
externalAccountingId
```

Relación:

```text
Engagement 1:N Invoice
```

---

## StatusHistory

```text
entityType
entityId
fromStatus
toStatus
reasonCode
comment
changedBy
changedAt
correlationId
metadata
```

Debe ser:

```text
append-only
```

---

## AuditLog

```text
actorUserId
action
entityType
entityId
changedFields
ipHash
userAgent
occurredAt
correlationId
```

Debe auditar:

* Altas.
* Ediciones.
* Exportaciones.
* Descargas.
* Permisos.
* Acciones sensibles.

Sin registrar valores sensibles completos.

---

## OutboxMessage

```text
eventType
aggregateType
aggregateId
payload
occurredAt
processedAt
attempts
lastError
```

Se inserta dentro de la transacción.

El Worker realiza los reintentos.

Los fallos permanentes pasan a revisión.



---

# 28. Relaciones críticas

```text
Company
   1:N
Opportunity
   1:N
Requirement
   1:N
Vacancy
```

```text
Candidate
   N:M
Vacancy
```

mediante:

```text
Application
```

Application es obligatoria para:

* Interview.
* Selection.
* Engagement.

En MVP:

```text
Application 0..1 Engagement
```

Engagement:

```text
1:N Invoice
```



---

# 29. Constraints obligatorios en PostgreSQL

```sql
UNIQUE(vacancyId, candidateId)
```

Evita Application duplicadas.

```sql
CHECK(
    filledPositions >= 0
    AND
    filledPositions <= positionsCount
)
```

Evita exceso de posiciones cubiertas.

```sql
UNIQUE(documentId, versionNo)
```

Controla versiones.

```sql
UNIQUE(provider, externalId)
```

Para operaciones idempotentes cuando aplique.

Además:

* FK con `RESTRICT`.
* Índices únicos parciales para registros activos.
* Índices en campos críticos.



---

# 30. Seguridad documental

Flujo:

```text
Browser
   ↓
Upload Session
   ↓
URL temporal
   ↓
Azure Blob
   ↓
Worker
   ↓
Hash
   ↓
Validación
   ↓
Malware Scan
   ↓
AVAILABLE
```

Antes del análisis:

```text
QUARANTINED
```

Después:

```text
AVAILABLE
```

Las descargas utilizan URLs temporales.

Los documentos:

```text
APPROVED
SIGNED
```

no pueden reemplazarse.

Una modificación crea una nueva versión.



---

# 31. Clasificación documental

## Interno

Ejemplos:

* Propuestas.
* Formatos.
* Notas operativas.

Controles:

* Autenticación.
* RBAC.
* Auditoría.

## Confidencial

Ejemplos:

* CV.
* Pruebas.
* Feedback.
* Acuerdos.
* Contratos.

Controles:

* Cifrado.
* Need-to-know.
* Descarga auditada.
* Retención.

## Restringido

Ejemplos:

* Identificación.
* Datos bancarios.
* Tarifas.
* Facturas.
* Documentos legales especiales.

Controles:

* Permiso específico.
* MFA.
* Acceso temporal.
* Alertas.
* Revisión periódica.



---

# 32. Permisos mínimos

## Identity

```text
user.read
user.manage
role.read
role.manage
audit.read
```

## Company

```text
company.read
company.create
company.update
company.archive
contact.manage
```

## Opportunity

```text
opportunity.read
opportunity.create
opportunity.update
opportunity.transition
opportunity.close
opportunity.reopen
opportunity.reassign
financialFields.read
financialFields.update
```

## Requirement

```text
requirement.read
requirement.create
requirement.update
requirement.complete
requirement.approveVersion
```

## Vacancy

```text
vacancy.read
vacancy.create
vacancy.update
vacancy.publish
vacancy.transition
vacancy.close
vacancy.reopen
vacancy.reassign
```

## Candidate

```text
candidate.read
candidate.create
candidate.update
candidate.archive
pii.read
compensation.read
consent.manage
```

## Application

```text
application.read
application.create
application.transition
application.evaluate
application.present
application.select
application.reject
application.reopen
```

## Interview

```text
interview.read
interview.schedule
interview.update
interview.cancel
interview.feedback.write
interview.internalFeedback.read
```

## Engagement

```text
engagement.read
engagement.create
engagement.update
engagement.transition
engagement.activate
engagement.end
engagement.renew
```

## Document

```text
document.read
document.upload
document.download
document.approve
document.reject
document.sign
document.restricted.read
```

## Billing

```text
invoice.read
invoice.create
invoice.update
invoice.issue
invoice.markPaid
financial.export
```

## Work / Reporting

```text
task.manageOwn
task.manageAll
activity.create
report.read
export.basic
export.sensitive
```



---

# 33. Eventos de dominio prioritarios

| Evento                     | Efectos                                                    |
| -------------------------- | ---------------------------------------------------------- |
| `CompanyCreated`           | Timeline, deduplicación secundaria y seguimiento inicial   |
| `OpportunityCreated`       | Pipeline, alertas, tareas y dashboard                      |
| `OpportunityStatusChanged` | Pipeline, alertas, tareas y dashboard                      |
| `RequirementCompleted`     | Notificar Talento y habilitar creación de Vacancy          |
| `VacancyOpened`            | Asignación, SLA de búsqueda y notificación                 |
| `ApplicationPresented`     | Actualizar Vacancy/Opportunity y solicitar feedback        |
| `InterviewScheduled`       | Calendar, recordatorios y tareas                           |
| `CandidateSelected`        | Checklist jurídico, actualización Vacancy y notificaciones |
| `EngagementReadyToStart`   | Tareas de inicio, seguimiento y disponibilidad             |
| `EngagementActivated`      | Tareas, facturación, seguimiento y disponibilidad          |
| `DocumentApproved`         | Checklist, notificación y posible transición               |
| `DocumentRejected`         | Checklist, notificación y posible transición               |
| `InvoiceDue`               | Alertas financieras, dashboard y seguimiento               |
| `InvoiceOverdue`           | Alertas financieras, dashboard y seguimiento               |
| `InvoicePaid`              | Alertas financieras, dashboard y seguimiento               |
| `ProcessPaused`            | Cancelar/recrear recordatorios, indicadores y timeline     |
| `ProcessReopened`          | Cancelar/recrear recordatorios, indicadores y timeline     |
| `ProcessClosed`            | Cancelar/recrear recordatorios, indicadores y timeline     |



---

