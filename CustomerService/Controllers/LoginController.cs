using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Email;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Controllers {
    [Route("api/users")]
    [ApiController]
    public class LoginController : ControllerBase {

        private readonly IUserRepository _customerRepository;
        // private readonly IMapper _mapper;

        public LoginController(IUserRepository customerRepository, IMapper mapper, IConfiguration config) {
            _customerRepository = customerRepository;
            // _mapper = mapper;
        }

        // verify user if has tokens delete if has not generate a token 
        // with help of this end-point.
        [HttpGet]
        [Route("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<User>> login() {
            var query = HttpContext.Request.Query;
            if (query.Count != 1) {
                return BadRequest();
            }
            string key = "";
            string value = "";
            foreach (var item in query) {
                key = item.Key;
                value = item.Value;
                if (key != "id") {
                    return BadRequest();
                }
            }
            var user = _customerRepository.GetAny(key, value).Result.FirstOrDefault();
            if (user == null) {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
