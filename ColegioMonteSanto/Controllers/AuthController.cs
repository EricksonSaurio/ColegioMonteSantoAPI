using ColegioMonteSanto.Data;
using ColegioMonteSanto.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ColegioMonteSantoContext _dbContext; //inyectar el contexto de base de datos

    public AuthController(IConfiguration configuration, ColegioMonteSantoContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        // Verifica las credenciales del usuario en la base de datos y obtiene el rol
        var user = _dbContext.Usuarios.Include(u => u.Rol)
                                       .FirstOrDefault(u => u.usuario == login.Username && u.clave == login.Password);

        if (user != null)
        {
            // Genera el token incluyendo el rol del usuario
            var token = GenerateToken(user.usuario, user.Rol.nombre_rol);
            return Ok(new { token });
        }

        return Unauthorized();
    }

    private string GenerateToken(string username, string role)
    {
        var keyString = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("La clave JWT no está configurada.");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),  // Se incluye el rol en el token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expirationMinutes = _configuration["Jwt:ExpirationInMinutes"] ?? throw new InvalidOperationException("La expiración JWT no está configurada.");
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(expirationMinutes)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
