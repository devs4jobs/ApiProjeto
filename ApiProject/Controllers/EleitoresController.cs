using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor) => Created("", new EleitorCore().Cadastrar(eleitor));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new EleitorCore().Achar(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor, string id) => Ok(new EleitorCore().Atualizar(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new EleitorCore().DeletarUm(id);
            return NoContent();
        }
       
    }
}