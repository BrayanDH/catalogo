using Microsoft.AspNetCore.Mvc;
using subcats.customClass;
using subcats.dto;
using System.Collections.Generic;

namespace subcats.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly Dao _dao;

        public ProveedoresController()
        {
            _dao = new Dao();
        }

        public IActionResult Index()
        {
            var proveedores = _dao.GetAllProveedores();
            return View(proveedores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _dao.InsertarProveedor(proveedor);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        public IActionResult Edit(int id)
        {
            var proveedor = _dao.GetProveedor(id.ToString());
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _dao.ActualizarProveedor(proveedor);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _dao.EliminarProveedor(id.ToString());
            return RedirectToAction(nameof(Index));
        }
    }
} 