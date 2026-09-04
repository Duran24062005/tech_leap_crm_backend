# Tech Leap CRM — Requerimientos funcionales

Documento separado desde `Requerimientos.md`. Contiene la visión, el alcance y los flujos funcionales del producto.

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

