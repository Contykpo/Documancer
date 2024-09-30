global using System.ComponentModel;
global using System.Data;
global using System.Globalization;
global using System.Linq.Dynamic.Core;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Text.Json;

global using Ardalis.Specification;

global using AutoMapper;
global using AutoMapper.QueryableExtensions;

global using Documancer.Application.Common.ExceptionHandlers;
global using Documancer.Application.Common.Extensions;
global using Documancer.Application.Common.Interfaces;
global using Documancer.Application.Common.Interfaces.Caching;
global using Documancer.Application.Common.Models;
global using Documancer.Application.Common.Security;
global using Documancer.Domain.Common.Enums;
global using Documancer.Domain.Common.Events;
global using Documancer.Domain.Events;
global using Documancer.Domain.Entities;

global using FluentValidation;

global using LazyCache;

global using MediatR;
global using MediatR.Pipeline;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Primitives;