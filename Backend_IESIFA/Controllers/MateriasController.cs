
using AutoMapper;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Materias;
using Backend_IESIFA.DTOs.Usuarios;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
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
                .Include(x => x.Docente)
                .ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);
        }


        //GET: api/materias/asignadas/{idDocente}
        [HttpGet("asignadasPaginacion/{idDocente:int}")]
        public async Task<ActionResult<List<MateriaDTO>>> AsignadasPaginacion(int idDocente, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Materias
                .Include(x => x.Grupo)
                .Include(x => x.Docente)
                .Where(x => x.IdDocente == idDocente)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var materias = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);
        }


        //GET: api/materias/disponiblesPaginacion
        [HttpGet("disponiblesPaginacion")]
        public async Task<ActionResult<List<MateriaDTO>>> DisponiblesPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {

            var queryable = context.Materias
                .Include(x => x.Grupo)
                .Include(x => x.Docente)
                .Where(x => x.IdDocente == null && x.Estado)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var materias = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<MateriaDTO>>(materias);

        }

        //PUT: api/asignar/{idMateria}/{idDocente}
        [HttpPut("asignar/{idMateria:int}/{idDocente:int}")]
        public async Task<ActionResult> Asignar(int idMateria, int idDocente)
        {
            
            //Si existe materia
            var materia = await context.Materias
                .FirstOrDefaultAsync(x => x.Id == idMateria);
            
            if (materia == null)
            {
                return NotFound($"La materia {idMateria} no existe.");
            }

            //Si existe docente
            bool docenteExiste = await context.Usuarios.AnyAsync(x => x.Id == idDocente);

            if (!docenteExiste)
            {
                return NotFound($"El docente {idDocente} no existe.");
            }

            //asignamos

            materia.IdDocente = idDocente;

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT: api/quitar/{idMateria}
        [HttpPut("quitar/{idMateria:int}")]
        public async Task<ActionResult> Quitar(int idMateria)
        {

            //Si existe materia
            var materia = await context.Materias
                .FirstOrDefaultAsync(x => x.Id == idMateria);

            if (materia == null)
            {
                return NotFound($"La materia {idMateria} no existe.");
            }

            materia.IdDocente = null;

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<MateriaDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Materias
                .Include(x => x.Grupo)
                .Include(x => x.Docente)
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
                .Include(x => x.Docente)
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
                .Include(x => x.Docente)
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
