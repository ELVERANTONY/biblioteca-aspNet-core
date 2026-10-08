namespace Biblioteca.Web.Models;

public class SocioPaginadoViewModel
{
    public IEnumerable<Socio> Socios { get; set; } = new List<Socio>();
    public PaginacionViewModel Paginacion { get; set; } = new PaginacionViewModel();
}
