namespace Biblioteca.Web.Models;

public class LibroPaginadoViewModel
{
    public IEnumerable<Libro> Libros { get; set; } = new List<Libro>();
    public PaginacionViewModel Paginacion { get; set; } = new PaginacionViewModel();
    public string? TituloBusqueda { get; set; }
}
