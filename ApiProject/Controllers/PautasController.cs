using Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        private readonly Conectionn _context;
        public PautasController(Conectionn conectionn)
        {
            _context = conectionn;
        }
        //GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Pauta>>> GetAll() => new PautaCore(_context).Procurar();

        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pauta>> GetByID(string id) => new PautaCore(_context).ProcurarPorId(id);

        // POST
        [HttpPost]
        public async Task<ActionResult<Pauta>> Post([FromBody] Pauta cliente) => new PautaCore(_context).Cadastrar(cliente);

        // PUT
        [HttpPut]
        public async Task<ActionResult<Pauta>> Put([FromBody]Pauta cliente) => new PautaCore(_context).Atualizar(cliente);

        // DELETE
        [HttpDelete("{id}")]
        public void Delete(string id) => new PautaCore(_context).Deletar(id);

    }
}