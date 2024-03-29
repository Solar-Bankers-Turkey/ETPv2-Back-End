using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DataTransferObjects.General;
using CustomerService.DataTransferObjects.Registration;
using CustomerService.DataTransferObjects.Verify;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers {

    [Route("api/v1/users")]
    [ApiController]
    public class RegistrationController : ControllerBase {
        private readonly IUserRepository _customerRepository;
        private readonly IMapper _mapper;

        public RegistrationController(IUserRepository customerRepository, IMapper mapper) {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// register (first step of registration).
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
        public async Task<ActionResult<RegisterOut>> register([FromBody] RegisterIn registerObject) {
            // check null object
            if (Utils.RepositoryUtils.isObjectEmpty<RegisterIn>(registerObject) || registerObject == null) {
                return BadRequest();
            }
            // host string creation for local: 'https://localhost:5001'
            string host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
            // check already exists later
            var result = await _customerRepository.GetAny("email", registerObject.email);
            var tempUser = result.FirstOrDefault();
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
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerObject.password, salt);
            User user = _mapper.Map<User>(registerObject);

            // ! generate persons wallet
            // ! remove dummy wallet id generation
            user.walletID = BCrypt.Net.BCrypt.HashPassword("dummy wallet").ToString();
            user.passwordHash = passwordHash;
            user.verified = false;
            string id;
            try {
                id = await _customerRepository.Create(user);
            } catch {
                // status code not modified
                return StatusCode(304);
            }
            // // send mail for next step                    
            // var To = registerObject.email;
            // var Subject = "Verify your account";
            // var Body = $"Please verify your email address by clicking here to move on to the next step in your energy trading platform membership. {Environment.NewLine} <br /><b><a href='{host}/api/v1/users/verify?id={id}'>Verify My Account</a></b>";

            // _emailSender.Send(To, Subject, Body);

            var userOut = _mapper.Map<RegisterOut>(user);

            return Created("", userOut);
        }

        /// <summary>
        /// verify (last step of registration).
        /// </summary>
        /// <remarks>
        /// This is the last part of registration.
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
        [Route("verify")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<IActionResult> verify([FromBody] VerifyIn verifyObject) {
            var query = HttpContext.Request.Query;
            if (query.Count != 1) {
                return BadRequest();
            }
            var kv = query.Where(a => a.Key == "id").FirstOrDefault();
            if (kv.Value == "") {
                return BadRequest();
            }
            var result = await _customerRepository.GetAny("id", kv.Value);
            var user = result.FirstOrDefault();
            if (user == null) {
                return BadRequest();
            }
            if (user.verified) {
                return Ok(new { message = "You have already verified your account." });
            }
            // ! add real customerType
            user.customerType = "consumer";
            user.info = verifyObject.info;
            user.info.registrationDate = DateTime.Now;
            user.verified = true;
            user.role = "standart";

            user.settings = _mapper.Map(verifyObject.settings, user.settings);

            _customerRepository.Update(user);

            var userOut = _mapper.Map<GeneralOut>(user);
            return Created("", userOut);
        }
    }
}
