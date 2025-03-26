using System;

namespace subcats.dto
{
    public class Proveedor
    {
        public int Id_proveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public DateTime? Fecha_creacion { get; set; }
        public DateTime? Fecha_actualizacion { get; set; }
    }
} 