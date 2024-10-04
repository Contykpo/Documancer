using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository(IApplicationDbContext context, IConfiguration configuration) : IUserRepository
    {
        // Interface implementation methods:

        public async Task<LoginResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await FindUserByEmailAsync(loginUserDTO.EmailAddress);

                if (user is null) { return new LoginResponse(false, "User not found."); }

                bool checkPassword = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password);

                if (checkPassword)
                {
                    return new LoginResponse(true, "Login successfully.", GenerateJWTToken(user));
                }
                else
                {
                    return new LoginResponse(false, "Invalid credentials. Failed to login.");
                }
            }
            catch (Exception exception)
            {
                return new LoginResponse(false, exception.Message);
            }
        }

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            try
            {
                if (await FindUserByEmailAsync(registerUserDTO.EmailAddress) != null)
                {
                    return new RegistrationResponse(false, "User already exists.");
                }

                context.Users.Add(new ApplicationUser()
                {
                    Name = registerUserDTO.Name,
                    Email = registerUserDTO.EmailAddress,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
                });

                await context.SaveChangesAsync();

                return new RegistrationResponse(true, "Registration completed.");
            }
            catch (Exception exception)
            {
                return new RegistrationResponse(false, exception.Message);
            }
        }


        // Class-specific methods:

        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        private string GenerateJWTToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name!),
                    new Claim(ClaimTypes.Email, user.Email!)
                };

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null!;
            }
        }
    }
}
