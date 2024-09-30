global using System.Security.Claims;

global using Documancer.Application.Common.Interfaces;
global using Documancer.Application.Common.Interfaces.Identity;
global using Documancer.Application.Common.Models;
global using Documancer.Infrastructure.Persistence;
global using Documancer.Infrastructure.Persistence.Extensions;
global using Documancer.Infrastructure.Services;
global using Documancer.Infrastructure.Services.Identity;
global using Documancer.Domain.Entities;

global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;