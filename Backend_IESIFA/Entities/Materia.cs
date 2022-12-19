using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Materia
    {
        public int Id { get; set; }
        [Required]
        public int IdGrupo { get; set; }
        public int? IdDocente { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [MaxLength(255)]
        public string Descripcion { get; set; }
        [Required]
        public bool Estado { get; set; }

        //referencia
        public Grupo Grupo { get; set; }
        public Usuario Docente { get; set; }

    }
}
