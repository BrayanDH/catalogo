namespace subcats.dto
{
    public class Producto
    {
        public int Id_producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Impuesto { get; set; }
        public decimal? Descuento { get; set; }
        public int Stock { get; set; }
        public DateTime? Fecha_creacion { get; set; }
        public DateTime? Fecha_actualizacion { get; set; }
        public string ImagenUrl { get; set; }
        public int? CategoriaId { get; set; }
    }
    
    // Keeping the old class for backward compatibility if needed
    public class PanSubCategoria
    {
        public int ID { get; set; }
        public int? CategoriaId { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public int? Estado { get; set; }
        public int? Position { get; set; }
    }
}