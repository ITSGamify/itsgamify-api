﻿using its.gamify.core.Models.Auth;
using its.gamify.core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace its.gamify.api.Controllers
{
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService authService = authService;

        [HttpPost("google/{token}")]
        public async Task<IActionResult> LoginGoogleAsync([FromRoute] string token)
        {
            var loginRes = await authService.LoginGoogleAsync(token, default);
            return loginRes is not null ? Ok(loginRes) : StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequestModel model)
        {
            try
            {
                var loginRes = await authService.LoginAsync(model.Email, model.Password);
                return loginRes is not null ? Ok(loginRes) : StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
