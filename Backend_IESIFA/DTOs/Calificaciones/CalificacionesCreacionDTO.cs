using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Calificaciones
{
    public class CalificacionesCreacionDTO
    {
        [Required]
        public int IdMateria { get; set; }
        [Required]
        public int IdPeriodo { get; set; }
        [Required]
        public int IdNivelEducativo { get; set; }
        [Required]
        public int IdDocente { get; set; }
        [Required]
        public int Evaluacion { get; set; }

        [Required]
        public List<CalificacionAlumnoCreacionDTO> Detalles { get; set; }
    }
}
