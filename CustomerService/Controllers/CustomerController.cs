using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Controllers {

    [Route("customers")]
    [ApiController]
    public class CustomerController : ControllerBase {

        private readonly IUserRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CustomerController(IUserRepository customerRepository, IMapper mapper, IConfiguration config) {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> registerFirstStep([FromBody] RegisterFirstStep registerFirst) {
            // ! check already exists later
            // ! check null object
            var salt = _config.GetValue<string>("salt");
            string passwordHash = salt + BCrypt.Net.BCrypt.HashPassword(registerFirst.password);
            User user = _mapper.Map<User>(registerFirst);
            // ! generate persons wallet
            user.passwordHash = passwordHash;
            try {
                _customerRepository.Create(user);
            } catch {
                // ! change status code with decent status code
                return StatusCode(304);
            }
            // ! create decent user link
            // ! send mail for next step
            return Created("", user);
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<User>> get() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                var customers = await _customerRepository.Get();
                return Ok(customers);
            }
            var result = _customerRepository.Query(query);
            return Ok(result.Result);
        }

        [HttpGet]
        [Route("exists")]
        public async Task<ActionResult<bool>> exist() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                return Ok(false);
            }
            var result = _customerRepository.Query(query);
            return Ok(result.Result != null);
        }

        [HttpGet]
        [Route("index")]
        public async Task<ActionResult> index() {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword("emre");
            bool verified = BCrypt.Net.BCrypt.Verify("emre", passwordHash);
            return Ok(verified);
        }
    }
}
