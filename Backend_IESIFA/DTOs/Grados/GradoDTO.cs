using Backend_IESIFA.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Grados
{
    public class GradoDTO
    {
        public int Id { get; set; }
        public int IdNivelEducativo { get; set; }
        public string NombreNivelEducativo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
