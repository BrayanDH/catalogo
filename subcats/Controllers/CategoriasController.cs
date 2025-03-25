using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;

namespace subcats.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController()
        {
            _categoriaService = new CategoriaService();
        }

        // GET: Categorias
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var categorias = _categoriaService.ObtenerTodasCategorias();
                return View(categorias);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener categorías: {ex.Message}";
                return View(new List<Categoria>());
            }
        }

        // GET: Categorias/Details/5
        public IActionResult Details(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var categoria = _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                {
                    return NotFound();
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener la categoría: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (_categoriaService.CrearCategoria(categoria))
                    {
                        TempData["SuccessMessage"] = "Categoría creada exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo crear la categoría.");
                    }
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear la categoría: {ex.Message}";
                return View(categoria);
            }
        }

        // GET: Categorias/Edit/5
        public IActionResult Edit(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var categoria = _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                {
                    return NotFound();
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener la categoría: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria categoria)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id != categoria.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (_categoriaService.ActualizarCategoria(categoria))
                    {
                        TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar la categoría.");
                    }
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar la categoría: {ex.Message}";
                return View(categoria);
            }
        }

        // GET: Categorias/Delete/5
        public IActionResult Delete(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var categoria = _categoriaService.ObtenerCategoria(id);
                if (categoria == null)
                {
                    return NotFound();
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener la categoría: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Categorias/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                if (_categoriaService.EliminarCategoria(id))
                {
                    TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la categoría.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar la categoría: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 