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
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitorCore().AcharTodos().Resultado);

        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PautaEleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // chamando o método de paginacão  
        [HttpGet("Paginas")]
        public async Task<IActionResult> PorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new PautaEleitorCore().PorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            // verifico se pagina que o usuario pediu é valida, se nao retorno um BadRequest
            if (Core.Resultado.Count == 0)
                return BadRequest("Essa pagina não existe!");

            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pauta)
        {
            var Core = new PautaEleitorCore(pauta).Votar();
            return Core.Status ? Created($"https://localhost/api/EleitoresPautas/{pauta.PautaId}", Core.Resultado) : BadRequest(Core.Resultado);

        }
        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] PautaEleitor pauta, string id)
        {
           var Core = new PautaEleitorCore().AtualizarUm(id, pauta);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
          
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Core = new PautaEleitorCore().DeletarId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}