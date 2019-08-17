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
                return Created("https://localhost", cadastro.Resultado);

            return BadRequest(cadastro);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(string id)
        {
            var eleitor = new EleitorCore().BuscaEleitorPorId(id);
            if (eleitor.Status)
                return Ok(eleitor.Resultado);
            return NoContent();
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var eleitor = new EleitorCore().BuscaEleitores();
            if (eleitor.Status)
                return Ok(eleitor.Resultado);
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id,[FromBody] Eleitor eleitor)
        {
            var resultado = new EleitorCore(eleitor).AtualizaEleitor(id);
            if (resultado.Status)
                return Accepted(resultado.Resultado);

            return BadRequest(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var resultado = new EleitorCore().DeletaEleitor(id);
            if (resultado.Status)
                return Accepted(resultado);

            return BadRequest(resultado);

        }

    }
}