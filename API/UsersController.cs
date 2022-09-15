using System.Threading.Tasks;
using AuthenticationSystem.Data.Models;
using AuthenticationSystem.Quickstart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.API
{
    [Route("users/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserStore _userStore;

        public UsersController(
            IUserStore userStore
        )
        {
            _userStore = userStore;
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> CreateUser([FromBody] Users user)
        {
            var response = await _userStore.SaveAppUser(user);
            return Json(new { response = response });
        }
    }
}
