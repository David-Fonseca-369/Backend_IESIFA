using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Entities
{
    public class Alumno
    {
        public int Id { get; set; }
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
        [Required, MaxLength(60)]
        public string NoCuenta { get; set; }       
        [Required,MinLength(18), MaxLength(18)]
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
        [Required]
        public Byte[] PasswordHash { get; set; }
        [Required]
        public Byte[] PasswordSalt { get; set; }
        [Required]
        public bool Estado { get; set; }

        //References 
        public Grupo Grupo { get; set; }
        public Genero Genero { get; set; }

    }
}
