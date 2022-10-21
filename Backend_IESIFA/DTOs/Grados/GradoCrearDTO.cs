using Backend_IESIFA.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Grados
{
    public class GradoCrearDTO
    {
        [Required]
        public int IdNivelEducativo { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }     
    }
}
