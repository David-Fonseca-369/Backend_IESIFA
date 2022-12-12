using AutoMapper;
using Backend_IESIFA.DTOs;
using Backend_IESIFA.DTOs.Alumnos;
using Backend_IESIFA.Entities;
using Backend_IESIFA.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Backend_IESIFA.Controllers
{
    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AlumnosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("Todos")]
        public async Task<ActionResult<List<AlumnoDTO>>> Todos()
        {
            var alumnos = await context.Alumnos
                .Include(x => x.Grupo)
                .Include(x => x.Genero)
                .ToListAsync();
            return mapper.Map<List<AlumnoDTO>>(alumnos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AlumnoDTO>> Get(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);

            if (alumno == null)
            {
                return NotFound($"El alumno {id} no existe");
            }

            return mapper.Map<AlumnoDTO>(alumno);

        }

        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<AlumnoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Alumnos
                .Include(x => x.Grupo)
                .Include(x => x.Genero)
                .AsQueryable();


            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var alumnos = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<AlumnoDTO>>(alumnos);

        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<AlumnoDTO>>> Filtrar([FromQuery] FiltrarDTO filtrarDTO)
        {
            var queryable = context.Alumnos
             .Include(x => x.Grupo)
             .Include(x => x.Genero)
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

            var alumnos = await queryable.Paginar(filtrarDTO.PaginacionDTO).ToListAsync();

            return mapper.Map<List<AlumnoDTO>>(alumnos);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] AlumnoCrearDTO alumnoCrear)
        {
            bool validarCorreo = await ValidarCorreo(alumnoCrear.Correo);

            if (validarCorreo)
            {
                return BadRequest("El correo ya existe.");
            }

            CrearPasswordHash(alumnoCrear.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var alumno = mapper.Map<Alumno>(alumnoCrear);
            alumno.Curp.ToUpper();
            alumno.PasswordHash = passwordHash;
            alumno.PasswordSalt = passwordSalt;
            alumno.Estado = true;

            await context.AddAsync(alumno);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar(int id, [FromBody] AlumnoEditarDTO alumnoEditar)
        {

            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);

            if (alumno == null)
            {
                return NotFound($"El alumno {id} no existe.");
            }

            string correoEntrante = alumnoEditar.Correo.ToLower().Trim();

            //Validar si el correo es distinto al agregado
            if (alumno.Correo != correoEntrante)
            {
                bool validarCorreo = await ValidarCorreo(correoEntrante);

                if (validarCorreo)
                {
                    return BadRequest("El correo ya existe.");
                }

            }

            alumno = mapper.Map(alumnoEditar, alumno);
            alumno.Curp.ToUpper();
            alumno.Correo.ToLower();

            //Validar longitud de contraseña, si se cambia
            if (!string.IsNullOrEmpty(alumnoEditar.Password))
            {
                if (alumnoEditar.Password.Length < 8)
                {
                    return BadRequest("La longitud mínima del password debe ser de 8 caracteres.");
                }

                if (alumnoEditar.Password.Length > 60)
                {
                    return BadRequest("La longitud máxima del password debe ser de 60 caracteres.");
                }

                //Cambiar contraseña
                CrearPasswordHash(alumnoEditar.Password, out byte[] passwordHash, out byte[] passwordSalt);

                alumno.PasswordHash = passwordHash;
                alumno.PasswordSalt = passwordSalt;

            }

            await context.SaveChangesAsync();


            return NoContent();

        }


        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);


            if (alumno == null)
            {
                return NotFound($"El alumno {id} no existe.");
            }

            alumno.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var alumno = await context.Alumnos.FirstOrDefaultAsync(x => x.Id == id);

            if (alumno == null)
            {
                return NotFound($"El alumno {id} no existe.");
            }

            alumno.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }



        private async Task<bool> ValidarCorreo(string correo)
        {

            var correoAlumno = await context.Alumnos.AnyAsync(x => x.Correo == correo.ToLower());

            if (correoAlumno)//si existe ya retorno que hay uno existente.
            {
                return correoAlumno;
            }

            //Paso a verificar el de usuario en caso de que no exista en el de alumno
            return await context.Usuarios.AnyAsync(x => x.Correo == correo.ToLower());
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
