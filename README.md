# TecnoGasHogar - Portal de Solicitudes de Servicio Técnico

Aplicación web **ASP.NET Core MVC (.NET 10)** con **Entity Framework Core + SQLite** para la **Evaluación Continua 1** del curso Programación I (USMP, ciclo 2026-2).

**Caso:** "TecnoGas Hogar" es una empresa peruana dedicada al mantenimiento e instalación de artefactos a gas en el hogar. Este prototipo permite al personal de atención registrar las solicitudes de servicio que llegan de los clientes y consultarlas en una lista.

## Funcionalidades

- **Registro de solicitud (Insert):** formulario con Cliente, Teléfono, Distrito, Tipo de Servicio y Descripción. Validaciones con DataAnnotations (`[Required]`).
- **Listado de solicitudes (Select):** tabla con todas las solicitudes registradas, ordenadas por fecha de registro (más recientes primero).
- Persistencia en **SQLite** mediante EF Core (solo Insert y Select, según lo solicitado).

## Tecnologías

- .NET 10 (ASP.NET Core MVC)
- C# 14
- Entity Framework Core 10 (SQLite)
- Bootstrap 5
- Git/GitHub (ramas y Pull Requests)
- Docker (despliegue en Render)

## Estructura del proyecto

```
Evaluacion_Continua_1_2026-2/        (raíz del repositorio)
├── Dockerfile                        # Imagen multi-etapa .NET 10
├── render.yaml                       # Blueprint de despliegue en Render
└── TecnoGasHogar/
    ├── Controllers/
    │   ├── HomeController.cs
    │   └── SolicitudServicioController.cs   # Register (Create) + List (Index)
    ├── Data/
    │   └── AppDbContext.cs           # DbContext con DbSet<SolicitudServicio>
    ├── Models/
    │   ├── SolicitudServicio.cs      # Entidad principal
    │   └── ErrorViewModel.cs
    ├── Migrations/                   # Migración inicial (InicialSolicitudes)
    ├── Views/
    │   ├── Home/
    │   ├── Shared/
    │   └── SolicitudServicio/
    │       ├── Create.cshtml         # Formulario de registro
    │       └── Index.cshtml          # Listado de solicitudes
    ├── appsettings.json              # Cadena de conexión SQLite
    └── Program.cs                    # Configuración de EF Core y aplicación de migraciones al iniciar
```

## Entidad `SolicitudServicio`

| Campo          | Tipo       | Notas                                        |
|----------------|------------|----------------------------------------------|
| `Id`           | int        | Clave primaria                               |
| `Cliente`      | string     | Requerido                                    |
| `Telefono`     | string     | Requerido                                    |
| `Distrito`     | string     | Requerido                                    |
| `TipoServicio` | string     | Instalación, Mantenimiento, Revisión, Fuga   |
| `Descripcion`  | string?    | Opcional                                     |
| `FechaRegistro`| DateTime   | `DateTime.Now` por defecto                   |

Cadena de conexión (`appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=tecnogas.db"
}
```

## Ejecución en local

Requisito: .NET SDK 10 y (opcional) la herramienta `dotnet-ef`.

```bash
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef database update      # genera la base de datos SQLite
dotnet run
```

La aplicación queda disponible en `https://localhost:xxxx` según el perfil de lanzamiento.

- Registrar solicitud: `/SolicitudServicio/Create`
- Listado de solicitudes: `/SolicitudServicio/Index`

> Las migraciones también se aplican automáticamente al iniciar la aplicación (`db.Database.Migrate()` en `Program.cs`).

## Flujo de trabajo en Git/GitHub

Ramas utilizadas:

- `main`
- `develop`
- `feature/modelo-sqlite` (Pregunta 1)
- `feature/registro-solicitud` (Pregunta 2)
- `feature/listado-solicitudes` (Pregunta 3)
- `feature/deploy-docker` (Pregunta 5)

Flujo: cada funcionalidad se desarrolló en su rama `feature/`, se integró a `develop` mediante **Pull Request** y, al finalizar, `develop` se fusionó a `main`. Commits descriptivos tipo `feat: ...`.

## Docker

Construcción y ejecución local de la imagen:

```bash
docker build -t tecnogashogar .
docker run -p 8080:8080 tecnogashogar
```

La imagen usa **.NET 10** (multi-etapa) y escucha en `http://0.0.0.0:$PORT` (`8080` por defecto), alineado con lo que espera **Render**.

## Despliegue en Render

### Opción A: Blueprint (`render.yaml`)

1. Crear cuenta en [Render](https://render.com) y conectar el repositorio GitHub `evaluacion20262`.
2. En el dashboard, usar **New → Blueprint** y seleccionar el repositorio. Render lee `render.yaml` y crea el Web Service `tecnogashogar`.
3. Esperar el build y el deploy (primera vez toma algunos minutos).

### Opción B: Web Service manual

1. **New → Web Service** → conectar el repositorio `evaluacion20262`.
2. Render detectará el `Dockerfile` (runtime Docker).
3. Asignar `Name: tecnogashogar`, `Region`, `Branch: main` y `Plan: Free`.
4. Crear y esperar el deploy.

### Variables de entorno

| Variable                 | Valor                          | Descripción                              |
|--------------------------|--------------------------------|------------------------------------------|
| `ASPNETCORE_ENVIRONMENT` | `Production`                   | Entorno de ejecución                     |
| `ASPNETCORE_URLS`        | `http://0.0.0.0:8080`          | Puerto HTTP (Render inyecta `$PORT`)     |

No se requiere base de datos externa: la migración se aplica al arrancar y SQLite usa `Data Source=tecnogas.db` dentro del contenedor.

### Verificación

- Registrar una solicitud desde `/SolicitudServicio/Create`.
- Confirmar que aparece en `/SolicitudServicio/Index`.

## Entregables

- Repositorio GitHub: [https://github.com/alejoortiz20/evaluacion20262](https://github.com/alejoortiz20/evaluacion20262)
- Aplicación publicada en Render: *(URL pública del Web Service)*