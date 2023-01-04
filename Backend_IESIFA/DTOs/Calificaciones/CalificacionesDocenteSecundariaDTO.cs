namespace Backend_IESIFA.DTOs.Calificaciones
{
    public class CalificacionesDocenteSecundariaDTO
    {
        public int IdMateria { get; set; }
        public string NombreMateria { get; set; }
        public int IdGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public int IdGrado { get; set; }
        public string NombreGrado { get; set; }
        public int IdNivelEducativo { get; set; }
        public string NombreNivelEducativo { get; set; }
        public int IdPeriodo { get; set; }
        public string NombrePeriodo { get; set; }
        public int Evaluacion { get; set; }
        public List<CalificacionSecundariaDTO> Calificaciones { get; set; }



    }
}
