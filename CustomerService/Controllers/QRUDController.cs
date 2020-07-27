using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DataTransferObjects;
using CustomerService.Models;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Controllers {

    [Route("api/users")]
    [ApiController]
    public class QRUDController : ControllerBase {
        private readonly IUserRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public QRUDController(IUserRepository customerRepository, IMapper mapper, IConfiguration config) {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _config = config;
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
        public async Task<ActionResult<IEnumerable<UserGeneralOut>>> get() {
            var query = HttpContext.Request.Query;
            IEnumerable<User> customers;
            if (query.Count == 0) {
                // get all current users
                customers = await _customerRepository.GetAll();
                if (customers == null) {
                    return NoContent();
                }
                return Ok(customers);
            }
            try {
                // execute the query and get results.
                customers = await _customerRepository.Query(query);
            } catch {
                return NoContent();
            }
            if (customers == null) {
                return NoContent();
            }
            // Users to UserGeneralOut conv.
            List<UserGeneralOut> customersOut = new List<UserGeneralOut>();
            foreach (User user in customers) {
                var userOut = _mapper.Map<UserGeneralOut>(user);
                // creating and adding id field as string.
                var id = Utils.RepositoryUtils.getVal(user, "Id");
                userOut.idString = id;
                customersOut.Add(userOut);
            }
            return Ok(customersOut);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<UserGeneralOut>> update([FromBody] UserGeneralIn modifiedUser) {
            var result = await _customerRepository.GetAny("id", modifiedUser.idString);
            var user = result.FirstOrDefault();
            if (user == null) {
                return BadRequest();
            }
            _mapper.Map(modifiedUser, user);
            if (modifiedUser.password != null) {
                var salt = _config.GetValue<string>("salt");
                user.passwordHash = BCrypt.Net.BCrypt.HashPassword(salt + modifiedUser.password);
            }
            _customerRepository.Update(user);
            return Ok(user);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<User>>> delete() {
            var query = HttpContext.Request.Query;
            if (query.Count == 0) {
                return BadRequest();
            }
            var customers = await _customerRepository.Query(query);
            foreach (User user in customers) {
                var id = Utils.RepositoryUtils.getVal(user, "Id");
                _customerRepository.Delete(id);
            }
            return Ok(customers);
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
