using AutoMapper;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Periodos;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [ApiController]
    [Route("api/periodos")]
    public class PeriodosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PeriodosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //GET: api/periodos/secundariaPaginacion 
        [HttpGet("secundariaPaginacion")]
        public async Task<ActionResult<List<PeriodoDTO>>> SecundariaPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Periodos
                .Where(x => x.IdNivelEducativo == 1)
                .OrderByDescending(x => x.FechaFin)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var periodos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<PeriodoDTO>>(periodos);
        }


        //GET : api/periodos/filtrarSecundaria
        [HttpGet("filtrarSecundaria")]
        public async Task<ActionResult<List<PeriodoDTO>>> FiltrarSecundaria([FromQuery] FiltrarDTO filtrarDTO)
        {

            var queryable = context.Periodos
           .Where(x => x.IdNivelEducativo == 1)
           .OrderByDescending(x => x.FechaFin)
           .AsQueryable();

            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable = queryable.Where(x => x.Nombre.Contains(filtrarDTO.Text));                  
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            
            var periodos = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<PeriodoDTO>>(periodos);

        }

        //GET: api/periodos/preparatoriaPaginacion
        [HttpGet("preparatoriaPaginacion")]
        public async Task<ActionResult<List<PeriodoDTO>>> PreparatoriaPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Periodos
                .Where(x => x.IdNivelEducativo == 2)
                .OrderByDescending(x => x.FechaFin)
                .AsQueryable(); ;

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var periodos = await queryable.Paginar(paginacionDTO).ToListAsync();


            return mapper.Map<List<PeriodoDTO>>(periodos);
        }

        //GET : api/periodos/filtrarPreparatoria
        [HttpGet("filtrarPreparatoria")]
        public async Task<ActionResult<List<PeriodoDTO>>> FiltrarPreparatoria([FromQuery] FiltrarDTO filtrarDTO)
        {

            var queryable = context.Periodos
           .Where(x => x.IdNivelEducativo == 2)
           .OrderByDescending(x => x.FechaFin)
           .AsQueryable();

            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable = queryable.Where(x => x.Nombre.Contains(filtrarDTO.Text));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var periodos = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<PeriodoDTO>>(periodos);

        }

        //POST: api/periodos/crearPeriodoSecundaria
        [HttpPost("crearPeriodoSecundaria")]
        public async Task<ActionResult> CrearPeriodoSecundaria([FromBody] PeriodoCreacionDTO periodoCreacionDTO)
        {

            //Validar que la fecha del periodo no este dentro de otro periodo.
            var resultTuple = await ValidarFechasSecundaria(periodoCreacionDTO.FechaInicio, periodoCreacionDTO.FechaFin);

            bool fechaEcontrada = resultTuple.Item1;
            string mensaje = resultTuple.Item2;

            if (fechaEcontrada)
            {
                return BadRequest(mensaje);
            }


            var periodo = mapper.Map<Periodo>(periodoCreacionDTO);

            periodo.IdNivelEducativo = 1;

            await context.AddAsync(periodo);
            await context.SaveChangesAsync();

            return NoContent();
        }


        //POST: api/periodos/crearPeriodoPreparatoria
        [HttpPost("crearPeriodoPreparatoria")]
        public async Task<ActionResult> CrearPeriodoPreparatoria([FromBody] PeriodoCreacionDTO periodoCreacionDTO)
        {

            //Validar que la fecha del periodo no este dentro de otro periodo.
            var resultTuple = await ValidarFechasPreparatoria(periodoCreacionDTO.FechaInicio, periodoCreacionDTO.FechaFin);

            bool fechaEcontrada = resultTuple.Item1;
            string mensaje = resultTuple.Item2;

            if (fechaEcontrada)
            {
                return BadRequest(mensaje);
            }


            var periodo = mapper.Map<Periodo>(periodoCreacionDTO);

            periodo.IdNivelEducativo = 2;

            await context.AddAsync(periodo);
            await context.SaveChangesAsync();

            return NoContent();
        }

        //GET : api/periodos/periodoActualSecundaria
        [HttpGet("periodoActualSecundaria")]
        public async Task<ActionResult<PeriodoDTO>> PeriodoActualSecundaria()
        {

            //DateTime hoy = new DateTime(2021, 09, 09); //Ejemplo
            DateTime hoy = DateTime.Now;

            var periodo = await context.Periodos
                .Where(x => x.IdNivelEducativo == 1 &&
                 hoy >= x.FechaInicio && x.FechaFin <= x.FechaFin)
                .FirstOrDefaultAsync();

            if (periodo is null)
            {
                return NotFound("No se encontró ningún periodo actual.");
            }


            return mapper.Map<PeriodoDTO>(periodo);
        }

        //GET : api/periodos/periodoActualPreparatoria
        [HttpGet("periodoActualPreparatoria")]
        public async Task<ActionResult<PeriodoDTO>> PeriodoActualPreparatoria()
        {

            //DateTime hoy = new DateTime(2021, 09, 09); //Ejemplo
            DateTime hoy = DateTime.Now;

            var periodo = await context.Periodos
                .Where(x => x.IdNivelEducativo == 2 &&
                 hoy >= x.FechaInicio && x.FechaFin <= x.FechaFin)
                .FirstOrDefaultAsync();

            if (periodo is null)
            {
                return NotFound("No se encontró ningún periodo actual.");
            }


            return mapper.Map<PeriodoDTO>(periodo);
        }



        private async Task<Tuple<bool, string>> ValidarFechasSecundaria(DateTime fechaInicioTemp, DateTime fechaFinTemp)
        {
            var periodos = await context.Periodos
                .Where(x => x.IdNivelEducativo == 1)
                .ToListAsync();

            if (periodos.Count > 0)
            {
                foreach (var item in periodos)
                {
                    bool periodoUsado = VerificarFechaItem(fechaInicioTemp, fechaFinTemp, item.FechaInicio, item.FechaFin);

                    if (periodoUsado)
                    {
                        return Tuple.Create<bool, string>(true, $"Las fechas indicadas ya se encuentran ocupadas en el periodo {item.Nombre} del {item.FechaInicio:dd/MM/yyyy} - {item.FechaFin:dd/MM/yyyy}.");
                    }
                }

                return Tuple.Create<bool, string>(false, null);

            }

            return Tuple.Create<bool, string>(false, null);
        }


        private async Task<Tuple<bool, string>> ValidarFechasPreparatoria(DateTime fechaInicioTemp, DateTime fechaFinTemp)
        {
            var periodos = await context.Periodos
                .Where(x => x.IdNivelEducativo == 2)
                .ToListAsync();

            if (periodos.Count > 0)
            {
                foreach (var item in periodos)
                {
                    bool periodoUsado = VerificarFechaItem(fechaInicioTemp, fechaFinTemp, item.FechaInicio, item.FechaFin);

                    if (periodoUsado)
                    {
                        return Tuple.Create<bool, string>(true, $"Las fechas indicadas ya se encuentran ocupadas en el periodo {item.Nombre} del {item.FechaInicio:dd/MM/yyyy} - {item.FechaFin:dd/MM/yyyy}.");
                    }
                }

                return Tuple.Create<bool, string>(false, null);

            }

            return Tuple.Create<bool, string>(false, null);
        }

        private bool VerificarFechaItem(DateTime fechaInicioTemp, DateTime fechaFinTemp, DateTime fechaInicio, DateTime fechaFin)
        {

            if (fechaInicioTemp >= fechaInicio && fechaInicioTemp <= fechaFin)
            {
                return true;
            }
            else if (fechaFinTemp >= fechaInicio && fechaFinTemp <= fechaFin)
            {
                return true;
            }

            return false;
        }


    }
}
