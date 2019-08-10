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
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaeleitor)
        {
            var Pauta = new Core.PautaEleitorCore(pautaeleitor);
            return Created("", null);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new Eleitor());

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new Eleitor());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]PautaEleitor pautaEleitor, string id)
        {
            var e = new PautaEleitorCore();
            e.Atualizar(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var e = new PautaEleitorCore();
            e.DeletarUm(id);
            return Ok();
        }

    }
}