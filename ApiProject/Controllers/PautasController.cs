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
        //Cadastrar Pauta pelo verbo post eu vou precisar de uma Pauta valida. !!!
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta) 
        {
            var cadastro = new PautaCore(pauta).CadastrarPauta();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{pauta.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        //aqui eu vou buscar minhas Pautas por paginas !
        [HttpGet("page")]
        public async Task<IActionResult> GetByPage([FromQuery]int itens, [FromQuery]int pagina)
        {
            var conteudo = new PautaCore().Paginacao(itens, pagina);

            if (conteudo.Resultado.Count == 0) { conteudo.Status = false; conteudo.Resultado = "Não temos a quantidade de itens nessa pagina"; };

            return conteudo.Status ? Ok(conteudo.Resultado) : BadRequest(conteudo.Resultado);
        }

        //Aqui vou buscar todas as lista por uma data declarada pelo usuario.
        [HttpGet("find-by-data")]
        public async Task<IActionResult> GetByData([FromQuery]string data) { var PautasData = new PautaCore().ProcurarPorData(data); return PautasData.Status ? Ok(PautasData.Resultado) : BadRequest(PautasData.Resultado); }

        //Aqui só procuro a Pauta pelo id e respondo com todas informações dela.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pauta = new PautaCore().ProcurarPorID(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }

        //Get all eu vou buscar Todas as pautas. 
        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new PautaCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}

        //Vou fazer a atualização de dados através do verbo Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Pauta pauta){var atualizar = new PautaCore().AtualizarPorID(id, pauta); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }

        //Aqui consigo apagar um elemento pelo verbo Delete.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new PautaCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}