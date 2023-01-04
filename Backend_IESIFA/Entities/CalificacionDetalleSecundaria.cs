using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class CalificacionDetalleSecundaria
    {
        public int Id { get; set; }
        [Required]
        public int IdCabecera { get; set; }
        [Required]
        public int IdAlumno { get; set; }
        public decimal PrimeraEvaluacion { get; set; }
        public decimal SegundaEvaluacion { get; set; }
        public decimal TerceraEvaluacion { get; set; }
        public decimal CuartaEvaluacion { get; set; }
        public decimal QuintaEvaluacion { get; set; }

        //References
        public CalificacionCabecera CalificacionCabecera { get; set; }
        public Alumno Alumno { get; set; }
    }
}
