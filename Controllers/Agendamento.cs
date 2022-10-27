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
    public class AgendamentoController : Controller
    {
        private readonly PetShopContext _context;
        public AgendamentoController(PetShopContext context)
        {
            // construtor
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Agendamento>> GetAll()
        {
            return _context.Agendamento.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> post(Agendamento model)
        {
            try
            {
                _context.Agendamento.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/agendamento", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpGet("{AgendamentoEmail}")]
        public ActionResult<List<Agendamento>> Get(String AgendamentoEmail)
        {
            try
            {
                var result = _context.Agendamento.Where(x => x.email == AgendamentoEmail);
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
        [HttpDelete("{AgendamentoId}")]
        public async Task<ActionResult> delete(int AgendamentoId)
        {
            try
            {
                //verifica se existe aluno a ser excluído
                var result = await _context.Agendamento.FindAsync(AgendamentoId);
                if (result == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(result);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{AgendamentoId}")]
        public async Task<IActionResult> put(int AgendamentoId, Agendamento dados)
        {
            try
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.Agendamento.FindAsync(AgendamentoId);
                if (AgendamentoId != result.id)
                {
                    return BadRequest();
                }
                result.email = dados.email;
                result.data = dados.data;
                result.horario = dados.horario;
                result.pet = dados.pet;
                result.especie = dados.especie;
                result.aprovado = dados.aprovado;
                await _context.SaveChangesAsync();
                return Created($"/api/agendamento/{dados.id}", dados);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }


        }
    }
}