# Guía del Proyecto y Reglas de Desarrollo para OpenCode

## 1. Contexto del Proyecto
* **Motor:** Unity
* **Lenguaje:** C#
* **Gestión de Datos:** ScriptableObjects (Actúan como el "estado global" o las "props" inmutables).
* **Estilo de Juego:** Aventura isométrica, similar a *Tunic*.
* **Estado Actual:** Prototipo inicial. No hay un patrón arquitectónico estricto (MVC/MVVM), pero se exige alta modularidad.

### Sistemas Principales (Core)
1.  **Combate y Enemigos:** Sistema de daño, hitboxes, estados de enemigos.
2.  **Puzzles e Interacción:** Lógica de entorno, interruptores, puertas.
3.  **Manipulación del Tiempo:** Mecánica de rebobinado (retroceso de posición y estado).

---

## 2. Estructura de Directorios (`src/`)

El código base se divide estrictamente en tres dominios dentro de la carpeta `src/`. OpenCode debe respetar esta jerarquía al crear o modificar archivos:

### `src/app/` (Capa de Presentación y Entrada)
* **Propósito:** Contiene todo lo que inicializa el juego y maneja el flujo de escenas.
* **Contenido:** * Scripts de inicialización (Bootstrappers).
    * Gestores de escenas y transiciones.
    * Controladores de UI principales.
    * Puntos de entrada de configuración.

### `src/core/` (Lógica de Negocio y Sistemas)
* **Propósito:** El "cerebro" del juego. Lógica pura que no depende de interfaces específicas.
* **Contenido:**
    * Sistemas de Combate (salud, daño, IA de enemigos).
    * Sistemas de Puzzles (validación de reglas, estado del mundo).
    * Sistema de Rebobinado de Tiempo (grabación de posiciones, buffers de tiempo).
    * Definición de *ScriptableObjects* para datos de juego.

### `src/shared/` (Utilidades y Helpers)
* **Propósito:** Código transversal y agnóstico que puede ser utilizado por `app/` o `core/` sin crear dependencias circulares.
* **Contenido:**
    * Funciones matemáticas personalizadas o utilidades de físicas.
    * Extensiones de C# (Extension Methods).
    * Manejadores de eventos genéricos.
    * Constantes globales o Enums compartidos.

---

## 3. Filosofía de Componentes Reutilizables (Estilo React)

Para emular el principio de reutilización de React en Unity, OpenCode debe aplicar estrictamente las siguientes reglas de diseño:

* **Composición sobre Herencia:** No crear árboles de herencia profundos (evitar clases base masivas como `EnemyBase -> MeleeEnemy -> SpecificEnemy`). En su lugar, componer objetos usando pequeños scripts independientes (ej. `HealthComponent`, `MovementComponent`, `TimeRecordComponent`).
* **Props y Estado (ScriptableObjects vs MonoBehaviour):** * **Props (Inmutables):** Usar *ScriptableObjects* para definir las estadísticas base y la configuración que no cambia durante el gameplay (ej. `EnemyStats`, `WeaponConfig`).
    * **Estado (Mutable):** Usar *MonoBehaviours* solo para el estado local y la lógica reactiva en tiempo real (ej. `CurrentHealth`, `CurrentPosition`).
* **Prefabs como "Componentes React":** La unidad fundamental de reutilización en la escena es el **Prefab**. 
    * Crear Prefabs base con la lógica modular acoplada.
    * Usar **Prefab Variants** para crear instancias específicas (ej. un enemigo de fuego es una variante del Prefab base de enemigo, inyectándole un *ScriptableObject* de daño de fuego).
* **Single Responsibility Principle (SRP):** Un script de MonoBehaviour debe hacer una sola cosa. Si un script maneja input, movimiento y rebobinado de tiempo, debe dividirse en tres scripts diferentes que se comunican a través de eventos o referencias directas.

---

## 4. Reglas de Operación para OpenCode

1.  **Análisis de Impacto:** Antes de sugerir un nuevo script, evalúa si su funcionalidad puede lograrse combinando o modificando componentes existentes en `src/shared/` o `src/core/`.
2.  **Ubicación Estricta:** Nunca mezclar lógica de negocio (`core/`) dentro de scripts de UI o escenas (`app/`).
3.  **Dependencias Unidireccionales:** `app/` puede conocer a `core/` y `shared/`. `core/` solo puede conocer a `shared/`. `shared/` no debe depender ni de `app/` ni de `core/`.
4.  **Generación de Respuestas:** Al proponer soluciones, describe la estructura de componentes, los campos públicos (para asignación en el Inspector) y los eventos/delegados a usar. **No generar código completo a menos que el usuario lo solicite explícitamente; enfocarse en la arquitectura.**