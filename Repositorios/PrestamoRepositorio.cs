using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio
{
    private readonly string _connectionString;

    public PrestamoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<(IEnumerable<PrestamoReporte> Prestamos, int Total)> ObtenerReportePaginadoAsync(DateTime desde, DateTime hasta, int pagina, int tamanoPagina)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("Desde", desde);
        parametros.Add("Hasta", hasta);
        parametros.Add("Pagina", pagina);
        parametros.Add("TamanoPagina", tamanoPagina);

        var prestamos = await connection.QueryAsync<PrestamoReporte>(
            "sp_ReportePrestamosPaginado",
            parametros,
            commandType: CommandType.StoredProcedure);

        var total = await connection.ExecuteScalarAsync<int>(
            "sp_ContarPrestamos",
            new { Desde = desde, Hasta = hasta },
            commandType: CommandType.StoredProcedure);

        return (prestamos, total);
    }
}
