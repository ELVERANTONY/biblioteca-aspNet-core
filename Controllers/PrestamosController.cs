using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly PrestamoRepositorio _prestamoRepositorio;
    private const int TamanoPagina = 15;

    public PrestamosController(PrestamoRepositorio prestamoRepositorio)
    {
        _prestamoRepositorio = prestamoRepositorio;
    }

    // GET: Prestamos/Reporte
    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta, int pagina = 1)
    {
        ViewData["Title"] = "Reporte de Préstamos";

        // Si no se proporcionan fechas, usar el mes actual
        var fechaDesde = desde ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var fechaHasta = hasta ?? DateTime.Now;

        // Validación: La fecha "Desde" no puede ser mayor que "Hasta"
        if (fechaDesde > fechaHasta)
        {
            ModelState.AddModelError("", "La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.");
            ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

            var viewModelError = new PrestamoPaginadoViewModel
            {
                Prestamos = new List<PrestamoReporte>(),
                Desde = fechaDesde,
                Hasta = fechaHasta,
                Paginacion = new PaginacionViewModel
                {
                    PaginaActual = 1,
                    TamanoPagina = TamanoPagina,
                    TotalRegistros = 0
                }
            };

            return View(viewModelError);
        }

        // Validación: Las fechas no pueden ser futuras
        if (fechaDesde > DateTime.Now || fechaHasta > DateTime.Now)
        {
            ModelState.AddModelError("", "Las fechas no pueden ser futuras.");
            ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

            var viewModelError = new PrestamoPaginadoViewModel
            {
                Prestamos = new List<PrestamoReporte>(),
                Desde = fechaDesde,
                Hasta = fechaHasta,
                Paginacion = new PaginacionViewModel
                {
                    PaginaActual = 1,
                    TamanoPagina = TamanoPagina,
                    TotalRegistros = 0
                }
            };

            return View(viewModelError);
        }

        ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

        var (prestamos, total) = await _prestamoRepositorio.ObtenerReportePaginadoAsync(fechaDesde, fechaHasta, pagina, TamanoPagina);

        var viewModel = new PrestamoPaginadoViewModel
        {
            Prestamos = prestamos,
            Desde = fechaDesde,
            Hasta = fechaHasta,
            Paginacion = new PaginacionViewModel
            {
                PaginaActual = pagina,
                TamanoPagina = TamanoPagina,
                TotalRegistros = total
            }
        };

        return View(viewModel);
    }
}
