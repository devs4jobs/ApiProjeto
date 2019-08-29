using System.Collections.Generic;
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
        //Cadastrar Eleitor pelo verbo post eu vou precisar de uma eleitor valida. !!!
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var cadastro = new EleitorCore(eleitor).CadastroEleitor();
            return cadastro.Status ? Created($"https://localhost:44323/api/Eleitores/{eleitor.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        //aqui eu vou buscar meus Eleitores por paginas !
        [HttpGet("page")]
        public async Task<IActionResult> GetByPage([FromQuery]int itens, [FromQuery]int pagina)
        {
            var conteudo = new EleitorCore().Paginacao(itens, pagina);

            if (conteudo.Resultado.Count == 0) { conteudo.Status = false; conteudo.Resultado = "Não temos a quantidade de itens nessa pagina"; };

            return conteudo.Status ? Ok(conteudo.Resultado) : BadRequest(conteudo.Resultado);
        }

        //Aqui vou buscar todas as lista por uma data declarada pelo usuario.
        [HttpGet("find-by-data")]
        public async Task<IActionResult> GetByData([FromQuery]string data) { var EleitoresData = new EleitorCore().ProcurarPorData(data); return EleitoresData.Status ? Ok(EleitoresData.Resultado) : BadRequest(EleitoresData.Resultado); }

        //Aqui só procuro o eleitor pelo id e respondo com todas informações dele.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) { var eleitor = new EleitorCore().ProcurarPorID(id); return eleitor.Status ? Ok(eleitor.Resultado) : BadRequest(eleitor.Resultado); }

        //Get all eu vou buscar Todos os Eleitores Cadastrados.
        [HttpGet]
        public async Task<IActionResult> GetAll() { var todos = new EleitorCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado); }

        //Vou fazer a atualização de dados através do verbo Put
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Eleitor eleitor) { var atualizar = new EleitorCore().AtualizarPorID(id, eleitor); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado); }

        //Aqui consigo apagar um elemento pelo verbo Delete.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) { var excluir = new EleitorCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado); }
    }
}