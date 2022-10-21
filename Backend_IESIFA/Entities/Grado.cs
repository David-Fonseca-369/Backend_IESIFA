using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Grado
    {
        public int Id { get; set; }
        [Required]
        public int IdNivelEducativo { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }

        //Referencias
        public NivelEducativo NivelEducativo { get; set; }
    }
}
