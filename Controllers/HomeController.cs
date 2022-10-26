using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;
namespace ProjetoEscola_API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EscolaContext? _context;
        public HomeController(

        IConfiguration configuration,
        EscolaContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Login([FromBody] Perfil usuario)
        {
            //verifica se existe aluno a ser excluído
            var perfil = _context.Perfil.Where(u => u.email == usuario.email &&

            u.senha == usuario.senha)

            .FirstOrDefault();

           
            if (perfil == null)
                return Unauthorized("Usuário ou senha inválidos");
            var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, perfil.nome),
            new Claim(ClaimTypes.Role, perfil.role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
};
            var token = GetToken(authClaims);
            perfil.senha = "";
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = perfil
            });
        }
        // preciso enviar meu token para saber qual perm eu tenho
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}",
        User.Identity.Name);

        [HttpGet]
        [Route("Admin")]
        [Authorize(Roles = "Admin")]
        public string admin() => "Admin";

        [HttpGet]
        [Route("Cliente")]
        [Authorize(Roles = "Cliente")]
        public string cliente() => "Cliente";

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new

            SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(3),
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey,

            SecurityAlgorithms.HmacSha256)

            );
            return token;
        }
    }
}