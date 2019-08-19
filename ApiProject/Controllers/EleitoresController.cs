using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var cadastro = new EleitorCore(eleitor).CadastroEleitor();
            return cadastro.Status ? Created($"https://localhost:44323/api/Eleitores/{eleitor.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){var eleitor = new EleitorCore().ProcurarPorID(id);return eleitor.Status ? Ok(eleitor.Resultado) : BadRequest(eleitor.Resultado); }

        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new EleitorCore().ProcurarTodos();return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Eleitor eleitor){var atualizar = new EleitorCore().AtualizarPorID(id, eleitor);return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new EleitorCore().DeletarPorID(id);return excluir.Status?  Ok(excluir.Resultado) :  BadRequest(excluir.Resultado);}
    }
}