using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiBiblioteca.Models;
using DataBaseFirst.Contexts;
using DataBaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly bibliotecaContext _context;

        public AuthController(IConfiguration configuration, bibliotecaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {

            var usuario = _context.t_personals.FirstOrDefault(u => u.correo == login.correo);
            if (usuario == null)
            {
                return NotFound("Usuario o contraseña incorrectos");
            }

            bool passwordValida = BCrypt.Net.BCrypt.Verify(login.password,usuario.password_hash);

            if (!passwordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var token = GenerateToken(usuario);
            return Ok(new { token });//si la autenticaciòn es correcta, retorna el token para que se almacene
            //en el localStorage del navegador

        }

        private string GenerateToken(t_personal personal)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").
                Get<JwtSettings>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("correo", personal.correo),
                new Claim("id_user", (personal.id_personal).ToString())

            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    //LoginModel tiene propósito específico: 	No mezcla lógica de autenticación con lógica de negocio del usuario.
    //Esta clase solo se se usa para recibir las credenciales desde el frontend
    public class LoginModel
    {
        public string correo { get; set; }
        public string password { get; set; }
    }

}
