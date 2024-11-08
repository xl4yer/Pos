using Microsoft.AspNetCore.Mvc;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : Controller
    {

        TempServices xservices;

        public TempController(TempServices xservices)
        {
            this.xservices = xservices;
        }

        [HttpGet]
        public async Task<List<temp>> Temp()
        {
            var ret = await xservices.Temp();
            return ret;
        }

        [HttpPost]
        public async Task<int> AddTemp([FromBody] temp t)
        {
            var ret = await xservices.AddTemp(t);
            return ret;
        }

        [HttpPut]
        public async Task<int> UpdateTemp([FromBody] temp t)
        {
            var ret = await xservices.UpdateTemp(t);
            return ret;
        }

        [HttpGet]
        public async Task<double> Total()
        {
            return await xservices.Total();
        }

    }
}
