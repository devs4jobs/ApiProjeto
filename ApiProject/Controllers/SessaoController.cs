using System;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;
namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessaoController : ControllerBase
    {
        //Cadastrar sessão eu vou precisar de uma Sessão valida. !!!
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var cadastro = new SessaoCore(sessao).CadastroSessao();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{sessao.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        //aqui eu vou buscar minhas sessões por paginas !
        [HttpGet("page")]
        public async Task<IActionResult> GetByPage([FromQuery]int itens, [FromQuery]int pagina)
        {
            var conteudo = new SessaoCore().Paginacao(itens, pagina);

            if (conteudo.Resultado.Count == 0) { conteudo.Status = false; conteudo.Resultado = "Não temos a quantidade de itens nessa pagina"; };

            return conteudo.Status ? Ok(conteudo.Resultado) : BadRequest(conteudo.Resultado);
        }

        //Aqui vou buscar todas as lista por uma data declarada pelo usuario.
        [HttpGet("find-by-data")]
        public async Task<IActionResult> GetByData([FromQuery]string data) { var SessaoData = new SessaoCore().ProcurarPorData(data); return SessaoData.Status ? Ok(SessaoData.Resultado) : BadRequest(SessaoData.Resultado); }

        //Vou buscar as sessões pelo ID dela e Exibir o Status Dela ! 
        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetStatusById(Guid id) { var pauta = new SessaoCore().StatusSessao(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }

        //Aqui só procuro a sessão pelo id e respondo com todas informações dela.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pauta = new SessaoCore().ProcurarPorID(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }
        //Get all eu vou buscar Todas as sessões estajam elas Abertas ou Fechadas 
        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new SessaoCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}
        //Vou fazer a atualização de dados através do verbo Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Sessao pauta){var atualizar = new SessaoCore().AtualizarPorID(id, pauta); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }
        //Aqui consigo apagar um elemento pelo verbo Delete.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new SessaoCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}