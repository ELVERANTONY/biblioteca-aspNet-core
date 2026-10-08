using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly SocioRepositorio _socioRepositorio;
    private const int TamanoPagina = 15;

    public SociosController(SocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    // GET: Socios
    public async Task<IActionResult> Index(int pagina = 1)
    {
        ViewData["Title"] = "Listado de Socios";

        var (socios, total) = await _socioRepositorio.ObtenerTodosPaginadoAsync(pagina, TamanoPagina);

        var viewModel = new SocioPaginadoViewModel
        {
            Socios = socios,
            Paginacion = new PaginacionViewModel
            {
                PaginaActual = pagina,
                TamanoPagina = TamanoPagina,
                TotalRegistros = total
            }
        };

        return View(viewModel);
    }

    // GET: Socios/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Crear Socio";
        return View();
    }

    // POST: Socios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (ModelState.IsValid)
        {
            // Verificar si el DNI ya existe
            if (await _socioRepositorio.ExisteDNIAsync(socio.DNI))
            {
                ModelState.AddModelError("DNI", "El DNI ya existe en el sistema.");
                ViewData["Title"] = "Crear Socio";
                return View(socio);
            }

            await _socioRepositorio.InsertarAsync(socio);
            TempData["Mensaje"] = "Socio creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Crear Socio";
        return View(socio);
    }
}
