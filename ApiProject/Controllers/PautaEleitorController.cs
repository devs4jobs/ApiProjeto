using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitorController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaeleitor)
        {
            var cadastro = new PautaEleitorCore(pautaeleitor).CadastrarPautaEleitor();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{pautaeleitor.PautaId}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pautaEleitor = new PautaEleitorCore().ProcurarPorID(id); return pautaEleitor.Status ? Ok(pautaEleitor.Resultado) : BadRequest(pautaEleitor.Resultado); }

        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new PautaEleitorCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]PautaEleitor pautaEleitor){var atualizar = new PautaEleitorCore().AtualizarPorID(id, pautaEleitor); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new PautaEleitorCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}