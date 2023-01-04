using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Periodos
{
    public class PeriodoDTO
    {
        public int Id { get; set; }
        public int IdNivelEducativo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
