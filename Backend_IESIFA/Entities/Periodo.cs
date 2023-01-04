using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Periodo
    {
        public int Id { get; set; }
        [Required]
        public int IdNivelEducativo { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }

        //References
        public NivelEducativo NivelEducativo { get; set; }
    }
}
