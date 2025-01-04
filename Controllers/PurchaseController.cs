using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PurchaseController : Controller
    {

        PurchaseServices xservices;
        IHubContext<Hub> _hub;
        public PurchaseController(PurchaseServices xservices, IHubContext<Hub> hubContext)
        {
            this.xservices = xservices;
            _hub = hubContext;
        }

        [HttpGet]
        public async Task<List<purchase>> GetPurchase()
        {
            var ret = await xservices.GetPurchase();
            return ret;
        }

        [HttpGet]
        public async Task<double> GetTodaySales()
        {
            return await xservices.GetTodaySales();
        }

        [HttpGet]
        public async Task<double> GetMonthSales()
        {
            return await xservices.GetMonthSales();
        }

        [HttpGet]
        public async Task<ActionResult<List<purchase>>> GetMonthlySales()
        {
            var result = await xservices.GetMonthlySales();
            return Ok(result);
        }

    }
}
