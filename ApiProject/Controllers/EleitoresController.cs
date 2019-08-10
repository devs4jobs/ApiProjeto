﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
       

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor)
        {
            var Eleitor = new Core.EleitorCore(eleitor);
            return Created("", null);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new Eleitor());

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new Eleitor());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor ,string id)
        {
            EleitorCore e = new EleitorCore();
            e.Atualizar(id);
            return Ok();
        }
         

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            PautaCore e = new PautaCore();
            e.DeletarUm(id);
            return Ok();
        }

    }
}