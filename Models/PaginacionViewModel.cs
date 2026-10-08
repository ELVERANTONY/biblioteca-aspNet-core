namespace Biblioteca.Web.Models;

public class PaginacionViewModel
{
    public int PaginaActual { get; set; } = 1;
    public int TamanoPagina { get; set; } = 15;
    public int TotalRegistros { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
    public bool TienePaginaAnterior => PaginaActual > 1;
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
}
