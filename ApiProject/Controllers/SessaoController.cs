using System;
using System.Collections.Generic;
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
        //Cadastrar sessão eu vou precisar de uma lista de ID's de Pautas !!!
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var cadastro = new SessaoCore(sessao).CadastroSessao();
            return cadastro.Status ? Created($"https://localhost:44323/api/Pautas/{sessao.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        //Vou buscar as sessões pelo ID dela e Exibir o Status Dela ! 
        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetStatusById(Guid id) { var pauta = new SessaoCore().StatusSessao(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){  var pauta = new SessaoCore().ProcurarPorID(id); return pauta.Status ? Ok(pauta.Resultado) : BadRequest(pauta.Resultado); }
        //Get all eu vou buscar Todas as sessões estajam elas Abertas ou Fechadas 
        [HttpGet]
        public async Task<IActionResult> GetAll() {var todos = new SessaoCore().ProcurarTodos(); return todos.Status ? Ok(todos.Resultado) : BadRequest(todos.Resultado);}
        //Vou fazer a votação atráves desse Verbo "PUT"
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Sessao pauta){var atualizar = new SessaoCore().AtualizarPorID(id, pauta); return atualizar.Status ? Ok(atualizar.Resultado) : BadRequest(atualizar.Resultado);
        }
        //Não vou disponibilizar o Deletar pelo fato de ter todos os registros de Salvos!.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id){var excluir = new SessaoCore().DeletarPorID(id); return excluir.Status ? Ok(excluir.Resultado) : BadRequest(excluir.Resultado);}
    }
}