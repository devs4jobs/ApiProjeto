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
            if (cadastro.Status == false)
            {
                return BadRequest("Ja Existe um eleitor com esse documento");
            }
            if (cadastro.Status)
                return Created("https://localhost", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new EleitorCore().AcharUm(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor, string id) => Ok(new EleitorCore().AtualizarUm(id, eleitor));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new EleitorCore().DeletarId(id));

    }
}