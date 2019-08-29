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
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().AcharTodos().Resultado);

        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new EleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // chamando o método de paginacão      
        [HttpGet("Paginas")]
        public async Task<IActionResult> PorPagina([FromQuery]string Ordem, [FromQuery] int numerodePaginas, [FromQuery]int qtdRegistros)
        {
            var Core = new EleitorCore().PorPaginacao(Ordem, numerodePaginas, qtdRegistros);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }

        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var Core = new EleitorCore(eleitor).CadastroEleitor();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{eleitor.Id}", Core.Resultado) : BadRequest(Core.Resultado);
        }

        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor, string id)
        {
            var Core = new EleitorCore().AtualizarUm(id, eleitor);
            return Core.Status ? Ok(Core.Resultado) : BadRequest(Core.Resultado);
        }
    
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Core = new EleitorCore().DeletarId(id);
            return Core.Status ? Accepted(Core.Resultado) : BadRequest(Core.Resultado);
        }
    }
}