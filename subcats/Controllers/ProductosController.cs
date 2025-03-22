using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;

namespace subcats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        Dao db = new Dao();

        [HttpPost]
        [Route("Add")]
        [EnableCors("AnotherPolicy")]
        public IActionResult InsertarProducto([FromBody] Producto producto)
        {
            try
            {
                db.InsertarProducto(producto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{productoId}")]
        [EnableCors("AnotherPolicy")]
        public Producto GetProducto(string productoId)
        {
            return db.GetProducto(productoId);
        }

        [HttpGet]
        [Route("GetAll")]
        [EnableCors("AnotherPolicy")]
        public List<Producto> GetAllProductos()
        {
            return db.GetAllProductos();
        }

        [HttpPut]
        [Route("Update")]
        [EnableCors("AnotherPolicy")]
        public IActionResult ActualizarProducto([FromBody] Producto producto)
        {
            try
            {
                db.ActualizarProducto(producto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{productoId}")]
        [EnableCors("AnotherPolicy")]
        public IActionResult EliminarProducto(string productoId)
        {
            try
            {
                db.EliminarProducto(productoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}