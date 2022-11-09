
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Usuarios
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }      
        public bool Estado { get; set; }
    }
}
