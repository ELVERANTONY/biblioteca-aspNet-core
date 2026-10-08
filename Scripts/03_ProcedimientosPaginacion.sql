USE BibliotecaDB;
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS CON PAGINACIÓN
-- =============================================

-- Listar libros activos con nombre del autor (PAGINADO)
CREATE OR ALTER PROCEDURE sp_ListarLibrosPaginado
    @Titulo NVARCHAR(150) = NULL,
    @Pagina INT = 1,
    @TamanoPagina INT = 15
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Inicio INT = (@Pagina - 1) * @TamanoPagina + 1;
    DECLARE @Fin INT = @Pagina * @TamanoPagina;
    
    SELECT 
        l.LibroId,
        l.Titulo,
        l.ISBN,
        l.AutorId,
        l.Ejemplares,
        l.Activo,
        a.Nombre AS NombreAutor
    FROM Libros l
    INNER JOIN Autores a ON l.AutorId = a.AutorId
    WHERE l.Activo = 1
        AND (@Titulo IS NULL OR l.Titulo LIKE '%' + @Titulo + '%')
    ORDER BY l.Titulo
    OFFSET (@Pagina - 1) * @TamanoPagina ROWS
    FETCH NEXT @TamanoPagina ROWS ONLY;
END
GO

-- Contar libros activos (para paginación)
CREATE OR ALTER PROCEDURE sp_ContarLibros
    @Titulo NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS Total
    FROM Libros l
    WHERE l.Activo = 1
        AND (@Titulo IS NULL OR l.Titulo LIKE '%' + @Titulo + '%');
END
GO

-- Listar socios activos (PAGINADO)
CREATE OR ALTER PROCEDURE sp_ListarSociosPaginado
    @Pagina INT = 1,
    @TamanoPagina INT = 15
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre
    OFFSET (@Pagina - 1) * @TamanoPagina ROWS
    FETCH NEXT @TamanoPagina ROWS ONLY;
END
GO

-- Contar socios activos (para paginación)
CREATE OR ALTER PROCEDURE sp_ContarSocios
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS Total
    FROM Socios
    WHERE Activo = 1;
END
GO

-- Reporte de préstamos por intervalo de fechas (PAGINADO)
CREATE OR ALTER PROCEDURE sp_ReportePrestamosPaginado
    @Desde DATETIME,
    @Hasta DATETIME,
    @Pagina INT = 1,
    @TamanoPagina INT = 15
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.PrestamoId,
        s.Nombre AS NombreSocio,
        s.DNI,
        STRING_AGG(l.Titulo, ', ') AS Libros,
        p.FechaPrestamo,
        p.FechaLimite,
        p.Estado
    FROM Prestamos p
    INNER JOIN Socios s ON p.SocioId = s.SocioId
    INNER JOIN DetallePrestamo dp ON p.PrestamoId = dp.PrestamoId
    INNER JOIN Libros l ON dp.LibroId = l.LibroId
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta
    GROUP BY p.PrestamoId, s.Nombre, s.DNI, p.FechaPrestamo, p.FechaLimite, p.Estado
    ORDER BY p.FechaPrestamo DESC
    OFFSET (@Pagina - 1) * @TamanoPagina ROWS
    FETCH NEXT @TamanoPagina ROWS ONLY;
END
GO

-- Contar préstamos (para paginación)
CREATE OR ALTER PROCEDURE sp_ContarPrestamos
    @Desde DATETIME,
    @Hasta DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(DISTINCT p.PrestamoId) AS Total
    FROM Prestamos p
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta;
END
GO
