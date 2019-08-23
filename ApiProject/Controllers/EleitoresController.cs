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
        //configuranto o post e tratando respostas http através do Swagger
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            //var recebendo o eleitor e o cadastrando atraves do metodo CadastroEleitor
            var cadastro = new EleitorCore(eleitor).CadastroEleitor();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }
        //configurando  fet por id

        [HttpGet("{id}")]
        //configuranto o post e tratando respostas http através do Swagger
        [ProducesResponseType(200, Type = typeof(Eleitor))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get(string id)
        {
            //armazeno nessa var o eleitor a ser exibido através do método ExibirEleitorId
            var exibe = new EleitorCore().ExibirEleitorId(id);
            if (exibe.Status)
                //retornando a variavel que contem o eleitor.status
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet]  
        //configurando o get de todos os eleitores 
        [ProducesResponseType(200, Type = typeof(List<Eleitor>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()   
        {
            //var que armazena o resultado do método ExibirTodos
            var exibe = new EleitorCore().ExibirTodos();
            if (exibe.Status)
                //retornando a variavel.status
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(string id)
        {
            var deleta = new EleitorCore().DeletarEleitorId(id);
            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(202, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Put(string id, [FromBody] Eleitor eleitor)
        {
            var atualiza = new EleitorCore().AtualizarId(eleitor, id);
            if (atualiza.Status)
                return Ok(atualiza.Resultado);
            return BadRequest(atualiza.Resultado);
        }
    }
}