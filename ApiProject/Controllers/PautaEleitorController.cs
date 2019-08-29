using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitorController : ControllerBase
    {
        //Cadastrar PautaEleitor(voto) pelo verbo post eu vou precisar de uma PautaEleitor valida. !!!
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaeleitor)
        {
            var cadastro = new PautaEleitorCore(pautaeleitor).CadastrarPautaEleitor();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{pautaeleitor.PautaId}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        //aqui eu vou buscar meus PautaEleitor(voto) por paginas !
        [HttpGet("page")]
        public async Task<IActionResult> GetByPage([FromQuery]int itens, [FromQuery]int pagina)
        {
            var conteudo = new PautaEleitorCore().Paginacao(itens, pagina);

            if (conteudo.Resultado.Count == 0) { conteudo.Status = false; conteudo.Resultado = "Não temos a quantidade de itens nessa pagina"; };

            return conteudo.Status ? Ok(conteudo.Resultado) : BadRequest(conteudo.Resultado);
        }

        //Aqui só procuro a PautaEleitor(voto) pelo id e respondo com todas informações dela.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pautaEleitor = new PautaEleitorCore().ProcurarPorID(id); return pautaEleitor.Status ? Ok(pautaEleitor.Resultado) : BadRequest(pautaEleitor.Resultado); }

        //Get all eu vou buscar Todas as PautaEleitor(voto
        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new PautaEleitorCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        //Vou fazer a atualização de dados através do verbo Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]PautaEleitor pautaEleitor){var atualizar = new PautaEleitorCore().AtualizarPorID(id, pautaEleitor); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }

        //Aqui consigo apagar um elemento pelo verbo Delete.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new PautaEleitorCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}