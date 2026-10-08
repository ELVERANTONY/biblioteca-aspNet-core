using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio
{
    private readonly string _connectionString;

    public SocioRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<(IEnumerable<Socio> Socios, int Total)> ObtenerTodosPaginadoAsync(int pagina, int tamanoPagina)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("Pagina", pagina);
        parametros.Add("TamanoPagina", tamanoPagina);

        var socios = await connection.QueryAsync<Socio>(
            "sp_ListarSociosPaginado",
            parametros,
            commandType: CommandType.StoredProcedure);

        var total = await connection.ExecuteScalarAsync<int>(
            "sp_ContarSocios",
            commandType: CommandType.StoredProcedure);

        return (socios, total);
    }

    public async Task<int> InsertarAsync(Socio socio)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("DNI", socio.DNI);
        parametros.Add("Nombre", socio.Nombre);
        parametros.Add("Email", socio.Email);

        return await connection.ExecuteScalarAsync<int>(
            "sp_InsertarSocio",
            parametros,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExisteDNIAsync(string dni)
    {
        using var connection = CreateConnection();
        var parametros = new DynamicParameters();
        parametros.Add("DNI", dni);

        var existe = await connection.ExecuteScalarAsync<int>(
            "sp_VerificarDNI",
            parametros,
            commandType: CommandType.StoredProcedure);

        return existe > 0;
    }
}
