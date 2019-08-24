using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessaoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Sessao> sessao)
        {
            var cadastro = new SessaoCore(sessao).CadastroSessao();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{sessao.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pauta = new SessaoCore().ProcurarPorID(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }

        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new SessaoCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Sessao pauta){var atualizar = new SessaoCore().AtualizarPorID(id, pauta); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new SessaoCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}