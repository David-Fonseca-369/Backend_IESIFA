using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{

    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("todos")]
        public async Task<ActionResult<List<Genero>>> Todos()
        {
            return await context.Generos.ToListAsync();
        }
    }
}
