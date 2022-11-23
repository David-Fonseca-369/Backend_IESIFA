using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Alumnos
{
    public class AlumnoEditarDTO
    {
        [Required]
        public int IdGrupo { get; set; }
        [Required]
        public int IdGenero { get; set; }
        [Required, MaxLength(60)]
        public string Nombre { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(60)]
        public string ApellidoMaterno { get; set; }
        [Required, MinLength(18), MaxLength(18)]
        public string Curp { get; set; }
        [MinLength(10), MaxLength(10)]
        public string Telefono { get; set; }
        [Required, MaxLength(255)]
        public string Direccion { get; set; }
        [MaxLength(60)]
        public string NombreTutor { get; set; }
        [MinLength(10), MaxLength(10)]
        public string TelefonoTutor { get; set; }
        [MaxLength(255)]
        public string DireccionTutor { get; set; }
        [Required, MaxLength(60), EmailAddress]
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}
