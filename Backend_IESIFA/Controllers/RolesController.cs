using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RolesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<List<Rol>> GetRoles()
        {
            var roles = await context.Roles
                .Where(x => x.Estado)
                .ToListAsync();

            return roles;
        }
    }
}
