using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using System.Collections.Generic;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitorControler : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaEleitor) => Ok(new PautaEleitorCore().Cadastrar(pautaEleitor));

        [HttpGet("{id}")]
        public async Task<ActionResult<PautaEleitor>> Get(string id) => Ok(new PautaEleitorCore().ProcurarID(id));
        [HttpGet]
        public async Task<ActionResult<List<PautaEleitor>>> GetAll() => Ok(new PautaEleitorCore().ProcurarTodos());

        [HttpPut("{id}")]
        public async Task<ActionResult<PautaEleitor>> Put(string id) => Ok(new PautaEleitorCore().Atualizar(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new PautaEleitorCore().Excluir(id);
            return NoContent();
        }

    }
}