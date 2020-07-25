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
        public string host = "https://localhost:5001";
        // public string host = "https://www.etp.solarbankers.org";

        public CustomerController(IUserRepository customerRepository, IMapper mapper, IConfiguration config, IEmailSender emailSender) {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _config = config;
            _emailSender = emailSender;
        }

        /// <summary>
        /// register (first step) end-point.
        /// </summary>
        /// <remarks>
        /// This is the first part of registration it can create email verification link.
        ///
        ///</remarks>
        /// <returns>nothing</returns>
        /// <response code="200">
        /// 
        /// Outcomes:
        /// 
        ///     if email does not exists in database it sends a verification email to the client,
        /// 
        ///     if email exists but not verified its sends a message and a link of verification,
        /// 
        ///     if email address is exists and verified redirects user to login screen
        /// 
        /// </response>
        /// <response code="400">if registration body is not valid</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult> register([FromBody] Register registerObject) {
            // check null object
            if (Utils.RepositoryUtils.isObjectEmpty<Register>(registerObject) || registerObject == null) {
                return BadRequest();
            }
            // check already exists later
            var tempUser = await _customerRepository.GetAny("email", registerObject.email);
            if (tempUser != null) {
                if (tempUser.verified == true) {
                    // ! redirect to sign in page
                    return Ok(new { error = "This email address already exists" });
                } else {
                    // send verification link page
                    var tempID = Utils.RepositoryUtils.getVal(tempUser, "Id");
                    return Ok(new { error = "This email address already exists please verify your email", Link = $"{host}/api/users/verify?id={tempID}" });
                }
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
            var Body = $"Please verify your email address by clicking here to move on to the next step in your energy trading platform membership. {Environment.NewLine} <br /><b><a href='{host}/api/users/verify?id={id}'>Verify My Account</a></b>";
            _emailSender.Send(To, Subject, Body);
            return Ok();
        }

        [HttpPost]
        [Route("verify")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
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

        /// <summary>
        /// general user query end-point.
        /// </summary>
        /// <remarks>
        /// 
        /// /get{query}
        /// 
        /// {query}  value can be null.
        /// 
        /// if request passed with null {query} string it will return all users in data base
        /// 
        /// this for development and debugging purpose.
        /// 
        /// Sample request:
        /// 
        ///     host:port/api/users/get?name=emre
        ///     host:port/api/users/get?email=emreocak@solarbankers.org
        ///
        /// </remarks>
        /// <returns>users array</returns>
        /// <response code="200">Returns the result of query as users model array format</response>
        /// <response code="400">if query is not valid</response>
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
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
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<User>> getWithId(string id) {
            var result = await _customerRepository.GetAny("id", id);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/email/{email}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<User>> getWithEmail(string email) {
            var result = await _customerRepository.GetAny("email", email);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/id/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<User>> deleteWithId(string id) {
            var result = await _customerRepository.GetAny("id", id);
            try {
                _customerRepository.Delete(id);
            } catch (System.Exception) {
                return StatusCode(500);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/email/{email}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
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
            return Ok(result);
        }

        [HttpGet]
        [Route("exists")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<bool>> exists() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                return Ok(new { exists = false });
            }
            var result = await _customerRepository.Query(query);
            var exists = result != null;
            return Ok(new { exists = exists });
        }

    }
}
