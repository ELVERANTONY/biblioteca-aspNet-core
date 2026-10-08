using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(150, ErrorMessage = "El título no puede exceder 150 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio")]
    [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un autor")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }

    [Required(ErrorMessage = "El número de ejemplares es obligatorio")]
    [Range(1, 1000, ErrorMessage = "El número de ejemplares debe estar entre 1 y 1000")]
    [Display(Name = "Ejemplares")]
    public int Ejemplares { get; set; }

    public bool Activo { get; set; } = true;

    // Propiedad de navegación para mostrar el nombre del autor
    [Display(Name = "Nombre del Autor")]
    public string? NombreAutor { get; set; }
}
