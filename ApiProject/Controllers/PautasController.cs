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
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var Core = new PautaCore(pauta).CadastroEleitor();
            return Core.Status ? Created($"https://localhost/api/Pautas/{pauta.Id}", Core.Resultado) : BadRequest(Core.Resultado);

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
        public async Task<IActionResult> Get() => Ok(new PautaCore().AcharTodos().Resultado);


        [HttpGet("Data")]
        // Buscar por data
        public async Task<IActionResult> AcharPordata([FromQuery] string DataComeco, [FromQuery] string DataFim)
        {
            var Cor = new PautaCore().BuscaPorData(DataComeco, DataFim);

            return Cor.Status ? Ok(Cor.Resultado) : BadRequest(Cor.Resultado);

        }


        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta, string id) => Ok(new PautaCore().AtualizarUm(id, pauta).Resultado);
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new PautaCore().DeletarId(id));

      

    }
}
