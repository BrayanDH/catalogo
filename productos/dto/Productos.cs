namespace productos.dto
{
    public class Producto
    {
        public int Id_producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public decimal Impuesto { get; set; }
        public decimal? Descuento { get; set; }
        public int Stock { get; set; }
        public DateTime? Fecha_creacion { get; set; }
        public DateTime? Fecha_actualizacion { get; set; }
    }
}