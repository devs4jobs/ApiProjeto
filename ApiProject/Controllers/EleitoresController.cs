﻿using System.Threading.Tasks;
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
                return Created("https://localhost", cadastro.Resultado);
            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            var exibe = new EleitorCore().ExibirEleitorId(id);
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var exibe = new EleitorCore().ExibirTodos();
            if (exibe.Status)
                return Ok(exibe.Resultado);
            return BadRequest(exibe.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleta = new EleitorCore().DeletarEleitorId(id);
            if (deleta.Status)
                return Ok(deleta.Resultado);
            return BadRequest(deleta.Resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Eleitor eleitor)
        {
            var atualiza = new EleitorCore().AtualizarId(eleitor, id);
            if (atualiza.Status)
                return Ok(atualiza.Resultado);
            return BadRequest(atualiza.Resultado);
        }
    }
}