namespace Backend_IESIFA.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 10;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina: value;
            }
        }
    }
}
