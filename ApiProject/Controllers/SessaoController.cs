using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SessaoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> insere([FromBody] Sessao sessao)
        {
            var cadastro = new SessaoCore(sessao).CadastroSessao();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpPost("AdicionarEleitor")]
        [ProducesResponseType(201, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> adicionaEleitor([FromBody] AdicionaPtEl addSessao)
        {
            var cadastro = new SessaoCore().adicionarEleitor(addSessao);
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpPost("AdicionarPauta")]
        [ProducesResponseType(201, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> adicionaPauta([FromBody] AdicionaPtEl addSessao)
        {
            var cadastro = new SessaoCore().adicionarPauta(addSessao);
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Sessao))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> buscaId(string id)
        {
            var exibe = new SessaoCore().ExibirSessaoId(id);
            if (exibe.Status)

                return Ok(exibe.Resultado);

            return BadRequest(exibe.Resultado);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Sessao>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> getAll()
        {
            var exibe = new SessaoCore().ExibirTodasSessoes();
            if (exibe.Status)

                return Ok(exibe.Resultado);

            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> delete(string id)
        {
            var deleta = new SessaoCore().DeletarSessaoId(id);
            if (deleta.Status)

                return Ok(deleta.Resultado);

            return BadRequest(deleta.Resultado);
        }
    }
}