using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService {

    [ApiController]
    public class BaseController : ControllerBase {

        [HttpGet]
        [Route("/login")]
        public async Task<IActionResult> login() {
            return Ok();
        }

    }
}
