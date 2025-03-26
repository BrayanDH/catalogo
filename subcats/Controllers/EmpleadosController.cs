using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;

namespace subcats.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly Dao _db;

        public EmpleadosController()
        {
            _db = new Dao();
        }

        // GET: Empleados
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var empleados = _db.GetAllEmpleados();
                return View(empleados);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los empleados: " + ex.Message;
                return View(new List<Empleado>());
            }
        }

        // GET: Empleados/Details/5
        public IActionResult Details(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var empleado = _db.GetEmpleado(id.ToString());
                if (empleado == null || empleado.Id_empleado == 0)
                {
                    return NotFound();
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el empleado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new Empleado());
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Empleado empleado)
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
                    int id = _db.InsertarEmpleado(empleado);
                    if (id > 0)
                    {
                        TempData["SuccessMessage"] = "Empleado creado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo crear el empleado.";
                    }
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el empleado: {ex.Message}";
                return View(empleado);
            }
        }

        // GET: Empleados/Edit/5
        public IActionResult Edit(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var empleado = _db.GetEmpleado(id.ToString());
                if (empleado == null || empleado.Id_empleado == 0)
                {
                    return NotFound();
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el empleado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Empleados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Empleado empleado)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                if (id != empleado.Id_empleado)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    bool actualizado = _db.ActualizarEmpleado(empleado);
                    if (actualizado)
                    {
                        TempData["SuccessMessage"] = "Empleado actualizado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo actualizar el empleado.";
                    }
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el empleado: {ex.Message}";
                return View(empleado);
            }
        }

        // GET: Empleados/Delete/5
        public IActionResult Delete(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var empleado = _db.GetEmpleado(id.ToString());
                if (empleado == null || empleado.Id_empleado == 0)
                {
                    return NotFound();
                }
                return View(empleado);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el empleado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
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
                _db.EliminarEmpleado(id.ToString());
                TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el empleado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 