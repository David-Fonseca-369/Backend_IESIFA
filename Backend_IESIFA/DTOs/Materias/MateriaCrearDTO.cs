using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Materias
{
    public class MateriaCrearDTO
    {
        [Required]
        public int IdGrupo { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }  
    }
}
