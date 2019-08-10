using Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautasController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string pauta) => Ok(new PautaCore().Create(pauta));


        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(string id) => Ok(new PautaCore().FindBy(id));


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(new PautaCore().FindAll());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => Ok(new PautaCore().Update(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Ok(new PautaCore().Delete(id));

    }
}