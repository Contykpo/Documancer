﻿using System.ComponentModel;
using System.Reflection;

using Documancer.Application.Common.ExceptionHandlers;
using Documancer.Application.Common.Security;
using Documancer.Application.Common.Security;

using Documancer.Domain.Identity;

using Documancer.Infrastructure.Constants.ClaimTypes;
using Documancer.Infrastructure.PermissionSet;
using Documancer.Infrastructure.Constants.ClaimTypes;
using Documancer.Infrastructure.PermissionSet;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ZiggyCreatures.Caching.Fusion;
using Documancer.Application.Common.ExceptionHandlers;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Documancer.Server.Services
{
    public class PermissionHelper
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFusionCache _fusionCache;
        private readonly TimeSpan _refreshInterval;

        public PermissionHelper(IServiceScopeFactory scopeFactory, IFusionCache fusionCache)
        {
            var scope = scopeFactory.CreateScope();
            
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            
            _fusionCache = fusionCache;
            
            _refreshInterval = TimeSpan.FromDays(1);
        }

        public async Task<IList<PermissionModel>> GetAllPermissionsByUserId(string userId)
        {
            var assignedClaims = await GetUserClaimsByUserId(userId).ConfigureAwait(false);
            
            IList<PermissionModel> allPermissions = new List<PermissionModel>();
            
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var moduleName = module.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? string.Empty;
                var moduleDescription = module.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ?? string.Empty;
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                allPermissions = allPermissions.Concat(fields.Select(field =>
                {
                    var claimValue = field.GetValue(null)?.ToString();
                    // Convert field name from PascalCase/CamelCase to space-separated words with first letter capitalized.
                    var name = System.Text.RegularExpressions.Regex.Replace(field.Name, "(\\B[A-Z])", " $1").Trim().ToLower();
                    // Capitalize the first letter.
                    name = char.ToUpper(name[0]) + name.Substring(1);
                    // Get the description attribute.
                    var helpText = field.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ?? string.Empty;

                    return new PermissionModel
                    {
                        UserId = userId,
                        ClaimValue = claimValue ?? string.Empty,
                        ClaimType = ApplicationClaimTypes.Permission,
                        Group = moduleName,
                        // Assigning the field name.
                        Name = name,
                        // Assigning the description as HelpText.
                        HelpText = helpText,
                        Description = moduleDescription,
                        Assigned = assignedClaims.Any(x => x.Value.Equals(claimValue))
                    };
                }).Where(pm => !string.IsNullOrEmpty(pm.ClaimValue))).ToList();
            }

            return allPermissions;
        }

        private async Task<IList<Claim>> GetUserClaimsByUserId(string userId)
        {
            var key = $"get-claims-by-{userId}";

            return await _fusionCache.GetOrSetAsync(key, async _ =>
            {
                var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false) ?? throw new NotFoundException($"not found application user: {userId}");
                return await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            },
            _refreshInterval).ConfigureAwait(false);
        }

        public async Task<IList<PermissionModel>> GetAllPermissionsByRoleId(string roleId)
        {
            var assignedClaims = await GetUserClaimsByRoleId(roleId).ConfigureAwait(false);
            
            IList<PermissionModel> allPermissions = new List<PermissionModel>();
            
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var moduleName = module.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? string.Empty;
                var moduleDescription = module.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ?? string.Empty;
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                allPermissions = allPermissions.Concat(fields.Select(field =>
                {
                    var claimValue = field.GetValue(null)?.ToString();
                    // Convert field name from PascalCase/CamelCase to space-separated words with first letter capitalized.
                    var name = System.Text.RegularExpressions.Regex.Replace(field.Name, "(\\B[A-Z])", " $1").Trim().ToLower();
                    // Capitalize the first letter.
                    name = char.ToUpper(name[0]) + name.Substring(1);
                    // Get the description attribute.
                    var helpText = field.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description ?? string.Empty;

                    return new PermissionModel
                    {
                        RoleId = roleId,
                        ClaimValue = claimValue ?? string.Empty,
                        ClaimType = ApplicationClaimTypes.Permission,
                        Group = moduleName,
                        // Assigning the field name.
                        Name = name,
                        // Assigning the description as HelpText.
                        HelpText = helpText,
                        Description = moduleDescription,
                        Assigned = assignedClaims.Any(x => x.Value.Equals(claimValue))
                    };
                }).Where(pm => !string.IsNullOrEmpty(pm.ClaimValue))).ToList();
            }

            return allPermissions;
        }

        private async Task<IList<Claim>> GetUserClaimsByRoleId(string roleId)
        {
            var key = $"get-claims-by-{roleId}";

            return await _fusionCache.GetOrSetAsync(key, async _ =>
            {
                var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new NotFoundException($"not found application role: {roleId}");
            
                return await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
            },
            _refreshInterval).ConfigureAwait(false);
        }
    }

}