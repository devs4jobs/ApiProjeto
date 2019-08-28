using System.Threading.Tasks;
using Core;
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

            return cadastro.Status ? (IActionResult)Created($"https://localhost/api/sessoes/{sessao.Id}", cadastro.Resultado) : (IActionResult)BadRequest(cadastro.Resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new SessaoCore().Lista().Resultado);

        [HttpGet("por-data")]
        public async Task<IActionResult> GetPorData([FromQuery] string Date, [FromQuery] string Time)
        {
            var retorno = new SessaoCore().PorData(Date, Time);

            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }

        [HttpGet("{direcao}/{Npagina}/{TPagina}")]
        public async Task<IActionResult> BuscaPorPagina(string Direcao ,int NPagina,int TPagina)
        {
            var retorno = new SessaoCore().PorPagina(NPagina, Direcao, TPagina);
            return retorno.Status ? Ok(retorno.Resultado) : BadRequest(retorno.Resultado);
        }
    }
}