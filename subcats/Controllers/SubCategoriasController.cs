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
    public class SubCategoriasController : ControllerBase
    {
        Dao db = new Dao();

        [HttpPost]
        [Route("Add")]
        [EnableCors("AnotherPolicy")]
        public IActionResult InsertarPanSubCategoria([FromBody] PanSubCategoria subCategoria)
        {
            try
            {
                db.InsertarPanSubCategoria(subCategoria);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("GetBySubCategoria/{subCatId}")]
        [EnableCors("AnotherPolicy")]
        public PanSubCategoria GetSubCategoria(string subCatId)
        {
            return db.PanSubCategoria(subCatId);
        }

        [HttpGet]
        [Route("GetByCategoria/{CatId}")]
        [EnableCors("AnotherPolicy")]
        public List<PanSubCategoria> GetSubCategoria2(string CatId)
        {
            return db.PanSubCategoria2(CatId);
        }


        [HttpGet]
        [Route("GetAll")]
        [EnableCors("AnotherPolicy")]
        public List<PanSubCategoria> GetAllSubCategorias()
        {
            return db.GetAllPanSubCategorias();
        }



        [HttpPut]
        [Route("Update")]
        [EnableCors("AnotherPolicy")]
        public IActionResult ActualizarPanSubCategoria([FromBody] PanSubCategoria subCategoria)
        {
            try
            {
                db.ActualizarPanSubCategoria(subCategoria);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut]
        [Route("Delete/{subCatId}")]
        [EnableCors("AnotherPolicy")]
        public IActionResult BorrarPanSubCategoria(string subCatId)
        {
            try
            {
                db.BorrarPanSubCategoria(subCatId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
