using AutoMapper;
using Backend_IESIFA.DTOs.Materias;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Usuarios;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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


        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Usuarios
                .Include(x => x.Rol)
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var usuarios = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<UsuarioDTO>>> Filtrar([FromQuery] FiltrarDTO filtrarDTO)
        {
            var queryable = context.Usuarios
             .Include(x => x.Rol)
             .AsQueryable();


            if (!string.IsNullOrEmpty(filtrarDTO.Text))
            {
                queryable = queryable
                    .Where(x => x.Nombre.Contains(filtrarDTO.Text)
                    || x.ApellidoPaterno.Contains(filtrarDTO.Text)
                    || x.ApellidoMaterno.Contains(filtrarDTO.Text)
                    || x.Correo.Contains(filtrarDTO.Text));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var usuarios = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<UsuarioDTO>>(usuarios);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuario == null)
            {
                return NotFound($"El usuario {id}, no existe.");
            }

            return mapper.Map<UsuarioDTO>(usuario);

        }


        private async Task<bool> ValidarCorreo(string correo)
        {
            //Validar el de alumno
            return await context.Usuarios.AnyAsync(x => x.Correo == correo.ToLower()); /*|| await context.Alumnos.AnyAsync(x => x.Correo == correo))*/
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            bool validarCorreo = await ValidarCorreo(usuarioCreacionDTO.Correo);
            if (validarCorreo)
            {
                return BadRequest("EL correo ya existe.");
            }

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


        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] UsuarioEditarDTO usuarioEditarDTO)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound($"El usuario {id}, no existe.");
            }

            string correoEntrante = usuarioEditarDTO.Correo.ToLower().Trim();

            //Validar si el correo es distinto al agregado
            if (usuario.Correo != correoEntrante)
            {
                bool validarCorreo = await ValidarCorreo(usuarioEditarDTO.Correo); 
                
                if (validarCorreo)
                {
                    return BadRequest("EL correo ya existe.");
                }
            }



            //Validar longitud de contraseña, si se cambia
            if (!string.IsNullOrEmpty(usuarioEditarDTO.Password))
            {
                if (usuarioEditarDTO.Password.Length < 8)
                {
                    return BadRequest("La longitud mínima del password debe ser de 8 caracteres.");
                }

                if (usuarioEditarDTO.Password.Length > 60)
                {
                    return BadRequest("La longitud máxima del password debe ser de 60 caracteres.");
                }
                
                //Cambiar contraseña
                CrearPasswordHash(usuarioEditarDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            
            }

            usuario.IdRol = usuarioEditarDTO.IdRol;
            usuario.Nombre = usuarioEditarDTO.Nombre.Trim();
            usuario.ApellidoPaterno = usuarioEditarDTO.ApellidoPaterno.Trim();
            usuario.ApellidoMaterno = usuarioEditarDTO.ApellidoMaterno.Trim();
            usuario.Correo = usuarioEditarDTO.Correo.Trim();

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);


            if (usuario == null)
            {
                return NotFound($"El usuario {id} no existe.");
            }

            usuario.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                return NotFound($"El usuario {id} no existe.");
            }

            usuario.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }



        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; //Aquí envía la llave
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //Envíar el password encriptado.
            }
        }
    }
}
