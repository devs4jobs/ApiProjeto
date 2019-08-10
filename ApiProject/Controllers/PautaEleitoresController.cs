using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitoresController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PautaEleitor pautaEleitor)
        {
            var PautaEleitor = new Core.PautaEleitorCore(pautaEleitor);
            return Created("", null);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new PautaEleitor());

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitor());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => NoContent();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted();
    }
}