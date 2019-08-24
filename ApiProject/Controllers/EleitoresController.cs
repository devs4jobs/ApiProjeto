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
            //caso não consiga exibir o resultado o BadRequest dara o resultado  
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
            //caso não consiga exibir o resultado o BadRequest dara o resultado  
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
            //caso não consiga exibir o resultado o BadRequest dara o resultado  
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        //configurando o delete
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(string id)
        {
            //varaivel que armazena o eleitor deletado 
            var deleta = new EleitorCore().DeletarEleitorId(id);
            if (deleta.Status)
                //retornando a mensagem de eleitor deletado
                return Ok(deleta.Resultado);
            //caso não consiga exibir o resultado o BadRequest dara o resultado  
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("{id}")]
        //configurando o pUt para atualizações
        [ProducesResponseType(202, Type = typeof(Eleitor))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        //as alteraçoes em um eleitor virão atravé sdo corpo da pagina 
        public async Task<IActionResult> Put(string id, [FromBody] Eleitor eleitor)
        {
            //vara atualiza recebe o eleitor já atualizado
            var atualiza = new EleitorCore().AtualizarId(eleitor, id);
            if (atualiza.Status)
                //retornando o resultado caso tenha sucesso
                return Ok(atualiza.Resultado);
            //returnando badrequest caso haja algum problema na atualização
            return BadRequest(atualiza.Resultado);
        }
    }
}