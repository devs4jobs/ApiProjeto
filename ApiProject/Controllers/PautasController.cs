﻿using System;
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
            var cadastro = new PautaCore(pauta).CadastroPauta();

            if (cadastro.Status)
                return Created($"https://localhost/api/pautas/{pauta.Id}", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var retorno=new PautaCore().ID(id).Resultado;

            if (retorno.Status)
                return Ok(retorno.Resultado);

            return BadRequest(retorno.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore().Lista().Resultado);

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Pauta pauta)
        {
            var cadastro = new PautaCore(pauta).AtualizaPauta();

            if (cadastro.Status)
                return Accepted($"https://localhost/api/pautas/{pauta.Id}", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cadastro = new PautaCore().DeletaPauta(id);
            if (cadastro.Status)
                return NoContent();
            return NotFound(cadastro.Resultado);
        }
    }
}
