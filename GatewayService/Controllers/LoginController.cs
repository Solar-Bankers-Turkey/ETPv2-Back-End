using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using GatewayService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthenticationExample.Controllers {
    [ApiController]
    [Route("api/v1")]
    public class LoginController : ControllerBase {
        private readonly IConfiguration _config;
        private List<User> appUsers = new List<User> {
            new User { FullName = "emre ocak", UserName = "emre", Password = "emre", UserRole = "super" },
            new User { FullName = "barış erbay", UserName = "barış", Password = "barış", UserRole = "standart" }
        };

        public LoginController(IConfiguration config) {
            _config = config;
        }

        [HttpGet]
        [Route("home")]
        // [Authorize(Roles = "super, standart")]
        public IActionResult home() {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            if (authHeader == null) {
                return Ok();
            }
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            // var role = tokenS.Claims.First(claim => claim.Type == "role").Value;
            // Console.WriteLine(role);
            // return Ok();
            return Redirect("https://localhost:5001/api/v1/users/get");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody] User login) {
            IActionResult response = Unauthorized();
            User user = AuthenticateUser(login);
            if (user != null) {
                var tokenString = GenerateJWTToken(user);
                response = Ok(new {
                    token = tokenString,
                        userDetails = user,
                });
            }
            return response;
        }

        User AuthenticateUser(User loginCredentials) {
            User user = appUsers.SingleOrDefault(x => x.UserName == loginCredentials.UserName && x.Password == loginCredentials.Password);
            return user;
        }

        string GenerateJWTToken(User userInfo) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new [] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role", userInfo.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience : _config["Jwt:Audience"],
                claims : claims,
                expires : DateTime.Now.AddMinutes(30),
                signingCredentials : credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
