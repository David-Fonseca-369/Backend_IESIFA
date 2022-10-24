using AutoMapper;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Grados;
using Backend_IESIFA.DTOs.Grupos;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Backend_IESIFA.Controllers
{

    [Route("api/grupos")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GruposController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("todos")]
        public async Task<ActionResult<List<GrupoDTO>>> Todos()
        {
            var grupos = await context.Grupos
                .Include(x => x.Grado)
                .ToListAsync();

            return mapper.Map<List<GrupoDTO>>(grupos);            
        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<GrupoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Grupos
                .Include(x => x.Grado)
                .AsQueryable();

            //Cuenta los registros y los expone en cabecera         
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            //Paginacion
            var grupos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<GrupoDTO>>(grupos);
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<GrupoDTO>> Get(int id)
        {
            var grupo = await context.Grupos
                .Include(x => x.Grado)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (grupo == null)
            {
                return NotFound($"El grupo {id}, no existe");
            }

            return mapper.Map<GrupoDTO>(grupo);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] GrupoCrearDTO grupoCrear)
        {
            var grupo = mapper.Map<Grupo>(grupoCrear);
            grupo.Estado = true;

            await context.AddAsync(grupo);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] GrupoCrearDTO grupoEditar)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.Id == id);
            
            if (grupo == null)
            {
                return NotFound($"El grupo {id}, no existe.");
            }

            //Mapeo  de grupoEditaer a grupo y lo almacenamos en la misma variable
            grupo = mapper.Map(grupoEditar, grupo);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult>Activar(int id)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.Id == id);

            if (grupo == null)
            {
                return NotFound($"El grupo {id}, no existe.");
            }

            grupo.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }


         [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult>Desactivar(int id)
        {
            var grupo = await context.Grupos.FirstOrDefaultAsync(x => x.Id == id);

            if (grupo == null)
            {
                return NotFound($"El grupo {id}, no existe.");
            }

            grupo.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
