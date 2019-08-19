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
            var Core = new EleitorCore(eleitor).CadastroEleitor();
            return Core.Status ? Created($"https://localhost/api/Eleitores/{eleitor.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new EleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        } 

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().AcharTodos().Resultado);

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor, string id) => Ok(new EleitorCore().AtualizarUm(id, eleitor).Resultado);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new EleitorCore().DeletarId(id));

    }
}