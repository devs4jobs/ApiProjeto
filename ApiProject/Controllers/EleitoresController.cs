using System;
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

            return cadastro.Status
                ? Created($"https://localhost/api/eleitores/{eleitor.Id}", cadastro.Resultado)
                : BadRequest(cadastro.Resultado);
        }

        [HttpGet("por-data")]
        public async Task<IActionResult> GetPorData([FromQuery] string Date,[FromQuery] string Time)
        {
            var retorno = new EleitorCore().PorData(Date,Time);

            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet("{direcao}/{Npagina}/{TPagina}")]
        public async Task<IActionResult> BuscaPorPagina(string Direcao, int NPagina, int TPagina)
        {
            var retorno = new EleitorCore().PorPagina(NPagina, Direcao, TPagina);
            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var retorno=new EleitorCore().ID(id);

            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().Lista().Resultado);




        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor)
        {
            var cadastro = new EleitorCore(eleitor).AtualizaEleitor();

            return cadastro.Status
                ? (IActionResult)Accepted($"https://localhost/api/eleitores/{eleitor.Id}", cadastro.Resultado)
                : (IActionResult)BadRequest(cadastro.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cadastro = new EleitorCore().DeletaEleitor(id);
            return cadastro.Status ? NoContent() : (IActionResult)BadRequest(cadastro.Resultado);
        }
    }
}