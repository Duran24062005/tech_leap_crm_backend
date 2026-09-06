# Módulos del monolito

Cada módulo es una librería independiente dentro de la solución y mantiene sus fronteras internas mediante las carpetas:

- Domain: entidades, invariantes y reglas propias.
- Application: casos de uso y puertos.
- Infrastructure: persistencia e integraciones del módulo.
- Contracts: DTOs y contratos públicos.

Los módulos no deben escribir directamente en las tablas o reglas de otro módulo. Las integraciones entre módulos deben pasar por contratos o eventos.
