using System;
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

            return cadastro.Status ? Created($"https://localhost/api/pautas/{pauta.Id}", cadastro.Resultado) : BadRequest(cadastro.Resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var retorno=new PautaCore().ID(id).Resultado;

            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet("por-data")]
        public async Task<IActionResult> GetPorData([FromQuery] string Date, [FromQuery] string Time)
        {
            var retorno = new PautaCore().PorData(Date, Time);

            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet("{direcao}/{Npagina}/{TPagina}")]
        public async Task<IActionResult> BuscaPorPagina(string Direcao, int NPagina, int TPagina)
        {
            var retorno = new PautaCore().PorPagina(NPagina, Direcao, TPagina);
            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore().Lista().Resultado);

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Pauta pauta)
        {
            var cadastro = new PautaCore(pauta).AtualizaPauta();

            return cadastro.Status ? Accepted($"https://localhost/api/pautas/{pauta.Id}", cadastro.Resultado) :BadRequest(cadastro.Resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cadastro = new PautaCore().DeletaPauta(id);
            return cadastro.Status ? NoContent() : NotFound(cadastro.Resultado);
        }
    }
}
