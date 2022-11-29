using Backend_IESIFA.DTOs.Login;
using Backend_IESIFA.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend_IESIFA.Controllers
{

    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public LoginController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpPost("general")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> General([FromBody] LoginUsuarioDTO login)
        {
            var correo = login.Correo.ToLower();

            //Buscar en usuario
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo && x.Estado);

            if (usuario == null)
            {
                //ALUMNOS
                //Si no existe, buscar en alumnos
                //pendiente
                return BadRequest("El usuario no existe.");
            }


            if (!VerificarPasswordHash(login.Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return BadRequest("Contraseña incorrecta.");
            }

            return ConstruirToken(usuario);

        }

        private RespuestaAutenticacionDTO ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("idUsuario",usuario.Id.ToString()),
                new Claim("rol",usuario.IdRol == 1 ? "Administrador" : usuario.IdRol == 2 ? "Docente" :  usuario.IdRol == 3 ? "Cajero" :"Indefinido"),
                new Claim("correo", usuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
                Nombre = usuario.Nombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expires
            };
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            //Recibe el password, lo encripta y lo compara con el password almacenado.

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo)); //Compara
            }
        }



    }
}
