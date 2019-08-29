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
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var Core = new EleitorCore(eleitor).CadastroEleitor();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{eleitor.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");

        }
        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new EleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        }
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().AcharTodos().Resultado);
        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor, string id) => Ok(new EleitorCore().AtualizarUm(id, eleitor).Resultado);
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new EleitorCore().DeletarId(id));



        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> AcharPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new EleitorCore().BuscaPorData(DataComeco, DataFim);

            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);

        }

    }
}