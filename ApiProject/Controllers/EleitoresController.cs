using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using Model.Db;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        // Refencia ao Contexto(db).
        private EleicaoContext _eleicaoContext;

        //Construtor contendo o contexto.
        public EleitoresController(EleicaoContext eleicaoContext) { _eleicaoContext = eleicaoContext; }
        //Request Post chamando o método do core.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Eleitor eleitor) => Created("", new EleitorCore(_eleicaoContext).Cadastrar(eleitor));
        //Request Get chamando o método do core.
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new EleitorCore(_eleicaoContext).AcharUm(id));
        //Request Get chamando o método do core.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore(_eleicaoContext).AcharTodos());
        //Request put chamando o método do core.
        [HttpPut("{Att}")]
        public async Task<IActionResult> Put([FromBody] Eleitor eleitor) => Ok(new EleitorCore(_eleicaoContext).Atualizar(eleitor));

        //Request delete chamando o método do core.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            new EleitorCore(_eleicaoContext).DeletarUm(id);
            return NoContent();
        }
    }
}