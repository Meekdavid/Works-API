Efficient effic
﻿using Microsoft.AspNetCore.Mvc;
using Works_API.Models.DTO;
using Works_API.Repositories;

namespace Works_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
                    
            //Validate the incoming request, I did this using FluentValidations


            //Check if user is authenticated
            //Check username and password
            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.password);
            if (User != null)
            {
                //Generate a JWT Token
               var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);  
            }
            return BadRequest("Username or Password is incorrect");
        }
    }
}
