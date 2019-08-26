using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var cadastro = new SessaoCore(sessao).IniciarSessao();

            if (cadastro.Status)
                return Created($"https://localhost/api/sessoes/{sessao.Id}", cadastro.Resultado);

            return BadRequest(cadastro.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new SessaoCore().Lista().Resultado);
    }
}