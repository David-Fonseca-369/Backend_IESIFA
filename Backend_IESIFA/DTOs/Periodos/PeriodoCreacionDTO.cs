using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Periodos
{
    public class PeriodoCreacionDTO
    {                
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
    }
}
