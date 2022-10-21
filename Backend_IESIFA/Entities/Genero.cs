using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Genero
    {
        public int Id { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }
    }
}
