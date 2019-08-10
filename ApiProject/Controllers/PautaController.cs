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
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var Eleitor = new Core.PautaCore(pauta);
            return Created("", null);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new Eleitor());

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new Eleitor());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta, string id)
        {
            var p = new PautaCore();
            p.Atualizar(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = new PautaCore();
            p.DeletarUm(id);
            return Ok();
        }

    }
}