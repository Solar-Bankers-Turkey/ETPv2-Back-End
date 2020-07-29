using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DataTransferObjects.Login;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Controllers {
    [Route("api/v1/users")]
    [ApiController]
    public class LoginController : ControllerBase {

        private readonly IUserRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public LoginController(IUserRepository customerRepository, IMapper mapper, IConfiguration config) {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// login control, user exists check
        /// </summary>
        /// <returns>boolean 'user exists' value</returns>
        /// <response code="200">Returns a boolean value that represents user exists or not</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> login([FromBody] LoginIn loginObject) {
            var result = await _customerRepository.GetAny("email", loginObject.email);
            var user = result.FirstOrDefault();
            if (user == null) {
                return BadRequest();
            }
            var exists = BCrypt.Net.BCrypt.Verify(loginObject.password, user.passwordHash);
            return Ok(exists);
        }
    }
}
