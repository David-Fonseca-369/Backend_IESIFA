
using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.DTOs.Grupos
{
    public class GrupoDTO
    {
        public int Id { get; set; }
        public int IdGrado { get; set; }        
        public string  NombreGrado { get; set; }
        public string NombreNivelEducativo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}
