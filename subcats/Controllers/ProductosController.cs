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
        [Route("Update/{productoId}")]
        [EnableCors("AnotherPolicy")]
        public IActionResult ActualizarProducto(string productoId, [FromBody] Producto producto)
        {
            try
            {
                // Asegurarnos de que el ID se asigne correctamente
                int id = int.Parse(productoId);
                producto.Id_producto = id;
                
                // Intenta actualizar el producto y verifica si se realizó algún cambio
                bool actualizado = db.ActualizarProducto(producto);
                
                if (actualizado)
                {
                    return Ok($"Producto con ID {productoId} actualizado correctamente");
                }
                else
                {
                    return NotFound($"No se encontró el producto con ID {productoId} o no se realizaron cambios");
                }
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