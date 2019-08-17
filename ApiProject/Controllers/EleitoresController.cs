using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var cadastro = new EleitorCore(eleitor).CadastroEleitor();
            if (cadastro.Status)
                return Created($"https://localhost:44323/api/Eleitores/{eleitor.Id}", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id){
            var eleitor = new EleitorCore().ProcurarPorID(id);
            if (eleitor.Status)
                return Ok(eleitor.Resultado);

            return BadRequest(eleitor.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()        {
            var todos = new EleitorCore().ProcurarTodos();
            if (todos.Status)
                return Ok(todos.Resultado);

            return BadRequest(todos.Resultado);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id ,[FromBody]Eleitor eleitor)
        {
            var atualizar = new EleitorCore().AtualizarPorID(id,eleitor);
            if (atualizar.Status)
                return Ok(atualizar.Resultado);

            return BadRequest(atualizar.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var excluir = new EleitorCore().DeletarPorID(id);
            if (excluir.Status)
                return Ok(excluir.Resultado);

            return BadRequest(excluir.Resultado);
        }
    }
}