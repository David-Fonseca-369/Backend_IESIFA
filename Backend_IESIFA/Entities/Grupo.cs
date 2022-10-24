using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Grupo
    {
        public int Id { get; set; }
        [Required]
        public int IdGrado { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required]
        public bool Estado { get; set; }

        //Reference
        public Grado Grado { get; set; }
    }
}
