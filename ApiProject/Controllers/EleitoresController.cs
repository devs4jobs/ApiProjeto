using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Core;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleitoresController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string eleitor) => Ok(new EleitorCore().Create(eleitor));


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(new EleitorCore().FindBy(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(new EleitorCore().FindAll());

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id) => Ok(new EleitorCore().Update(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) => Ok(new EleitorCore().Delete(id));

    }
}