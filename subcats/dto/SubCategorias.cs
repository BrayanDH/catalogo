namespace subcats.dto
{
    public class Producto
    {
        public int Id_producto { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }
        
        [System.ComponentModel.DataAnnotations.StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }
        
        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }
        
        [System.ComponentModel.DataAnnotations.Range(0, 100, ErrorMessage = "El impuesto debe estar entre 0 y 100")]
        public decimal Impuesto { get; set; }
        
        [System.ComponentModel.DataAnnotations.Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal? Descuento { get; set; }
        
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
        
        public DateTime? Fecha_creacion { get; set; }
        public DateTime? Fecha_actualizacion { get; set; }
        public int? CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        
        // La imagen no debe ser requerida
        public byte[] Imagen { get; set; } = null;
        
        // Propiedad para el formulario de carga de archivos
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile ImagenFile { get; set; }
        
        public Producto()
        {
            // Valores por defecto para evitar errores de validación
            Precio = 0.01m;
            Impuesto = 0;
            Stock = 0;
        }
    }
    
    // Keeping the old class for backward compatibility if needed
    public class PanSubCategoria
    {
        public int ID { get; set; }
        public int? CategoriaId { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "La descripción es obligatoria")]
        public string Description { get; set; }
        
        public int? UserId { get; set; }
        public int? Estado { get; set; }
        public int? Position { get; set; }
    }

    public class CargoEmpleado
    {
        public int Id_cargo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? Fecha_creacion { get; set; }
        public DateTime? Fecha_actualizacion { get; set; }
    }
}