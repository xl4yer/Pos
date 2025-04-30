using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QtyController : Controller
    {
        QtyServices xservices;
        IHubContext<Hub> _hub;
        public QtyController(QtyServices xservices, IHubContext<Hub> hubContext)
        {
            this.xservices = xservices;
            _hub = hubContext;
        }

        [HttpGet]
        public async Task<List<Qty>> GetQty()
        {
            var ret = await xservices.GetQty();
            return ret;
        }

        [HttpPost]
        public async Task<int> AddQty([FromBody] Qty q)
        {
            var ret = await xservices.AddQty(q);
            return ret;
        }
    }
}
