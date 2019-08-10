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
    public class PautasController  : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var Pauta = new Core.PautaCore(pauta); 
            return Created("", null);
        }
         

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new Pauta());

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new Pauta());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => NoContent();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted();
    }
}