

# Tech Leap — CRM de Empleabilidad

## Plan Scrum, Requerimientos y Backlog Técnico

> **Documento base:** Diseño técnico y operativo del CRM de Empleabilidad Tech Leap v1.0
> **Fecha del documento fuente:** 2 de septiembre de 2026
> **Arquitectura:** Monolito modular API-first
> **Equipo:** 3 integrantes

---

# 1. Objetivo del proyecto

El CRM de Tech Leap tiene como objetivo centralizar y controlar el proceso completo de empleabilidad:

```text
Empresa
   ↓
Contacto
   ↓
Oportunidad
   ↓
Requerimiento
   ↓
Vacante
   ↓
Candidato
   ↓
Application
   ↓
Entrevista / Prueba
   ↓
Selección
   ↓
Vinculación
   ↓
Documentos
   ↓
Seguimiento
   ↓
Facturación operativa
   ↓
Cierre / Renovación
```

La decisión arquitectónica principal es construir un **monolito modular API-first**, con PostgreSQL como fuente única de verdad, almacenamiento privado de archivos y un Worker asíncrono para automatizaciones e integraciones. 

---

# 2. Alcance funcional del MVP

El MVP debe incluir:

## 2.1 CRM comercial

* Empresas.
* Contactos.
* Oportunidades.
* Responsables.
* Próxima acción.
* Pipeline.

## 2.2 ATS

* Requerimientos.
* Vacantes.
* Candidatos.
* Skills.
* Applications.
* Entrevistas.
* Pruebas.
* Selección.

## 2.3 Operación

* Tareas.
* Actividades.
* Seguimientos.
* Alertas.
* Historial.
* Reasignaciones.

## 2.4 Vinculación

* Checklist documental.
* Condiciones.
* Inicio.
* Seguimiento.
* Finalización.
* Renovación.

## 2.5 Documentos

* Archivos privados.
* Versiones.
* Estados de aprobación.
* Control de acceso.

## 2.6 Facturación

* Facturas asociadas a empresa y vinculación.
* Estado.
* Fechas.

> La facturación del CRM es **operativa** y no reemplaza un ERP contable.

## 2.7 Reportes

* Reportes operativos.
* Exportaciones filtradas a CSV/XLSX.
* PDF solamente para reportes definidos.



---

# 3. Funcionalidades fuera del MVP

No deben incluirse en la primera versión:

* Portal autoservicio para empresas.
* Portal autoservicio para talentos.
* Matching mediante IA.
* Asistente conversacional.
* Predicción de cierres.
* Predicción de renovaciones.
* Automatización de WhatsApp.
* Extracción automática desde LinkedIn.
* Contabilidad completa.
* Nómina.
* Firma electrónica propia.
* Reemplazo de Google Workspace.



---

# 4. Principios obligatorios

## 4.1 Una sola fuente de verdad

Cada dato debe tener una fuente principal.

Las integraciones no deben crear maestros paralelos.

## 4.2 Seguridad en backend

El frontend puede ocultar o deshabilitar acciones, pero **nunca decide la seguridad**.

Las reglas de negocio y autorización deben ejecutarse en backend.

## 4.3 Trazabilidad

Las acciones importantes deben conservar:

* Actor.
* Fecha.
* Motivo.
* Estado.
* `correlationId`.

## 4.4 Automatización

Primero se estabiliza el flujo y sus excepciones.

Después se automatiza.

## 4.5 Modularidad

Se debe diseñar para poder extraer módulos posteriormente, pero inicialmente operar como una sola aplicación.



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
* Vite.
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
    /web
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

### `/apps/web`

SPA React:

* Rutas.
* Layouts.
* Componentes.
* Permisos visuales.
* Cliente API generado desde OpenAPI.

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

# 13. Flujo operativo principal

## Paso 1 — Crear empresa

Actor:

* Comercial.
* Líder.

Debe:

1. Buscar duplicados.
2. Utilizar `taxId` normalizado.
3. Revisar dominio.
4. Revisar teléfono.
5. Revisar similitud del nombre.
6. Evitar creación silenciosa de duplicados.
7. Crear Company en `PROSPECT`.
8. Asignar `ownerUserId`.
9. Crear Contact principal cuando corresponda.
10. Crear Activity.
11. Programar próxima acción.



---

# 14. Crear oportunidad

Actor:

* Comercial.
* Líder.

Precondiciones:

* Company válida.
* Contacto principal.
* Necesidad inicial.
* Modalidad.
* Responsable.
* Próxima acción.
* Fecha.

Debe:

1. Verificar oportunidad equivalente abierta.
2. Crear Opportunity en `IDENTIFIED`.
3. Registrar probabilidad si existe.
4. Registrar valor si existe.
5. Marcar valores como estimados.
6. Crear Task de seguimiento.
7. Registrar Timeline.



---

# 15. Levantar Requirement

Actor:

* Comercial.
* Líder para excepciones.

Debe contener según modalidad:

* Posiciones.
* Perfil.
* Skills.
* Experiencia.
* Ubicación.
* Modalidad de trabajo.
* Rango económico.
* Fecha objetivo.
* Etapas de selección.

Debe:

1. Crear Requirement en borrador.
2. Versionarlo.
3. Validar completitud.
4. Mantener versiones aprobadas inmutables.
5. Cambiar Opportunity a:

```text
REQUIREMENT_CAPTURED
```

6. Notificar a Talento.
7. Crear tarea de revisión.



---

# 16. Crear Vacancy

Precondición:

```text
Requirement completo
```

Debe:

1. Crear Vacancy en `DRAFT`.
2. Heredar datos del Requirement.
3. Permitir ajustes propios de la Vacancy.
4. No modificar el Requirement aprobado.
5. Validar campos obligatorios.
6. Publicar:

```text
DRAFT
  ↓
OPEN
  ↓
SEARCHING
```

7. Crear tareas/SLA.
8. Notificar al owner de Talento.



---

# 17. Asociar Candidate

La relación Candidate ↔ Vacancy se realiza mediante:

```text
Application
```

Nunca directamente.

Precondiciones:

* Vacancy abierta.
* Candidate activo.
* Autorización vigente.
* Retención vigente.
* Perfil mínimo.

Debe:

1. Crear Application.
2. Aplicar restricción única.
3. Registrar score si aplica.
4. Registrar razón de asociación si aplica.
5. Iniciar en:

```text
ASSOCIATED
```

6. Registrar responsable.
7. Crear Activity.
8. Permitir asociación masiva con resumen de errores.



---

# 18. Estados de Application

```text
ASSOCIATED
    ↓
PRESELECTED
    ↓
VALIDATED
    ↓
PRESENTED
    ↓
INTERVIEW
    ↓
TECH_TEST
    ↓
SELECTED
    ↓
ENGAGED
```

También pueden existir:

```text
NOT_SELECTED
WITHDRAWN
```

### Regla crítica

Los estados:

* Presentado.
* Entrevista.
* Prueba.
* Seleccionado.
* No seleccionado.

pertenecen a **Application**, no a Candidate.

Candidate únicamente representa disponibilidad global.



---

# 19. Cambios de estado

Nunca se debe permitir:

```http
PATCH /application/{id}
```

para modificar libremente:

```text
status
```

Las transiciones deben ejecutarse mediante una operación explícita.

Ejemplo:

```http
POST /applications/{id}/transitions
```

Debe validar:

* `targetStatus`
* `expectedVersion`
* `reasonCode`
* `comment`
* `effectiveAt`
* permisos
* estado actual
* condiciones de negocio
* concurrencia

Debe crear:

```text
StatusHistory
```

y publicar el evento correspondiente en:

```text
Outbox
```



---

# 20. Seleccionar Candidate

Precondiciones:

* Application elegible.
* Vacancy activa.
* Decisión de empresa registrada.
* Condiciones registradas.

Debe:

1. Confirmar candidato.
2. Confirmar Vacancy.
3. Confirmar número de posición.
4. Confirmar condiciones.
5. Cambiar Application a:

```text
SELECTED
```

6. Registrar:

   * `selectedAt`
   * `selectedBy`

7. No rechazar automáticamente otras Applications sin verificar posiciones.

8. Cambiar Vacancy a `FILLED` solamente cuando:

```text
filledPositions >= positionsCount
```

9. Crear checklist.
10. Crear tarea para Jurídica/Administrativa.

Importante:

> `SELECTED` **no significa** que el candidato ya tenga una vinculación activa.



---

# 21. Crear Engagement

Precondiciones:

* Application `SELECTED`.
* Company validada.
* Condiciones económicas aprobadas.
* Modalidad definida.
* Fechas definidas.

Debe:

1. Crear Engagement de forma idempotente.
2. Asociarlo con:

   * Application.
   * Company.
   * Candidate.
   * Vacancy.
3. Iniciar en:

```text
PENDING_DOCUMENTS
```

o:

```text
IN_VALIDATION
```

4. Validar:

   * Documentos.
   * Acuerdo/contrato.
   * Fechas.
   * Valor.
   * Ciclo de facturación.

5. Pasar a:

```text
READY_TO_START
```

y posteriormente:

```text
ACTIVE
```

6. Crear tareas.
7. Crear seguimiento.
8. Preparar facturación.



---

# 22. Registrar seguimiento

Un seguimiento se registra como:

```text
Activity
```

Puede ser:

* Llamada.
* Correo.
* Reunión.
* Nota.
* Seguimiento.

Debe:

1. Registrar Activity.
2. Actualizar `lastActivityAt`.
3. Crear/completar Task de próxima acción.
4. Exigir una razón si no existe próxima acción.
5. Notificar al responsable cuando corresponda.

Esto permite detectar procesos estancados.



---

# 23. Pausar, cerrar y reabrir

## PAUSE

Debe guardar:

* `previousActiveStatus`
* `reasonCode`
* `comment`
* `reviewAt`

Además:

* Cancelar recordatorios aplicables.
* Reprogramarlos cuando corresponda.

## CLOSE

Debe exigir un resultado.

Ejemplos:

* Ganada.
* Perdida.
* Cubierta.
* Cerrada.
* No seleccionada.
* Retirada.
* Finalizada.

## REOPEN

No borra el cierre anterior.

Debe crear una nueva transición.

## REASSIGN

Debe:

* Conservar owner anterior en historial.
* Crear tareas para nuevo responsable.

Las reversiones sensibles requieren permiso de Líder.



---

# 24. Estados de Opportunity

```text
IDENTIFIED
   ↓
CONTACTED
   ↓
REQUIREMENT_CAPTURED
   ↓
IN_PROGRESS
   ↓
PROFILES_SENT
   ↓
INTERVIEWS
   ↓
NEGOTIATION
   ↓
WON / LOST
```

También:

```text
PAUSED
```

Las condiciones de cada transición deben validarse en backend. 

---

# 25. Estados de Vacancy

```text
DRAFT
 ↓
OPEN
 ↓
SEARCHING
 ↓
PROFILES_SUBMITTED
 ↓
EVALUATING
 ↓
FILLED
 ↓
CLOSED
```

También:

```text
PAUSED
```

Las transiciones deben conservar historial y validar las condiciones correspondientes. 

---

# 26. Estados de Engagement

```text
PENDING_DOCUMENTS
        ↓
IN_VALIDATION
        ↓
PENDING_AGREEMENT
        ↓
READY_TO_START
        ↓
ACTIVE
        ↓
ENDED
```

También:

```text
ON_HOLD
CANCELLED
RENEWED
```



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

# 34. Épicas del proyecto

| ID      | Épica                     | Prioridad |
| ------- | ------------------------- | --------- |
| EPIC-01 | Plataforma y Arquitectura | P0        |
| EPIC-02 | Identity & Access         | P0        |
| EPIC-03 | CRM                       | P0        |
| EPIC-04 | ATS                       | P0        |
| EPIC-05 | Engagements               | P0        |
| EPIC-06 | Documents                 | P1        |
| EPIC-07 | Work                      | P0        |
| EPIC-08 | Billing                   | P1        |
| EPIC-09 | Reporting                 | P1        |
| EPIC-10 | Integrations / Worker     | P1        |
| EPIC-11 | Security / Audit          | P0        |
| EPIC-12 | DevOps / Observability    | P0        |

---

# 35. Distribución del equipo

## Integrante 1 — Backend / Datos / Arquitectura

Responsabilidades:

* .NET.
* EF Core.
* PostgreSQL.
* Arquitectura modular.
* Casos de uso.
* Reglas de negocio.
* API.
* Migraciones.
* Transacciones.
* Outbox.

---

## Integrante 2 — Seguridad / Backend transversal / QA

Responsabilidades:

* Auth0.
* RBAC.
* Policies.
* Auditoría.
* Seguridad.
* Pruebas.
* Concurrencia.
* Integraciones.
* Validaciones.
* Revisión técnica.

---

## Integrante 3 — Frontend

Responsabilidades:

* React.
* TypeScript.
* MUI.
* Formularios.
* Tablas.
* Navegación.
* Permisos visuales.
* Consumo API.
* Manejo de errores.
* Pruebas E2E.

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
/apps/web
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
