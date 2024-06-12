using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GameOfThrones.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager; // Gerencia operações relacionadas ao usuário, como encontrar usuários e obter papéis
    private readonly SignInManager<IdentityUser> _signInManager; // Gerencia o processo de login do usuário

    public AuthController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login", Name = "login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false); // Verifica as credenciais do usuário

        if (result.Succeeded)
        {
            //========================= Obtém o usuário e seus papéis
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null)
            {
                return Unauthorized();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            //=========================

            //========================= Criação e retorno para o cliente do token JWT 
            var keyString = _configuration["JWT_KEY"];
            if (string.IsNullOrEmpty(keyString))
            {
                return BadRequest("JWT key is missing.");
            }
            var key = Encoding.ASCII.GetBytes(keyString); // Obtém a chave de assinatura
            var tokenHandler = new JwtSecurityTokenHandler(); // Instancia o manipulador de tokens JWT

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                   [
                       new Claim(ClaimTypes.Name, login.Username), // Adiciona a reivindicação (claim) do nome do usuário
                    new Claim(ClaimTypes.Role, role) // Adiciona a reivindicação do papel do usuário
                   ]),
                Expires = DateTime.UtcNow.AddHours(1), // Definie o tempo de expiração do token (uma hora, neste caso)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature) // Define as credenciais de assinatura
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); // Cria o token
            var tokenString = tokenHandler.WriteToken(token); // Converte o token em uma string compactada
            return Ok(new { Token = tokenString }); // Retorna o token ao cliente
            //=========================
        }

        return Unauthorized(); // Retorna não autorizado se as credenciais forem inválidas
    }

    [HttpPost("register", Name = "register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel register)
    {
        var user = new IdentityUser { UserName = register.UserName };
        var result = await _userManager.CreateAsync(user, register.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, register.Role);
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}

