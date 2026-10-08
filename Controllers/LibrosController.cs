using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _libroRepositorio;
    private const int TamanoPagina = 15;

    public LibrosController(LibroRepositorio libroRepositorio)
    {
        _libroRepositorio = libroRepositorio;
    }

    // GET: Libros
    public async Task<IActionResult> Index(string? titulo, int pagina = 1)
    {
        ViewData["Title"] = "Listado de Libros";
        ViewData["TituloBusqueda"] = titulo;

        var (libros, total) = await _libroRepositorio.ObtenerTodosPaginadoAsync(titulo, pagina, TamanoPagina);

        var viewModel = new LibroPaginadoViewModel
        {
            Libros = libros,
            TituloBusqueda = titulo,
            Paginacion = new PaginacionViewModel
            {
                PaginaActual = pagina,
                TamanoPagina = TamanoPagina,
                TotalRegistros = total
            }
        };

        return View(viewModel);
    }

    // GET: Libros/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalles del Libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // GET: Libros/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Crear Libro";
        await CargarAutores();
        return View();
    }

    // POST: Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (ModelState.IsValid)
        {
            await _libroRepositorio.InsertarAsync(libro);
            TempData["Mensaje"] = "Libro creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Crear Libro";
        await CargarAutores();
        return View(libro);
    }

    // GET: Libros/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar Libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        await CargarAutores();
        return View(libro);
    }

    // POST: Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        if (id != libro.LibroId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _libroRepositorio.ActualizarAsync(libro);
            TempData["Mensaje"] = "Libro actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["Title"] = "Editar Libro";
        await CargarAutores();
        return View(libro);
    }

    // GET: Libros/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Eliminar Libro";

        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        return View(libro);
    }

    // POST: Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _libroRepositorio.EliminarAsync(id);
        TempData["Mensaje"] = "Libro eliminado exitosamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutores()
    {
        var autores = await _libroRepositorio.ObtenerAutoresAsync();
        ViewBag.Autores = new SelectList(autores, "AutorId", "Nombre");
    }
}
