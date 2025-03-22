using Microsoft.AspNetCore.Mvc;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;

namespace subcats.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Dao _db = new Dao();

        // GET: Productos
        public IActionResult Index()
        {
            try
            {
                var productos = _db.GetAllProductos();
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar productos: {ex.Message}";
                return View(new List<Producto>());
            }
        }

        // GET: Productos/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
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
            return View(new Producto());
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.InsertarProducto(producto);
                    TempData["SuccessMessage"] = "Producto creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el producto: {ex.Message}";
                return View(producto);
            }
        }

        // GET: Productos/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }
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
                        return View(producto);
                    }
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el producto: {ex.Message}";
                return View(producto);
            }
        }

        // GET: Productos/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
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
    }
}