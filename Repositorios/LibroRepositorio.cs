using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio
{
    private readonly string _connectionString;

    public LibroRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<(IEnumerable<Libro> Libros, int Total)> ObtenerTodosPaginadoAsync(string? titulo, int pagina, int tamanoPagina)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("Titulo", titulo);
        parametros.Add("Pagina", pagina);
        parametros.Add("TamanoPagina", tamanoPagina);

        var libros = await connection.QueryAsync<Libro>(
            "sp_ListarLibrosPaginado",
            parametros,
            commandType: CommandType.StoredProcedure);

        var total = await connection.ExecuteScalarAsync<int>(
            "sp_ContarLibros",
            new { Titulo = titulo },
            commandType: CommandType.StoredProcedure);

        return (libros, total);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("LibroId", id);

        return await connection.QueryFirstOrDefaultAsync<Libro>(
            "sp_ObtenerLibroPorId",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Libro libro)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("Titulo", libro.Titulo);
        parametros.Add("ISBN", libro.ISBN);
        parametros.Add("AutorId", libro.AutorId);
        parametros.Add("Ejemplares", libro.Ejemplares);

        return await connection.ExecuteScalarAsync<int>(
            "sp_InsertarLibro",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ActualizarAsync(Libro libro)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("LibroId", libro.LibroId);
        parametros.Add("Titulo", libro.Titulo);
        parametros.Add("ISBN", libro.ISBN);
        parametros.Add("AutorId", libro.AutorId);
        parametros.Add("Ejemplares", libro.Ejemplares);

        var filasAfectadas = await connection.ExecuteAsync(
            "sp_ActualizarLibro",
            parametros,
            commandType: CommandType.StoredProcedure);

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("LibroId", id);

        var filasAfectadas = await connection.ExecuteAsync(
            "sp_EliminarLibro",
            parametros,
            commandType: CommandType.StoredProcedure);

        return filasAfectadas > 0;
    }

    public async Task<IEnumerable<Autor>> ObtenerAutoresAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Autor>(
            "sp_ListarAutores",
            commandType: CommandType.StoredProcedure);
    }
}
