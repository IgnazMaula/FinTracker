using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace FinTracker.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var userId = string.Empty;

            // Get JWT token from Authorization header
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                // No token found, return hardcoded user id for now
                userId = "hardcoded-user-id";  // Replace with a hardcoded or static ID
            }
            else
            {
                // Token exists, extract the user ID from JWT
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;  // "sub" is the claim for the user id
            }

            return userId;
        }
    }
}
