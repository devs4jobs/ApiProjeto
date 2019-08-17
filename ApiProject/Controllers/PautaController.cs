using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta) => Created("", new PautaCore().Cadastrar(pauta));



        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new PautaCore().Achar(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta, string id) => Ok(new PautaCore().Atualizar(id));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            new PautaCore().DeletarUm(id);
            return NoContent();
        } 
       

    }
}