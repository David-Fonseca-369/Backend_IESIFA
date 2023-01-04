using Backend_IESIFA.Validations;
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Calificaciones
{
    public class CalificacionAlumnoCreacionDTO
    {
        [Required]
        public int IdAlumno { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string NoCuenta { get; set; }
        [Required]
        [RangoCalificacion]
        public decimal Calificacion { get; set; }
    }
}
