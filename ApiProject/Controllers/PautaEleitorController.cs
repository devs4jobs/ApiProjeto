using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitorController : ControllerBase
    {
        // Classe ainda par ser implantada.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaeleitor) => Created("", new PautaEleitorCore().Cadastrar(pautaeleitor));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new PautaEleitorCore().Achar(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitorCore().AcharTodos());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]PautaEleitor pautaEleitor, string id) => Ok(new PautaEleitorCore().Atualizar(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new PautaEleitorCore().DeletarUm(id);
            return NoContent();
        } 
    }
}