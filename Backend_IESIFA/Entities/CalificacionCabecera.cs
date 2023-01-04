namespace Backend_IESIFA.Entities
{
    public class CalificacionCabecera
    {
        public int Id { get; set; }
        public int IdMateria { get; set; }
        public int IdPeriodo { get; set; }
        public int IdNivelEducativo { get; set; }
        public int IdDocente { get; set; }
        public int Evaluacion { get; set; }
        public DateTime UltimaModificacion { get; set; }

        //References
        public Materia Materia { get; set; }
        public Periodo Periodo { get; set; }
        public NivelEducativo NivelEducativo { get; set; }
        public Usuario Docente { get; set; }
    }
}
