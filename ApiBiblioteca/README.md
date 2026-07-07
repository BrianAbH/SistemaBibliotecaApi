# ApiBiblioteca - SistemaBiblioteca

Este README contiene las especificaciones de la API contenida en el proyecto `ApiBiblioteca`.

Resumen
------

- API REST en ASP.NET Core (.NET 10) para gestionar una biblioteca.
- Usa Entity Framework Core en modo Database-First (proyecto `databaseFirst`).
- Persistencia: SQL Server.

Contexto de datos
------------------

- DbContext: `DataBaseFirst.Contexts.bibliotecaContext`.
- Modelos: `DataBaseFirst.Models` (ej.: `prestamos`, `libros`, `usuarios`, `categorias`).

Controladores y endpoints
-------------------------

- `AuthController` — autenticación (cuando esté configurado).
- `CategoriasController` — CRUD para categorías.
- `LibrosController` — CRUD para libros. PATCH utilizado para desactivar (`estado = false`).
- `PrestamosController` — CRUD para préstamos. PATCH para desactivar.
- `UsuariosController` — CRUD para usuarios. PATCH para desactivar.

Ejecución
---------

1. Abrir la solución `SistemaBiblioteca.slnx` en Visual Studio 2026.
2. Establecer `ApiBiblioteca` como proyecto de inicio.
3. Construir y ejecutar. Asegurarse de que la base de datos SQL Server esté accesible.

Configuración de la conexión
---------------------------

El contexto `bibliotecaContext` contiene una cadena de conexión por defecto en `OnConfiguring`. Para producción o entornos seguros, mover la cadena de conexión a `appsettings.json` o usar secretos/variables de entorno y registrar el contexto en `Program.cs`.
