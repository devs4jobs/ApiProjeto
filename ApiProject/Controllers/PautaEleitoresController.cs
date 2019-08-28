using System;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotacoesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor Voto)
        {
            var cadastro = new PautaEleitorCore(Voto).Votacao();

            return cadastro.Status
                ? (IActionResult)Created($"https://localhost/api/votacoes/{Voto.PautaId}", cadastro.Resultado)
                : (IActionResult)BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var retorno = new PautaEleitorCore().ID(id);

            return retorno.Status ? (IActionResult)Ok(retorno.Resultado) : (IActionResult)BadRequest(retorno.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitorCore().Lista().Resultado);

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PautaEleitor Voto)
        {
            var cadastro = new PautaEleitorCore(Voto).AtualizaVoto();

            return cadastro.Status
                ? (IActionResult)Accepted($"https://localhost/api/votacoes/{Voto.PautaId}", cadastro.Resultado)
                : (IActionResult)BadRequest(cadastro.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cadastro = new EleitorCore().DeletaEleitor(id);
            return cadastro.Status ? NoContent() : (IActionResult)NotFound(cadastro.Resultado);
        }
    }
}
