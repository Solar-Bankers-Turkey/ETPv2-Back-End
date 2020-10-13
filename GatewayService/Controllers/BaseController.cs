using GatewayService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers {
    [ApiController]
    [Route("api/v1")]
    public class UserController : ControllerBase {
        [HttpGet]
        [Route("getuser")]
        [Authorize(Roles = "super, standart")]
        public IActionResult GetUserData() {
            return Ok("This is a response from user method");
        }

        [HttpGet]
        [Route("getsuper")]
        [Authorize(Roles = "super")]
        public IActionResult GetAdminData() {
            return Ok("This is a response from Admin method");
        }
    }
}
