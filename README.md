# Biblioteca.Web - MVC con Dapper

Proyecto de laboratorio para el curso de Desarrollo de Aplicaciones Empresariales Avanzadas.

## Descripción

Aplicación web ASP.NET Core MVC que implementa un sistema de gestión de biblioteca utilizando:
- **Patrón MVC** (Modelo-Vista-Controlador)
- **Dapper** como ORM para acceso a datos
- **SQL Server** como base de datos (ejecutado en Docker)

## Estructura del Proyecto

```
Biblioteca.Web/
├── Controllers/
│   ├── HomeController.cs
│   ├── LibrosController.cs
│   ├── SociosController.cs
│   └── PrestamosController.cs
├── Models/
│   ├── Libro.cs
│   ├── Socio.cs
│   ├── Autor.cs
│   ├── PrestamoReporte.cs
│   └── ErrorViewModel.cs
├── Repositorios/
│   ├── LibroRepositorio.cs
│   ├── SocioRepositorio.cs
│   └── PrestamoRepositorio.cs
├── Views/
│   ├── Libros/
│   ├── Socios/
│   ├── Prestamos/
│   ├── Home/
│   └── Shared/
├── Scripts/
│   ├── 01_CrearBaseDatos.sql
│   └── 02_ProcedimientosAlmacenados.sql
├── appsettings.json
└── Program.cs
```

## Requisitos Previos

- .NET 10.0 SDK
- Docker Desktop
- SQL Server 2022 (contenedor Docker)

## Configuración

### 1. Base de Datos (Docker)

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name biblioteca-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Ejecutar Scripts SQL

```bash
# Copiar scripts al contenedor
docker cp Scripts/01_CrearBaseDatos.sql biblioteca-sql:/tmp/
docker cp Scripts/02_ProcedimientosAlmacenados.sql biblioteca-sql:/tmp/

# Ejecutar scripts
docker exec biblioteca-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C -i /tmp/01_CrearBaseDatos.sql
docker exec biblioteca-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C -i /tmp/02_ProcedimientosAlmacenados.sql
```

### 3. Aplicación

```bash
dotnet restore
dotnet run
```

La aplicación estará disponible en `http://localhost:5000`

## Funcionalidades

### Libros
- Listado con búsqueda por título
- Crear, editar, eliminar (lógica) y ver detalles
- Validación de campos con DataAnnotations
- Lista desplegable de autores

### Socios
- Listado de socios activos
- Crear nuevos socios
- Validación de DNI único

### Préstamos
- Reporte por intervalo de fechas
- Vista de préstamos con socio, libros, fechas y estado

## Recorrido de una Petición

### Ejemplo: Crear Libro

1. **Ruta**: `GET /Libros/Create`
2. **Controlador**: `LibrosController.Create()`
3. **Repositorio**: `LibroRepositorio.ObtenerAutoresAsync()`
4. **Procedimiento Almacenado**: `sp_ListarAutores`
5. **Vista**: `Views/Libros/Create.cshtml`

**Flujo de datos**:
- Se usa `ViewBag.Autores` para pasar la lista de autores al dropdown
- Se usa `ViewData["Title"]` para el título de la página
- El modelo `Libro` se usa para el formulario (vista fuertemente tipada)

## Tecnologías Utilizadas

- ASP.NET Core 10.0
- Dapper 2.1.66
- Microsoft.Data.SqlClient 6.0.1
- SQL Server 2022
- Bootstrap 5
- Razor Views

## Autor

Arévalo Sermeño, Edwin William
Curso: Desarrollo de Aplicaciones Empresariales Avanzadas
Sección: 6 - C24 - Sección C - D
