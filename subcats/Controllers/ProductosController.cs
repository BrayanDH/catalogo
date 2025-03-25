using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace subcats.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Dao _db = new Dao();
        private readonly CategoriaService _categoriaService = new CategoriaService();

        // GET: Productos
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var productos = _db.GetAllProductos();
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los productos: " + ex.Message;
                return View(new List<Producto>());
            }
        }

        // GET: Productos/Details/5
        public IActionResult Details(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Si el producto tiene categoría, cargar su nombre
                if (producto.CategoriaId.HasValue)
                {
                    var categoria = _categoriaService.ObtenerCategoria(producto.CategoriaId.Value);
                    ViewBag.NombreCategoria = categoria?.Nombre ?? "Categoría no encontrada";
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Cargar las categorías para el select
            CargarCategorias();

            return View(new Producto());
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
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
                    _db.InsertarProducto(producto);
                    TempData["SuccessMessage"] = "Producto creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }

                // Recargar las categorías para el select en caso de error
                CargarCategorias();
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el producto: {ex.Message}";
                CargarCategorias();
                return View(producto);
            }
        }

        // GET: Productos/Edit/5
        public IActionResult Edit(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Cargar las categorías para el select
                CargarCategorias();

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producto producto)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                if (id != producto.Id_producto)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    bool actualizado = _db.ActualizarProducto(producto);
                    if (actualizado)
                    {
                        TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo actualizar el producto. Verifique los datos e intente nuevamente.";
                        CargarCategorias();
                        return View(producto);
                    }
                }
                CargarCategorias();
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el producto: {ex.Message}";
                CargarCategorias();
                return View(producto);
            }
        }

        // GET: Productos/Delete/5
        public IActionResult Delete(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Si el producto tiene categoría, cargar su nombre
                if (producto.CategoriaId.HasValue)
                {
                    var categoria = _categoriaService.ObtenerCategoria(producto.CategoriaId.Value);
                    ViewBag.NombreCategoria = categoria?.Nombre ?? "Categoría no encontrada";
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Delete/5
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
                _db.EliminarProducto(id.ToString());
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Método privado para cargar las categorías en ViewBag
        private void CargarCategorias()
        {
            try
            {
                var categorias = _categoriaService.ObtenerTodasCategorias();
                ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            }
            catch (Exception)
            {
                ViewBag.Categorias = new SelectList(new List<Categoria>(), "Id", "Nombre");
            }
        }
    }
}