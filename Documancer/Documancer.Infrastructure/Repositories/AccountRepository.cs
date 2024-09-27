using Documancer.Domain.Entities.Authentication;
using Documancer.Application.DataTransferObjects.Request.Account;
using Documancer.Application.DataTransferObjects.Response;
using Documancer.Application.DataTransferObjects.Response.Account;
using Documancer.Application.Contracts;
using Documancer.Application.Extensions;
using Documancer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Documancer.Infrastructure.Repositories
{
    public class AccountRepository(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context) : IAccount
    {

        #region Methods

        // Interface methods:

        public async Task<GeneralResponse> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model)
        {
            if (await FindRoleByNameAsync(model.RoleName) is null)
            {
                return new GeneralResponse(false, "Role not found.");
            }
            if (await FindUserByEmailAsync(model.UserEmail) is null)
            {
                return new GeneralResponse(false, "User not found.");
            }

            var user = await FindUserByEmailAsync(model.UserEmail);
            var previousRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            
            var removeOldRole = await userManager.RemoveFromRoleAsync(user, previousRole);            
            var error = CheckResponse(removeOldRole);

            if (!string.IsNullOrEmpty(error)) { return new GeneralResponse(false, error); }

            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            var response = CheckResponse(result);

            if (!string.IsNullOrEmpty(error))
            {
                return new GeneralResponse(false, response);
            }
            else
            {
                return new GeneralResponse(true, "Role changed.");
            }
        }

        public async Task<GeneralResponse> CreateAccountAsync(CreateAccountDTO model)
        {
            try
            {
                if (await FindUserByEmailAsync(model.EmailAddress) != null)
                {
                    return new GeneralResponse(false, "User is already created.");
                }

                var user = new ApplicationUser()
                {
                    Name = model.Name,
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PasswordHash = model.Password
                };

                var result = await userManager.CreateAsync(user, model.Password);
            
                string error = CheckResponse(result);

                if (!string.IsNullOrEmpty(error))
                {
                    return new GeneralResponse(false, error);
                }

                var (flag, message) = await AssignUserToRole(user, new IdentityRole() { Name = model.Role });

                return new GeneralResponse(flag, message);
            }
            catch (Exception exception)
            {
                return new GeneralResponse(false, exception.Message);
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                if ((await FindRoleByNameAsync(Constant.Role.Admin)) != null) { return; }

                var admin = new CreateAccountDTO()
                {
                    Name = "Admin",
                    Password = "Admin**123",
                    EmailAddress = "admin@admin.com",
                    Role = Constant.Role.Admin
                };

                await CreateAccountAsync(admin);
            }
            catch { }
        }

        public async Task<GeneralResponse> CreateRoleAsync(CreateRoleDTO model)
        {
            try
            {
                if ((await FindRoleByNameAsync(model.Name)) == null)
                {
                    var response = await roleManager.CreateAsync(new IdentityRole(model.Name));
                    var error = CheckResponse(response);

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }
                    else
                    {
                        return new GeneralResponse(true, $"{model.Name} role created.");
                    }
                }

                return new GeneralResponse(false, $"{model.Name} role is already created.");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<IEnumerable<GetRoleDTO>> GetRolesAsync() => (await roleManager.Roles.ToListAsync()).Adapt<IEnumerable<GetRoleDTO>>();

        public async Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRolesAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();

            if (allUsers is null) { return null; }

            var list = new List<GetUsersWithRolesResponseDTO>();

            foreach (var user in allUsers)
            {
                var getUserRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                var getRoleInfo = await roleManager.Roles.FirstOrDefaultAsync(role => role.Name.ToLower() == getUserRole.ToLower());

                list.Add(new GetUsersWithRolesResponseDTO()
                {
                    Name = user.Name,
                    EmailAddress= user.Email,
                    RoleId = getRoleInfo.Id,
                    RoleName = getRoleInfo.Name
                });
            }

            return list;
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginDTO model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.EmailAddress);

                if (user is null) { return new LoginResponse(false, "User not found."); }

                SignInResult result;
                
                try
                {
                    result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                }
                catch
                {
                    return new LoginResponse(false, "Invalid credentials.");
                }

                if (!result.Succeeded) { return new LoginResponse(false, "Invalid credentials."); }

                string jwtToken = await GenerateToken(user);
                string refreshToken = GenerateRefreshToken();
                
                if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return new LoginResponse(false, "An error occured while logging in. Please retry or contact an administrator.");
                }
                else
                {
                    var saveResult = await SaveRefreshToken(user.Id, refreshToken);

                    if (saveResult.Flag)
                    {
                        return new LoginResponse(true, $"{user.Name} succesfully logged in.", jwtToken, refreshToken);
                    }
                    else
                    {
                        return new LoginResponse();
                    }
                }
            }
            catch (Exception exception)
            {
                return new LoginResponse(false, exception.Message);
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model)
        {
            var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == model.Token);

            if (token == null) { return new LoginResponse(); }

            var user = await userManager.FindByIdAsync(token.UserId);

            string newToken = await GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();

            var saveResult = await SaveRefreshToken(user.Id, newRefreshToken);

            if (saveResult.Flag)
            {
                return new LoginResponse(true, $"{user.Name} successfully re-logged in.", newToken, newRefreshToken);
            }
            else
            {
                return new LoginResponse();
            }
        }

        // Class-specific methods:

        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await userManager.FindByEmailAsync(email);
        private async Task<IdentityRole> FindRoleByNameAsync(string roleName) => await roleManager.FindByNameAsync(roleName);

        private async Task<GeneralResponse> AssignUserToRole(ApplicationUser user, IdentityRole role)
        {
            if (user is null || role is null)
            {
                return new GeneralResponse(false, "Model state cannot be empty.");
            }
            if (await FindRoleByNameAsync(role.Name) == null)
            {
                await CreateRoleAsync(role.Adapt(new CreateRoleDTO()));
            }

            IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);

            string error = CheckResponse(result);

            if (!string.IsNullOrEmpty(error))
            {
                return new GeneralResponse(false, error);
            }
            else
            {
                return new GeneralResponse(true, $"{user.Name} assigned to {role.Name} role.");
            }
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(user)).FirstOrDefault().ToString()),
                    new Claim("Fullname", user.Name)
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

        private async Task<GeneralResponse> SaveRefreshToken(string userId, string token)
        {
            try
            {
                var user = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);

                if (user == null)
                {
                    context.RefreshTokens.Add(new RefreshToken() { UserId = userId, Token = token });
                }
                else
                {
                    user.Token = token;
                }

                await context.SaveChangesAsync();

                return new GeneralResponse(true, null!);
            }
            catch (Exception exception)
            {
                return new GeneralResponse(false, exception.Message);
            }
        }

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private static string CheckResponse(IdentityResult result)
        {
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(_ => _.Description);

                return string.Join(Environment.NewLine, errors);
            }

            return null!;
        }

        #endregion
    }
}
