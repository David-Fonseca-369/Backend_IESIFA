﻿namespace Backend_IESIFA.DTOs
{
    public class FiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        public PaginacionDTO PaginacionDTO {
            get { return new PaginacionDTO() { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina };  }         
        }

        public string Text { get; set; }

    }
}
