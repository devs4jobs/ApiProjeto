using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresPautaController : ControllerBase
    {
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pauta)
        {
            var Core = new PautaEleitorCore(pauta).Votar();
            return Core.Status ? Created($"https://localhost/api/EleitoresPautas/{pauta.PautaId}", Core.Resultado) : BadRequest("Esse cadastro já existe.");

        }
        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PautaEleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        } 
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitorCore().AcharTodos().Resultado);
        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] PautaEleitor pauta, string id) => Ok(new PautaEleitorCore().AtualizarUm(id, pauta).Resultado);
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new PautaEleitorCore().DeletarId(id));

    }
}