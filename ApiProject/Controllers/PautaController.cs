using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using Model.Db;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        // Refencia ao Contexto(db).
        private EleicaoContext _eleicaoContext;
        //Construtor contendo o contexto.
        public PautasController(EleicaoContext eleicaoContext) { _eleicaoContext = eleicaoContext;}
        //Request Post chamando o método do core.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pauta pauta) => Created("", new PautaCore(_eleicaoContext).Cadastrar(pauta));
        //Request Get chamando o método do core.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new PautaCore(_eleicaoContext).AcharUm(id));
        //Request Get chamando o método do core.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaCore(_eleicaoContext).AcharTodos());
        //Request put chamando o método do core.
        [HttpPut("{Att}")]
        public async Task<IActionResult> Put([FromBody] Pauta pauta) => Ok(new PautaCore(_eleicaoContext).Atualizar(pauta));

        //Request delete chamando o método do core.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            new PautaCore(_eleicaoContext).DeletarUm(id);
            return NoContent();
        }
    }
}