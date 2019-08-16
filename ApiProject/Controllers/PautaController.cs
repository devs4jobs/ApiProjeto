using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using Model.Db;
using System.Linq;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        // Refencia ao Contexto(db).
        private EleicaoContext _eleicaoContext;
        //Construtor contendo o contexto.
        public PautasController(EleicaoContext eleicaoContext) { _eleicaoContext = eleicaoContext; }
        //Request Post chamando o método do core.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta)
        {
            var Core = new PautaCore(_eleicaoContext).Cadastrar(pauta);
            if (pauta == null)
                return BadRequest();
            return Created("", Core);
        }
        //Request Get chamando o método do core.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var Core = new PautaCore(_eleicaoContext).AcharUm(id);
            if (Core == null)
                return BadRequest();
            return Ok(Core);
        }
        //Request Get chamando o método do core.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore(_eleicaoContext).AcharTodos());
        //Request put chamando o método do core.
        [HttpPut("{Att}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta)

        {
            var Core = new PautaCore(_eleicaoContext).Atualizar(pauta);
            if (Core == null)
                return BadRequest();
            return Ok();
        } 

        //Request delete chamando o método do core.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Core = new PautaCore(_eleicaoContext);
            if (!Core.VerificaId(id))
                return BadRequest("Esse registro não existe!");
            Core.DeletarUm(id);
            return NoContent();
        }
    }
}