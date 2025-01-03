using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : Controller
    {

        TempServices xservices;
        IHubContext<Hub> _hub;
        public TempController(TempServices xservices, IHubContext<Hub> hubContext)
        {
            this.xservices = xservices;
            _hub = hubContext;
        }

        [HttpGet]
        public async Task<List<temp>> GetTemp()
        {
            var ret = await xservices.GetTemp();
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

        [HttpPost]
        public async Task<int> DeleteTemp([FromBody] temp t)
        {
            var ret = await xservices.DeleteTemp(t);
            return ret;
        }

        [HttpPost]
        public async Task<int> TempToPurchase()
        {
            var ret = await xservices.TempToPurchase();
            return ret; 
        }

        [HttpGet]
        public async Task<double> Total()
        {
            return await xservices.Total();
        }

    }
}
