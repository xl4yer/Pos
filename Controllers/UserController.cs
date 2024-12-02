using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Models;
using Pos.Services;

namespace Pos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        UserServices xservices;

        public UserController(UserServices xservices)
        {
            this.xservices = xservices;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<users>> UserLogin(string username, string password)
        {
            var ret = await xservices.UserLogin(username, password);
            return ret;
        }

        [HttpGet]
        public async Task<List<users>> GetUsers()
        {
            var ret = await xservices.GetUsers();
            return ret;
        }

        [HttpPost]
        public async Task<int> AddUser([FromBody] users u)
        {
            var ret = await xservices.AddUser(u);
            return ret;
        }

        [HttpPut]
        public async Task<int> UpdateUser([FromBody] users u)
        {
            var ret = await xservices.UpdateUser(u);
            return ret;
        }
    }
}
