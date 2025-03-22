namespace subcats.dto
{
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
