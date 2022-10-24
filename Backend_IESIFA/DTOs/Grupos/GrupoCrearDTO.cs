using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Grupos
{
    public class GrupoCrearDTO
    {
        [Required]
        public int IdGrado { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }      
    }
}
