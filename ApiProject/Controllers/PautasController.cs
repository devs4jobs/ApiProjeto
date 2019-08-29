using System.Collections.Generic;
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
        [ProducesResponseType(201, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var cadastro = new PautaCore(pauta).CadastroPauta();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("buscaPorId")]
        [ProducesResponseType(200, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var exibe = new PautaCore().ExibirPautaId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPorData")]
        [ProducesResponseType(200, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDate([FromQuery]string dataCadastro)
        {
            var exibe = new PautaCore().ExibirPautaDataCadastro(dataCadastro);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPaginada")]
        [ProducesResponseType(200, Type = typeof(List<Pauta>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll([FromQuery] int page,[FromQuery] int sizePage)
        {
            var exibe = new PautaCore().ExibirTodasPautas(page, sizePage);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("deletePorId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromQuery]string id)
        {
            var deleta = new PautaCore().DeletarPautaId(id);
            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("atualizaPorId")]
        [ProducesResponseType(200, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromQuery]string id, [FromBody] Pauta pauta)
        {
            var atualiza = new PautaCore().AtualizarPautaId(pauta, id);
            if (atualiza.Status)
                return Ok(atualiza.Resultado);
            return BadRequest(atualiza.Resultado);
        }
    }
}
