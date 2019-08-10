using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;
namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PautaEleitorControler : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string id) => Ok(new PautaEleitorCore().Create(id));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new PautaEleitorCore().FindBy(id));
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new PautaEleitorCore().FindAll());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => Ok(new PautaEleitorCore().Update(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Ok(new PautaEleitorCore().Delete(id));

    }
}