﻿using System;
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
                return Created($"https://localhost/api/eleitores/{eleitor.Id}", cadastro.Resultado);

            
            return BadRequest(cadastro);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) => Ok(new EleitorCore().ID(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().Lista());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => NoContent();

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted();

    }
}