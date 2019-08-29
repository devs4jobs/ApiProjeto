using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var Core = new SessaoCore(sessao).Cadastro();
            return Core.Status ? Created($"https://localhost/api/Sessoes/{sessao.Id}", Core.Resultado) : BadRequest(Core.Resultado);

        }
        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new SessaoCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        } 
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new SessaoCore().AcharTodos().Resultado);
   
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new SessaoCore().DeletarId(id));

        // chamando o metodo para mostrar o status da sessao
        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(string id) => Ok(new SessaoCore().RetornaStatus(id).Resultado);


        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> AcharPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new SessaoCore().BuscaPorData(DataComeco, DataFim);

            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);

        }

    }
}