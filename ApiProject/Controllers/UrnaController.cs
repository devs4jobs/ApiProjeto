using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrnaController : ControllerBase
    {      
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Urna))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Post([FromBody] Urna urna)
        {
            
            var cadastro = new UrnaCore(urna).CadastroUrna();
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);
            
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]     
        [ProducesResponseType(200, Type = typeof(Urna))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Get(string id)
        {       
            var exibe = new UrnaCore().ExibirUrnaId(id);
            if (exibe.Status)
               
                return Ok(exibe.Resultado);
           
            return BadRequest(exibe.Resultado);
        }

        [HttpGet]      
        [ProducesResponseType(200, Type = typeof(List<Urna>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
           
            var exibe = new UrnaCore().ExibirTodasUrnas();
            if (exibe.Status)
                
                return Ok(exibe.Resultado);
             
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]  
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(string id)
        {
           
            var deleta = new UrnaCore().DeletarUrnaId(id);
            if (deleta.Status)
               
                return Ok(deleta.Resultado);
            
            return BadRequest(deleta.Resultado);
        }
        [HttpPut("{id}")]     
        [ProducesResponseType(202, Type = typeof(Urna))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        
        public async Task<IActionResult> Put(string id, [FromBody] Urna urna)
        {
            
            var atualiza = new UrnaCore().AtualizarUrnaId(urna, id);
            if (atualiza.Status)
              
                return Ok(atualiza.Resultado);
            
            return BadRequest(atualiza.Resultado);
        }
    }
}