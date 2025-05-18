using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mysqlx.Crud;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        ProductServices xservices;
        IHubContext<Hub> _hub;

        public ProductController(ProductServices xservices, IHubContext<Hub> hubContext)
        {
            this.xservices = xservices;
            _hub = hubContext;
        }

       
        [HttpGet]
        public async Task<List<products>> GetProducts()
        {
            var ret = await xservices.GetProducts();
            return ret;
        }

        [HttpPost]
        public async Task<int> AddProduct([FromBody] products p)
        {
            var ret = await xservices.AddProduct(p);
            return ret;
        }

        [HttpPut]
        public async Task<int> UpdateProduct([FromBody] products p)
        {
            var ret = await xservices.UpdateProduct(p);
            return ret;
        }

        [HttpGet]
        public async Task<List<products>> SearchProduct(string s)
        {
            var ret = await xservices.SearchProduct(s);
            return ret;
        }
    }
}
