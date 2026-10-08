USE BibliotecaDB;
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS PARA LIBROS
-- =============================================

-- Listar libros activos con nombre del autor
CREATE OR ALTER PROCEDURE sp_ListarLibros
    @Titulo NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
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
    ORDER BY l.Titulo;
END
GO

-- Obtener libro por ID
CREATE OR ALTER PROCEDURE sp_ObtenerLibroPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    
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
    WHERE l.LibroId = @LibroId;
END
GO

-- Insertar libro
CREATE OR ALTER PROCEDURE sp_InsertarLibro
    @Titulo NVARCHAR(150),
    @ISBN NVARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares);
    
    SELECT SCOPE_IDENTITY() AS LibroId;
END
GO

-- Actualizar libro
CREATE OR ALTER PROCEDURE sp_ActualizarLibro
    @LibroId INT,
    @Titulo NVARCHAR(150),
    @ISBN NVARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Libros
    SET Titulo = @Titulo,
        ISBN = @ISBN,
        AutorId = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END
GO

-- Eliminar libro (lógica)
CREATE OR ALTER PROCEDURE sp_EliminarLibro
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Libros
    SET Activo = 0
    WHERE LibroId = @LibroId;
END
GO

-- Listar autores activos
CREATE OR ALTER PROCEDURE sp_ListarAutores
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT AutorId, Nombre, Nacionalidad, Activo
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS PARA SOCIOS
-- =============================================

-- Listar socios activos
CREATE OR ALTER PROCEDURE sp_ListarSocios
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- Insertar socio
CREATE OR ALTER PROCEDURE sp_InsertarSocio
    @DNI NVARCHAR(8),
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Socios (DNI, Nombre, Email)
    VALUES (@DNI, @Nombre, @Email);
    
    SELECT SCOPE_IDENTITY() AS SocioId;
END
GO

-- Verificar si DNI existe
CREATE OR ALTER PROCEDURE sp_VerificarDNI
    @DNI NVARCHAR(8)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS Existe
    FROM Socios
    WHERE DNI = @DNI AND Activo = 1;
END
GO

-- =============================================
-- PROCEDIMIENTOS ALMACENADOS PARA PRÉSTAMOS
-- =============================================

-- Reporte de préstamos por intervalo de fechas
CREATE OR ALTER PROCEDURE sp_ReportePrestamos
    @Desde DATETIME,
    @Hasta DATETIME
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
    ORDER BY p.FechaPrestamo DESC;
END
GO
