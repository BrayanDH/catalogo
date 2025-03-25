using System;
using System.ComponentModel.DataAnnotations;

namespace subcats.dto
{
    public class Categoria
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; }
        
        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
        public string Descripcion { get; set; }
        
        public DateTime? FechaCreacion { get; set; }
        
        public DateTime? FechaActualizacion { get; set; }
    }
} 