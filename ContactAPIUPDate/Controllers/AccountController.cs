﻿using Core.API.Services;
using Microsoft.AspNetCore.Mvc;
using Model.APi.Entities;

namespace ContactAPIUPDate.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;


        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUp, string role)
        {
            try
            {
                var result = await _accountRepository.SignUpAsync(signUp, role);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            try
            {
                var result = await _accountRepository.LoginAsync(signInModel);
                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
    }
}
