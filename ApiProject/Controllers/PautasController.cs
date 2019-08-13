using Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta) => Ok(new PautaCore().Cadastrar(pauta));


        [HttpGet("{id}")]
        public async Task<ActionResult<Pauta>> GetId(string id) => Ok(new PautaCore().ProcurarID(id));


        [HttpGet]
        public async Task<ActionResult<List<Pauta>>> GetAll() => Ok(new PautaCore().ProcurarTodos());

        [HttpPut("{id}")]
        public async Task<ActionResult<Pauta>> Put(string id) => Ok(new PautaCore().Atualizar(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new PautaCore().Excluir(id);
            return NoContent();
        }

    }
}