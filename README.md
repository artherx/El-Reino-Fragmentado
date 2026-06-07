# PROJECT.md — Contexto General del Proyecto


---

## Identidad del Proyecto

- **Motor:** Unity (C# + ScriptableObjects)
- **Género:** Acción-aventura isométrica, similar a *Tunic*
- **Estado:** Prototipo inicial

---

## Sistemas Principales

| Sistema | Descripción |
|---|---|
| **Combate** | Mecánicas de ataque, daño, hitboxes y enemigos |
| **Puzzles** | Interacción con el mundo, activación de mecanismos |
| **Devolución del tiempo** | El personaje retrocede su posición a un estado anterior |

---

## Estructura de Carpetas

```
Assets/
└── src/
    ├── app/        → Escenas, puntos de entrada, bootstrapping
    ├── core/       → Lógica de negocio, sistemas del juego
    └── shared/     → Utilidades, helpers, componentes reutilizables
```

Cada carpeta tiene su propio `RULES.md` con reglas específicas. **Siempre consulta el RULES.md de la carpeta antes de crear o modificar archivos en ella.**

---

## Principio Rector: Reutilización de Componentes

Este proyecto usa un modelo inspirado en componentes de React aplicado a Unity:

- Un **componente** es un MonoBehaviour o ScriptableObject con **una sola responsabilidad**.
- Los componentes **no conocen a otros componentes directamente**; se comunican por eventos o interfaces.
- La lógica reutilizable vive en `shared/`. Si algo puede usarse en más de un sistema, va ahí.
- Los Prefabs son la unidad de composición: se construyen ensamblando componentes pequeños, no un MonoBehaviour monolítico.

---

## Convenciones Globales

- **Namespaces:** Todo el código usa namespaces que reflejan la carpeta. Ej: `Game.Core.Combat`, `Game.Shared.Events`.
- **ScriptableObjects para datos:** Los valores de configuración (stats, velocidades, cooldowns) siempre van en un SO, nunca hardcodeados.
- **Prefabs con variantes:** La unidad base se define como Prefab; las variaciones usan Prefab Variants, no duplicados.
- **Naming:** PascalCase para clases y métodos, camelCase para variables privadas, `_camelCase` para campos serializados privados.
- **Sin patrón formal aún:** El agente NO debe imponer MVC/ECS. Debe respetar el modelo de componentes descrito arriba.

---

## Archivos de Referencia

| Archivo | Propósito |
|---|---|
| `src/app/RULES.md` | Reglas para escenas y bootstrapping |
| `src/core/RULES.md` | Reglas para sistemas de juego |
| `src/shared/RULES.md` | Reglas para utilidades y componentes base |
| `src/core/SYSTEMS.md` | Descripción de cada sistema y su estado |