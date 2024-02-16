using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authAPI
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : Controller
  {
    private AuthApiContext _context;
    private readonly TokenService tokenService;
    private readonly UserRepository repository;
    private IConfiguration _configuration;

    public UserController(IConfiguration configuration) {
      _context = new AuthApiContext();
      _configuration = configuration;
      tokenService = new TokenService(_configuration);
      repository = new UserRepository();
    }

    [HttpPost("Cadastrar")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
      string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
      
      Usuario u = userDto.ToEntity();
      u.PasswordHash = passwordHash;
      
      repository.Create(u);

      return Created();
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
      Usuario user = GetByUsername(loginDto.Username);
      
      if (user == null)
      {
        return BadRequest("Usuário incorreto");
      }

      if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
      {
        return BadRequest("Senha incorreta");
      }
      string token = tokenService.CreateToken(user);
      
      LoginResponseDto res = new LoginResponseDto();
      res.Username = user.Username;
      res.Password = user.PasswordHash;
      res.Token = token;
      res.Role = user.Role.Name;
      res.UserID = user.UserId;
      return Ok(res);
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public List<Usuario> Get()
    {
      return _context.Usuario.Include("Role").ToList();
    }

    [Authorize]
    [HttpGet("Username/{username}")]
    public Usuario GetByUsername(string username)
    {
      return repository.GetByUsername(username);
    }

    [Authorize]
    [HttpGet("{id}")]
    public Usuario FindById(int id)
    {
      return _context.Usuario.FirstOrDefault(u => u.UserId == id);
    }

    [Authorize]
    [HttpGet("GetUserWithToken/{token}")]
    public Usuario GetUserId(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

      if (jwtToken != null)
      {
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");

            if (userIdClaim != null)
            {
                int userId = Convert.ToInt32(userIdClaim.Value);
                return FindById(userId);
            }
      }
      return null;
    }
  }
}