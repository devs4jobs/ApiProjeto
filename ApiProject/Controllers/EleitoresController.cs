using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
using System.Collections.Generic;


namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        private readonly Conectionn _context;
        public EleitoresController(Conectionn conectionn)
        {
            _context = conectionn;
        }
        //GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Eleitor>>> GetAll() => new EleitorCore(_context).Procurar();

        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Eleitor>> GetByID(string id) => new EleitorCore(_context).ProcurarPorId(id);

        // POST
        [HttpPost]
        public async Task<ActionResult<Eleitor>> Post([FromBody] Eleitor cliente) => new EleitorCore(_context).Cadastrar(cliente);

        // PUT
        [HttpPut]
        public async Task<ActionResult<Eleitor>> Put([FromBody] Eleitor cliente) => new EleitorCore(_context).Atualizar(cliente);

        // DELETE
        [HttpDelete("{id}")]
        public void Delete(string id) => new EleitorCore(_context).Deletar(id);

    }
}