using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        [HttpPost]
        //post frombody de um eleitor
        [ProducesResponseType(201, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var cadastro = new EleitorCore(eleitor).CadastroEleitor();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("buscaPorId")]
        //buscando por id
        [ProducesResponseType(200, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get([FromQuery]string id)
        {
            var exibe = new EleitorCore().ExibirEleitorId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPorData")]
        //buscando por data
        [ProducesResponseType(200, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDate([FromQuery] string dataCadastro)
        {
            var exibe = new EleitorCore().ExibirEleitorDataCadastro(dataCadastro);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet("buscaPaginada")]
        //configurando como sera a paginação formquery
        [ProducesResponseType(200, Type = typeof(List<Eleitor>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll([FromQuery] int page,[FromQuery] int sizePage)
        {var exibe = new EleitorCore().ExibirTodos(page, sizePage);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("deletePorId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var deleta = new EleitorCore().DeletarEleitorId(id);

            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("atualizaPorId")]
        [ProducesResponseType(200, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put([FromQuery] string id, [FromBody] Eleitor eleitor)
        {
            var atualiza = new EleitorCore().AtualizarId(eleitor, id);
            if (atualiza.Status)
                return Ok(atualiza.Resultado);
            return BadRequest(atualiza.Resultado);
        }
    }
}