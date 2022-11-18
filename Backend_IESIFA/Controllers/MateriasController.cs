
using AutoMapper;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Materias;
using Backend_IESIFA.DTOs.Usuarios;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Backend_IESIFA.Controllers
{
    [Route("api/materias")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MateriasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("todos")]
        public async Task<ActionResult<List<MateriaDTO>>> Todos()
        {
            var materias = await context.Materias
                .Include(x => x.Grupo)
                .ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);
        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<MateriaDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Materias
                .Include(x => x.Grupo)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var materias = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<MateriaDTO>>> Filtrar([FromQuery] FiltrarDTO filtrarDTO)
        {

            var queryable = context.Materias
                .Include(x => x.Grupo)
                .AsQueryable();


            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {

                queryable = queryable
                    .Where(x => x.Nombre.Contains(filtrarDTO.Text)
                    || x.Grupo.Nombre.Contains(filtrarDTO.Text));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var materias = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MateriaEditarDTO>> Get(int id)
        {
            var materia = await context.Materias
                .Include(x => x.Grupo)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (materia == null)
            {
                return NotFound($"La materia {id}, no existe.");
            }

            return mapper.Map<MateriaEditarDTO>(materia);

        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] MateriaCrearDTO materiaCrear)
        {
            var materia = mapper.Map<Materia>(materiaCrear);
            materia.Estado = true;

            await context.AddAsync(materia);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] MateriaCrearDTO materiaEditar)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.Id == id);

            if (materia == null)
            {
                return NotFound($"La materia {id}, no existe.");
            }

            materia = mapper.Map(materiaEditar, materia);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.Id == id);

            if (materia == null)
            {
                return NotFound($"La materia {id}, no existe.");
            }

            materia.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var materia = await context.Materias.FirstOrDefaultAsync(x => x.Id == id);

            if (materia == null)
            {
                return NotFound($"La materia {id}, no existe.");
            }

            materia.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();

        }







    }
}
