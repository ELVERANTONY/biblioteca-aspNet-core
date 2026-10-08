namespace Biblioteca.Web.Models;

public class PrestamoPaginadoViewModel
{
    public IEnumerable<PrestamoReporte> Prestamos { get; set; } = new List<PrestamoReporte>();
    public PaginacionViewModel Paginacion { get; set; } = new PaginacionViewModel();
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}
