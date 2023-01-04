using Backend_IESIFA.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Alumnos
{
    public class AlumnoDTO
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public int IdGenero { get; set; }
        public string NombreGenero { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NoCuenta { get; set; }
        public string Curp { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string NombreTutor { get; set; }
        public string TelefonoTutor { get; set; }
        public string DireccionTutor { get; set; }
        public string Correo { get; set; }       
        public bool Estado { get; set; }

    }
}
