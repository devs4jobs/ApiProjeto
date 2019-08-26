﻿using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessoesController : ControllerBase
    {
        // Chamando o metodo de cadastro
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sessao sessao)
        {
            var Core = new SessaoCore(sessao).Cadastro();
            return Core.Status ? Created($"https://localhost/api/Sessoes/{sessao.Id}", Core.Resultado) : BadRequest("Esse cadastro já existe.");

        }
        //Chamando  o metodo de achar por id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new SessaoCore().AcharUm(id);
            return Core.Status ? Ok(Core.Resultado) : BadRequest("Esse cadastro não existe!");
        } 
        // Chamando o metodo achar todos
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new SessaoCore().AcharTodos().Resultado);
        // Chamando o metodo de atualização
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Sessao sessao, string id) => Ok(new SessaoCore().AtualizarUm(id, sessao).Resultado);
        // chamando o metodo para deletar um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Accepted(new SessaoCore().DeletarId(id));

    }
}