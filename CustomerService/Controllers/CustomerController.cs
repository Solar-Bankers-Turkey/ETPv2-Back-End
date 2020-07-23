using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers {
    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase {
        private readonly IUserRepository _customerRepository;
        // public Crypto encoder { get; set; }
        // public string DefaultLanguage { get; set; }
        public CustomerController(IUserRepository customerRepository) {
            _customerRepository = customerRepository;
            // encoder = new Crypto();
            // DefaultLanguage = "TR";
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllCustomers() {
            var customers = await _customerRepository.Get();
            return Ok(customers);
        }

        [HttpGet]
        [Route("index")]
        public async Task<ActionResult> index() {
            return Ok();
        }
    }
}
