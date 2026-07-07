# SistemaBiblioteca

Descripción
-----------
API REST para gestión de una biblioteca. Proyecto dividido en dos proyectos principales:

- `ApiBiblioteca`: API ASP.NET Core que expone endpoints.
- `databaseFirst`: modelos generados por Database-First (Entity Framework Core) y el DbContext `bibliotecaContext`.

Tecnologías
----------

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (Database First)
- SQL Server

Estructura relevante
--------------------

- DataBaseFirst.Contexts.bibliotecaContext: contexto de datos (DbContext) que expone DbSet para `categorias`, `libros`, `prestamos`, `t_personal`, `usuarios`.
- DataBaseFirst.Models: clases de entidad generadas por EF (por ejemplo `prestamos`, `libros`, `usuarios`).
- ApiBiblioteca/Controllers: controladores expuestos por la API, actualmente:
  - AuthController
  - CategoriasController
  - LibrosController
  - PrestamosController
  - UsuariosController

Especificaciones de API
----------------------

- Rutas base: `api/[controller]` (por ejemplo `GET /api/libros`, `GET /api/prestamos`).
- Endpoints CRUD estándar implementados para entidades principales:
  - GET collection
  - GET by id
  - POST para crear
  - PUT para actualizar
  - PATCH para desactivar (soft-delete): muchos controladores marcan `estado = false` en lugar de borrar físicamente.

Notas sobre la base de datos y configuración
-------------------------------------------

- El proveedor usado es SQL Server. El `bibliotecaContext` incluye una cadena de conexión en el método `OnConfiguring` (Server=localhost;Database=sistemaBiblioteca;User ID=brian;Password=123456;...).
- Recomendación: mover la cadena de conexión a `appsettings.json` y registrar el contexto en `Program.cs` mediante `AddDbContext<bibliotecaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))` para mayor flexibilidad y seguridad.

Cómo ejecutar
-------------

1. Abrir la solución `SistemaBiblioteca.slnx` en Visual Studio 2026.
2. Establecer `ApiBiblioteca` como proyecto de inicio.
3. Construir (Build) la solución.
4. Ejecutar la API (F5 o Ctrl+F5). Asegurarse de que el servidor SQL esté accesible y la cadena de conexión sea correcta.

Ejemplo rápido
--------------

- Obtener todos los préstamos activos:

  curl -X GET "https://localhost:5001/api/prestamos"

- Crear un préstamo (ejemplo JSON):

  {
	"id_usuario": 1,
	"id_libro": 2,
	"fechaPrestamo": "2026-07-07",
	"fechaDevolucion": "2026-07-21",
	"estado": true
  }
