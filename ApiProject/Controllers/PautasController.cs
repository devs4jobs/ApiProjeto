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
            var cadastro = new PautaCore(pauta).CadastroPauta();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var exibe = new PautaCore().ExibirPautaId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exibe = new PautaCore().ExibirTodasPautas();
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPautaId(string id)
        {
            var deleta = new PautaCore().DeletarPautaId(id);
            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Pauta pauta)
        {
            var atualiza = new PautaCore().AtualizarPautaId(pauta, id);
            if (atualiza.Status)
                return Ok(atualiza.Resultado);
            return BadRequest(atualiza.Resultado);
        }
    }
}
