using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace authAPI
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private AuthApiContext _context;
    private IConfiguration _configuration;
    public UserController(IConfiguration configuration) {
      _context = new AuthApiContext();
      _configuration = configuration;
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
      string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
      
      Usuario u = new Usuario();
      u.Username = userDto.Username;
      u.PasswordHash = passwordHash;
      u.RoleId = userDto.RoleId;
      await _context.Usuario.AddAsync(u);
      await _context.SaveChangesAsync();
      return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
      Usuario user = await this.GetByUsername(loginDto.Username);
      
      if (user == null)
      {
        return BadRequest("Usuário incorreto");
      }

      if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
      {
        return BadRequest("Senha incorreta");
      }
      string token = CreateToken(user);
      LoginResponseDto res = new LoginResponseDto();
      res.Username = user.Username;
      res.Password = user.PasswordHash;
      res.Token = token;
      res.Role = user.Role.Name;
      return Ok(res);
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public List<Usuario> Get()
    {
      return _context.Usuario.Include("Role").ToList();
    }

    [Authorize]
    [HttpGet("{username}")]
    public async Task<Usuario> GetByUsername(string username)
    {
      var result = await _context.Usuario.Include("Role").FirstOrDefaultAsync(u => u.Username == username);
      if (result == null)
      {
        return null;
      }
      return result;
    }

    private string CreateToken(Usuario user)
    {
      List<Claim> claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.Name)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
      );

      var jwt = new JwtSecurityTokenHandler().WriteToken(token);

      return jwt;
    }

    [Authorize]
    [HttpPost("verificarToken")]
    public IActionResult VerificarToken()
    {
        // Se a execução chegar aqui, o token é válido
        return Ok("Token válido");
    }

    public static IEnumerable<Claim> GetClaims(Usuario user)
    {
        var result = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.Name)
        };
        return result;
    }
  }
}