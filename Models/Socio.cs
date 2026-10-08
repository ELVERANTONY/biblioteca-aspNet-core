using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Socio
{
    public int SocioId { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 caracteres")]
    [Display(Name = "DNI")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un email válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
