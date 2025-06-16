# Especificación del Proyecto: App de Notas con Notebooks y Tags

## Descripción General

Una aplicación de escritorio nativa para Windows desarrollada en C# con WinUI 3, diseñada para la gestión de notas organizadas por notebooks y tags. La aplicación seguirá un enfoque offline-first con capacidades completas de exportación e importación para portabilidad de datos.

## Características Principales

- Gestión offline completa sin requerimientos de conexión
- Organización flexible mediante notebooks (carpetas) y sistema de tags
- Soporte para archivos de texto (.txt), Markdown (.md) y JSON (futuro)
- Exportación/importación completa de datos en formato JSON
- Interfaz moderna siguiendo Design System de Windows 11
- Arquitectura modular y extensible para futuras funcionalidades
- Sistema de backup automático y manual
- Compatibilidad total con aplicaciones empaquetadas y desempaquetadas

## Tecnologías y Arquitectura

### Stack Tecnológico
- **Framework**: WinUI 3 (Windows App SDK 1.7)
- **Lenguaje**: C# (.NET 8.0)
- **Base de datos**: SQLite (embebida)
- **ORM**: Entity Framework Core
- **Patrones**: MVVM con servicios de negocio
- **Inyección de dependencias (futuro)**: Microsoft.Extensions.DependencyInjection
- **Testing (futuro)**: NUnit/MSTest para pruebas unitarias

### Estructura del Proyecto
```
YourNotesApp/
├── FluentNotes/ (Proyecto principal WinUI 3)
│   ├── Views/ (Interfaces de usuario)
│   │   └── Dialogs/ (Diálogos del sistema)
│   ├── ViewModels/ (Lógica de presentación)
│   ├── Models/ (Modelos de datos)
│   ├── Services/ (Servicios de negocio)
│   │   ├── Interfaces/ (Contratos de servicios)
│   │   └── Implementations/ (Implementaciones concretas)
│   │       └── Configuration/ (Servicios de configuración)
│   ├── Data/ (Acceso a datos y repositorios)
│   ├── Utils/ (Utilidades y constantes)
│   │   ├── Constants/ (Constantes de la aplicación)
│   │   └── Providers/ (Proveedores de funcionalidad)
│   └── Converters/ (Convertidores de datos)
├── FluentNotes.Core/ (Biblioteca de clases)
└── FluentNotes.Tests/ (Pruebas unitarias)
```

## Modelo de Datos

### Modelo de Base de Datos

**Entidades Principales**

| Entidad | Propiedades | Descripción |
|---------|------------|-------------|
| **Notebooks** | Id, Name, Description, Color, CreatedAt, UpdatedAt, IsDeleted | Contenedores principales para organizar notas, con soporte para soft delete |
| **Notes** | Id, NotebookId, Title, Content, FileType, CreatedAt, UpdatedAt, IsDeleted | Contenido principal con soporte para txt/md/json y soft delete |
| **Tags** | Id, Name, Color, CreatedAt | Sistema de etiquetado para clasificación cruzada |
| **NoteTags** | NoteId, TagId | Relación muchos-a-muchos entre notas y tags |
| **AppSettings** | Key, Value, ValueType | Configuración de la aplicación en formato clave-valor |

**Características del modelo:**
- **Soft Delete**: Campo IsDeleted para notebooks y notas permite papelera de reciclaje
- **Timestamps**: Tracking automático de creación y modificación
- **Relaciones**: Foreign keys para integridad referencial
- **Flexibilidad**: Soporte para múltiples tipos de archivo
- **Configuración**: Sistema extensible de settings

## Sistema de Exportación/Importación

### Formato de Exportación JSON

**Estructura simplificada del archivo de exportación:**
```
{
  "version": "1.0",
  "exportedAt": "2024-06-13T10:30:00Z",
  "notebooks": [ ... ],
  "notes": [ ... ],
  "tags": [ ... ],
  "noteTags": [ ... ],
  "settings": { ... }
}
```

**Características del formato:**
- **Versionado**: Control de versiones para compatibilidad futura
- **Timestamps**: Formato ISO 8601 estándar
- **Estructura completa**: Incluye todas las entidades y relaciones
- **Autocontenido**: Archivo único con toda la información necesaria
- **Validación**: JSON Schema para verificar integridad

### Sistema de Papelera (Soft Delete)

**Funcionamiento:**
- **Marcado de eliminación**: Items marcados como IsDeleted=true en lugar de eliminación física
- **Vista de papelera**: Sección dedicada para gestionar elementos eliminados
- **Retención configurable**: 30 días por defecto antes de eliminación permanente
- **Restauración**: Capacidad de recuperar notebooks y notas eliminados
- **Limpieza automática**: Proceso programado para eliminación final

**Navegación de papelera:**
- **Acceso**: Sección específica en navegación principal
- **Filtros**: Ver notebooks eliminados, notas eliminadas, o todo
- **Acciones**: Restaurar, eliminar permanentemente, vaciar papelera
- **Información**: Fecha de eliminación y tiempo restante antes de borrado final

### Arquitectura de Almacenamiento

**Estructura de Datos Local**
```
%LOCALAPPDATA%/FluentNotes/ (aplicaciones desempaquetadas)
O
Ubicación sistema para aplicaciones empaquetadas/
├── configs.json (configuración para apps desempaquetadas)
├── notes.db (SQLite - metadatos, estructura, relaciones)
├── content/
│   ├── notes/
│   │   ├── {notebook-id}/
│   │   │   ├── {note-id}.md
│   │   │   ├── {note-id}.txt
│   │   │   └── {note-id}.json
│   │   └── orphaned/ (notas sin notebook)
│   │       ├── {note-id}.md
│   │       └── {note-id}.txt
│   └── templates/ (plantillas futuras)
│       ├── default.md
│       └── meeting-notes.md
├── backups/ (exportaciones automáticas)
│   ├── auto_backup_2024-06-13.json
│   └── manual_export_2024-06-12.json
└── logs/ (registro de actividad)
    └── app.log
```

**Sistema Híbrido de Almacenamiento**
- **SQLite**: Metadatos, índices, relaciones entre entidades y búsquedas rápidas
- **Archivos individuales**: Contenido real de las notas para fácil acceso externo
- **Configuración adaptativa**: Sistema que detecta el tipo de empaquetado y ajusta rutas automáticamente
- **Organización por IDs**: Estructura basada en identificadores únicos para consistencia
- **Naming convention**: Uso de UUIDs o IDs únicos para evitar conflictos
- **Carpeta orphaned**: Notas creadas rápidamente sin notebook asignado
- **Sistema de inicialización**: Configuración automática de directorios y configuraciones iniciales

**Ejemplo de Estructura de IDs**
```
notebook-id: "nb_20240613_001"
note-id: "nt_20240613_15:30:45_abc123"
tag-id: "tg_urgent_001"
attachment-id: "att_20240613_img_001"
```

## Diseño de Interfaz de Usuario

### Flujo de Navegación

**1. Pantalla de Bienvenida**
- Onboarding para nuevos usuarios con diálogo de configuración inicial
- Opción de importar datos existentes
- Configuración inicial básica y detección de primer uso

**2. Vista Dashboard**
- Resumen de actividad y estadísticas
- Accesos rápidos a notebooks frecuentes
- Búsqueda global prominente
- Notas recientes y destacadas

**3. Vista Principal de Trabajo (3 columnas)**
- Navegación lateral con notebooks y tags
- Lista central de notas (estilo visor de correos)
- Editor de contenido en panel derecho
- Diseño responsivo y colapsable

**4. Vistas de Administración**
- Gestión completa de notebooks (grid/lista)
- Administración de tags
- Configuración y preferencias

### Patrones de Interacción

**Creación de Contenido**
- **SplitButton** en toolbar principal para creación rápida
  - Botón principal: Nueva nota (va a "orphaned")
  - Dropdown: Nuevo notebook, Nuevo tag
- **FAB alternativo** en navegación lateral o título personalizado
- **Modales ligeros** específicos para cada tipo de elemento
- **Validación en tiempo real** durante la creación

**Menús Contextuales**

*Para Notebooks (clic derecho en navegación):*
```
├── ✏️ Editar
├── 📋 Duplicar
├── 📊 Ver estadísticas  
├── ───────────────
├── 🔄 Mover notas a...
├── 📤 Exportar
├── ───────────────
└── 🗑️ Eliminar
```

*Para Tags (clic derecho en lista de tags):*
```
├── ✏️ Renombrar
├── 🎨 Cambiar color
├── 📊 Ver notas con este tag
├── ───────────────
├── 🔗 Fusionar con...
├── 📤 Exportar notas
├── ───────────────
└── 🗑️ Eliminar
```

*Para Notas (clic derecho en lista central):*
```
├── 📝 Abrir en editor
├── 👁️ Vista previa
├── 📋 Duplicar
├── ───────────────
├── 📁 Mover a notebook...
├── #️⃣ Gestionar tags
├── 📤 Exportar
├── ───────────────
├── ⭐ Marcar como favorita
├── 📌 Fijar al inicio
├── ───────────────
└── 🗑️ Eliminar
```

**Gestión y Edición**
- **Edición inline** para cambios rápidos (nombres, títulos)
- **Vistas de administración** para gestión masiva
- **Confirmaciones inteligentes** mostrando impacto de eliminaciones
- **Operaciones por lotes** en vistas de administración
- **Undo/Redo** para operaciones críticas

**Navegación Jerárquica**
- **Headers clickeables**: "Notebooks" → Vista administración completa
- **Items específicos**: "Notebook Personal" → Vista de notas filtradas
- **Breadcrumbs** para orientación en vistas profundas
- **Shortcuts de teclado** para usuarios avanzados
- **Estados vacíos** con guidance contextual

## Funcionalidades Core

### Gestión de Notas
- Editor de texto plano y Markdown
- Guardado automático
- Historial de cambios (soft delete)
- Búsqueda de texto completo
- Filtrado por notebooks y tags

### Organización
- Notebooks como contenedores principales
- Tags para clasificación cruzada
- Notas huérfanas para contenido sin categorizar
- Favoritos y notas destacadas

### Portabilidad de Datos
- Exportación completa en formato JSON
- Importación con validación y merge
- Backup automático programado
- Sincronización manual entre dispositivos

### Configuración
- Temas y personalización visual
- Preferencias de editor
- Configuración de backup
- Gestión de datos y limpieza

## Funcionalidades Futuras

### Expansión de Funcionalidades
- **Editor JSON**: Syntax highlighting y validación para archivos .json
- **Modo Picture-in-Picture**: Ventanas flotantes para notas de referencia
- **Adjuntos**: Soporte para imágenes y documentos
- **Plantillas**: Templates predefinidos para tipos de notas
- **Temas avanzados**: Personalización completa de interfaz

### Mejoras de Productividad
- **Búsqueda avanzada**: Filtros complejos y operadores
- **Vista enfocada**: Modo de escritura sin distracciones
- **Estadísticas**: Métricas de productividad y uso
- **Integración**: Posible conexión con servicios de nube (OneDrive, etc.)

## Consideraciones Técnicas

### Rendimiento y Optimización
- **Lazy loading**: Carga diferida de notas y contenido pesado para mejorar tiempo de inicio
- **Virtualización**: Listas virtualizadas para manejo eficiente de grandes cantidades de elementos
- **Indexación**: Full-Text Search nativo de SQLite para búsquedas instantáneas
- **Cache**: Sistema de cache en memoria configurable para contenido frecuentemente accedido
- **Consultas optimizadas**: Uso de índices y prepared statements para mejor rendimiento

### Gestión de Datos y Confiabilidad
- **Transacciones**: Operaciones atómicas para mantener consistencia de datos
- **Migraciones**: Sistema de migración automática de esquema de base de datos
- **Integridad**: Constraints y validaciones para prevenir corrupción de datos
- **Backup**: Sistema robusto de backup con verificación de integridad
- **Recovery**: Capacidad de recuperación automática ante errores

### Seguridad y Estabilidad
- **Validación**: Sanitización de entrada y prevención de inyección de datos
- **Manejo de errores**: Recovery automático y logging para debugging
- **Rollback**: Capacidad de deshacer operaciones críticas
- **Monitoring**: Sistema de logging configurable para troubleshooting

## Criterios de Éxito y Calidad

### Usabilidad y Experiencia de Usuario
- **Curva de aprendizaje mínima**: Interfaz intuitiva siguiendo patrones familiares de Windows
- **Flujo eficiente**: Máximo 3 clicks para cualquier acción común
- **Feedback visual**: Indicadores claros de estado y progreso de operaciones
- **Accesibilidad**: Soporte para navegación por teclado y lectores de pantalla

### Rendimiento y Confiabilidad
- **Tiempo de inicio**: Aplicación funcional en menos de 3 segundos
- **Respuesta instantánea**: Operaciones comunes completadas en menos de 500ms
- **Estabilidad**: Cero pérdida de datos durante operación normal
- **Recovery**: Recuperación automática ante fallos inesperados

### Mantenibilidad y Escalabilidad
- **Código limpio**: Arquitectura modular con responsabilidades bien definidas
- **Documentación**: Cobertura completa de APIs y funcionalidades principales
- **Extensibilidad**: Capacidad de agregar nuevas funcionalidades sin reestructuración mayor
- **Performance**: Manejo eficiente de hasta 10,000 notas sin degradación perceptible