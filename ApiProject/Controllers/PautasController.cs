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
    public class PautasController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var Core = new PautaCore(pauta).CadastroEleitor();
            return Core.Status ? Created($"https://localhost/api/Pautas/{pauta.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new EleitorCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        } 

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore().AcharTodos().Resultado);

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta, string id) => Ok(new PautaCore().AtualizarUm(id, pauta).Resultado);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new PautaCore().DeletarId(id));

    }
}