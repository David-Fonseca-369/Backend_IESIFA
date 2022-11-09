using AutoMapper;
using Backend_IESIFA.DTOs.Usuarios;
using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_IESIFA.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UsuariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("Todos")]
        public async Task<ActionResult<List<UsuarioDTO>>> Todos()
        {
            var usuarios = await context.Usuarios.ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            //validar correo | tanto del usuario como del alumno 

            CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuario usuario = new()
            {
                IdRol = usuarioCreacionDTO.IdRol,
                Nombre = usuarioCreacionDTO.Nombre.Trim(),
                ApellidoMaterno = usuarioCreacionDTO.ApellidoPaterno.Trim(),
                ApellidoPaterno = usuarioCreacionDTO.ApellidoMaterno.Trim(),
                Correo = usuarioCreacionDTO.Correo.ToLower().Trim(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Estado = true
            };

            await context.AddAsync(usuario);

            await context.SaveChangesAsync();

            return NoContent();

        }

        private static void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; //Aquí envía la llave
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //Envíar el password encriptado.
            }

        }




    }
}
