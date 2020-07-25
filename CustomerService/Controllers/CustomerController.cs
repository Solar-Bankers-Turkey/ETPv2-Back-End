using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Email;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Controllers {

    [Route("api/users")]
    [ApiController]
    public class CustomerController : ControllerBase {
        private readonly IUserRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private IEmailSender _emailSender;
        public string mainUrl = "https://www.etp.solarbankers.org";

        public CustomerController(IUserRepository customerRepository, IMapper mapper, IConfiguration config, IEmailSender emailSender) {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _config = config;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<User>> register([FromBody] Register registerObject) {
            // check null object
            if (Utils.RepositoryUtils.isObjectEmpty<Register>(registerObject) || registerObject == null) {
                return BadRequest();
            }
            // check already exists later
            var tempUser = await _customerRepository.GetAny("email", registerObject.email);
            if (tempUser != null) {
                return Ok(new { error = "This email address already exists" });
            }
            var salt = _config.GetValue<string>("salt");
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(salt + registerObject.password);
            User user = _mapper.Map<User>(registerObject);
            // ! generate persons wallet
            user.passwordHash = passwordHash;
            // mark as not verified.
            user.verified = false;
            string id;
            try {
                id = await _customerRepository.Create(user);
            } catch {
                // status code not modified
                return StatusCode(304);
            }
            // send mail for next step                    
            var To = registerObject.email;
            var Subject = "Verify your account";
            var Body = $"Please verify your email address by clicking here to move on to the next step in your energy trading platform membership. {Environment.NewLine} <br /><b><a href='{mainUrl}/api/users/verify?id={id}'>Verify My Account</a></b>";
            _emailSender.Send(To, Subject, Body);
            return Ok();
        }

        [HttpPost]
        [Route("verify")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> verify([FromBody] Verify verifyObject) {
            var query = HttpContext.Request.Query;
            if (query.Count != 1) {
                Console.WriteLine(1);
                return BadRequest();
            }
            var kv = query.Where(a => a.Key == "id").FirstOrDefault();
            if (kv.Value == "") {
                Console.WriteLine(2);
                return BadRequest();
            }
            var user = await _customerRepository.GetAny("id", kv.Value);
            if (user == null) {
                Console.WriteLine(3);
                return BadRequest();
            }
            if (user.verified) {
                return Ok(new { message = "You have already verified your account." });
            }
            // ! add real customerType
            user.customerType = "consumer";
            Detail detail = _mapper.Map<Detail>(verifyObject);
            user.detail = detail;
            if (verifyObject.region == null) {
                user.detail.region = "Turkey";
            }
            if (verifyObject.language == null) {
                user.detail.language = "TR";
            }
            user.verified = true;
            try {
                _customerRepository.Update(user);
            } catch {
                return Problem("database update error", "mongodb", 500, "database error", "database");
            }
            return Created("", user);
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<User>>> get() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                var customers = await _customerRepository.Get();
                return Ok(customers);
            }
            try {
                // ! nested query and object query
                var result = await _customerRepository.Query(query);
                if (result == null) {
                    return NoContent();
                }
                return Ok(result);
            } catch {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("get/id/{id}")]
        public async Task<ActionResult<User>> getWithId(string id) {
            var result = await _customerRepository.GetAny("id", id);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/email/{email}")]
        public async Task<ActionResult<User>> getWithEmail(string email) {
            var result = await _customerRepository.GetAny("email", email);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/id/{id}")]
        public async Task<ActionResult<User>> deleteWithId(string id) {
            try {
                _customerRepository.Delete(id);
            } catch (System.Exception) {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpDelete]
        [Route("delete/email/{email}")]
        public async Task<ActionResult<User>> deleteWithEmail(string email) {
            var result = await _customerRepository.GetAny("email", email);
            if (result == null) {
                return StatusCode(304);
            }
            var id = Utils.RepositoryUtils.getVal(result, "Id");
            try {
                _customerRepository.Delete(id);
            } catch {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet]
        [Route("exists")]
        public async Task<ActionResult<bool>> exists() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                return Ok(false);
            }
            var result = await _customerRepository.Query(query);
            var exists = result != null;
            return Ok(exists);
        }

        // [HttpGet]
        // [Route("index")]
        // public async Task<ActionResult> index() {
        // string passwordHash = BCrypt.Net.BCrypt.HashPassword("emre");
        // bool verified = BCrypt.Net.BCrypt.Verify("emre", passwordHash);
        // return Ok(verified);
        // return Ok();
        // }

    }
}
