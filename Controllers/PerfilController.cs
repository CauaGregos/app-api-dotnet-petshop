using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : Controller
    {
        private readonly EscolaContext _context;
        public PerfilController(EscolaContext context)
        {
            // construtor
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Perfil>> GetAll()
        {
            return _context.Perfil.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> post(Perfil model)
        {
            try
            {
                _context.Perfil.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/perfil/{model.email}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpGet("{PerfilId}")]
        public ActionResult<List<Perfil>> Get(int PerfilId)
        {
            try
            {
                var result = _context.Perfil.Find(PerfilId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpDelete("{PerfilId}")]
        public async Task<ActionResult> delete(int PerfilId)
        {
            try
            {
                //verifica se existe Perfil a ser excluído
                var Perfil = await _context.Perfil.FindAsync(PerfilId);
                if (Perfil == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Perfil);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{PerfilId}")]
        public async Task<IActionResult> put(int PerfilId, Perfil dadosPerfilAlt)
        {
            try
            {
                //verifica se existe Perfil a ser alterado
                var result = await _context.Perfil.FindAsync(PerfilId);
                if (PerfilId != result.id)
                {
                    return BadRequest();
                }
                result.nome = dadosPerfilAlt.nome;
                result.email = dadosPerfilAlt.email;
                result.senha = dadosPerfilAlt.senha;
                await _context.SaveChangesAsync();
                return Created($"/api/perfil/{dadosPerfilAlt.email}", dadosPerfilAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }


        }
    }
}