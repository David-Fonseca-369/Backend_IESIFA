using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [Route("api/nivelesEducativos")]
    [ApiController]
    public class NivelesEducativosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public NivelesEducativosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet("todos")]
        public async Task<List<NivelEducativo>> Todos()
        {
            return await context.NivelesEducativos.ToListAsync();
        }
    }
}
