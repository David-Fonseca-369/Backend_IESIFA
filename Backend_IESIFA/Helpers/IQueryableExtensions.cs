using Backend_IESIFA.DTOs;
using System.Linq;

namespace Backend_IESIFA.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)//omite un número específico de elementos en una secuencia y luego devuelve los elementos restantes
                .Take(paginacionDTO.RecordsPorPagina);//devuelve un número específico de elementos continuos desde el inicio de una secuencia
        }
    }
}
