using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using System.Collections.Generic;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor) => Ok(new EleitorCore().Cadastrar(eleitor));


        [HttpGet("{id}")]
        public async Task<ActionResult<Eleitor>> Get(string id) => Ok(new EleitorCore().ProcurarID(id));

        [HttpGet]
        public async Task<ActionResult<List<Eleitor>>> GetAll() => Ok(new EleitorCore().ProcurarTodos());

        [HttpPut("{id}")]
        public async Task<ActionResult<Eleitor>> Put(string id) => Ok(new EleitorCore().Atualizar(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new EleitorCore().Excluir(id);
           return NoContent();
        }
    }
}