using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var cadastro = new PautaCore(pauta).CadastrarPauta();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{pauta.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pauta = new PautaCore().ProcurarPorID(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }

        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new PautaCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Pauta pauta){var atualizar = new PautaCore().AtualizarPorID(id, pauta); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new PautaCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}