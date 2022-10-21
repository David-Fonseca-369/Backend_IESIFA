using Backend_IESIFA.DTOs.Grados;
using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [Route("api/grados")]
    [ApiController]
    public class GradosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GradosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("todos")]
        public async Task<ActionResult<List<GradoDTO>>> Todos()
        {
            var grados = await context.Grados
                .Include(x => x.NivelEducativo)
                .ToListAsync();

            return grados.Select(x => new GradoDTO
            {
                Id = x.Id,
                IdNivelEducativo = x.IdNivelEducativo,
                NombreNivelEducativo = x.NivelEducativo.Nombre,
                Nombre = x.Nombre,
                Estado = x.Estado
            }).ToList();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<GradoDTO>> Get(int id)
        {
            var grado = await context.Grados
                .Include(x => x.NivelEducativo)
                .FirstOrDefaultAsync(x => x.Id == id);

            return new GradoDTO
            {
                Id = grado.Id,
                IdNivelEducativo = grado.IdNivelEducativo,
                NombreNivelEducativo = grado.NivelEducativo.Nombre,
                Nombre = grado.Nombre,
                Estado = grado.Estado
            };
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] GradoCrearDTO gradoCrearDTO)
        {
            var nuevoGrado = new Grado
            {

                IdNivelEducativo = gradoCrearDTO.IdNivelEducativo,
                Nombre = gradoCrearDTO.Nombre,
                Estado = true //true por defecto
            };

            await context.AddAsync(nuevoGrado);

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult>Editar( int id, [FromBody] GradoCrearDTO gradoEditarDTO)
        {

            var grado = await context.Grados.FirstOrDefaultAsync(x => x.Id == id);

            if (grado == null)
            {
                return NotFound($"El grado {id}, no existe.");
            }

            grado.IdNivelEducativo = gradoEditarDTO.IdNivelEducativo;
            grado.Nombre = gradoEditarDTO.Nombre;

            await context.SaveChangesAsync();


            return NoContent();
        }


        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult>Activar(int id)
        {
            var grado = await context.Grados.FirstOrDefaultAsync(x => x.Id == id);

            if (grado == null)
            {
                return NotFound($"El grado {id}, no existe.");
            }

            grado.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();

        }
        
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult >Desactivar(int id)
        {
            var grado = await context.Grados.FirstOrDefaultAsync(x => x.Id == id);

            if (grado == null)
            {
                return NotFound($"El grado {id}, no existe.");
            }

            grado.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();

        }




    }
}
