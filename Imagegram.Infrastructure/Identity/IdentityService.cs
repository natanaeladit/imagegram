using Imagegram.Application.Common.Interfaces;
using Imagegram.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> CreateAccountAsync(string email, string password, string name)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var authResult = await generateAuthenticationResponseForUserAsync(user);
                authResult.Result = true;
                return authResult;
            }
            else
            {
                AuthResult authResult = new AuthResult();
                authResult.Errors = new List<string>();
                foreach (var err in result.Errors)
                {
                    authResult.Errors.Add(err.Description);
                }
                return authResult;
            }
        }

        public async Task<bool> AccountExistsByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return returnLoginError();
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (isPasswordCorrect)
            {
                var authResult = await generateAuthenticationResponseForUserAsync(user);
                authResult.Result = true;
                return authResult;
            }

            return returnLoginError();
        }

        private AuthResult returnLoginError()
        {
            AuthResult authResult = new AuthResult();
            authResult.Errors = new List<string>() { "Email / Password incorrect" };
            return authResult;
        }

        private Task<AuthResult> generateAuthenticationResponseForUserAsync(ApplicationUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("id", user.Id),
               new Claim(ClaimTypes.NameIdentifier, user.Name),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JwtConfig:TokenLifetimeHours"])),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = jwtHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(new AuthResult
            {
                Result = true,
                Token = jwtHandler.WriteToken(token),
            });
        }
    }
}
