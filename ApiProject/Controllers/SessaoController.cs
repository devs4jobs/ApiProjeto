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
        //post de sessao 
        [ProducesResponseType(201, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var cadastro = new SessaoCore(sessao).CadastroSessao();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpPost("AdicionarPautaEleitor")]
        //adicionando pauta e eleitor em uma sessão
        [ProducesResponseType(201, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> addPautaEleitor([FromBody] AdicionaPtEl addSessao)
        {
            var cadastro = new SessaoCore().adicionarPautaEleitor(addSessao);
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("buscaPorId")]
        //buscando a sessao por id
        [ProducesResponseType(200, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromQuery]string id)
        {
            var exibe = new SessaoCore().ExibirSessaoId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPorData")]
        //buscando por data
        [ProducesResponseType(200, Type = typeof(Sessao))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDate([FromQuery]string dataCadastro)
        {
            var exibe = new SessaoCore().ExibirSessaoDataCadastro(dataCadastro);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPaginada")]
        //paginando todas as sessaoes
        [ProducesResponseType(200, Type = typeof(List<Sessao>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll([FromQuery] int page,[FromQuery] int sizePage)
        {
            var exibe = new SessaoCore().ExibirTodasSessoes(page, sizePage);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("deletePorId")]
        //deletando uma sessao
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromQuery]string id)
        {
            var deleta = new SessaoCore().DeletarSessaoId(id);
            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }
    }
}