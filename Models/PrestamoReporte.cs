using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class PrestamoReporte
{
    [Display(Name = "ID Préstamo")]
    public int PrestamoId { get; set; }

    [Display(Name = "Socio")]
    public string NombreSocio { get; set; } = string.Empty;

    [Display(Name = "DNI")]
    public string DNI { get; set; } = string.Empty;

    [Display(Name = "Libros")]
    public string Libros { get; set; } = string.Empty;

    [Display(Name = "Fecha Préstamo")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime FechaPrestamo { get; set; }

    [Display(Name = "Fecha Límite")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime FechaLimite { get; set; }

    [Display(Name = "Estado")]
    public string Estado { get; set; } = string.Empty;
}
