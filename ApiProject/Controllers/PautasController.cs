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
        //configurando a criaçãlo de uma nova pauta através do usuário
        [ProducesResponseType(201, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        //o post sera feito atravé sdo corpo da pagina
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            //var cadastro rrecebendo o resultado do método CadastroPauta
            var cadastro = new PautaCore(pauta).CadastroPauta();
            if (cadastro.Status)
                //returnando´funçãe Created recebendo como assinatura nossa url localhost e a variavel cadastro com o dinamico Resultado
                return Created("https://localhost", cadastro.Resultado);
            //caso haja insucesso BadRequest tratará com o Resultado
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        //configurando a exibição filtrada de Pauta
        [ProducesResponseType(200, Type = typeof(Pauta))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get(string id)
        {
            var exibe = new PautaCore().ExibirPautaId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet]
        //configurando o get de todos as pautas para exibição
        [ProducesResponseType(200, Type = typeof(List<Pauta>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            //var exibe recebe o resultado do método ExibirTodasPautas
            var exibe = new PautaCore().ExibirTodasPautas();
            if (exibe.Status)
                //retornando a variavel com seu status
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        //configurando deleção de uma pauta
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeletarPautaId(string id)
        {
            //var deleta recebe o resultado de DeletarPautaId com o id da pauta como argumento
            var deleta = new PautaCore().DeletarPautaId(id);
            if (deleta.Status)
                //retonando status Ok (variável deleta com dinamico Resultado) 
                return Ok(deleta.Resultado);
            //retornando badRequest caso nao haja sucesso na deleção da pauta
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("{id}")]
        //configurando o Put de atualização de uma pauta
        [ProducesResponseType(202, Type = typeof(Pauta))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        //a atualização da pauta vira através do corpo da página
        public async Task<IActionResult> Put(string id, [FromBody] Pauta pauta)
        {
            //var atualiza recebendo o método AtualizarPautaId com o id do usurario inserido como argumento
            var atualiza = new PautaCore().AtualizarPautaId(pauta, id);
            if (atualiza.Status)
                //retorno da variavel.Status
                return Ok(atualiza.Resultado);
            //Badrequest caso algo de errado aconteça
            return BadRequest(atualiza.Resultado);
        }
    }
}
